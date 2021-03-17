﻿using UnityEngine;

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
        gameUnit = new GameUnit(StatsManager._maxHealth_Lvl2_LaserTurret, StatsManager._maxShiled_Lvl2_LaserTurret, StatsManager._defensePoints_Lvl2_LaserTurret);
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

            turretData.HelperObjectInit(gameObject.transform.GetChild(1).gameObject);
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }

        gameObject.name = previousTurret.name;
        gameUnit.name = previousTurret.name;
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


    // // Function for creating building
    // public void ConstructBuildingAfterUpgrade(TurretLaser turretLaser)
    // {
    //     // type = turretLaser.type;

    //     // healthPoints = turretLaser.healthPoints;
    //     // shieldPoints = turretLaser.shieldPoints;
    //     // InitStaticsLevel_2();
        
    //     // this.gameObject.name = turretLaser.name + " 2";
    //     // this.tag = TagConstants.buildingTag;
    //     // this.gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
    //     // this.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;


    //     // // Reaplcing reference in Resource Manager class
    //     // for (int i = 0; i < ResourceManager.Instance.laserTurretsList.Count; i++)
    //     // {
    //     //     if (turretLaser == ResourceManager.Instance.laserTurretsList[i])
    //     //     {
    //     //         ResourceManager.Instance.laserTurretsList[i] = this;
    //     //         break;
    //     //     }
    //     // }


    //     // _tileOccupied = turretLaser._tileOccupied;

    //     // HelperObjectInit();
    //     // InitBarrels();
    //     // isPowerON = ResourceManager.Instance.IsPowerOn();
    // }
























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

            // Debug.Log(lineRenderer +" "+lineRenderer1);
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
        Debug.Log("Turn off lasers");
        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;

        isBarrelFacingEnemy = false;
        isBarrel1FacingEnemy = false;

        laserTurretData.isLasersEnabled = false;
        turretData.isFacingEnemy = false;
    }
}