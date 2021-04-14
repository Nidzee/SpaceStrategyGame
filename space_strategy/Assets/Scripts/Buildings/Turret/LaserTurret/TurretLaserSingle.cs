using UnityEngine;

public class TurretLaserSingle : TurretLaser
{
    private GameObject barrel;
    private GameObject firePoint;
    public LineRenderer lineRenderer;

    private Quaternion targetRotationForBarrel = new Quaternion();
    private bool isBarrelFacingEnemy = false;



    public override void ConstructBuilding(Model model)
    {
        damagePoints = 5;
        // Data initialization
        type = 1;
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


        // Init barrels
        InitBarrels();
        // Add to resource manager list
        ResourceManager.Instance.laserTurretsList.Add(this);
    }

    public void ConstructBuildingFromFile_LaserSingle()
    {
        damagePoints = 5;

        ResourceManager.Instance.laserTurretsList.Add(this);

        InitBarrels();

        if (upgradeTimer != 0)
        {
            StartCoroutine(UpgradeLogic());
        }
    }
    






    private void InitBarrels()
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;
            gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;

            barrel = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
            barrel.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            barrel.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;
            barrel.GetComponent<SpriteRenderer>().sortingOrder = 3;

            firePoint = barrel.transform.GetChild(0).gameObject;

            lineRenderer = barrel.gameObject.GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
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

                if (!isLasersEnabled && attackState)
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
        isBarrelFacingEnemy = false;
        isLasersEnabled = false;
        isFacingEnemy = false;
    }

    public void TurnOffLasers()
    {
        TurnOffLaserDamage();
        isAttackStart = false;

        lineRenderer.enabled = false;

        isBarrelFacingEnemy = false;

        isLasersEnabled = false;
        isFacingEnemy = false;
    }
}