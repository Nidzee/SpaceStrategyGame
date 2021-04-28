using UnityEngine;

public class TurretLaserTriple : TurretLaser
{
    [SerializeField] private  GameObject barrel;
    [SerializeField] private  GameObject barrel1;
    [SerializeField] private  GameObject barrel2;

    [SerializeField] private  GameObject firePoint;
    [SerializeField] private  GameObject firePoint1;
    [SerializeField] private  GameObject firePoint2;

    [SerializeField] private  LineRenderer lineRenderer;
    [SerializeField] private  LineRenderer lineRenderer1;
    [SerializeField] private  LineRenderer lineRenderer2;

    private Quaternion targetRotationForBarrel = new Quaternion();
    private Quaternion targetRotationForBarrel1 = new Quaternion();
    private Quaternion targetRotationForBarrel2 = new Quaternion();

    private bool isBarrelFacingEnemy = false;
    private bool isBarrel1FacingEnemy = false;
    private bool isBarrel2FacingEnemy = false;


    public void ConstructBuildingAfterUpgrade(Turette previousTurret)
    {
        // Data initialization
        damagePoints = 15;
        int health = 0;
        int shield = 0;
        int defense = 0;
        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl3_LaserTurret_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl3_LaserTurret_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl3_LaserTurret_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl3_LaserTurret_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl3_LaserTurret_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl3_LaserTurret_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl3_LaserTurret_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl3_LaserTurret_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl3_LaserTurret_Base_Lvl_3;
            break;
        }
        CreateGameUnit(health, shield, defense);
        

        // Rest data initialization from previous turret
        InitTurretDataFromPreviousTurret(previousTurret);


        // Init rest of Data
        InitData();



        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;
        lineRenderer2.enabled = false;

        // Reaplcing reference in Resource Manager class
        for (int i = 0; i < ResourceManager.Instance.laserTurretsList.Count; i++)
        {
            if (previousTurret == ResourceManager.Instance.laserTurretsList[i])
            {
                ResourceManager.Instance.laserTurretsList[i] = this;
                break;
            }
        }
    }

    public void ConstructBuildingFromFile_LaserTriple()
    {
        damagePoints = 15;

        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;
        lineRenderer2.enabled = false;

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
            RotateBarrel1TowardsEnemy();
            RotateBarrel2TowardsEnemy();

            if (isBarrelFacingEnemy && isBarrel1FacingEnemy && isBarrel2FacingEnemy)
            {
                if (!isAttackStart)
                {
                    isAttackStart = true;
                    base.Attack();
                }

                if (!isLasersEnabled && isAttackState)
                {
                    lineRenderer.enabled = true;
                    lineRenderer1.enabled = true;
                    lineRenderer2.enabled = true;

                    isLasersEnabled = true;
                }

                lineRenderer.SetPosition(0, barrel.transform.position);
                lineRenderer1.SetPosition(0, barrel1.transform.position);
                lineRenderer2.SetPosition(0, barrel2.transform.position);

                lineRenderer.SetPosition(1, target.transform.position);
                lineRenderer1.SetPosition(1, target.transform.position);
                lineRenderer2.SetPosition(1, target.transform.position);
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

    private void RotateBarrel1TowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel1.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel1 = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel1.transform.rotation = Quaternion.RotateTowards(barrel1.transform.rotation, targetRotationForBarrel1, barrelTurnSpeed * Time.deltaTime);

            if (barrel1.transform.rotation == targetRotationForBarrel1 && !isBarrel1FacingEnemy)
            {
                isBarrel1FacingEnemy = true;
            }
        }
    }

    private void RotateBarrel2TowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel2.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel2 = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel2.transform.rotation = Quaternion.RotateTowards(barrel2.transform.rotation, targetRotationForBarrel2, barrelTurnSpeed * Time.deltaTime);

            if (barrel2.transform.rotation == targetRotationForBarrel2 && !isBarrel2FacingEnemy)
            {
                isBarrel2FacingEnemy = true;
            }
        }
    }

    public override void ResetCombatMode()
    {
        TurnOffLaserDamage();
        isAttackStart = false;
        
        isBarrelFacingEnemy = false;
        isBarrel1FacingEnemy = false;
        isBarrel2FacingEnemy = false;

        isLasersEnabled = false;
        isFacingEnemy = false;
    }

    public void TurnOffLasers()
    {
        isBarrelFacingEnemy = false;
        isBarrel1FacingEnemy = false;
        isBarrel2FacingEnemy = false;
        
        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;
        lineRenderer2.enabled = false; 

        isFacingEnemy = false;
        isLasersEnabled = false;
    }
}