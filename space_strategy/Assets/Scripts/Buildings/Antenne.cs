//////////////////////////////////////////////////////////////////////////////




// All functionality is int GameHendler because we can create only 1 instance of Antenne
// But when Antenne dies we dont want to reset buttons timers 




//////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class Antenne : AliveGameUnit, IBuilding
{
    // public GameUnit gameUnit;
    public AntenneData antenneData;
    public AntenneSavingData antenneSavingData;

    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void AntenneDestroy(AliveGameUnit gameUnit);
    public event AntenneDestroy OnAntenneDestroyed = delegate{};


    public void InitStatsAfterBaseUpgrade()
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_1;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_2;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_3;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_3;
            break;
        }

        UpgradeStats(health, shield, defense);

        OnDamageTaken(this);
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

    public void ConstructBuilding(Model model)
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_1;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_1;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_2;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_2;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Antenne_Base_Lvl_3;
            shield = StatsManager._maxShiled_Antenne_Base_Lvl_3;
            defense = StatsManager._maxDeffensePoints_Antenne_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);
        
        antenneData = new AntenneData(this);
        antenneSavingData = new AntenneSavingData();

        GarageStaticData.garage_counter++;
        gameObject.name = "AN1";


        
        gameObject.AddComponent<BuildingMapInfo>();
        BuildingMapInfo info = gameObject.GetComponent<BuildingMapInfo>();
        info.mapPoints = new Transform[2];
        info.mapPoints[0] = model.BTileZero.transform;
        info.mapPoints[1] = model.BTileOne.transform;





        OnDamageTaken += GameViewMenu.Instance.buildingsManageMenuReference.ReloadHPSP;
        OnAntenneDestroyed += GameViewMenu.Instance.buildingsManageMenuReference.RemoveFromBuildingsMenu;

        antenneData.ConstructBuilding(model);
        TurnAntenneButtonsON();


        ResourceManager.Instance.antenneReference = this;
        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    public void Invoke() // TODO
    {
        UIPannelManager.Instance.ResetPanels("AntenneMenu");

        antenneData.Invoke();
    }

    private void TurnAntenneButtonsON()
    {
        if (!GameHendler.Instance.isAntenneOnceCreated)
        {
            // Roll animation to open special panel with this buttons
            GameHendler.Instance.isAntenneOnceCreated = true;

            GameHendler.Instance.antenneButtonsPanel.SetActive(true);
        }

        GameHendler.Instance.resourceDropButton.interactable = ResourceManager.Instance.IsPowerOn();
        GameHendler.Instance.impusleAttackButton.interactable = ResourceManager.Instance.IsPowerOn();
    }


    public void DestroyBuilding()
    {
        GameHendler.Instance.resourceDropButton.interactable = false;
        GameHendler.Instance.impusleAttackButton.interactable = false;


        antenneData.DestroyBuilding();
        OnAntenneDestroyed(this);


        Destroy(gameObject);
        ResourceManager.Instance.antenneReference = null;
        ResourceManager.Instance.DestroyBuildingAndRescanMap();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == TagConstants.enemyAttackRange)
        {
            Debug.Log("Damage");
            TakeDamage(collider.GetComponent<EnemyAttackRange>().damagePoints);
        }
    }
}
