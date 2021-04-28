using UnityEngine;
using Pathfinding;

public class UnitMovingState : IUnitState
{
    public float _nextWaypointDistance = 0.25f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        // If we lost home
        if (!unit.Home)
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            unit.rigidBodyRef.velocity = Vector2.zero;
            return unit.noSignalState;
        }

        // If we approached shaft
        if (unit.isApproachShaft)
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            unit.isApproachShaft = false;
            unit.rigidBodyRef.velocity = Vector2.zero;
            return unit.extractingState;
        }
        
        // If we approached storage
        else if (unit.isApproachStorage)
        {   
            unit.ChangeDestination((int)UnitDestinationID.Null);
            unit.isApproachStorage = false;
            unit.rigidBodyRef.velocity = Vector2.zero;
            return unit.resourceLeavingState;
        }
        
        // If we approached home
        else if (unit.isApproachHome)
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            unit.rigidBodyRef.velocity = Vector2.zero;
            return unit.idleState;
        }
        
        else 
            return unit.movingState;
    }

    private void DoMyState(Unit unit)
    {
        if (unit.Home)
        {
            // If we lost job on way to it
            if (!unit.WorkPlace 
            && unit.Destination != unit.Home.GetUnitDestination().position 
            && unit.Destination != unit.Storage.GetUnitDestination().position)
            {
                unit.ChangeDestination((int)UnitDestinationID.Home);
                unit.RebuildPath();
            }

            // If we get job on way to home
            if (unit.WorkPlace 
            && unit.Destination == unit.Home.GetUnitDestination().position)
            {
                unit.ChangeDestination((int)UnitDestinationID.WorkPlace);
                unit.RebuildPath();
            }

            if (unit.GetComponent<AIDestinationSetter>().target)
            {
                if (unit.path != null)
                {
                    if (unit.path.vectorPath.Count == unit.currentWaypoint)
                    {
                        Debug.Log("Reached the end of the path!");
                        unit.rigidBodyRef.velocity = Vector2.zero;
                    }

                    if (IsThereWaypointsToFollow(unit))
                    {
                        Vector2 movingDirection = (unit.path.vectorPath[unit.currentWaypoint] - unit.transform.position).normalized;
                        unit.rigidBodyRef.velocity = movingDirection * (UnitStaticData.moveSpeed * 20 * Time.deltaTime);

                        if (IsWaypointReached(unit))
                        {
                            unit.currentWaypoint++;
                        }
                    }
                }
                else
                {
                    Debug.Log("Error! Path is not initialized!");
                }
            }
            else
            {
                Debug.Log("There is no target to go to!");
            }
        }
    }

    private bool IsThereWaypointsToFollow(Unit unit)
    {
        return unit.currentWaypoint < unit.path.vectorPath.Count;
    }

    private bool IsWaypointReached(Unit unit)
    {
        float distance = Vector2.Distance(unit.transform.position, unit.path.vectorPath[unit.currentWaypoint]);

        return (distance <= _nextWaypointDistance);
    }
}