//G21328023 - K.A. Idusha Piumika

using NUnit.Framework;
using NSubstitute;

//Unit tests for TrafficController class
//Covers Level 1 (L1R1-L1R5) and Level 2 (L2R1-L2R4) requirements
[TestFixture]
public class TrafficControllerTests
{
    //─── LEVEL 1 TESTS ────────────────────────────────────────────────────────

    //L1R1 & L1R2 - Constructor should store ID in lowercase
    [Test]
    public void Constructor_ShouldConvertIDToLowercase()
    {
        var c = new TrafficController("ABC123");
        Assert.That(c.GetIntersectionID(), Is.EqualTo("abc123"));
    }

    //L1R2 - Mixed case ID should be converted to lowercase
    [Test]
    public void Constructor_MixedCaseID_ShouldBeLowercase()
    {
        var c = new TrafficController("AbC");
        Assert.That(c.GetIntersectionID(), Is.EqualTo("abc"));
    }

    //L1R2 - Lowercase ID should remain unchanged
    [Test]
    public void Constructor_LowercaseID_ShouldRemainSame()
    {
        var c = new TrafficController("abc");
        Assert.That(c.GetIntersectionID(), Is.EqualTo("abc"));
    }

    //L1R4 - Constructor should set vehicle to amber and pedestrian to wait
    [Test]
    public void Constructor_ShouldSetInitialStates()
    {
        var c = new TrafficController("id");
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo("amber"));
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("wait"));
    }

    //L1R4 - Default vehicle signal should be amber
    [Test]
    public void Constructor_DefaultVehicle_ShouldBeAmber()
    {
        var c = new TrafficController("id");
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo("amber"));
    }

    //L1R4 - Default pedestrian signal should be wait
    [Test]
    public void Constructor_DefaultPedestrian_ShouldBeWait()
    {
        var c = new TrafficController("id");
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("wait"));
    }

    //L1R3 - SetIntersectionID should convert uppercase to lowercase
    [Test]
    public void SetIntersectionID_ShouldConvertToLowercase()
    {
        var c = new TrafficController("id");
        c.SetIntersectionID("NEWID");
        Assert.That(c.GetIntersectionID(), Is.EqualTo("newid"));
    }

    //L1R3 - SetIntersectionID mixed case should be stored as lowercase
    [Test]
    public void SetIntersectionID_MixedCase_ShouldBeLowercase()
    {
        var c = new TrafficController("id");
        c.SetIntersectionID("NeWiD");
        Assert.That(c.GetIntersectionID(), Is.EqualTo("newid"));
    }

    //L1R5 - Valid combinations should return true
    [TestCase("red", "wait")]
    [TestCase("green", "walk")]
    [TestCase("AMBER", "WAIT")]
    [TestCase("redamber", "wait")]
    [TestCase("oosv", "oosp")]
    public void SetStateDirect_ValidInputs_ReturnsTrue(string v, string p)
    {
        var c = new TrafficController("id");
        bool result = c.SetStateDirect(v, p);
        Assert.That(result, Is.True);
    }

    //L1R5 - Invalid combinations should return false
    [TestCase("blue", "wait")]
    [TestCase("red", "run")]
    [TestCase("invalid", "invalid")]
    [TestCase("yellow", "walk")]
    [TestCase("green", "jog")]
    public void SetStateDirect_InvalidInputs_ReturnsFalse(string v, string p)
    {
        var c = new TrafficController("id");
        bool result = c.SetStateDirect(v, p);
        Assert.That(result, Is.False);
    }

    //L1R5 - Invalid input must not change current state
    [Test]
    public void SetStateDirect_Invalid_DoesNotChangeState()
    {
        var c = new TrafficController("id");
        c.SetStateDirect("blue", "run");
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo("amber"));
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("wait"));
    }

    //L1R5 - Uppercase valid input should be stored as lowercase
    [Test]
    public void SetStateDirect_ValidUppercase_ShouldStoreLowercase()
    {
        var c = new TrafficController("id");
        c.SetStateDirect("GREEN", "WALK");
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo("green"));
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("walk"));
    }

    //L1R5 - Out-of-service states should be stored correctly
    [Test]
    public void SetStateDirect_OosStates_ShouldBeStored()
    {
        var c = new TrafficController("id");
        c.SetStateDirect("oosv", "oosp");
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo("oosv"));
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("oosp"));
    }

    //─── LEVEL 2 TESTS ───────────────────────────────────────────────────────

    //L2R2 - Three-param constructor with valid states should initialise correctly
    [Test]
    public void Constructor_ValidStates_ShouldWork()
    {
        var c = new TrafficController("id", "green", "wait");
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo("green"));
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("wait"));
    }

    //L2R2 - Valid start states (including mixed case) should all work correctly
    [TestCase("red", "wait")]
    [TestCase("GREEN", "WALK")]
    [TestCase("amber", "wait")]
    [TestCase("redamber", "wait")]
    public void Constructor_ValidStartStates_ShouldSetCorrectly(string v, string p)
    {
        var c = new TrafficController("id", v, p);
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo(v.ToLower()));
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo(p.ToLower()));
    }

    //L2R2 - Invalid state combination should throw ArgumentException
    [Test]
    public void Constructor_InvalidStates_ShouldThrowException()
    {
        Assert.Throws<System.ArgumentException>(() =>
        {
            new TrafficController("id", "blue", "run");
        });
    }

    //L2R2 - Invalid vehicle state alone should throw ArgumentException
    [Test]
    public void Constructor_InvalidVehicle_ShouldThrow()
    {
        Assert.Throws<System.ArgumentException>(() =>
        {
            new TrafficController("id", "yellow", "wait");
        });
    }

    //L2R2 - Invalid pedestrian state alone should throw ArgumentException
    [Test]
    public void Constructor_InvalidPedestrian_ShouldThrow()
    {
        Assert.Throws<System.ArgumentException>(() =>
        {
            new TrafficController("id", "red", "jog");
        });
    }

    //L2R2 - oosv is not a valid initialisation state and should throw
    [Test]
    public void Constructor_Oosv_ShouldThrow()
    {
        Assert.Throws<System.ArgumentException>(() =>
        {
            new TrafficController("id", "oosv", "wait");
        });
    }

    //L2R2 - Exception message must match the requirement exactly
    [Test]
    public void Constructor_InvalidStates_ShouldThrowCorrectMessage()
    {
        var ex = Assert.Throws<System.ArgumentException>(() =>
            new TrafficController("id", "blue", "wait"));
        Assert.That(ex.Message, Is.EqualTo(
            "Argument Exception: TrafficController can only be initialised to the following states: " +
            "'green', 'amber', 'red', 'redamber' for the vehicle signals and 'wait' or 'walk' for the pedestrian signal"));
    }

    //L2R2 - Intersection ID should still be lowercased in the three-param constructor
    [Test]
    public void Constructor_WithStates_IDShouldBeLowercase()
    {
        var c = new TrafficController("ABC", "red", "wait");
        Assert.That(c.GetIntersectionID(), Is.EqualTo("abc"));
    }

    //L2R1 - Valid vehicle state transitions should return true
    [TestCase("green", "amber", true)]
    [TestCase("amber", "red", true)]
    [TestCase("red", "redamber", true)]
    [TestCase("redamber", "green", true)]
    public void SetCurrentState_ValidTransitions_ReturnTrue(string start, string next, bool expected)
    {
        var c = new TrafficController("id", start, "wait");
        bool result = c.SetCurrentState(next, "wait");
        Assert.That(result, Is.EqualTo(expected));
    }

    //L2R1 - Invalid vehicle state transitions should return false
    [TestCase("green", "red")]
    [TestCase("amber", "green")]
    [TestCase("red", "green")]
    [TestCase("red", "amber")]
    [TestCase("green", "redamber")]
    public void SetCurrentState_InvalidTransitions_ReturnFalse(string start, string next)
    {
        var c = new TrafficController("id", start, "wait");
        bool result = c.SetCurrentState(next, "wait");
        Assert.That(result, Is.False);
    }

    //L2R1 - Invalid transition must not change vehicle state
    [Test]
    public void SetCurrentState_Invalid_DoesNotChangeVehicleState()
    {
        var c = new TrafficController("id", "green", "wait");
        c.SetCurrentState("red", "wait");
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo("green"));
    }

    //L2R1 - Invalid transition must not change pedestrian state
    [Test]
    public void SetCurrentState_Invalid_DoesNotChangePedState()
    {
        var c = new TrafficController("id", "green", "wait");
        c.SetCurrentState("red", "wait");
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("wait"));
    }

    //L2R1 - Valid transition should update vehicle state correctly
    [Test]
    public void SetCurrentState_Valid_ShouldUpdateVehicleState()
    {
        var c = new TrafficController("id", "red", "wait");
        c.SetCurrentState("redamber", "wait");
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo("redamber"));
    }

    //L2R1 - Same state is not a valid transition and should return false
    [Test]
    public void SetCurrentState_SameState_ShouldReturnFalse()
    {
        var c = new TrafficController("id", "red", "wait");
        bool result = c.SetCurrentState("red", "wait");
        Assert.That(result, Is.False);
    }

    //L2R1 - amber -> red transition should set pedestrian to walk
    //(vehicles stopped = pedestrians allowed to cross)
    [Test]
    public void SetCurrentState_AmberToRed_PedestrianShouldBeWalk()
    {
        var c = new TrafficController("id", "amber", "wait");
        c.SetCurrentState("red", "walk");
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("walk"));
    }

    //L2R1 - red -> redamber transition should set pedestrian back to wait
    //(vehicles about to move = pedestrians must stop)
    [Test]
    public void SetCurrentState_RedToRedAmber_PedestrianShouldBeWait()
    {
        var c = new TrafficController("id", "amber", "wait");
        c.SetCurrentState("red", "walk");      //move to red/walk first
        c.SetCurrentState("redamber", "wait"); //then transition to redamber
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("wait"));
    }

    //L2R1 - A full cycle should complete and end at green/wait
    [Test]
    public void SetCurrentState_FullCycle_ShouldEndAtGreenWait()
    {
        var c = new TrafficController("id", "green", "wait");
        c.SetCurrentState("amber", "wait");
        c.SetCurrentState("red", "walk");
        c.SetCurrentState("redamber", "wait");
        c.SetCurrentState("green", "wait");
        Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo("green"));
        Assert.That(c.GetCurrentPedestrianSignalState(), Is.EqualTo("wait"));
    }

    //L2R4 - GetStatusReport should concatenate all three manager statuses in order
    [Test]
    public void GetStatusReport_ShouldCombineAllStatuses()
    {
        var v = Substitute.For<IVehicleSignalManager>();
        var p = Substitute.For<IPedestrianSignalManager>();
        var t = Substitute.For<ITimeManager>();

        v.GetStatus().Returns("VehicleSignal,OK,");
        p.GetStatus().Returns("PedestrianSignal,OK,");
        t.GetStatus().Returns("Timer,OK,");

        var c = new TrafficController("id", v, p, t, null, null);
        Assert.That(c.GetStatusReport(), Is.EqualTo("VehicleSignal,OK,PedestrianSignal,OK,Timer,OK,"));
    }

    //L2R4 - GetStatusReport result should contain vehicle status
    [Test]
    public void GetStatusReport_ShouldContainVehicleStatus()
    {
        var v = Substitute.For<IVehicleSignalManager>();
        var p = Substitute.For<IPedestrianSignalManager>();
        var t = Substitute.For<ITimeManager>();

        v.GetStatus().Returns("VehicleSignal,OK,");
        p.GetStatus().Returns("PedestrianSignal,OK,");
        t.GetStatus().Returns("Timer,OK,");

        var c = new TrafficController("id", v, p, t, null, null);
        Assert.That(c.GetStatusReport(), Does.Contain("VehicleSignal"));
    }

    //L2R4 - GetStatusReport result should contain timer status
    [Test]
    public void GetStatusReport_ShouldContainTimerStatus()
    {
        var v = Substitute.For<IVehicleSignalManager>();
        var p = Substitute.For<IPedestrianSignalManager>();
        var t = Substitute.For<ITimeManager>();

        v.GetStatus().Returns("VehicleSignal,OK,");
        p.GetStatus().Returns("PedestrianSignal,OK,");
        t.GetStatus().Returns("Timer,OK,");

        var c = new TrafficController("id", v, p, t, null, null);
        Assert.That(c.GetStatusReport(), Does.Contain("Timer"));
    }

    //L2R4 - GetStatusReport should correctly include FAULT entries from managers
    [Test]
    public void GetStatusReport_WithFault_ShouldCombine()
    {
        var v = Substitute.For<IVehicleSignalManager>();
        var p = Substitute.For<IPedestrianSignalManager>();
        var t = Substitute.For<ITimeManager>();

        v.GetStatus().Returns("VehicleSignal,OK,FAULT,");
        p.GetStatus().Returns("PedestrianSignal,OK,");
        t.GetStatus().Returns("Timer,OK,");

        var c = new TrafficController("id", v, p, t, null, null);
        Assert.That(c.GetStatusReport(), Is.EqualTo("VehicleSignal,OK,FAULT,PedestrianSignal,OK,Timer,OK,"));
    }

    //L2R4 - GetStatus on vehicle manager should be called exactly once
    [Test]
    public void GetStatusReport_VehicleGetStatus_CalledOnce()
    {
        var v = Substitute.For<IVehicleSignalManager>();
        var p = Substitute.For<IPedestrianSignalManager>();
        var t = Substitute.For<ITimeManager>();

        v.GetStatus().Returns("VehicleSignal,OK,");
        p.GetStatus().Returns("PedestrianSignal,OK,");
        t.GetStatus().Returns("Timer,OK,");

        var c = new TrafficController("id", v, p, t, null, null);
        c.GetStatusReport();
        v.Received(1).GetStatus();
    }

    //L2R4 - GetStatus on pedestrian manager should be called exactly once
    [Test]
    public void GetStatusReport_PedGetStatus_CalledOnce()
    {
        var v = Substitute.For<IVehicleSignalManager>();
        var p = Substitute.For<IPedestrianSignalManager>();
        var t = Substitute.For<ITimeManager>();

        v.GetStatus().Returns("VehicleSignal,OK,");
        p.GetStatus().Returns("PedestrianSignal,OK,");
        t.GetStatus().Returns("Timer,OK,");

        var c = new TrafficController("id", v, p, t, null, null);
        c.GetStatusReport();
        p.Received(1).GetStatus();
    }

    //L2R4 - GetStatus on timer manager should be called exactly once
    [Test]
    public void GetStatusReport_TimerGetStatus_CalledOnce()
    {
        var v = Substitute.For<IVehicleSignalManager>();
        var p = Substitute.For<IPedestrianSignalManager>();
        var t = Substitute.For<ITimeManager>();

        v.GetStatus().Returns("VehicleSignal,OK,");
        p.GetStatus().Returns("PedestrianSignal,OK,");
        t.GetStatus().Returns("Timer,OK,");

        var c = new TrafficController("id", v, p, t, null, null);
        c.GetStatusReport();
        t.Received(1).GetStatus();
    }
}