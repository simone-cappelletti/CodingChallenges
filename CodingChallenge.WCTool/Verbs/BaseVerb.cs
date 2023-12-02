using CommandLine;
using static CodingChallenge.WCTool.Enums;

namespace CodingChallenge.WCTool.Verbs
{
    /// <summary>
    /// Base verb abstract class
    /// </summary>
    internal abstract class BaseVerb : IVerb
    {
        /// <summary>
        /// Verbose mode
        /// </summary>
        [Option('v', "verbose", Required = false, HelpText = "Enable detailed information about execution and errors", Default = false)]
        protected bool Verbose { get; set; }

        /// <summary>
        /// Base file directory
        /// </summary>
        protected static string FileDir { get; } = AppDomain.CurrentDomain.BaseDirectory;

        /// <inheritdoc/>
        public abstract ProgramReturnCode ExecuteVerb();
    }
}
