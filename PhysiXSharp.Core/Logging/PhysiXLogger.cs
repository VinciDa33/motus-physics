namespace PhysiXSharp.Core.Logging;

public class PhysiXLogger(string logSource)
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
    
    public void Log(string message)
    {
        if (!_doLogging)
            return;
        
        Console.ForegroundColor = _messageLogColor;
        Console.WriteLine($"{logSource}: {message}");
    }

    public void LogWarning(string warningMessage)
    {
        if (!_doLogging)
            return;
        
        Console.ForegroundColor = _warningLogColor;
        Console.WriteLine($"{logSource}: {warningMessage}");
        Console.ForegroundColor = _messageLogColor;
    }

    public void LogError(string errorMessage)
    {
        if (!_doLogging)
            return;
        
        Console.ForegroundColor = _errorLogColor;
        Console.WriteLine($"{logSource}: {errorMessage}");
        Console.ForegroundColor = _messageLogColor;
    }
}