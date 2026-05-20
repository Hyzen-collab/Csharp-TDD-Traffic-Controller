//Interface for timer management
public interface ITimeManager
{
    //Returns a status of timer channels
    //Example: "Timer,OK,FAULT,OK,"
    string GetStatus();

    //A delay for the specified milliseconds
    //CD: Returns bool indicating success
    bool Delay(int milliseconds);
} 