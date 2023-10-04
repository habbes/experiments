using JsonNet = Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stj = System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JsonWriterPerf;

[MemoryDiagnoser]
//[InProcess]
public class Benchmarks
{
    public Stream stream = new MemoryStream();
    public List<Person> data = new List<Person>();
    public List<Person> finalData = new List<Person>();
    public string jsonData;
    public string finalJsonData;


    [Params(4, 1000)]
    public int dataCount;

    [GlobalSetup]
    public void Setup()
    {
        int friendsCount = 100;

        data = new List<Person>();

        for (int i = 0; i < dataCount; i++)
        {
            var friends = new List<Person>();
            for (int j = 0; j < friendsCount; j++)
            {
                friends.Add(new Person
                {
                    Name = $"P{i}Friend{j}",
                    Age = 20 + i + j,
                    Active = j % 2 == 0,
                    Address = "Add",
                    Experience = 10,
                    Added = j % 2 != 0,
                    Amount = 20 + i,
                    Foo = 10,
                    Bar = 10,
                    SomeBool = false,
                    SomeInt = 20 * i,
                    SomeString = $"Str{i}",
                    AnotherInt = 25,
                });
            }

            var person = new Person
            {
                Name = $"Person{i}",
                Age = 20 + i,
                Active = i % 2 == 0,
                Address = "Add",
                Experience = 10,
                Added = i % 2 != 0,
                Amount = 20 + i,
                Foo = 10,
                Bar = 10,
                SomeBool = false,
                SomeInt = 20 * i,
                SomeString = $"Str{i}",
                AnotherInt = 25,
                Friends = friends
            };

            data.Add(person);
        }

        finalData = new List<Person>(data)
        {
             
        };

        for (var i = 0; i < dataCount; i++)
        {
            finalData.Add(new Person
            {
                Name = "Sam",
                Age = 32,
                Active = false,
                Friends = new List<Person>()
                {
                    new Person
                    {
                        Name = "Jale",
                        Age = 30,
                        Active = true
                    }
                }
            });
        }



        jsonData = Stj.JsonSerializer.Serialize(data);
        finalJsonData = Stj.JsonSerializer.Serialize(finalData);
        stream.Seek(0, SeekOrigin.Begin);
    }

    [GlobalCleanup]
    public void Teardown()
    {
        stream.Close();
    }

    public string ReadStream()
    {
        stream.Position = 0;
        var reader = new StreamReader(stream, leaveOpen: true);
        return reader.ReadToEnd();
    }

    [Benchmark]
    public void SerializePayloadJToken()
    {
        var token = JToken.Parse(jsonData);
        var array = (JArray)token;

        for (var i = 0; i < dataCount; i++)
        {
            array.Add(new JObject
            {
                {  "Name",  "Sam" },
                { "Age", 32 },
                { "Active", false },
                {
                    "Friends",
                    new JArray()
                    {
                        new JObject
                        {
                            { "Name", "Jale" },
                            { "Age", 30 },
                            { "Active", true },
                            { "Friends", new JArray() }
                        }
                    }
                }
            });
        }
        
        stream.Position = 0;
        using StreamWriter sw = new StreamWriter(stream, leaveOpen: true);
        using JsonNet.JsonWriter writer = new JsonNet.JsonTextWriter(sw);
        array.WriteTo(writer);
        writer.Flush();
    }

    [Benchmark]
    public void SerializePayloadJsonNode()
    {
        var node = JsonNode.Parse(jsonData);
        var array = node.AsArray();

        for (var i = 0; i < dataCount; i++)
        {
            array.Add(new JsonObject() {
                {  "Name",  "Sam" },
                { "Age", 32 },
                { "Active", false },
                {
                    "Friends",
                    new JsonArray()
                    {
                        new JsonObject
                        {
                            { "Name", "Jale" },
                            { "Age", 30 },
                            { "Active", true },
                            { "Friends", new JsonArray() }
                        }
                    }
                }
            });
        }
        
        stream.Position = 0;
        using var writer = new Utf8JsonWriter(stream);
        array.WriteTo(writer);
        writer.Flush();
    }

    [Benchmark]
    public void SerializeExtensibleJsonElement()
    {
        var doc = JsonDocument.Parse(jsonData);
        var jsonElement = doc.RootElement;
        stream.Position = 0;

        var array = new ExtensibleJsonArray(jsonElement);
        var obj = new ExtensibleJsonObject();
        obj.Add("Name", "Sam");
        obj.Add("Age", 32);
        obj.Add("Active", false);
        var friends = new ExtensibleJsonArray();
        var friend = new ExtensibleJsonObject();
        friend.Add("Name", "Jale");
        friend.Add("Age", 30);
        friend.Add("Active", true);
        friend.Add("Friends", new ExtensibleJsonArray());
        friends.Add(friend);
        obj.Add("Friends", friends);
        array.Add(obj);

        using var writer = new Utf8JsonWriter(stream);
        array.WriteTo(writer);
        writer.Flush();
    }

