namespace DPlay.Generator
{
    /// <summary>
    ///     This is any exception thrown during generation.
    /// </summary>
    public class GeneratorException : System.Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GeneratorException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public GeneratorException(string message) : base(message) { }
    }
}
