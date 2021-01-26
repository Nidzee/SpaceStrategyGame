using UnityEngine;
using System.Collections.Generic;

public class Turette : AliveGameUnit, IBuilding
{
    public List<Enemy> enemiesInsideRange;
    public Enemy target;
    public bool attackState = false;

    private bool isFacingEnemy = false;
    private bool isTurnedInIdleMode = true;

    private float turnSpeed = 200f;
    private float coolDownTurnTimer = 3f;

    private Quaternion idelRotation = new Quaternion();


    private void Awake()              // Initializing helper GameObject - Dispenser
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.turretRange;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.noninteractibleRadiusLayer);
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }


    private void Update()
    {
        if (attackState)
        {
            CombatMode();
        }
        else
        {
            IdleMode();
        }
    }

    private void IdleMode()
    {
        // Random Turret turn - imitate patroling
        if (isTurnedInIdleMode)
        {
            coolDownTurnTimer -= Time.deltaTime;
            if (coolDownTurnTimer < 0)
            {
                coolDownTurnTimer = 5f;
                isTurnedInIdleMode = false;
                idelRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, -360f)));
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, idelRotation, turnSpeed * Time.deltaTime);

            if (transform.rotation == idelRotation)
            {
                isTurnedInIdleMode = true;
            }
        }
    }

    private void CombatMode() // Correct
    {
        TurnTowardsEnemy();
        
        if (isFacingEnemy)
        {
            Attack();
        }
    }

    private void TurnTowardsEnemy() // Correct
    {
        // Turnig logic
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            if (transform.rotation == targetRotation)
            {
                Debug.Log("Fire");
                isFacingEnemy = true;
            }
        }
    }


    public virtual void Attack() // Every turret has its own atttack pattern
    {
    
    }

    public virtual void Invoke()
    {
        Debug.Log("Selected Turret - go menu now");
    }
}
