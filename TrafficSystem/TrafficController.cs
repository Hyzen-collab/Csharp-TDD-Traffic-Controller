//G21328023 - K.A. Idusha Piumika

using System;

//TrafficController manages vehicle and pedestrian signals at a single intersection.
//Implements Level 1 and Level 2 requirements using a TDD approach.
public class TrafficController
{
    //Stores the intersection identifier, always in lowercase
    private string intersectionID;

    //Tracks the current state of the vehicle traffic signal
    private string currentVehicleSignalState;

    //Tracks the current state of the pedestrian signal
    private string currentPedestrianSignalState;

    //Injected dependencies — used by Level 2 methods and the DI constructor
    private IVehicleSignalManager vehicleManager;
    private IPedestrianSignalManager pedestrianManager;
    private ITimeManager timeManager;
    private IWebService webService;
    private IEmailService emailService;

    #region Constructors

    //L1R1 & L1R2 & L1R4:
    //Single-parameter constructor. Accepts the intersection ID (any case)
    //converts it to lowercase, and sets default signal states.
    public TrafficController(string id)
    {
        intersectionID = (id ?? "").ToLower();
        currentVehicleSignalState = "amber";
        currentPedestrianSignalState = "wait";
    }

    //L2R2:
    //Three-parameter constructor that sets the intersection ID and both
    //starting signal states. Only valid operational states are accepted —
    //any invalid combination throws an ArgumentException with the exact
    //required message. All values are stored in lowercase.
    public TrafficController(string id, string vehicleStartState, string pedestrianStartState)
    {
        intersectionID = (id ?? "").ToLower();

        string v = (vehicleStartState ?? "").ToLower();
        string p = (pedestrianStartState ?? "").ToLower();

        //oosv/oosp are NOT valid starting states for this constructor
        string[] validVehicle = { "red", "redamber", "green", "amber" };
        string[] validPedestrian = { "wait", "walk" };

        if (!Array.Exists(validVehicle, s => s == v) ||
            !Array.Exists(validPedestrian, s => s == p))
        {
            throw new ArgumentException(
                "Argument Exception: TrafficController can only be initialised to the following states: " +
                "'green', 'amber', 'red', 'redamber' for the vehicle signals and 'wait' or 'walk' for the pedestrian signal");
        }

        currentVehicleSignalState = v;
        currentPedestrianSignalState = p;
    }

    //L2R3:
    //Six-parameter dependency injection constructor for unit testing.
    //Accepts interfaces for all five dependencies alongside the intersection ID.
    //Defaults signal states to amber/wait as per L1R4.
    public TrafficController(
        string id,
        IVehicleSignalManager iVehicleSignalManager,
        IPedestrianSignalManager iPedestrianSignalManager,
        ITimeManager iTimeManager,
        IWebService iWebService,
        IEmailService iEmailService)
    {
        intersectionID = (id ?? "").ToLower();
        currentVehicleSignalState = "amber";
        currentPedestrianSignalState = "wait";

        vehicleManager = iVehicleSignalManager;
        pedestrianManager = iPedestrianSignalManager;
        timeManager = iTimeManager;
        webService = iWebService;
        emailService = iEmailService;
    }

    #endregion

    #region Getters & Setters

    //Returns the stored intersection ID (always lowercase)
    public string GetIntersectionID() => intersectionID;

    //L1R3: Updates the intersection ID, converting any uppercase to lowercase
    public void SetIntersectionID(string id)
    {
        intersectionID = (id ?? "").ToLower();
    }

    //Returns the current vehicle signal state
    public string GetCurrentVehicleSignalState() => currentVehicleSignalState;

    //Returns the current pedestrian signal state
    public string GetCurrentPedestrianSignalState() => currentPedestrianSignalState;

    #endregion

    #region Level 1 Methods

    //L1R5:
    //Directly sets both signal states without transition validation.
    //Valid vehicle states:    "red", "redamber", "green", "amber", "oosv"
    //Valid pedestrian states: "wait", "walk", "oosp"
    //Comparison is case-insensitive; values are stored in lowercase.
    //Returns true if both states are valid and were applied; false otherwise.
    public bool SetStateDirect(string vehicleSignalState, string pedestrianSignalState)
    {
        string v = (vehicleSignalState ?? "").ToLower();
        string p = (pedestrianSignalState ?? "").ToLower();

        string[] validVehicle = { "red", "redamber", "green", "amber", "oosv" };
        string[] validPedestrian = { "wait", "walk", "oosp" };

        if (Array.Exists(validVehicle, s => s == v) &&
            Array.Exists(validPedestrian, s => s == p))
        {
            currentVehicleSignalState = v;
            currentPedestrianSignalState = p;
            return true;
        }

        //Invalid input — state is left unchanged
        return false;
    }

    #endregion

    #region Level 2 Methods

    //L2R1:
    //Attempts a state transition following the standard UK traffic light sequence:
    //  green    -> amber    (pedestrian: wait)
    //  amber    -> red      (pedestrian: walk — vehicles stopped, pedestrians may cross)
    //  red      -> redamber (pedestrian: wait — vehicles about to move, pedestrians must stop)
    //  redamber -> green    (pedestrian: wait)
    //Any other transition is invalid. Returns true on success, false if invalid
    //State is never changed on an invalid transition
    public bool SetCurrentState(string vehicleSignal, string pedestrianSignal)
    {
        string v = (vehicleSignal ?? "").ToLower();

        //green -> amber: vehicles slowing, pedestrians still waiting
        if (currentVehicleSignalState == "green" && v == "amber")
        {
            currentVehicleSignalState = "amber";
            currentPedestrianSignalState = "wait";
            return true;
        }

        //amber -> red: vehicles stopped, pedestrians may now cross
        if (currentVehicleSignalState == "amber" && v == "red")
        {
            currentVehicleSignalState = "red";
            currentPedestrianSignalState = "walk";
            return true;
        }

        //red -> redamber: vehicles about to move, pedestrians must stop
        if (currentVehicleSignalState == "red" && v == "redamber")
        {
            currentVehicleSignalState = "redamber";
            currentPedestrianSignalState = "wait";
            return true;
        }

        //redamber -> green: vehicles moving, pedestrians waiting
        if (currentVehicleSignalState == "redamber" && v == "green")
        {
            currentVehicleSignalState = "green";
            currentPedestrianSignalState = "wait";
            return true;
        }

        //Transition is not permitted — state unchanged
        return false;
    }

    //L2R4:
    //Retrieves status strings from all three manager dependencies and
    //concatenates them in the required order: vehicle, pedestrian, timer
    public string GetStatusReport()
    {
        string vehicleStatus = vehicleManager?.GetStatus() ?? "";
        string pedestrianStatus = pedestrianManager?.GetStatus() ?? "";
        string timerStatus = timeManager?.GetStatus() ?? "";

        return vehicleStatus + pedestrianStatus + timerStatus;
    }

    #endregion
} 