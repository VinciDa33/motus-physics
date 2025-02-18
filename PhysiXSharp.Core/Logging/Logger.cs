namespace PhysiXSharp.Core.Logging;

public class Logger(string logSource)
{
    private bool _doLogging = true;
    private ConsoleColor _messageLogColor = ConsoleColor.White;
    private ConsoleColor _warningLogColor = ConsoleColor.Yellow;
    private ConsoleColor _errorLogColor = ConsoleColor.Red;

    public void EnableLogging()
    {
        _doLogging = true;
    }

    public void DisableLogging()
    {
        _doLogging = false;
    }
    
    /// <summary>
    /// Writes a message to the console with this logger object as its source.
    /// </summary>
    /// <param name="message">Message to be written.</param>
    public void Log(string message)
    {
        if (!_doLogging)
            return;
        
        Console.ForegroundColor = _messageLogColor;
        Console.WriteLine($"{logSource}: {message}");
    }

    /// <summary>
    /// Writes a warning message to the console with this logger object as its source.
    /// </summary>
    /// <param name="message">Warning to be written.</param>
    public void LogWarning(string warningMessage)
    {
        if (!_doLogging)
            return;
        
        Console.ForegroundColor = _warningLogColor;
        Console.WriteLine($"{logSource}: {warningMessage}");
        Console.ForegroundColor = _messageLogColor;
    }

    /// <summary>
    /// Writes an error message to the console with this logger object as its source.
    /// </summary>
    /// <param name="message">Error to be written.</param>
    public void LogError(string errorMessage)
    {
        if (!_doLogging)
            return;
        
        Console.ForegroundColor = _errorLogColor;
        Console.WriteLine($"{logSource}: {errorMessage}");
        Console.ForegroundColor = _messageLogColor;
    }
}