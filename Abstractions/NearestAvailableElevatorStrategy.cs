using ElevatorSimulationApp.Interface_Classes;
using ElevatorSimulationApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulationApp.Abstractions
{
    public class NearestAvailableElevatorStrategy : IElevatorDispatchStrategy
    {
        public IElevator? DispatchElevator(List<IElevator> elevators, int requestedFloor)
        {
            IElevator? nearestElevator = null;
            int minDistance = int.MaxValue;

            foreach (var elevator in elevators)
            {
                // Ensure the elevator is available (not stuck, under maintenance, or in emergency mode)
                if (elevator.IsStuck || elevator.IsUnderMaintenance || elevator.FireEmergency)
                    continue;

                // Dispatch based on distance
                int distance = Math.Abs(elevator.CurrentFloor - requestedFloor);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestElevator = elevator;
                }
            }

            return nearestElevator;
        }

        public IElevator? DispatchFreightElevator(List<IElevator> elevators, double loadWeight, int requestedFloor)
        {
            IFreightElevator? suitableFreightElevator = null;
            int minDistance = int.MaxValue;

            foreach (var elevator in elevators)
            {
                if (elevator is IFreightElevator freightElevator)
                {
                    // Cast to IElevator to check common elevator properties
                    var commonElevator = (IElevator)elevator;

                    // Check if the freight elevator can handle the load and is available
                    if (freightElevator.CanAcceptLoad(loadWeight) && !commonElevator.IsStuck && !commonElevator.IsUnderMaintenance && !commonElevator.FireEmergency)
                    {
                        int distance = Math.Abs(commonElevator.CurrentFloor - requestedFloor);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            suitableFreightElevator = freightElevator;
                        }
                    }
                }
            }

            return (IElevator?)suitableFreightElevator;
        }
    }
}
