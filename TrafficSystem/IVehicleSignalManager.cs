//Interface for managing vehicle signals
public interface IVehicleSignalManager
{
    //Returns a status of all vehicle signals
    // Example: "VehicleSignal,OK,OK,FAULT,"
    string GetStatus();

    //Logs if an engineer is required
    //CD: Accepts string message and returns bool
    bool LogEngineerRequired(string message);
} 

