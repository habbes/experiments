using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonWriterPerf;

internal class ExtensibleJsonObject
{
    private JsonElement? _jsonElement;
    private Dictionary<string, object>? _added = null;

    public ExtensibleJsonObject()
    {

    }

    public ExtensibleJsonObject(JsonElement jsonElement)
    {
        _jsonElement = jsonElement;
    }

    public void Add(string name, object value)
    {
        if (_added == null)
        {
            _added = new Dictionary<string, object>();
        }

        _added[name] = value;
    }

    public void WriteTo(Utf8JsonWriter writer)
    {
        if (_added == null && _jsonElement.HasValue)
        {
            _jsonElement.Value.WriteTo(writer);
            return;
        }

        writer.WriteStartObject();

        if (_jsonElement.HasValue)
        {
            foreach (var item in _jsonElement.Value.EnumerateObject())
            {
                item.WriteTo(writer);
            }
        }

        if (_added != null)
        {
            foreach (var item in _added)
            {
                writer.WritePropertyName(item.Key);
                var value = item.Value;

                if (value is string stringValue)
                {
                    writer.WriteStringValue(stringValue);
                }
                else if (value is bool boolValue)
                {
                    writer.WriteBooleanValue(boolValue);
                }
                else if (value is int intValue)
                {
                    writer.WriteNumberValue(intValue);
                }
                else if (value is ExtensibleJsonArray array)
                {
                    array.WriteTo(writer);
                }
                else if (value is ExtensibleJsonObject obj)
                {
                    obj.WriteTo(writer);
                }
                else if (value is JsonElement jsonElement)
                {
                    jsonElement.WriteTo(writer);
                }
                else
                {
                    throw new NotImplementedException($"Unsupported type {item}");
                }
            }
        }

        writer.WriteEndObject();
    }
}
