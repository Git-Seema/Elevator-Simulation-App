using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationApp.Interfaces
{
    public interface IFreightElevator
    {
        double CurrentLoad { get; }
        double MaxWeightCapacity { get; }

        bool CanAcceptLoad(double weight);
        void AddLoad(double weight);
        void RemoveLoad(double weight);
    }

}
