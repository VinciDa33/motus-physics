namespace PhysiXSharp.Core.Logging;

public class Logger(string logSource)
{
    private bool _doLogging = true;
    private bool _doWarnings = true;
    private bool _doErrors = true;
    public ConsoleColor MessageLogColor = ConsoleColor.White;
    public ConsoleColor WarningLogColor = ConsoleColor.Yellow;
    public ConsoleColor ErrorLogColor = ConsoleColor.Red;

    /// <summary>
    /// Enables logging.
    /// Enabled by default.
    /// </summary>
    public void EnableLogging()
    {
        _doLogging = true;
    }

    /// <summary>
    /// Disables logging of all messages.
    /// </summary>
    public void DisableLogging()
    {
        _doLogging = false;
    }
    
    /// <summary>
    /// Enables logging of warnings.
    /// Enabled by default.
    /// </summary>
    public void EnableWarnings()
    {
        _doWarnings = true;
    }
    
    /// <summary>
    /// Disables logging of warnings.
    /// </summary>
    public void DisableWarnings()
    {
        _doWarnings = false;
    }
    
    /// <summary>
    /// Enables logging of errors.
    /// Enabled by default.
    /// </summary>
    public void EnableErrors()
    {
        _doErrors = true;
    }
    
    /// <summary>
    /// Disables logging of errors.
    /// </summary>
    public void DisableErrors()
    {
        _doErrors = false;
    }
    
    /// <summary>
    /// Writes a message to the console with this logger object as its source.
    /// </summary>
    /// <param name="message">Message to be written.</param>
    public void Log(string message)
    {
        if (!_doLogging)
            return;
        
        Console.ForegroundColor = MessageLogColor;
        Console.WriteLine($"{logSource}: {message}");
    }

    /// <summary>
    /// Writes a warning message to the console with this logger object as its source.
    /// </summary>
    /// <param name="warningMessage">Warning to be written.</param>
    public void LogWarning(string warningMessage)
    {
        if (!_doLogging)
            return;
        
        if (!_doWarnings)
            return;
        
        Console.ForegroundColor = WarningLogColor;
        Console.WriteLine($"{logSource}: {warningMessage}");
        Console.ForegroundColor = MessageLogColor;
    }

    /// <summary>
    /// Writes an error message to the console with this logger object as its source.
    /// </summary>
    /// <param name="errorMessage">Error to be written.</param>
    public void LogError(string errorMessage)
    {
        if (!_doLogging)
            return;
        
        if (!_doErrors)
            return;
        
        Console.ForegroundColor = ErrorLogColor;
        Console.WriteLine($"{logSource}: {errorMessage}");
        Console.ForegroundColor = MessageLogColor;
    }
}