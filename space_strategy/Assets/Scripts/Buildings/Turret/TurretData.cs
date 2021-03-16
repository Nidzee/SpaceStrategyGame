using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TurretData 
{
    public GameObject _tileOccupied;
    public GameObject center;

    public Enemy target;
    public List<Enemy> enemiesInsideRange;

    public bool isCreated;
    public bool isFacingEnemy;
    public bool attackState;
    public bool isPowerON;
    public bool isMenuOpened;
    public bool isTurnedInIdleMode;

    public Quaternion idleRotation = new Quaternion();
    public Quaternion targetRotation = new Quaternion();

    public float coolDownTurnTimer;
    public float upgradeTimer;    

    public int level;
    public int type;

    public TurretCombatState combatState;
    public TurretIdleState idleState;
    public TurretPowerOffState powerOffState;
    public ITurretState currentState;


    public TurretData()
    {
        isCreated = true;
        isPowerON = ResourceManager.Instance.IsPowerOn();
        enemiesInsideRange = new List<Enemy>();

        _tileOccupied = null;

        isFacingEnemy = false;
        attackState = false;
        isMenuOpened = false;
        
        idleRotation = new Quaternion();
        targetRotation = new Quaternion();

        isTurnedInIdleMode = true;
        coolDownTurnTimer = 3f;
        upgradeTimer = 0f;    

        combatState = new TurretCombatState();
        idleState = new TurretIdleState();
        powerOffState = new TurretPowerOffState();
        currentState = idleState;
    }

    public void InitTurretDataFromPreviousTurret(Turette previousTurret)
    {
        // Add hp sp

        level = (previousTurret.turretData.level + 1);
        type = previousTurret.turretData.type;

        _tileOccupied = previousTurret.turretData._tileOccupied;
    }

    IEnumerator UpgradeLogic(Turette turretUpgrading)
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += StatsManager._timerStep_Turret * Time.deltaTime;

            if (isMenuOpened)
            {
                switch(level)
                {
                    case 1:
                    TurretStaticData.turretMenuReference.level2.fillAmount = upgradeTimer;
                    break;

                    case 2:
                    TurretStaticData.turretMenuReference.level3.fillAmount = upgradeTimer;
                    break;

                    case 3:
                    Debug.Log("Error! Didnt fin circle to reload!");
                    break;
                }
            }

            yield return null;
        }

        upgradeTimer = 0;

        TurretUpgrading(turretUpgrading);
    }

    private void TurretUpgrading(Turette turretUpgrading)
    {
        Debug.Log("TURRET LEVEL UP!");

        Turette temp = null;

        // Replace old turret with new turret HERE
        switch (type)
        {
            case (int)TurretType.LaserTurret:
            {
                TurretLaser turretLaser = null;
                switch (level)
                {
                    case 1:
                    turretLaser = GameObject.Instantiate(PrefabManager.Instance.doubleTuretteLaserPrefab, turretUpgrading.transform.position, turretUpgrading.transform.rotation).GetComponent<TurretLaserDouble>();
                    turretLaser.GetComponent<TurretLaserDouble>().ConstructBuildingAfterUpgrade(turretUpgrading);
                    break;

                    case 2:
                    turretLaser = GameObject.Instantiate(PrefabManager.Instance.tripleTuretteLaserPrefab, turretUpgrading.transform.position, turretUpgrading.transform.rotation).GetComponent<TurretLaserTriple>();
                    turretLaser.GetComponent<TurretLaserTriple>().ConstructBuildingAfterUpgrade(turretUpgrading);
                    break;
                }
                turretLaser.gameObject.transform.GetChild(1).transform.rotation = turretUpgrading.gameObject.transform.GetChild(1).transform.rotation;
                temp = turretLaser;
            }
            break;
            // Upgrade misile turret
            case (int)TurretType.MisileTurret:
            {
                TurretMisile turretMisile = null;
                switch (level)
                {
                    case 1:
                    turretMisile = GameObject.Instantiate(PrefabManager.Instance.doubleturetteMisilePrefab, turretUpgrading.transform.position, turretUpgrading.transform.rotation).GetComponent<TurretMisileDouble>();
                    turretMisile.GetComponent<TurretMisileDouble>().ConstructBuildingAfterUpgrade(turretUpgrading);
                    break;

                    case 2:
                    turretMisile = GameObject.Instantiate(PrefabManager.Instance.truipleturetteMisilePrefab, turretUpgrading.transform.position, turretUpgrading.transform.rotation).GetComponent<TurretMisileTriple>();
                    turretMisile.GetComponent<TurretMisileTriple>().ConstructBuildingAfterUpgrade(turretUpgrading);
                    break;
                }
                turretMisile.gameObject.transform.GetChild(1).transform.rotation = turretUpgrading.gameObject.transform.GetChild(1).transform.rotation;
                temp = turretMisile;
            }
            break;
        }

        // If menu was opened for this turret reinitializing menu data with new turret
        if (isMenuOpened)
        {
            TurretStaticData.turretMenuReference.ReloadPanel(temp);
        }

        GameViewMenu.Instance.buildingsManageMenuReference.ReplaceTurretScrollItem(turretUpgrading, temp);
        GameObject.Destroy(turretUpgrading.gameObject);
    }

    public void StartUpgrade(Turette turretUpgrading)
    {
        turretUpgrading.StartCoroutine(UpgradeLogic(turretUpgrading));
    }

    public void InitStatsAfterBaseUpgrade_LT(out int newHealth, out int newShield, out int newDefense)
    {
        Debug.Log("InitStatsAfterBaseUpgrade_LT");
        switch (level)
        {
            case 1:
            newHealth = StatsManager._maxHealth_Lvl1_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret;
            newShield = StatsManager._maxShiled_Lvl1_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret;
            newDefense = StatsManager._defensePoints_Lvl1_LaserTurret;
            break;

            case 2:
            newHealth = StatsManager._maxHealth_Lvl2_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret;
            newShield = StatsManager._maxShiled_Lvl2_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret;
            newDefense = StatsManager._defensePoints_Lvl2_LaserTurret;
            break;

            case 3:
            newHealth = StatsManager._maxHealth_Lvl3_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret;
            newShield = StatsManager._maxShiled_Lvl3_LaserTurret + StatsManager._baseUpgradeStep_LaserTurret;
            newDefense = StatsManager._defensePoints_Lvl3_LaserTurret;
            break;

            default:
            newHealth = 0;
            newShield = 0;
            newDefense = 0;
            break;
        }
    }

    public void InitStatsAfterBaseUpgrade_MT(out int newHealth, out int newShield, out int newDefense)
    {
        switch (level)
        {
            case 1:
            newHealth = StatsManager._maxHealth_Lvl1_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret;
            newShield = StatsManager._maxShiled_Lvl1_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret;
            newDefense = StatsManager._defensePoints_Lvl1_MisileTurret;
            break;

            case 2:
            newHealth = StatsManager._maxHealth_Lvl2_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret;
            newShield = StatsManager._maxShiled_Lvl2_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret;
            newDefense = StatsManager._defensePoints_Lvl2_MisileTurret;
            break;

            case 3:
            newHealth = StatsManager._maxHealth_Lvl3_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret;
            newShield = StatsManager._maxShiled_Lvl3_MisileTurret + StatsManager._baseUpgradeStep_MisileTurret;
            newDefense = StatsManager._defensePoints_Lvl3_MisileTurret;
            break;

            default:
            newHealth = 0;
            newShield = 0;
            newDefense = 0;
            break;
        }
    }

    public void UpgradeStatsAfterShtabUpgrade(out int newHealth, out int newShield, out int newDefense)
    {
        newHealth = 0;
        newShield = 0;
        newDefense = 0;

        switch(type)
        {
            case 1:
            InitStatsAfterBaseUpgrade_LT(out newHealth, out newShield, out newDefense);
            break;

            case 2:
            InitStatsAfterBaseUpgrade_MT(out newHealth, out newShield, out newDefense);
            break;
        }
    }

    public void ConstructBuilding_MT()
    {
        type = (int)TurretType.MisileTurret;
    }
    
    public void ConstructBuilding_LT()
    {
        type = (int)TurretType.LaserTurret;
    }

    public void ConstructBuilding(Model model)
    {
        // Always level 1 because we can build only first-level buildings
        level = 1;
        _tileOccupied = model.BTileZero;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
    }

    public void TurretLifeCycle(Turette turret)
    {
        if (isCreated)
        {
            currentState = currentState.DoState(turret);
        }
    }

    public void DestroyBuilding()
    {
        if (isMenuOpened)
        {
            TurretStaticData.turretMenuReference.ExitMenu();
        }
                
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
    }

    public void HelperObjectInit(GameObject centerObject)
    {
        center = centerObject;
    }

}

public enum TurretType
{
    LaserTurret = 1,
    MisileTurret = 2
}