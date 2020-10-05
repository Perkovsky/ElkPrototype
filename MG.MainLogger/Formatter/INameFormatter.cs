namespace MG.MainLogger.Formatter
{
	public interface INameFormatter
	{
        /// <summary>
        /// Clean up a name so that it matches the formatting.
        /// </summary>
        /// <remarks>
        /// Examples:
        ///     MgLoggerIndex -> mg-logger-index (kebab case)
        ///     MgLoggerIndex -> mg_logger_index (snake case)
        /// </remarks>
        /// <param name="name"></param>
        /// <returns>string</returns>
        string SanitizeName(string name);
    }
}