    [Benchmark]
    public void SerializeJsonElement()
    {
        var doc = JsonDocument.Parse(finalJsonData);
        var jsonElement = doc.RootElement;
        stream.Position = 0;

        using var writer = new Utf8JsonWriter(stream);
        jsonElement.WriteTo(writer);
        writer.Flush();
    }

    [Benchmark]
    public int TraverseJToken()
    {
        var array = (JArray)JToken.Parse(finalJsonData);
        int count = 0;
        int totalProperties = 0;
        int maxAge = 0;
        int allActive = 0;
        foreach (var item in array)
        {
            count++;
            var person = (JObject)item;
            foreach (var property in person)
            {
                var key = property.Key;
                if (key != null)
                {
                    // useless statement to make sure these don't get optimized away
                    totalProperties++;
                }

                if (property.Value == null)
                {
                    continue;
                }

                if (key == "Friends")
                {
                    var friends = (JArray)property.Value!;
                    foreach (var f in friends)
                    {
                        var friend = (JObject)f;
                        foreach (var fProp in friend)
                        {
                            if (fProp.Value == null)
                            {
                                continue;
                            }

                            if (fProp.Key == "Active")
                            {
                                allActive += fProp.Value.Value<bool>() == true ? 1 : 0;
                            }
                            else if (fProp.Key == "Age")
                            {
                                maxAge += fProp.Value.Value<int>();
                            }
                        }
                    }
                }

                if (property.Key == "Active")
                {
                    allActive += property.Value.Value<bool>() == true ? 1 : 0;
                }
                else if (property.Key == "Age")
                {
                    maxAge += property.Value.Value<int>();
                }
            }
        }


        Check(count == dataCount * 2);
        return totalProperties + maxAge + allActive;
    }

    [Benchmark]
    public int TraverseJsonNode()
    {
        var array = JsonNode.Parse(finalJsonData)!.AsArray();
        int count = 0;
        int totalProperties = 0;
        int maxAge = 0;
        int allActive = 0;
        foreach (var item in array)
        {
            count++;
            var person = item!.AsObject();
            foreach (var property in person)
            {
                var key = property.Key;
                if (key != null)
                {
                    // useless statement to make sure these don't get optimized away
                    totalProperties++;
                }

                if (property.Value == null)
                {
                    continue;
                }

                if (key == "Friends")
                {
                    var friends = (JsonArray)property.Value.AsArray();
                    foreach (var f in friends)
                    {
                        var friend = (JsonObject)f!;
                        foreach (var fProp in friend)
                        {
                            if (fProp.Value == null)
                            {
                                continue;
                            }

                            if (fProp.Key == "Active")
                            {
                                allActive += fProp.Value.GetValue<bool>() == true ? 1 : 0;
                            }
                            else if (fProp.Key == "Age")
                            {
                                maxAge += fProp.Value.GetValue<int>();
                            }
                        }
                    }
                }

                if (property.Key == "Active")
                {
                    allActive += property.Value.GetValue<bool>() == true ? 1 : 0;
                }
                else if (property.Key == "Age")
                {
                    maxAge += property.Value.GetValue<int>();
                }
            }
        }

        Check(count == dataCount * 2);
        return totalProperties + maxAge + allActive;
    }

    [Benchmark]
    public int TraverseJsonElement()
    {
        var array = JsonDocument.Parse(finalJsonData)!.RootElement;
        int count = 0;
        int totalProperties = 0;
        int maxAge = 0;
        int allActive = 0;
        foreach (var item in array.EnumerateArray())
        {
            count++;
            foreach (var property in item.EnumerateObject())
            {
                var key = property.Name;
                if (key != null)
                {
                    // useless statement to make sure these don't get optimized away
                    totalProperties++;
                }

                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }

                if (key == "Friends")
                {
                    var friends = property.Value;
                    foreach (var friend in friends.EnumerateArray())
                    {
                        foreach (var fProp in friend.EnumerateObject())
                        {
                            if (fProp.Value.ValueKind == JsonValueKind.Null)
                            {
                                continue;
                            }

                            if (fProp.Name == "Active")
                            {
                                allActive += fProp.Value.GetBoolean() == true ? 1 : 0;
                            }
                            else if (fProp.Name == "Age")
                            {
                                maxAge += fProp.Value.GetInt32();
                            }
                        }
                    }
                }

                if (property.Name == "Active")
                {
                    allActive += property.Value.GetBoolean() == true ? 1 : 0;
                }
                else if (property.Name == "Age")
                {
                    maxAge += property.Value.GetInt32();
                }
            }
        }

        Check(count == dataCount * 2);
        return totalProperties + maxAge + allActive;
    }

    public void Check(bool test)
    {
        if (!test)
        {
            throw new Exception("Failed check");
        }
    }

}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool Active { get; set; }
    public string Address { get; set; }
    public int Experience { get; set; }
    public bool Added { get; set; }
    public int Amount { get; set; }
    public int Foo { get; set; }
    public int Bar { get; set; }
    public bool SomeBool { get; set; }
    public int SomeInt { get; set; }
    public string SomeString { get; set; }
    public int AnotherInt { get; set; }
    public List<Person> Friends { get; set; } = new List<Person>();
}
