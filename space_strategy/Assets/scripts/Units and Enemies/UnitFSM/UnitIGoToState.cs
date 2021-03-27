using UnityEngine;
using Pathfinding;


public class UnitIGoToState : IUnitState
{
    [SerializeField] private float _nextWaypointDistance = 0.25f; // Distance between enemy and waypoint to be reached to go to the next waypoint.
    private Seeker _seeker = null;                             // Seeker component that allows us to use A* seeker functionality.
    private Path _path = null;                                 // Path to the player.
    private int _currentWaypoint = 0;                          // Store current waypoint along path that we are targeting.

    private bool flag = false;

    
    private void StateReset(Unit unit)
    {
        flag = false;
        unit.isPathInit = false;
    }

    public IUnitState DoState(Unit unit)
    {
        if (!flag)
        {
            _seeker = unit.GetComponent<Seeker>();
            flag = true;
        }

        DoMyState(unit);

        // If we lost home
        if (!unit.unitData.home)
        {
            unit.ChangeDestination((int)UnitDestinationID.Null);
            StateReset(unit);
            return unit.unitData.unitIHomelessState;
        }

        // if we lost job on way
        if (!unit.unitData.workPlace 
            && unit.unitData.destination != unit.unitData.home.GetUnitDestination().position 
            && unit.unitData.destination != unit.unitData.storage.GetUnitDestination().position)
        {
            unit.ChangeDestination((int)UnitDestinationID.Home);// unit.GetComponent<AIDestinationSetter>().target = unit.home.GetUnitDestination();// unit.destination = unit.home.GetUnitDestination().position;

            StateReset(unit);
            return unit.unitData.unitIGoToState;
        }

        // if we get job on way to home
        if (unit.unitData.workPlace 
            && unit.unitData.destination == unit.unitData.home.GetUnitDestination().position)
        {
            unit.ChangeDestination((int)UnitDestinationID.WorkPlace);// unit.GetComponent<AIDestinationSetter>().target = unit.workPlace.GetUnitDestination();// unit.destination = unit.workPlace.GetUnitDestination().position;

            StateReset(unit);
            return unit.unitData.unitIGoToState;
        }

        // If we approached shaft
        if (unit.unitData.isApproachShaft)
        {
            StateReset(unit);
            unit.unitData.isApproachShaft = false;
            return unit.unitData.unitIGatherState;
        }
        
        // If we approached storage
        else if (unit.unitData.isApproachStorage)
        {   
            StateReset(unit);
            unit.unitData.isApproachStorage = false;
            return unit.unitData.unitResourceLeavingState;
        }
        
        // If we approached home
        else if (unit.unitData.isApproachHome)
        {
            StateReset(unit);
            return unit.unitData.unitIdleState;
        }
        
        else 
            return unit.unitData.unitIGoToState;
    }


    private void DoMyState(Unit unit)
    {
        if (unit.GetComponent<AIDestinationSetter>().target)
        {
            if (!unit.isPathInit)
            {
                unit.isPathInit = true;
                _seeker.StartPath(unit.transform.position, unit.GetComponent<AIDestinationSetter>().target.position, OnPathComplete);

                Debug.Log(_path);
                // Debug.Log(_path.vectorPath[_path.vectorPath.Count - 1].x + " " + _path.vectorPath[_path.vectorPath.Count - 1].y + "      " + unit.GetComponent<AIDestinationSetter>().target.position);
            }

            if (IsPathExists() && _path.vectorPath.Count == _currentWaypoint)
            {
                Debug.Log("Reached the end of the path!");
                unit.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            if (IsPathExists() && IsThereWaypointsToFollow())
            {
                // Debug.Log(_path.vectorPath[_path.vectorPath.Count - 1].x + " " + _path.vectorPath[_path.vectorPath.Count - 1].y + "      " + unit.GetComponent<AIDestinationSetter>().target.position);
                Debug.Log(_path.vectorPath.Count + "   " + _currentWaypoint);
                
                Vector2 movingDirection = (_path.vectorPath[_currentWaypoint] - unit.transform.position).normalized;
                unit.GetComponent<Rigidbody2D>().velocity = movingDirection * (UnitStaticData.moveSpeed * 30 * Time.deltaTime);

                if (IsWaypointReached(unit.transform))
                {
                    // isPathInit = false;
                    _currentWaypoint++;
                }
            }
        }

        else
        {
            Debug.Log("Erase direction");
            _path.vectorPath.Clear();
            unit.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
        // Debug.Log(enemyTransform.position + "       " + _path.vectorPath[_currentWaypoint]);
        float distance = Vector2.Distance(enemyTransform.position, _path.vectorPath[_currentWaypoint]);

        // Debug.Log(_currentWaypoint);

        if (distance <= _nextWaypointDistance)
        {
            Debug.Log("Reached point!");
            return true;
        }

        else
        {
            return false;
        }
    }
}

