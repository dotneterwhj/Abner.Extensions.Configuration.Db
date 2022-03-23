using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Abner.Extensions.Configuration.Db
{
    internal sealed class JsonParser
    {
        private JsonParser() { }

        private readonly Dictionary<string, string> _data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _paths = new Stack<string>();


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
                    if (doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        EnterContext(key);
                        VisitElement(doc.RootElement);
                        ExitContext();
                    }
                    else if (doc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        EnterContext(key);
                        VisitValue(doc.RootElement);
                        ExitContext();
                    }
                    else
                    {
                        throw new FormatException(string.Format("json：{0}序列化失败", json));
                    }
                }
            }
            catch (JsonException jsex)
            {
                Debug.WriteLine("json：{0}序列化失败，已直接存储为字符串，异常信息：{1}", new object[] { json, jsex });
                _data[key] = json;
            }

            return _data;
        }

        private void VisitValue(JsonElement value)
        {
            switch (value.ValueKind)
            {
                case JsonValueKind.Object:
                    VisitElement(value);
                    break;
                case JsonValueKind.Array:
                    var isEmpty = true;
                    int index = 0;
                    foreach (var item in value.EnumerateArray())
                    {
                        isEmpty = false;
                        EnterContext(index.ToString());
                        VisitValue(item);
                        ExitContext();
                        index++;
                    }
                    if (isEmpty && _paths.Count > 0)
                    {
                        _data[GetContext()] = null;
                    }
                    break;
                case JsonValueKind.String:
                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                case JsonValueKind.Null:
                    var key = GetContext();
                    if (_data.ContainsKey(key))
                    {
                        throw new FormatException(string.Format("json中存在相同的key:{0}", key));
                    }
                    _data[key] = value.ToString();
                    break;
                default:
                    throw new FormatException(string.Format("unknow JsonValueKind:{0}", value.ValueKind));
            }
        }

        private void VisitElement(JsonElement element)
        {
            var isEmpty = true;

            foreach (var property in element.EnumerateObject())
            {
                isEmpty = false;
                EnterContext(property.Name);
                VisitValue(property.Value);
                ExitContext();
            }

            if (isEmpty && _paths.Count > 0)
            {
                _data[GetContext()] = null;
            }
        }

        private void EnterContext(string context)
        {
            var key = context;

            // 表示栈不为空
            if (_paths.Count > 0)
            {
                // 取栈顶元素但不出栈
                key = _paths.Peek() + ConfigurationPath.KeyDelimiter + context;
            }

            _paths.Push(key);
        }

        private void ExitContext() => _paths.Pop();

        private string GetContext() => _paths.Peek();
    }
}
