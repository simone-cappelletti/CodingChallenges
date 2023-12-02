using static CodingChallenge.WCTool.Enums;

namespace CodingChallenge.WCTool.Verbs
{
    /// <summary>
    /// Base verb interface
    /// </summary>
    internal interface IVerb
    {
        /// <summary>
        /// Execute the verb
        /// </summary>
        /// <returns><see cref="ProgramReturnCode"/> casted to int</returns>
        ProgramReturnCode ExecuteVerb();
    }
}
