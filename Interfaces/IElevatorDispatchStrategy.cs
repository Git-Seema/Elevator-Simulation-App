using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationApp.Interface_Classes
{
    // Interface for dispatching elevators
    public interface IElevatorDispatchStrategy
    {
        IElevator DispatchElevator(List<IElevator> elevators, int requestedFloor);
    }
}
