using ElevatorSimulationApp.Abstractions;
using ElevatorSimulationApp.Interface_Classes;
using ElevatorSimulationApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Program
{
    static bool FireEmergencyActive = false; // Global fire emergency flag

    public static async Task Main(string[] args)
    {
        List<IElevator> elevators = new List<IElevator>
        {
            new StandardElevator(15),  // Standard elevator with 15-passenger capacity
            new FreightElevator(4, 2500)  // Freight elevator with 2500kg capacity
        };

        bool running = true;

        while (running)
        {
            DisplayMenu();  // Ensure the menu is displayed only once per iteration

            Console.Write("Select an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    if (!FireEmergencyActive)
                        await CallElevator(elevators);
                    else
                        Console.WriteLine("Cannot call an elevator during a fire emergency!");
                    break;

                case "2":
                    ReportStuckElevator(elevators);
                    break;

                case "3":
                    TriggerFireEmergency(elevators);
                    break;

                case "4":
                    SetMaintenanceMode(elevators);
                    break;

                case "5":
                    ViewElevatorStatus(elevators);
                    break;

                case "6":
                    ResetElevatorStatus(elevators);
                    break;

                case "7":
                    running = false;
                    Console.WriteLine("Exiting program...");
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    public static void DisplayMenu()
    {
        Console.WriteLine("\nOptions:");
        Console.WriteLine("1. Call Elevator");
        Console.WriteLine("2. Report Stuck Elevator");
        Console.WriteLine("3. Trigger Fire Emergency");
        Console.WriteLine("4. Set Elevator Maintenance Mode");
        Console.WriteLine("5. View Elevator Status");
        Console.WriteLine("6. Reset Elevator Status");
        Console.WriteLine("7. Exit");
    }

    public static async Task CallElevator(List<IElevator> elevators)
    {
        Console.Write("Select elevator (1 for Standard, 2 for Freight): ");
        int elevatorIndex = int.Parse(Console.ReadLine()) - 1;

        var elevator = elevators[elevatorIndex];

        if (elevator.IsStuck || elevator.FireEmergency)
        {
            Console.WriteLine("This elevator is unavailable due to an emergency or being stuck. Please reset it first.");
            return;  // Exit early if the elevator is unavailable
        }

        if (elevator.IsUnderMaintenance)
        {
            Console.WriteLine("This elevator is under maintenance and cannot be called.");
            return;
        }

        if (elevator is StandardElevator)
            await CallStandardElevator((StandardElevator)elevator);
        else if (elevator is FreightElevator)
            await CallFreightElevator((FreightElevator)elevator);
    }

    public static async Task CallStandardElevator(StandardElevator elevator)
    {
        Console.Write("Enter number of passengers (Max 15): ");
        int passengers = int.Parse(Console.ReadLine());

        if (passengers > elevator.MaxCapacity)
        {
            Console.WriteLine($"The elevator can hold a maximum of {elevator.MaxCapacity} passengers.");
            return;
        }

        List<int> destinations = new List<int>();
        for (int i = 0; i < passengers; i++)
        {
            Console.Write($"Enter destination for passenger {i + 1}: ");
            destinations.Add(int.Parse(Console.ReadLine()));
        }

        elevator.AddPassengers(passengers, destinations);
        await elevator.MoveToNextRequestedFloorAsync();
    }

    public static async Task CallFreightElevator(FreightElevator elevator)
    {
        Console.Write("Enter load weight (in kg, Max 2500kg): ");
        double weight = double.Parse(Console.ReadLine());

        if (!elevator.CanAcceptLoad(weight))
        {
            Console.WriteLine($"The load exceeds the maximum capacity of {elevator.MaxWeight} kg.");
            return;
        }

        elevator.AddLoad(weight);

        Console.Write("Enter the destination floor for the load: ");
        int destination = int.Parse(Console.ReadLine());

        elevator.SetLoadDestination(destination);
        await elevator.MoveToNextRequestedFloorAsync();
    }

    public static void ReportStuckElevator(List<IElevator> elevators)
    {
        Console.Write("Select elevator to report as stuck (1 or 2): ");
        int elevatorIndex = int.Parse(Console.ReadLine()) - 1;

        elevators[elevatorIndex].ReportStuck();
        Console.WriteLine("Elevator reported as stuck.");
    }

    public static void TriggerFireEmergency(List<IElevator> elevators)
    {
        FireEmergencyActive = true;

        foreach (var elevator in elevators)
        {
            elevator.TriggerFireEmergency();
        }
        Console.WriteLine("Fire emergency triggered! All elevators stopped.");
    }

    public static void SetMaintenanceMode(List<IElevator> elevators)
    {
        Console.Write("Select elevator (1 or 2): ");
        int elevatorIndex = int.Parse(Console.ReadLine()) - 1;

        Console.Write("Enter maintenance mode (true/false): ");
        bool status = bool.Parse(Console.ReadLine());

        elevators[elevatorIndex].SetMaintenanceMode(status);
    }

    public static void ViewElevatorStatus(List<IElevator> elevators)
    {
        for (int i = 0; i < elevators.Count; i++)
        {
            var elevator = elevators[i];
            Console.WriteLine($"\nElevator {i + 1} Status:");
            Console.WriteLine($"Type: {(elevator is FreightElevator ? "Freight" : "Standard")}");
            Console.WriteLine($"Current Floor: {elevator.CurrentFloor}");
            Console.WriteLine($"Passenger Count: {elevator.PassengerCount}");
            Console.WriteLine($"Is Stuck: {elevator.IsStuck}");
            Console.WriteLine($"Fire Emergency: {elevator.FireEmergency}");
            Console.WriteLine($"Under Maintenance: {elevator.IsUnderMaintenance}");
        }
    }

    public static void ResetElevatorStatus(List<IElevator> elevators)
    {
        Console.WriteLine("1. Reset a specific elevator");
        Console.WriteLine("2. Reset all elevators");
        Console.Write("Select an option: ");
        string option = Console.ReadLine();

        if (option == "1")
        {
            Console.Write("Select elevator to reset (1 or 2): ");
            int elevatorIndex = int.Parse(Console.ReadLine()) - 1;

            elevators[elevatorIndex].ResetStatus();
            Console.WriteLine($"Elevator {elevatorIndex + 1} status has been reset.");
        }
        else if (option == "2")
        {
            foreach (var elevator in elevators)
            {
                elevator.ResetStatus();
            }
            FireEmergencyActive = false;
            Console.WriteLine("All elevators have been reset.");
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }
}


