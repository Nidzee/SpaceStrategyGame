using UnityEngine;

public class TurretLaserDouble : TurretLaser
{
    private GameObject barrel;
    private GameObject barrel1;

    private GameObject firePoint;
    private GameObject firePoint1;

    private LineRenderer lineRenderer;
    private LineRenderer lineRenderer1;

    private Quaternion targetRotationForBarrel = new Quaternion();
    private Quaternion targetRotationForBarrel1 = new Quaternion();

    private bool isBarrelFacingEnemy = false;
    private bool isBarrel1FacingEnemy = false;



    public void ConstructBuildingAfterUpgrade(Turette previousTurret)
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);
        
        turretData = new TurretData(this);
        laserTurretData = new LTData();






        OnDamageTaken += TurretStaticData.turretMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnTurretDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.turretRange;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer); // Means that it is noninteractible
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretRangeLayer;

            turretData.center = (gameObject.transform.GetChild(1).gameObject);
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }

        gameObject.name = previousTurret.name;
        tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;
        turretData.InitTurretDataFromPreviousTurret(previousTurret);

        InitBarrels();

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

    public void ConstructBuildingFromFile_LaserDouble()
    {
        laserTurretData = new LTData();

        ResourceManager.Instance.laserTurretsList.Add(this);

        turretData._myTurret = this;

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
            barrel = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
            barrel.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            barrel1 = gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
            barrel1.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);

            firePoint = barrel.transform.GetChild(0).gameObject;
            firePoint1 = barrel1.transform.GetChild(0).gameObject;

            lineRenderer = barrel.gameObject.GetComponent<LineRenderer>();
            lineRenderer1 = barrel1.gameObject.GetComponent<LineRenderer>();
        }
    }


    // Attack pattern
    public override void Attack()
    {
        if (turretData.isFacingEnemy)
        {
            RotateBarrelTowardsEnemy();
            RotateBarrel1TowardsEnemy();

            if (isBarrelFacingEnemy && isBarrel1FacingEnemy)
            {
                if (!laserTurretData.isLasersEnabled && turretData.attackState)
                {
                    lineRenderer.enabled = true;
                    lineRenderer1.enabled = true;
                    laserTurretData.isLasersEnabled = true;
                }

                lineRenderer.SetPosition(0, barrel.transform.position);
                lineRenderer1.SetPosition(0, barrel1.transform.position);

                lineRenderer.SetPosition(1, turretData.target.transform.position);
                lineRenderer1.SetPosition(1, turretData.target.transform.position);
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

    private void RotateBarrel1TowardsEnemy()
    {
        if (turretData.target)
        {
            Vector3 targetPosition = turretData.target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel1.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel1 = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel1.transform.rotation = Quaternion.RotateTowards(barrel1.transform.rotation, targetRotationForBarrel1, laserTurretData.barrelTurnSpeed * Time.deltaTime);

            if (barrel1.transform.rotation == targetRotationForBarrel1 && !isBarrel1FacingEnemy)
            {
                isBarrel1FacingEnemy = true;
            }
        }
    }

    public override void ResetCombatMode()
    {
        isBarrelFacingEnemy = false;
        isBarrel1FacingEnemy = false;

        laserTurretData.isLasersEnabled = false;
        turretData.isFacingEnemy = false;
    }

    public void TurnOffLasers()
    {
        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;

        isBarrelFacingEnemy = false;
        isBarrel1FacingEnemy = false;

        laserTurretData.isLasersEnabled = false;
        turretData.isFacingEnemy = false;
    }
}