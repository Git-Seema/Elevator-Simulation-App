using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationApp.Abstractions
{
    public class StandardElevator : Elevator
    {
        public StandardElevator(int maxCapacity) : base(maxCapacity) { }

        public override async Task MoveToFloorAsync(int destinationFloor)
        {
            Console.WriteLine("Standard Elevator: Starting movement...");
            await base.MoveToFloorAsync(destinationFloor);
        }
    }

}
