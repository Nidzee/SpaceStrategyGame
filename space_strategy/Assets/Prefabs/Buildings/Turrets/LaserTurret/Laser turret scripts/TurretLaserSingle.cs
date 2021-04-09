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

        turretData = new TurretData(this);
        laserTurretData = new LTData();






        base.ConstructBuilding(model);

        turretData.type = 1;

        LTStaticData.turetteLaser_counter++;
        gameObject.name = "TL" + LTStaticData.turetteLaser_counter;
        ResourceManager.Instance.laserTurretsList.Add(this);

        InitBarrels();
    }

    public void ConstructBuildingFromFile_LaserSingle()
    {
        laserTurretData = new LTData();

        ResourceManager.Instance.laserTurretsList.Add(this);

        InitBarrels();

        
        if (turretData.upgradeTimer != 0)
        {
            StartCoroutine(turretData.UpgradeLogic());
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
        // Debug.Log(lineRenderer.enabled);
        if (turretData.isFacingEnemy)
        {
            Debug.Log("TEST");

            RotateBarrelTowardsEnemy();

            if (isBarrelFacingEnemy)
            {
                if (!laserTurretData.isLasersEnabled && turretData.attackState)
                {
                    lineRenderer.enabled = true;
                    laserTurretData.isLasersEnabled = true;
                }

                lineRenderer.SetPosition(0, firePoint.transform.position);
                lineRenderer.SetPosition(1, turretData.target.transform.position);
            }
        }
    }

    private void RotateBarrelTowardsEnemy()
    {
        if (turretData.target)
        {
            Vector3 targetPosition = turretData.target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, targetRotationForBarrel, laserTurretData.barrelTurnSpeed * Time.deltaTime);


            if ( barrel.transform.rotation == targetRotationForBarrel && !isBarrelFacingEnemy)
            {
                isBarrelFacingEnemy = true;
            }
        }
    }

    public override void ResetCombatMode()
    {
        isBarrelFacingEnemy = false;
        laserTurretData.isLasersEnabled = false;
        turretData.isFacingEnemy = false;
    }

    public void TurnOffLasers()
    {
        Debug.Log("TurnOffLasers");
        lineRenderer.enabled = false;

        isBarrelFacingEnemy = false;

        laserTurretData.isLasersEnabled = false;
        turretData.isFacingEnemy = false;
    }
}