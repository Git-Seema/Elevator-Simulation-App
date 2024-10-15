using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulationApp.Enum;

namespace ElevatorSimulationApp.Abstractions
{
    public class FreightElevator : Elevator
    {
        public double MaxWeight { get; private set; } // Maximum load capacity in kg
        public double CurrentWeight { get; private set; } // Current load weight in kg

        public FreightElevator(int maxCapacity, double maxWeight) : base(maxCapacity)
        {
            MaxWeight = maxWeight;
            CurrentWeight = 0;
        }

        public override async Task MoveToFloorAsync(int destinationFloor)
        {
            if (IsUnderMaintenance || IsStuck || FireEmergency)
            {
                Console.WriteLine("Freight Elevator cannot move due to emergency or maintenance.");
                return;
            }

            IsMoving = true;
            Direction = destinationFloor > CurrentFloor ? Direction.Up : Direction.Down;

            while (CurrentFloor != destinationFloor && !IsStuck)
            {
                await Task.Delay(1000); // Simulate floor-to-floor movement
                CurrentFloor += Direction == Direction.Up ? 1 : -1;
                Console.WriteLine($"Freight Elevator at floor {CurrentFloor}.");
            }

            IsMoving = false;
            Direction = Direction.Stationary;
        }

        public bool CanAcceptLoad(double loadWeight)
        {
            return CurrentWeight + loadWeight <= MaxWeight && !IsUnderMaintenance && !IsStuck;
        }

        public void AddLoad(double loadWeight)
        {
            if (CanAcceptLoad(loadWeight))
            {
                CurrentWeight += loadWeight;
                Console.WriteLine($"{loadWeight} kg load added. Current weight: {CurrentWeight} kg.");
            }
            else
            {
                Console.WriteLine("Cannot add load. Freight elevator is either full, under maintenance, or stuck.");
            }
        }

        public void RemoveLoad(double loadWeight)
        {
            CurrentWeight = Math.Max(0, CurrentWeight - loadWeight);
            Console.WriteLine($"{loadWeight} kg load removed. Current weight: {CurrentWeight} kg.");
        }
    }

}
