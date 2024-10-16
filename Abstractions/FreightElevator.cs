using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulationApp.Enum;
using ElevatorSimulationApp.Interfaces;

namespace ElevatorSimulationApp.Abstractions
{
    public class FreightElevator : Elevator
    {
        public double MaxWeight { get; private set; }
        public double CurrentLoad { get; private set; }
        private int LoadDestination { get; set; }  // Track load destination separately

        public FreightElevator(int maxCapacity, double maxWeight) : base(maxCapacity)
        {
            MaxWeight = maxWeight;
            CurrentLoad = 0;
        }

        public bool CanAcceptLoad(double weight) => CurrentLoad + weight <= MaxWeight;

        public void AddLoad(double weight)
        {
            if (CanAcceptLoad(weight))
            {
                CurrentLoad += weight;
                Console.WriteLine($"Added {weight} kg. Current load: {CurrentLoad} kg.");
            }
            else
            {
                Console.WriteLine("Load exceeds maximum capacity.");
            }
        }

        public void SetLoadDestination(int floor)
        {
            LoadDestination = floor;
            Console.WriteLine($"Load destination set to floor {floor}.");
        }

        public override async Task MoveToNextRequestedFloorAsync()
        {
            Console.WriteLine("Freight elevator moving...");

            // Move to the specified load destination
            while (CurrentFloor != LoadDestination && !IsStuck)
            {
                await Task.Delay(1000);  // Simulate floor movement
                CurrentFloor += CurrentFloor < LoadDestination ? 1 : -1;
                Console.WriteLine($"Elevator at floor {CurrentFloor}.");
            }

            Console.WriteLine($"Arrived at floor {LoadDestination}.");
            Unload();  // Unload the freight
        }

        private void Unload()
        {
            Console.WriteLine($"Unloaded {CurrentLoad} kg at floor {LoadDestination}.");
            CurrentLoad = 0;  // Reset the load after delivery
        }
    }
}
