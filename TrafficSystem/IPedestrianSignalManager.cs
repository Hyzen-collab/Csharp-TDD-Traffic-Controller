//Interface for managing pedestrian signals
public interface IPedestrianSignalManager
{
    //Returns a status of all pedestrian signals
    //Example: "PedestrianSignal,OK,OK,OK,"
    string GetStatus();
} 