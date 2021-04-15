using UnityEngine;
using Pathfinding;


public class UnitIGoToState : IUnitState
{
    public float _nextWaypointDistance = 0.25f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        // If we lost home
        if (!unit.Home)
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            unit._rb.velocity = Vector2.zero;
            return unit.unitIHomelessState;
        }

        // If we approached shaft
        if (unit.isApproachShaft)
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            unit.isApproachShaft = false;
            unit._rb.velocity = Vector2.zero;
            return unit.unitIGatherState;
        }
        
        // If we approached storage
        else if (unit.isApproachStorage)
        {   
            unit.ChangeDestination((int)UnitDestinationID.Null);
            unit.isApproachStorage = false;
            unit._rb.velocity = Vector2.zero;
            return unit.unitResourceLeavingState;
        }
        
        // If we approached home
        else if (unit.isApproachHome)
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            unit._rb.velocity = Vector2.zero;
            return unit.unitIdleState;
        }
        
        else 
            return unit.unitIGoToState;
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
                if (unit._path != null)
                {
                    if (unit._path.vectorPath.Count == unit._currentWaypoint)
                    {
                        Debug.Log("Reached the end of the path!");
                        unit._rb.velocity = Vector2.zero;
                    }

                    if (IsThereWaypointsToFollow(unit))
                    {
                        Vector2 movingDirection = (unit._path.vectorPath[unit._currentWaypoint] - unit.transform.position).normalized;
                        unit._rb.velocity = movingDirection * (UnitStaticData.moveSpeed * 20 * Time.deltaTime);

                        if (IsWaypointReached(unit))
                        {
                            unit._currentWaypoint++;
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
        return unit._currentWaypoint < unit._path.vectorPath.Count;
    }

    private bool IsWaypointReached(Unit unit)
    {
        float distance = Vector2.Distance(unit.transform.position, unit._path.vectorPath[unit._currentWaypoint]);

        return (distance <= _nextWaypointDistance);
    }
}