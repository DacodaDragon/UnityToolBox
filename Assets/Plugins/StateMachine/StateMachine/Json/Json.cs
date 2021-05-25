using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ToolBox.StateMachine
{
    public static class Json
    {
        private static readonly JsonSerializer serializer;

        static Json()
        {
            serializer = new JsonSerializer()
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            serializer.Converters.Add(new Vector2Converter());
            serializer.Converters.Add(new RectConverter());
        }

        public static T To<T>(string value)
        {
            return (T)To(value, typeof(T));
        }

        public static object To(string value, Type type)
        {
            using (JsonTextReader reader = new JsonTextReader(new StringReader(value)))
            {
                return serializer.Deserialize(reader, type);
            }
        }

        public static string From(object value)
        {
            return From(value, null);
        }

        private static string From(object value, Type type)
        {
            StringBuilder sb = new StringBuilder(256);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                serializer.Serialize(jsonWriter, value);
            }

            return sw.ToString();
        }
    }
}
