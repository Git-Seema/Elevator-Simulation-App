using ElevatorSimulationApp.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationApp.Interface_Classes
{
    using System.Threading.Tasks;

    public interface IElevator
    {
        int CurrentFloor { get; }
        int MaxCapacity { get; }
        int PassengerCount { get; }
        bool IsMoving { get; }
        Direction Direction { get; }
        bool IsUnderMaintenance { get; }
        bool IsStuck { get; set; }
        bool FireEmergency { get; set; }

        Task MoveToNextRequestedFloorAsync();
        void AddPassengers(int count, List<int> destinations);
        void RemovePassengersAtFloor(int floor);
        void TriggerEmergencyStop();
        void SetMaintenanceMode(bool status);
        void ReportStuck();
        void TriggerFireEmergency();
        void ResetStatus();
    }
}
