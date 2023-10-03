using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonWriterPerf;

internal class ExtensibleJsonArray
{
    private JsonElement? _jsonElement;
    private List<object>? _added;

    public ExtensibleJsonArray(JsonElement? jsonElement = default)
    {
        _jsonElement = jsonElement;
    }

    public void Add(object element)
    {
        if (_added == null)
        {
            _added = new List<object>();
        }

        _added.Add(element);
    }

    public void WriteTo(Utf8JsonWriter writer)
    {
        if (_added == null && _jsonElement.HasValue)
        {
            _jsonElement.Value.WriteTo(writer);
            return;
        }

        writer.WriteStartArray();

        if (_jsonElement.HasValue)
        {
            foreach (JsonElement element in _jsonElement.Value.EnumerateArray())
            {
                element.WriteTo(writer);
            }
        }
        
        if (_added != null)
        {
            foreach (var item in _added)
            {
                if (item is ExtensibleJsonObject obj)
                {
                    obj.WriteTo(writer);
                }
                else if (item is JsonElement json)
                {
                    json.WriteTo(writer);
                }
                else
                {
                    throw new NotImplementedException($"Unsupported {item}");
                }
            }
        }
        
        writer.WriteEndArray();
    }
}
