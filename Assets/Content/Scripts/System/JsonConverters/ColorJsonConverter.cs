using System;
using Newtonsoft.Json;
using UnityEngine;

public class ColorJsonConverter : JsonConverter<Color>
{
    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("r"); writer.WriteValue(value.r);
        writer.WritePropertyName("g"); writer.WriteValue(value.g);
        writer.WritePropertyName("b"); writer.WriteValue(value.b);
        writer.WritePropertyName("a"); writer.WriteValue(value.a);
        writer.WriteEndObject();
    }

    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        float r = 0, g = 0, b = 0, a = 1; 
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject) break;
            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propName = reader.Value.ToString();
                reader.Read();
                float value = Convert.ToSingle(reader.Value);
                switch (propName)
                {
                    case "r": r = value; break;
                    case "g": g = value; break;
                    case "b": b = value; break;
                    case "a": a = value; break;
                }
            }
        }
        return new Color(r, g, b, a);
    }
}