using UnityEngine;
using Pathfinding;


public class BomberGoToState : IBomberState
{
    [SerializeField] private float _nextWaypointDistance = 0.25f; // Distance between enemy and waypoint to be reached to go to the next waypoint.
   


    public IBomberState DoState(EnemyBomber bomber)
    {
        DoMyState(bomber);


        // If destination building is deleted - rebuild path
        // If colided new building - rebuild path
        // If colided target building - Attack state
        // If collided Bash object - Bash state


        if (bomber.isBashIntersects)
        {
            bomber.isBashIntersects = false;
            bomber.rb.velocity = Vector2.zero;
            return bomber.bomberBashState;
        }

        if (bomber.isReachedTarget)
        {
            Debug.Log("Reached attack state!");

            bomber.isReachedTarget = false;
            bomber.rb.velocity = Vector2.zero;
            return bomber.bomberAttackState;
        }

        return bomber.bomberGoToState;
    }


    private void DoMyState(EnemyBomber bomber)
    {
        if (bomber._path != null)
        {
            if (bomber.destinationBuilding == null)
            {
                if (bomber._path != null)
                {
                    bomber._path = null;
                    bomber.RebuildCurrentPath();
                }
            }


            if (bomber._path.vectorPath.Count == bomber._currentWaypoint)
            {
                Debug.Log("Reached the end of the path!");
                bomber.rb.velocity = Vector2.zero;

                // Create new path to another building or start destroying here
            }

            if (IsThereWaypointsToFollow(bomber))
            {
                Vector2 movingDirection = (bomber._path.vectorPath[bomber._currentWaypoint] - bomber.transform.position).normalized;
                bomber.rb.velocity = movingDirection * (BomberStaticData.moveSpeed * Time.deltaTime);

                if (IsWaypointReached(bomber))
                {
                    bomber._currentWaypoint++;
                }
            }
        }

        else
        {
            Debug.Log("Error! Path is not initialized!");
        }
    }

    private bool IsThereWaypointsToFollow(EnemyBomber bomber)
    {
        return bomber._currentWaypoint < bomber._path.vectorPath.Count;
    }

    private bool IsWaypointReached(EnemyBomber bomber)
    {
        float distance = Vector2.Distance(bomber.transform.position, bomber._path.vectorPath[bomber._currentWaypoint]);

        return (distance <= _nextWaypointDistance);
    }
}