using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationApp.Interface_Classes
{
    // Interface for floors
    public interface IFloor
    {
        int FloorNumber { get; }
        Task CallElevatorAsync(int passengers);
    }
}
