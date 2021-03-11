using UnityEngine;
using Pathfinding;


public class UnitIGoToState : IUnitState
{
    [SerializeField] private float _nextWaypointDistance = 0f;  // Distance between enemy and waypoint to be reached to go to the next waypoint.
    private Seeker _seeker = null;                             // Seeker component that allows us to use A* seeker functionality.
    private Path _path = null;                                 // Path to the player.
    private int _currentWaypoint = 0;                          // Store current waypoint along path that we are targeting.
    //private bool _isEndOfPathReached = false;                  // Check if end of path is reached.

    private bool flag = false;

    public IUnitState DoState(Unit unit)
    {
        if (!flag)
        {
            _seeker = unit.GetComponent<Seeker>();
            flag = true;
        }

        DoMyState(unit);

        if (!unit.home)
        {
            flag = false;

            return unit.unitIHomelessState;
        }

        if (!unit.workPlace && unit.destination != unit.home.angar.transform.position && unit.destination != unit.storage.storageConsumer.transform.position) // if we lost job on way
        {
            unit.GetComponent<AIDestinationSetter>().target = unit.home.angar.transform;

            unit.destination = unit.home.angar.transform.position;

            flag = false;

            return unit.unitIGoToState;
        }

        if (unit.destination == unit.home.angar.transform.position && unit.workPlace) // if we get job on way to home
        {

            unit.GetComponent<AIDestinationSetter>().target = unit.workPlace.dispenser.transform;
            


            unit.destination = unit.workPlace.dispenser.transform.position;

            flag = false;

            return unit.unitIGoToState;
        }

        if (unit.isApproachShaft)
        {
            unit.isApproachShaft = false;

            flag = false;

            return unit.unitIGatherState;
        }
        
        else if (unit.isApproachStorage)
        {   
            unit.isApproachStorage = false;

            flag = false;

            return unit.unitResourceLeavingState;
        }
        
        else if (unit.isApproachHome)
        {
            // unit.isApproachHome = false;
            flag = false;

            return unit.unitIdleState;
        }
        
        else 
            return unit.unitIGoToState;
    }

    private void DoMyState(Unit unit)
    {
        if (unit.GetComponent<AIDestinationSetter>().target)
        {
            // Start path - creates new path every frame
            _seeker.StartPath(unit.transform.position, unit.GetComponent<AIDestinationSetter>().target.position, OnPathComplete);

            if (IsPathExists() && IsThereWaypointsToFollow())
            {
                Vector2 movingDirection = (_path.vectorPath[_currentWaypoint] - unit.transform.position).normalized;
                unit.GetComponent<Rigidbody2D>().velocity = movingDirection * (Unit.moveSpeed * Time.deltaTime);

                if (IsWaypointReached(unit.transform))
                {
                    _currentWaypoint++;
                }
            }
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            _path = path;
            _currentWaypoint = 0;
        }
    }

    private bool IsPathExists()
    {
        return _path != null;
    }

    private bool IsThereWaypointsToFollow()
    {
        return _currentWaypoint < _path.vectorPath.Count;
    }

    private bool IsWaypointReached(Transform enemyTransform)
    {
        float distance = Vector2.Distance(enemyTransform.position, _path.vectorPath[_currentWaypoint]);
        return distance < _nextWaypointDistance;
    }
}

