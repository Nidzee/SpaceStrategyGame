using UnityEngine;
using System.Collections.Generic;

public class Turette : AliveGameUnit, IBuilding
{
    public TurretData turretData;
    public TurretSavingData savingData;

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void TurretDestroy(AliveGameUnit gameUnit);
    public event TurretDestroy OnTurretDestroyed = delegate{};


    public void InitStatsAfterShtabUpgrade()
    {
        int newHealth = 0;
        int newShield = 0;
        int newDefense = 0;

        turretData.UpgradeStatsAfterShtabUpgrade(out newHealth, out newShield, out newDefense);

        UpgradeStats(newHealth, newShield, newDefense);

        OnDamageTaken(this);
    }




    public TurretSavingData newSavingData = new TurretSavingData();

    private void Update()
    {
        if (turretData != null)
        {
            turretData.currentState = turretData.currentState.DoState(this);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (name == "TM1")
            {
                newSavingData.name = this.name;
                newSavingData.positionAndOccupationTileName = turretData._tileOccupied.name;
                newSavingData.rotation_building = turretData.rotation_building;
                newSavingData.rotation_center = turretData.center.transform.rotation.z;
                newSavingData.rotation_center_w = turretData.center.transform.rotation.w;
                newSavingData.type = turretData.type;
                newSavingData.level = turretData.level;
                newSavingData.healthPoints = healthPoints;
                newSavingData.shieldPoints = shieldPoints;
                newSavingData.maxCurrentHealthPoints = maxCurrentHealthPoints;
                newSavingData.maxCurrentShieldPoints = maxCurrentShieldPoints;
                newSavingData.deffencePoints = deffencePoints;
                newSavingData.isShieldOn = isShieldOn;
                newSavingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;
                newSavingData.isPowerOn = turretData.isPowerON;
                newSavingData.upgradeTimer = turretData.upgradeTimer;

                switch (turretData.type)
                {
                    case 1:
                    var laserTurret = this.GetComponent<TurretLaser>();
                    ResourceManager.Instance.laserTurretsList.Remove(laserTurret);
                    break;

                    case 2:
                    var misileTurret = this.GetComponent<TurretMisile>();
                    ResourceManager.Instance.misileTurretsList.Remove(misileTurret);
                    break;
                }

                GameHendler.Instance.turretSavingData = newSavingData;
                Destroy(gameObject);
            }
        }
    }

    public void StartUpgrade()
    {
        turretData.StartUpgrade();
    }

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyBuilding();
            return;
        }

        OnDamageTaken(this);
    }

    public virtual void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("TurretMenu");

        TurretStaticData.turretMenuReference.ReloadPanel(this);
    }

    public override void UpgradeStats(int newHealth, int NewShield, int newDefense)
    {
        base.UpgradeStats(newHealth, NewShield, newDefense);
    }

    public virtual void ConstructBuilding(Model model)
    {
        turretData.level = 1;
        turretData.rotation_building = model.rotation;
        turretData._tileOccupied = model.BTileZero;
        turretData._tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;


        
        gameObject.AddComponent<BuildingMapInfo>();///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[1];
        info.mapPoints[0] = model.BTileZero.transform;




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


        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();

        
        turretData.isCreated = true;
        turretData.isPowerON = ResourceManager.Instance.IsPowerOn();

        if (turretData.isPowerON)
        {
            turretData.currentState = turretData.idleState;
        }
        else
        {
            turretData.currentState = turretData.powerOffState;
        }
    }

    public virtual void DestroyBuilding()
    {
        if (turretData.isMenuOpened)
        {
            TurretStaticData.turretMenuReference.ExitMenu();
        }
                
        turretData._tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        OnTurretDestroyed(this);
    }

    public void TurnTurretOFF()
    {
        turretData.isPowerON = false;
    }

    public void TurnTurretON()
    {
        turretData.isPowerON = true;
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }

    public virtual void ResetCombatMode(){}
 
    public virtual void Attack(){}














    public void SaveData()
    {

    }

    public void ConstructBuildingFromFile(TurretSavingData savingData)
    {
        name = savingData.name;

        InitGameUnitFromFile(
        savingData.healthPoints, 
        savingData.maxCurrentHealthPoints,
        savingData.shieldPoints,
        savingData.maxCurrentShieldPoints,
        savingData.deffencePoints,
        savingData.isShieldOn,
        savingData.shieldGeneratorInfluencers);



        turretData = new TurretData(this);
        turretData.enemiesInsideRange = new List<Enemy>();

        turretData.isCreated = true;
        turretData.isFacingEnemy = false;
        turretData.attackState = false;
        turretData.isPowerON = savingData.isPowerOn;
        turretData.isMenuOpened = false;
        turretData.isTurnedInIdleMode = true;

        turretData.idleRotation = new Quaternion();
        turretData.targetRotation = new Quaternion();
        turretData.combatState = new TurretCombatState();
        turretData.idleState = new TurretIdleState();
        turretData.powerOffState = new TurretPowerOffState();
        turretData.currentState = turretData.idleState;

        turretData._tileOccupied = GameObject.Find(savingData.positionAndOccupationTileName);
        turretData.rotation_building = savingData.rotation_building;
        turretData.rotation_center = savingData.rotation_center;
        turretData.rotation_center_w = savingData.rotation_center_w;
        turretData.type = savingData.type;
        turretData.level = savingData.level;
        turretData.upgradeTimer = savingData.upgradeTimer;
        turretData.coolDownTurnTimer = 3f;

        

        turretData.isPowerON = ResourceManager.Instance.IsPowerOn();

        if (turretData.isPowerON)
        {
            turretData.currentState = turretData.idleState;
        }
        else
        {
            turretData.currentState = turretData.powerOffState;
        }





        OnDamageTaken += TurretStaticData.turretMenuReference.ReloadSlidersHP_SP;
        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnTurretDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;




        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.turretRange;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer); // Means that it is noninteractible
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretRangeLayer;

            turretData.center = gameObject.transform.GetChild(1).gameObject;

            turretData.center.transform.rotation = new Quaternion(0,0,savingData.rotation_center, savingData.rotation_center_w);
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }
}