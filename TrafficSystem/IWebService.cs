//Interface for web service logging
public interface IWebService
{
    //Logs when an engineer is required
    //CD: Accepts string parameter, returns void
    void LogEngineerRequired(string message);

    //Logs when a fault is detected
    //CD: Accepts bool parameter, returns void
    void FaultDetected(bool fault);
} 