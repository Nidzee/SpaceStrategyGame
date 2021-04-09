﻿using UnityEngine;

public class TurretMisileTriple : TurretMisile
{
    private GameObject barrel;
    private GameObject barrel1;
    private GameObject barrel2;

    private GameObject firePoint;
    private GameObject firePoint1;
    private GameObject firePoint2;


    public void ConstructBuildingAfterUpgrade(Turette turretMisile)
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);
        
        turretData = new TurretData(this);
        misileTurretData = new MTData();






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

        gameObject.name = turretMisile.name;
        tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;
        turretData.InitTurretDataFromPreviousTurret(turretMisile);
        
        InitBarrels();

        // // Reaplcing reference in Resource Manager class
        for (int i = 0; i < ResourceManager.Instance.misileTurretsList.Count; i++)
        {
            if (turretMisile == ResourceManager.Instance.misileTurretsList[i])
            {
                ResourceManager.Instance.misileTurretsList[i] = this;
                break;
            }
        }
    }

    public void ConstructBuildingFromFile_MisileTriple()
    {
        misileTurretData = new MTData();

        ResourceManager.Instance.misileTurretsList.Add(this);

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

            barrel2 = gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject;
            barrel2.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);

            firePoint = barrel.transform.GetChild(0).gameObject;
            firePoint1 = barrel1.transform.GetChild(0).gameObject;
            firePoint2 = barrel2.transform.GetChild(0).gameObject;
        }
    }

    // Attack pattern
    public override void Attack()
    {
        if (!misileTurretData.isFired)
        {
            GameObject misile = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint.transform.position, barrel.transform.rotation);
            misile.GetComponent<Rigidbody2D>().AddForce(transform.forward * 100f);
            misile.GetComponent<Misile>().target = base.turretData.target;

            GameObject misile1 = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint1.transform.position, barrel1.transform.rotation);
            misile1.GetComponent<Rigidbody2D>().AddForce(transform.forward * 100f);
            misile1.GetComponent<Misile>().target = base.turretData.target;

            GameObject misile2 = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint2.transform.position, barrel2.transform.rotation);
            misile2.GetComponent<Rigidbody2D>().AddForce(transform.forward * 100f);
            misile2.GetComponent<Misile>().target = base.turretData.target;


            Instantiate(MTStaticData._misileLaunchParticles, firePoint.transform.position, barrel.transform.rotation); 
            Instantiate(MTStaticData._misileLaunchParticles, firePoint1.transform.position, barrel1.transform.rotation); 
            Instantiate(MTStaticData._misileLaunchParticles, firePoint2.transform.position, barrel2.transform.rotation); 


            misileTurretData.isFired = true;
        }
        else // Cooldown
        {
            misileTurretData.coolDownTimer -= Time.deltaTime;
            if (misileTurretData.coolDownTimer < 0)
            {
                misileTurretData.coolDownTimer = 1f;
                misileTurretData.isFired = false;
            }
        }
    }
}