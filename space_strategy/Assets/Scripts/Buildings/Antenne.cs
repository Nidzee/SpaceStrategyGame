//////////////////////////////////////////////////////////////////////////////




// All functionality is int GameHendler because we can create only 1 instance of Antenne
// But when Antenne dies we dont want to reset buttons timers 




//////////////////////////////////////////////////////////////////////////////

public class Antenne :  AliveGameUnit, IBuilding
{
    public GameUnit gameUnit;
    public AntenneData antenneData;
    public AntenneSavingData antenneSavingData;

    public delegate void DamageTaken(GameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void AntenneDestroy(GameUnit gameUnit);
    public event AntenneDestroy OnAntenneDestroyed = delegate{};


    public void InitStatsAfterBaseUpgrade()
    {
        gameUnit.UpgradeStats(
        StatsManager._maxHealth_Antenne + StatsManager._baseUpgradeStep_Antenne, 
        StatsManager._maxShiled_Antenne + StatsManager._baseUpgradeStep_Antenne, 
        StatsManager._maxDefensePoints_Antenne);

        OnDamageTaken(gameUnit);
    }



    public override void TakeDamage(int damagePoints)
    {
        if (!gameUnit.TakeDamage(damagePoints))
        {
            DestroyBuilding();
            return;
        }

        OnDamageTaken(gameUnit);
    }

    public void ConstructBuilding(Model model)
    {
        gameUnit = new GameUnit(StatsManager._maxHealth_Antenne, StatsManager._maxShiled_Antenne, StatsManager._maxDefensePoints_Antenne);
        antenneData = new AntenneData(this);
        antenneSavingData = new AntenneSavingData();


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
        OnAntenneDestroyed(gameUnit);


        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        ResourceManager.Instance.antenneReference = null;
        AstarPath.active.Scan();
    }

    // private void ReloadBuildingsManageMenuInfo()
    // {
    //     GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___AfterAntenneDestroying();
    // }
}
