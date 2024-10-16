using ElevatorSimulationApp.Enum;
using ElevatorSimulationApp.Interface_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationApp.Abstractions
{
    public abstract class Elevator : IElevator
    {
        public int CurrentFloor { get; protected set; }
        public int MaxCapacity { get; protected set; }
        public int PassengerCount { get; protected set; }
        public bool IsMoving { get; protected set; }
        public Direction Direction { get; protected set; }
        public bool IsUnderMaintenance { get; protected set; }
        public bool IsStuck { get; set; }
        public bool FireEmergency { get; set; }

        // Dictionary to store destinations and passenger counts
        protected Dictionary<int, int> PassengerDestinations = new Dictionary<int, int>();

        protected Elevator(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            CurrentFloor = 0;
            PassengerCount = 0;
            IsMoving = false;
            Direction = Direction.Stationary;
        }

        public virtual async Task MoveToNextRequestedFloorAsync()
        {
            while (PassengerDestinations.Any() && !IsStuck && !FireEmergency)
            {
                int nextFloor = PassengerDestinations.Keys.Min();
                Direction = nextFloor > CurrentFloor ? Direction.Up : Direction.Down;

                while (CurrentFloor != nextFloor && !IsStuck)
                {
                    await Task.Delay(1000);
                    CurrentFloor += Direction == Direction.Up ? 1 : -1;
                    Console.WriteLine($"Elevator at floor {CurrentFloor}.");
                }

                Console.WriteLine($"Arrived at floor {nextFloor}.");
                RemovePassengersAtFloor(nextFloor);
                AllowNewPassengers();
            }

            Direction = Direction.Stationary;
            IsMoving = false;
        }

        public void AddPassengers(int count, List<int> destinations)
        {
            if (PassengerCount + count > MaxCapacity)
            {
                Console.WriteLine("Elevator is full.");
                return;
            }

            foreach (var destination in destinations)
            {
                if (PassengerDestinations.ContainsKey(destination))
                    PassengerDestinations[destination]++;
                else
                    PassengerDestinations[destination] = 1;
            }

            PassengerCount += count;
            Console.WriteLine($"{count} passengers added. Destinations: {string.Join(", ", destinations)}.");
        }

        public void RemovePassengersAtFloor(int floor)
        {
            if (PassengerDestinations.ContainsKey(floor))
            {
                int exitingPassengers = PassengerDestinations[floor];
                PassengerCount -= exitingPassengers;
                PassengerDestinations.Remove(floor);
                Console.WriteLine($"{exitingPassengers} passengers exited at floor {floor}.");
            }
            else
            {
                Console.WriteLine($"No passengers exited at floor {floor}.");
            }

            Console.WriteLine($"Remaining passengers: {PassengerCount}.");
        }

        public void AllowNewPassengers()
        {
            Console.Write("Enter number of new passengers: ");
            int newPassengers = int.Parse(Console.ReadLine());

            if (newPassengers > 0)
            {
                List<int> newDestinations = new List<int>();
                for (int i = 0; i < newPassengers; i++)
                {
                    Console.Write($"Enter destination for new passenger {i + 1}: ");
                    newDestinations.Add(int.Parse(Console.ReadLine()));
                }

                AddPassengers(newPassengers, newDestinations);
            }
            else
            {
                Console.WriteLine("No new passengers entered.");
            }
        }

        public void TriggerEmergencyStop() => IsStuck = true;

        public void SetMaintenanceMode(bool status) => IsUnderMaintenance = status;

        public void ReportStuck() => TriggerEmergencyStop();

        public void TriggerFireEmergency() => FireEmergency = true;

        public void ResetStatus()
        {
            IsStuck = false;
            FireEmergency = false;
            IsMoving = false;
            Console.WriteLine("Elevator status reset.");
        }
    }
}
