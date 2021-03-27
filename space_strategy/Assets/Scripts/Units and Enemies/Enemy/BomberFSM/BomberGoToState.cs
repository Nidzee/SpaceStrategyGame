using UnityEngine;
using Pathfinding;


public class BomberGoToState : IBomberState
{
    [SerializeField] private float _nextWaypointDistance = 0.25f; // Distance between enemy and waypoint to be reached to go to the next waypoint.
    private Seeker _seeker = null;                             // Seeker component that allows us to use A* seeker functionality.
    private Path _path = null;                                 // Path to the player.
    private int _currentWaypoint = 0;                          // Store current waypoint along path that we are targeting.

    private bool flag = false;

    
    private void StateReset(EnemyBomber bomber)
    {
        flag = false;
        bomber.isPathInit = false;
    }


    public IBomberState DoState(EnemyBomber bomber)
    {
        if (!flag)
        {
            _seeker = bomber.GetComponent<Seeker>();
            flag = true;
        }

        DoMyState(bomber);


        return bomber.bomberGoToState;
    }


    private void DoMyState(EnemyBomber bomber)
    {
        if (!bomber.isPathInit)
        {
            bomber.isPathInit = true;
            _seeker.StartPath(bomber.transform.position, bomber.GetComponent<AIDestinationSetter>().target.position, OnPathComplete);
        }

        if (IsPathExists() && _path.vectorPath.Count == _currentWaypoint)
        {
            Debug.Log("Reached the end of the path!");
            bomber.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            // Create new path to another building
        }

        if (IsPathExists() && IsThereWaypointsToFollow())
        {
            Vector2 movingDirection = (_path.vectorPath[_currentWaypoint] - bomber.transform.position).normalized;
            bomber.GetComponent<Rigidbody2D>().velocity = movingDirection * (BomberStaticData.moveSpeed * 30 * Time.deltaTime);

            if (IsWaypointReached(bomber.transform))
            {
                _currentWaypoint++;
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

        return (distance <= _nextWaypointDistance);
    }
}