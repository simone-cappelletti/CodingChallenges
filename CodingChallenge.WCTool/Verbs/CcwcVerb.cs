using CommandLine;
using static CodingChallenge.WCTool.Enums;

namespace CodingChallenge.WCTool.Verbs
{
    /// <summary>
    /// CCWC verb
    /// </summary>
    [Verb("ccwc", HelpText = "Ccwc verb for coding challenge")]
    internal class CcwcVerb : BaseVerb
    {
        /// <summary>
        /// Default options
        /// </summary>
        public string[] _defaultOptions = ["c", "l", "m"];

        /// <summary>
        /// Bytes counter
        /// </summary>
        [Option('c', "count", Required = false, HelpText = "Bytes count")]
        public string Count { get; set; }

        /// <summary>
        /// Number of lines
        /// </summary>
        [Option('l', "lines", Required = false, HelpText = "Number of lines")]
        public string Lines { get; set; }

        /// <summary>
        /// Number of words
        /// </summary>
        [Option('w', "words", Required = false, HelpText = "Number of words")]
        public string Words { get; set; }

        /// <summary>
        /// Number of characters
        /// </summary>
        [Option('m', "characters ", Required = false, HelpText = "Number of characters ")]
        public string Characters { get; set; }

        /// <inheritdoc/>
        public override ProgramReturnCode ExecuteVerb()
        {
            var result = ProgramReturnCode.KO;

            if (!string.IsNullOrWhiteSpace(Count))
            {
                var count = File.ReadAllBytes(Path.Combine(FileDir, Count));
                Console.WriteLine($"{count.Length} {Count}");
                result = ProgramReturnCode.OK;
            }

            else if (!string.IsNullOrWhiteSpace(Lines))
            {
                var lines = File.ReadLines(Path.Combine(FileDir, Lines));
                Console.WriteLine($"{lines.Count()} {Lines}");
                result = ProgramReturnCode.OK;
            }

            else if (!string.IsNullOrWhiteSpace(Words))
            {
                var words = File.ReadAllText(Path.Combine(FileDir, Words)).Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine($"{words.Count()} {Words}");
                result = ProgramReturnCode.OK;
            }

            else if (!string.IsNullOrWhiteSpace(Characters))
            {
                var characters = File.ReadAllText(Path.Combine(FileDir, Characters)).ToCharArray();
                Console.WriteLine($"{characters.Length} {Characters}");
                result = ProgramReturnCode.OK;
            }

            else
            {
                result = ProgramReturnCode.INVALID_INPUT;
            }

            return result;
        }
    }
}
