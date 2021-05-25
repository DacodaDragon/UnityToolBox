using Newtonsoft.Json;
using System;
using UnityEngine;

namespace ToolBox.StateMachine
{
    public class RectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Rect);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Rect rect = (Rect)value;
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(rect.x);
            writer.WritePropertyName("y");
            writer.WriteValue(rect.y);
            writer.WritePropertyName("w");
            writer.WriteValue(rect.width);
            writer.WritePropertyName("h");
            writer.WriteValue(rect.height);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            float x, y, w, h;
            x = y = w = h = 0;

            reader.Read();


            while (reader.TokenType == JsonToken.PropertyName)
            {
                switch ((string)reader.Value)
                {
                    case "y": y = (float)reader.ReadAsDecimal(); break;
                    case "x": x = (float)reader.ReadAsDecimal(); break;
                    case "w": w = (float)reader.ReadAsDecimal(); break;
                    case "h": h = (float)reader.ReadAsDecimal(); break;
                }
                reader.Read();
            }

            return new Rect(x, y, w, h);
        }
    }
}
