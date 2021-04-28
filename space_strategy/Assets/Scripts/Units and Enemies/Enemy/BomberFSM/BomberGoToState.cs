using UnityEngine;

public class BomberMovingState : IBomberState
{
    // Distance between enemy and waypoint to be reached to go to the next waypoint.
    private float _nextWaypointDistance = 0.25f;
   
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
            bomber.rigidBodyRef.velocity = Vector2.zero;
            return bomber.bashState;
        }

        if (bomber.isReachedTarget)
        {
            Debug.Log("Reached attack state!");

            bomber.isReachedTarget = false;
            bomber.rigidBodyRef.velocity = Vector2.zero;
            return bomber.attackState;
        }

        return bomber.movingState;
    }

    private void DoMyState(EnemyBomber bomber)
    {
        if (bomber.path != null)
        {
            if (bomber.destinationBuilding == null)
            {
                if (bomber.path != null)
                {
                    bomber.path = null;
                    bomber.RebuildCurrentPath();
                }
            }

            if (bomber.path.vectorPath.Count == bomber.currentWaypoint)
            {
                Debug.Log("Reached the end of the path!");
                bomber.rigidBodyRef.velocity = Vector2.zero;

                // Create new path to another building or start destroying here
            }

            if (IsThereWaypointsToFollow(bomber))
            {
                Vector2 movingDirection = (bomber.path.vectorPath[bomber.currentWaypoint] - bomber.transform.position).normalized;
                bomber.rigidBodyRef.velocity = movingDirection * (BomberStaticData.moveSpeed * Time.deltaTime);

                if (IsWaypointReached(bomber))
                {
                    bomber.currentWaypoint++;
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
        return bomber.currentWaypoint < bomber.path.vectorPath.Count;
    }

    private bool IsWaypointReached(EnemyBomber bomber)
    {
        float distance = Vector2.Distance(bomber.transform.position, bomber.path.vectorPath[bomber.currentWaypoint]);

        return (distance <= _nextWaypointDistance);
    }
}