using UnityEngine;

public class TurretLaserSingle : TurretLaser
{
    [SerializeField] private GameObject barrel;         // Init in inspector
    [SerializeField] private GameObject firePoint;      // Init in inspector
    [SerializeField] private LineRenderer lineRenderer; // Init in inspector

    private Quaternion targetRotationForBarrel = new Quaternion();
    private bool isBarrelFacingEnemy = false;

    public override void ConstructBuilding(Model model)
    {
        // Data initialization
        type = 1;
        damagePoints = 5;
        int health = 0;
        int shield = 0;
        int defense = 0;
        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl1_LaserTurret_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl1_LaserTurret_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl1_LaserTurret_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl1_LaserTurret_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl1_LaserTurret_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl1_LaserTurret_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl1_LaserTurret_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl1_LaserTurret_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl1_LaserTurret_Base_Lvl_3;
            break;
        }
        CreateGameUnit(health, shield, defense);
        gameObject.name = "LT" + LTStaticData.turetteLaser_counter;
        LTStaticData.turetteLaser_counter++;



        // Rest data initialization
        base.ConstructBuilding(model);

        lineRenderer.enabled = false;

        // Add to resource manager list
        ResourceManager.Instance.laserTurretsList.Add(this);
    }

    public void ConstructBuildingFromFile_LaserSingle()
    {
        damagePoints = 5;
        lineRenderer.enabled = false;

        ResourceManager.Instance.laserTurretsList.Add(this);

        if (upgradeTimer != 0)
        {
            StartCoroutine(UpgradeLogic());
        }
    }
    
    public override void Attack()
    {
        if (isFacingEnemy)
        {
            RotateBarrelTowardsEnemy();

            if (isBarrelFacingEnemy)
            {
                if (!isAttackStart)
                {
                    isAttackStart = true;
                    base.Attack();
                }

                if (!isLasersEnabled && isAttackState)
                {
                    lineRenderer.enabled = true;
                    isLasersEnabled = true;
                }

                lineRenderer.SetPosition(0, firePoint.transform.position);
                lineRenderer.SetPosition(1, target.transform.position);
            }
        }
    }

    private void RotateBarrelTowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, targetRotationForBarrel, barrelTurnSpeed * Time.deltaTime);


            if ( barrel.transform.rotation == targetRotationForBarrel && !isBarrelFacingEnemy)
            {
                isBarrelFacingEnemy = true;
            }
        }
    }

    public override void ResetCombatMode()
    {
        TurnOffLaserDamage();
        isAttackStart = false;
        
        isBarrelFacingEnemy = false;
        isLasersEnabled = false;
        isFacingEnemy = false;
    }

    public void TurnOffLasers()
    {

        lineRenderer.enabled = false;

        isBarrelFacingEnemy = false;

        isLasersEnabled = false;
        isFacingEnemy = false;
    }
}