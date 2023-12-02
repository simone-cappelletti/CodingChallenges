using CommandLine;
using CommandLine.Text;
using static CodingChallenge.WCTool.Enums;

namespace CodingChallenge.WCTool.Verbs
{
    [Verb("info", HelpText = "Prints application info")]
    internal class InfoVerb : BaseVerb
    {
        public override ProgramReturnCode ExecuteVerb()
        {
            Console.WriteLine($"Welcome to {new HeadingInfo(AppDomain.CurrentDomain.FriendlyName, "1.0.0")}");

            return ProgramReturnCode.OK;
        }
    }
}
