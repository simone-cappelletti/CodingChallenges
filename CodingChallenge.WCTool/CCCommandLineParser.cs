using CodingChallenge.WCTool.Verbs;
using CommandLine;
using static CodingChallenge.WCTool.Enums;

namespace CodingChallenge.WCTool
{
    /// <summary>
    /// Coding challenge command line parser
    /// </summary>
    public class CCCommandLineParser
    {
        private readonly string _input;
        private string[] _args;

        public CCCommandLineParser(string input)
        {
            _input = input;
        }

        /// <summary>
        /// Execute verb
        /// </summary>
        /// <returns>Exit code</returns>
        public int Execute()
        {
            var result = (int)ProgramReturnCode.KO;

            try
            {
                ValidateArgs();

                result = (int)Parser.Default
                     .ParseArguments<InfoVerb, CcwcVerb>(_args)
                     .MapResult
                     (
                         (InfoVerb infoVerb) => infoVerb.ExecuteVerb(),
                         (CcwcVerb ccwcVerb) => ccwcVerb.ExecuteVerb(),
                         (IEnumerable<Error> errors) => ProgramReturnCode.KO
                     );
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

            var args = _input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (args.Length <= 0)
            {
                throw new ArgumentException("Invalid input data");
            }

            _args = args;
        }
    }
}
