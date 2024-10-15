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
        public bool IsStuck { get; protected set; }
        public bool FireEmergency { get; protected set; }

        protected Elevator(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            PassengerCount = 0;
            CurrentFloor = 0;
            IsMoving = false;
            Direction = Direction.Stationary;
        }

        public virtual async Task MoveToFloorAsync(int destinationFloor)
        {
            if (IsUnderMaintenance || IsStuck || FireEmergency)
            {
                Console.WriteLine("Elevator cannot move due to emergency or maintenance.");
                return;
            }

            IsMoving = true;
            Direction = destinationFloor > CurrentFloor ? Direction.Up : Direction.Down;

            while (CurrentFloor != destinationFloor && !IsStuck)
            {
                await Task.Delay(1000); // Simulate floor-to-floor movement
                CurrentFloor += Direction == Direction.Up ? 1 : -1;
                Console.WriteLine($"Elevator at floor {CurrentFloor}.");
            }

            IsMoving = false;
            Direction = Direction.Stationary;
        }

        public virtual bool CanAcceptPassengers(int count)
        {
            return !IsUnderMaintenance && PassengerCount + count <= MaxCapacity && !IsStuck;
        }

        public virtual void AddPassengers(int count)
        {
            if (CanAcceptPassengers(count))
            {
                PassengerCount += count;
                Console.WriteLine($"{count} passengers added.");
            }
            else
            {
                Console.WriteLine("Cannot add passengers. Elevator is either full, under maintenance, or stuck.");
            }
        }

        public virtual void RemovePassengers(int count)
        {
            PassengerCount = Math.Max(0, PassengerCount - count);
            Console.WriteLine($"{count} passengers removed.");
        }

        public void TriggerEmergencyStop()
        {
            IsMoving = false;
            IsStuck = true;
            Console.WriteLine("Emergency stop triggered. Elevator is stuck.");
        }

        public void SetMaintenanceMode(bool status)
        {
            IsUnderMaintenance = status;
            Console.WriteLine($"Maintenance mode {(status ? "enabled" : "disabled")}.");
        }

        public void ReportStuck()
        {
            IsStuck = true;
            Console.WriteLine("Elevator is stuck.");
        }
    }

}
