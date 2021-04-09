using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TurretData 
{
    public int ID;

    public int rotation_building;
    public float rotation_center;
    public float rotation_center_w;

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

    public Turette _myTurret;


    public TurretData(Turette thisTurret)
    {
        _myTurret = thisTurret;

        isPowerON = ResourceManager.Instance.IsPowerOn();
        enemiesInsideRange = new List<Enemy>();

        _tileOccupied = null;

        isCreated = true;
        isFacingEnemy = false;
        attackState = false;
        isMenuOpened = false;
        isTurnedInIdleMode = true;

        
        idleRotation = new Quaternion();
        targetRotation = new Quaternion();

        coolDownTurnTimer = 3f;
        upgradeTimer = 0f;    

        combatState = new TurretCombatState();
        idleState = new TurretIdleState();
        powerOffState = new TurretPowerOffState();
        currentState = idleState;
    }



    public void InitTurretDataFromPreviousTurret(Turette previousTurret)
    {
        level = (previousTurret.turretData.level + 1);
        type = previousTurret.turretData.type;

        _tileOccupied = previousTurret.turretData._tileOccupied;
    }

    public IEnumerator UpgradeLogic()
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

        TurretUpgrading();
    }

    private void TurretUpgrading()
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
                    turretLaser = GameObject.Instantiate(PrefabManager.Instance.doubleTuretteLaserPrefab, _myTurret.transform.position, _myTurret.transform.rotation).GetComponent<TurretLaserDouble>();
                    turretLaser.GetComponent<TurretLaserDouble>().ConstructBuildingAfterUpgrade(_myTurret);
                    break;

                    case 2:
                    turretLaser = GameObject.Instantiate(PrefabManager.Instance.tripleTuretteLaserPrefab, _myTurret.transform.position, _myTurret.transform.rotation).GetComponent<TurretLaserTriple>();
                    turretLaser.GetComponent<TurretLaserTriple>().ConstructBuildingAfterUpgrade(_myTurret);
                    break;
                }
                turretLaser.gameObject.transform.GetChild(1).transform.rotation = _myTurret.gameObject.transform.GetChild(1).transform.rotation;
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
                    turretMisile = GameObject.Instantiate(PrefabManager.Instance.doubleturetteMisilePrefab, _myTurret.transform.position, _myTurret.transform.rotation).GetComponent<TurretMisileDouble>();
                    turretMisile.GetComponent<TurretMisileDouble>().ConstructBuildingAfterUpgrade(_myTurret);
                    break;

                    case 2:
                    turretMisile = GameObject.Instantiate(PrefabManager.Instance.truipleturetteMisilePrefab, _myTurret.transform.position, _myTurret.transform.rotation).GetComponent<TurretMisileTriple>();
                    turretMisile.GetComponent<TurretMisileTriple>().ConstructBuildingAfterUpgrade(_myTurret);
                    break;
                }
                turretMisile.gameObject.transform.GetChild(1).transform.rotation = _myTurret.gameObject.transform.GetChild(1).transform.rotation;
                temp = turretMisile;
            }
            break;
        }

        // If menu was opened for this turret reinitializing menu data with new turret
        if (isMenuOpened)
        {
            TurretStaticData.turretMenuReference.ReloadPanel(temp);
        }

        GameViewMenu.Instance.buildingsManageMenuReference.ReplaceTurretScrollItem(_myTurret, temp);
        GameObject.Destroy(_myTurret.gameObject);
    }

    public void StartUpgrade()
    {
        _myTurret.StartCoroutine(UpgradeLogic());
    }

    public void InitStatsAfterBaseUpgrade_LT(out int newHealth, out int newShield, out int newDefense)
    {
        newHealth = 0;
        newShield = 0;
        newDefense = 0;
        
        switch (level)
        {
            case 1:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl1_LaserTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl1_LaserTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl1_LaserTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl1_LaserTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl1_LaserTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl1_LaserTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl1_LaserTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl1_LaserTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl1_LaserTurret_Base_Lvl_3;
                break;
            }
            break;

            case 2:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_3;
                break;
            }
            break;


            case 3:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl3_LaserTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl3_LaserTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl3_LaserTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl3_LaserTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl3_LaserTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl3_LaserTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl3_LaserTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl3_LaserTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl3_LaserTurret_Base_Lvl_3;
                break;
            }
            break;
        }
    }

    public void InitStatsAfterBaseUpgrade_MT(out int newHealth, out int newShield, out int newDefense)
    {
        newHealth = 0;
        newShield = 0;
        newDefense = 0;
        
        switch (level)
        {
            case 1:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl1_MisileTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl1_MisileTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl1_MisileTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl1_MisileTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl1_MisileTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl1_MisileTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl1_MisileTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl1_MisileTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl1_MisileTurret_Base_Lvl_3;
                break;
            }
            break;

            case 2:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl2_MisileTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl2_MisileTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl2_MisileTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl2_MisileTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl2_MisileTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl2_MisileTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl2_MisileTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl2_MisileTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl2_MisileTurret_Base_Lvl_3;
                break;
            }
            break;


            case 3:
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                newHealth = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_1;
                newShield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_1;
                newDefense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_1;
                break;

                case 2:
                newHealth = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_2;
                newShield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_2;
                newDefense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_2;
                break;

                case 3:
                newHealth = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_3;
                newShield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_3;
                newDefense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_3;
                break;
            }
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

























}

public enum TurretType
{
    LaserTurret = 1,
    MisileTurret = 2
}