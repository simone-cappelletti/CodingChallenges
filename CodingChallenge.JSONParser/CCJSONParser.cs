using System.Text;
using System.Text.Json;

namespace CodingChallenge.JSONParser
{
    public class CCJSONParser
    {
        private readonly string _input;
        private readonly StringBuilder _sb = new StringBuilder();

        public CCJSONParser(string input)
        {
            _input = input;
        }

        public string Execute()
        {
            var result = string.Empty;

            try
            {
                ValidateArgs();
                ParseJson(_input);

                result = _sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        private void ValidateArgs()
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(_input);
        }

        private void ParseJson(string json, int depth = 0)
        {
            using var doc = JsonDocument.Parse(json);
            var rootElement = doc.RootElement;

            switch (rootElement.ValueKind)
            {
                case JsonValueKind.Object:

                    _sb.AppendLine($"{{");

                    var objectsDepth = depth + 1;
                    var objectsElements = rootElement.EnumerateObject();
                    var lastObjectElement = objectsElements.Last();
                    foreach (var element in objectsElements)
                    {
                        _sb.Append($"{AddTab(objectsDepth)}\"{element.Name}\":");

                        ParseJson(element.Value.GetRawText(), objectsDepth);

                        if (!element.Equals(lastObjectElement))
                            _sb.AppendLine(",");
                        else
                            _sb.AppendLine("");
                    }

                    _sb.Append($"{AddTab(depth)}}}");

                    break;

                case JsonValueKind.Array:

                    _sb.Append("[");

                    var arrayDepth = depth + 1;
                    var arrayElements = rootElement.EnumerateArray();
                    var lastArrayElement = arrayElements.Last();
                    foreach (var element in arrayElements)
                    {
                        _sb.AppendLine();
                        _sb.Append($"{AddTab(arrayDepth)}");

                        ParseJson(element.GetRawText(), arrayDepth);

                        if (!element.Equals(lastArrayElement))
                            _sb.Append(",");
                        else
                            _sb.AppendLine("");
                    }

                    _sb.Append($"{AddTab(depth)}]");

                    break;

                case JsonValueKind.String:

                    _sb.Append($"\"{rootElement.GetString() ?? "null"}\"");

                    break;

                case JsonValueKind.Number:

                    // Int32 to simplify
                    _sb.Append($"\"{rootElement.GetInt32()}\"");

                    break;

                case JsonValueKind.True:

                    _sb.Append("true");

                    break;

                case JsonValueKind.False:

                    _sb.Append("false");

                    break;

                case JsonValueKind.Null:

                    _sb.Append("null");

                    break;

                case JsonValueKind.Undefined:

                    throw new NotImplementedException();

                    break;
            }
        }

        private string AddTab(int depth)
        {
            return string.Empty.PadLeft(depth, '\t');
        }
    }
}
