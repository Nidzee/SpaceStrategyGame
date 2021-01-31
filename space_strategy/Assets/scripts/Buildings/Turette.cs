using UnityEngine;
using System.Collections.Generic;

public class Turette : AliveGameUnit, IBuilding
{
    public static TurretMenu turretMenuReference; // Reference to UI panel

    public List<Enemy> enemiesInsideRange;
    public Enemy target;

    public bool isFacingEnemy = false;
    private bool isTurnedInIdleMode = true;
    public bool attackState = false;
    public bool isCreated = false;

    private float turnSpeed = 200f;
    private float coolDownTurnTimer = 3f;

    private Quaternion idleRotation = new Quaternion();
    public Quaternion targetRotation = new Quaternion();

    public int level = 1;

    public bool isMenuOpened = false;


    // Reloads sliders if Turret Menu is opened
    public override void TakeDamage(float DamagePoints)
    {
        base.TakeDamage(DamagePoints);

        if (isMenuOpened)
        {
            turretMenuReference.ReloadSlidersHP_SP();
        }
    }

    // Function for displaying info
    public virtual void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("TurretMenu");
        
        if (!turretMenuReference) // executes once
        {
            turretMenuReference = GameObject.Find("TurretMenu").GetComponent<TurretMenu>();
        }
    }

#region  Terret function
    // Life cycle
    private void Update()
    {
        if (isCreated)
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
    }

    // Initializing helper GameObject - Dispenser
    public void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.turretRange;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer); // Means that it is noninteractible
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretRangeLayer;
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }

        isCreated = true;
    }

    private void IdleMode()
    {
        RandomIdleTurn();
    }

    private void CombatMode()
    {
        TurnTowardsEnemy();
        
        if (isFacingEnemy)
        {
            Attack();
        }
    }

    private void RandomIdleTurn()
    {
        if (isTurnedInIdleMode)
        {
            coolDownTurnTimer -= Time.deltaTime;
            if (coolDownTurnTimer < 0)
            {
                coolDownTurnTimer = 5f;
                isTurnedInIdleMode = false;
                idleRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, -360f)));
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, idleRotation, turnSpeed * Time.deltaTime);

            if (transform.rotation == idleRotation)
            {
                isTurnedInIdleMode = true;
            }
        }
    }


    // Turning turret logic - correct!
    private void TurnTowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            if (transform.rotation == targetRotation && !isFacingEnemy)
            {
                isFacingEnemy = true;
            }
        }
    }

    // TODO
    public void Upgrade()
    {
        Debug.Log("Turret Upgrade REDO!");
        level++;
    }

    // Every turret has its own attack pattern
    public virtual void Attack(){}

#endregion

}
