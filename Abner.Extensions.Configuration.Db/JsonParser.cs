using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Abner.Extensions.Configuration.Db
{
    internal sealed class JsonParser
    {
        private JsonParser() { }

        private readonly Dictionary<string, string> _data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public static Dictionary<string, string> Parse(string key, string json)
         => new JsonParser().ParseJson(key, json);

        private Dictionary<string, string> ParseJson(string key, string json)
        {
            var jsonDocumentOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json, jsonDocumentOptions))
                {
                    if (doc.RootElement.ValueKind != JsonValueKind.Object)
                    {

                    }
                }
            }
            catch (JsonException jsex)
            {
                _data[key] = json;
            }

            return _data;
        }
    }
}
