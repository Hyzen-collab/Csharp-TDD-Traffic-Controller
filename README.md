# 🚦 C# TDD Smart Traffic Controller

A C# project developed using **Test Driven Development (TDD)** methodology for the CO2401 Software Development module at the University of Lancashire. The system implements an intelligent traffic controller managing vehicle signals, pedestrian signals, and timing through interface-based dependency injection and mock-based unit testing.

> **Course:** CO2401 – Software Development Assignment  
> **Author:** K.A. Idusha Piumika  
> **Student ID:** G21328023  
> **Date:** 27/03/2026

---

## 📋 Project Overview

The `TrafficController` class was built incrementally following a structured TDD workflow across two specification levels. All external dependencies were abstracted using interfaces and tested using mock objects, ensuring isolated and reliable unit tests.

---

## 🛠️ Technologies Used

| Tool | Purpose |
|---|---|
| C# (.NET) | Primary programming language |
| Visual Studio | IDE |
| NUnit 4.4.0 | Unit testing framework |
| NSubstitute 5.3.0 | Mocking framework for dependency injection |
| Microsoft.NET.Test.Sdk 17.8.0 | Test runner configuration |

---

## 📁 Project Structure

```
csharp-tdd-traffic-controller/
│
├── TrafficSystem/
│   ├── TrafficController.cs          # Core controller class
│   ├── IVehicleSignalManager.cs      # Interface for vehicle signals
│   ├── IPedestrianSignalManager.cs   # Interface for pedestrian signals
│   ├── ITimeManager.cs               # Interface for time management
│   ├── IWebService.cs                # Interface for web service
│   ├── IEmailService.cs              # Interface for email service
│   └── TrafficSystem.csproj
│
├── TrafficSystem.Tests/
│   ├── TrafficControllerTests.cs     # All NUnit unit tests
│   └── TrafficSystem.Tests.csproj
│
└── README.md
```

---

## ⚙️ Features Implemented

### Level 1
- Constructor initialization with intersection ID
- Intersection ID conversion to lowercase automatically
- Getter methods for current state
- Direct state setting via `SetStateDirect()` with valid state validation

### Level 2
- State transition logic in `SetCurrentState()` method
- Constructor validation — throws `ArgumentException` for invalid initial states
- Interface-based dependency injection for all external services
- `GetStatusReport()` method combining status from all injected dependencies

---

## 🔴🟢 TDD Workflow — Red, Green, Refactor

This project strictly followed the TDD cycle:

1. **Red** — Write a failing unit test that defines the expected behaviour
2. **Green** — Write the minimum implementation to make the test pass
3. **Refactor** — Clean up and improve the code while keeping all tests passing

Test cases were structured using the **Arrange-Act-Assert (AAA)** pattern:

```csharp
[Test]
public void Constructor_ShouldConvertIDToLowercase()
{
    // Arrange & Act
    var c = new TrafficController("ABC123");

    // Assert
    Assert.That(c.GetIntersectionID(), Is.EqualTo("abc123"));
}
```

---

## 🧪 Testing Highlights

### Parameterized Tests
Used `[TestCase]` to reduce redundancy and cover multiple scenarios in a single test method:

```csharp
[TestCase("green")]
[TestCase("amber")]
[TestCase("red")]
public void SetStateDirect_ValidStates_ShouldSucceed(string state)
{
    var c = new TrafficController("id");
    c.SetStateDirect(state);
    Assert.That(c.GetCurrentVehicleSignalState(), Is.EqualTo(state));
}
```

### Mocking with NSubstitute
External dependencies were mocked to isolate `TrafficController` logic:

```csharp
[Test]
public void GetStatusReport_ShouldCombineAllStatuses()
{
    var v = Substitute.For<IVehicleSignalManager>();
    var p = Substitute.For<IPedestrianSignalManager>();
    var t = Substitute.For<ITimeManager>();

    v.GetStatus().Returns("VehicleSignal,OK");
    p.GetStatus().Returns("PedestrianSignal,OK");
    t.GetStatus().Returns("Timer,OK");

    var c = new TrafficController("id", v, p, t, null, null);
    var result = c.GetStatusReport();

    Assert.That(result, Is.EqualTo("VehicleSignal,OK,PedestrianSignal,OK,Timer,OK"));
}
```

---

## ✅ Requirements Checklist

| Level One | Level Two | Level Three |
|---|---|---|
| L1R1 ✅ | L2R1 ✅ | L3R1 ❌ |
| L1R2 ✅ | L2R2 ✅ | L3R2 ❌ |
| L1R3 ✅ | L2R3 ✅ | L3R3 ❌ |
| L1R4 ✅ | L2R4 ✅ | L3R4 ❌ |
| L1R5 ✅ | | L3R5 ❌ |

---

## 🚀 How to Run

### Prerequisites
- Visual Studio 2022 or later
- .NET 8.0 SDK or later

### Steps

1. Clone the repository:
```bash
git clone https://github.com/Hyzen-collab/csharp-tdd-traffic-controller.git
```

2. Open `TrafficSystem.sln` in Visual Studio

3. Restore NuGet packages:
```
Tools → NuGet Package Manager → Restore Packages
```

4. Run all tests:
```
Test → Run All Tests (Ctrl + R, A)
```

All 55 tests should pass with 0 failures.

---

## 📚 Key Learnings

- Writing tests **before** implementation leads to cleaner, more focused code
- Mock objects allow isolated testing without relying on real service implementations
- The **Arrange-Act-Assert** pattern improves test readability and maintainability
- Parameterized tests reduce code duplication across similar test scenarios
- Interface-based design makes classes significantly more testable and modular

---

## 🔮 Future Improvements

- [ ] Complete Level 3 requirements
- [ ] Add more edge case coverage for complex state transitions
- [ ] Improve test naming conventions for better maintainability
- [ ] Explore advanced mocking for time-based logic
- [ ] Add integration tests alongside unit tests

---

## 📜 License

This project was created for academic purposes as part of the CO2401 Software Development module at the University of Lancashire.
