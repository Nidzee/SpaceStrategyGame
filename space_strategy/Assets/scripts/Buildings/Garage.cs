using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Garage :  AliveGameUnit, IBuilding
{
    public static GarageMenu garageMenuReference;                    // Reference to UI panel (same field for all Garages)
    private static int garage_counter = 0;                           // For understanding which building number is this    
    
    public static Tile_Type PlacingTileType {get; private set;}      // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}      // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}      // Static field - Specific prefab for creating building
    
    private GameObject tileOccupied = null;                          // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null;                         // Reference to real MapTile on which building is set
    private const int garageCapacity = 5;                            // Constant field - All garages have same capacity
    
    private Unit unitRef = null;                                     // Reference tu some Unit for algorithms
    public List<Unit> garageMembers = new List<Unit>();              // Units that are living here
    public GameObject angar;                                         // ANgar position (for Unit FSM transitions)
    public bool isMenuOpened = false;

    // Points where unit is chilling at ANGAR
    public GameObject relaxPoint1;
    public GameObject relaxPoint2;
    public GameObject relaxPoint3;
    public GameObject relaxPoint4;
    public GameObject relaxPointCENTER;

    private float timerForCreatingUnit = 0f;
    private int _queue = 0;
    public int clicks = 0;
    public int numberOfUnitsToCome = garageCapacity;

    private static float _timerStep = 0.5f;

    private static int _crystalNeedForBuilding;
    private static int _ironNeedForBuilding;
    private static int _gelNeedForBuilding;
    private static int _crystalNeedForUnitCreation;
    private static int _ironNeedForForUnitCreation;
    private static int _gelNeedForForUnitCreation;

    private static int _maxHealth; 
    private static int _maxShiled; 
    private static int _maxDefensePoints; 

    private static int _baseUpgradeStep;




    // Initialize only once
    public static string GetResourcesNeedToBuildAsText()
    {
        return _crystalNeedForBuilding.ToString() + " " + _ironNeedForBuilding.ToString() +" "+_gelNeedForBuilding.ToString();
    }
    
    public static void InitUnitCreationCost() // Initializing only once
    {
        garageMenuReference.InitUnitCostButton(_crystalNeedForUnitCreation, _ironNeedForForUnitCreation, _gelNeedForForUnitCreation);
    }






    public static void GetResourcesNeedToBuild(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        crystalNeed = _crystalNeedForBuilding;
        ironNeed = _ironNeedForBuilding;
        gelNeed = _gelNeedForBuilding;
    }

    public static void GetResourcesNeedToCreateUnit(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        crystalNeed = _crystalNeedForUnitCreation;
        ironNeed = _ironNeedForForUnitCreation;
        gelNeed = _gelNeedForForUnitCreation;
    }

    public static void UpgradeStatisticsAfterBaseUpgrade()
    {
        _maxHealth += _baseUpgradeStep;
        _maxShiled += _baseUpgradeStep;
    }







    private void InitStatsAfterCreation()
    {
        healthPoints = _maxHealth;
        maxCurrentHealthPoints = _maxHealth;

        shieldPoints = _maxShiled;
        maxCurrentShieldPoints = _maxShiled;

        deffencePoints = _maxDefensePoints;
    }

    public void InitStatsAfterBaseUpgrade()
    {
        RecalculateStats();

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            garageMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP/SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadGarageHPSP(this);
    }

    private void RecalculateStats()
    {
        healthPoints = ((_maxHealth + _baseUpgradeStep) * healthPoints) / _maxHealth;
        maxCurrentHealthPoints = (_maxHealth + _baseUpgradeStep);

        shieldPoints = ((_maxShiled + _baseUpgradeStep) * shieldPoints) / _maxShiled;
        maxCurrentShieldPoints = (_maxShiled + _baseUpgradeStep);

        deffencePoints = _maxDefensePoints; // not changing at all
    }


    IEnumerator CreateUnitLogic()
    {
        while (true)
        {
            while (timerForCreatingUnit < 1)
            {
                timerForCreatingUnit += _timerStep * Time.deltaTime;

                if (isMenuOpened)
                {
                    garageMenuReference.loadingBar.fillAmount = timerForCreatingUnit;
                }

                yield return null;
            }

            timerForCreatingUnit = 0f;
            CreateUnit();

            _queue--;
            if (_queue == 0)
            {
                yield break;
            }
        }
    }


    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.garagePrefab;

        _maxHealth = 120; 
        _maxShiled = 100; 
        _maxDefensePoints = 10; 

        _crystalNeedForUnitCreation = 5;
        _ironNeedForForUnitCreation = 5;
        _gelNeedForForUnitCreation = 5;

        _crystalNeedForBuilding = 10;
        _ironNeedForBuilding = 10;
        _gelNeedForBuilding = 10;

        _baseUpgradeStep = 25;
    }

    // Function for creating building
    public void Creation(Model model)
    {
        InitStatsAfterCreation();

        garage_counter++;
        this.gameObject.name = "G" + Garage.garage_counter;
        ResourceManager.Instance.garagesList.Add(this);

        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;


        HelperObjectInit();
        AddHomelessUnit();

        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    // Initializing helper GameObject - Angar or throwing ERROR if it is impossible
    private void HelperObjectInit()                     
    {
        if (gameObject.transform.childCount != 0)
        {
            angar = gameObject.transform.GetChild(0).gameObject;

            angar.tag = TagConstants.garageAngarTag;
            angar.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            angar.transform.position = tileOccupied1.transform.position;

            relaxPoint1 = angar.transform.GetChild(0).gameObject;
            relaxPoint2 = angar.transform.GetChild(1).gameObject;
            relaxPoint3 = angar.transform.GetChild(2).gameObject;
            relaxPoint4 = angar.transform.GetChild(3).gameObject;
            relaxPointCENTER = angar.transform.GetChild(4).gameObject;            
        }
        else
        {
            Debug.LogError("ERROR!      No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }

    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("GarageMenu");
        
        if (!garageMenuReference) // executes once
        {
            garageMenuReference = GameObject.Find("GarageMenu").GetComponent<GarageMenu>();

            InitUnitCreationCost();
        }

        garageMenuReference.ReloadPanel(this);
    }

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints < 0)
        {
            DestroyGarage();
            return;
        }

        UpdateUI();
    }





    // No need for slider reload because they became free from work and they are not involved in shaft process
    public void AddHomelessUnit()
    {
        if (ResourceManager.Instance.homelessUnits.Count != 0)
        {
            for (int i = 0; i < garageCapacity; i++)
            {
                unitRef = ResourceManager.Instance.homelessUnits[(ResourceManager.Instance.homelessUnits.Count)-1];

                AddUnit(unitRef);

                ResourceManager.Instance.homelessUnits.Remove(unitRef);
                ResourceManager.Instance.avaliableUnits.Add(unitRef);

                // Check there are still homeless units (decrements above!)
                if (ResourceManager.Instance.homelessUnits.Count == 0)
                {
                    // Reload unit images because we add new unit
                    if (isMenuOpened)
                    {
                        garageMenuReference.ReloadPanel(this);
                    }

                    GameViewMenu.Instance.ReloadMainUnitCount();

                    return;
                }
            }
        }
    }

    // Formal function
    public void RemoveUnit(Unit deadUnit)
    {
        deadUnit.home = null;
        garageMembers.Remove(deadUnit);

        clicks--;
        numberOfUnitsToCome++;
    }

    // Using with homeless units adding
    public void AddUnit(Unit newUnit)
    {
        newUnit.home = this;
        garageMembers.Add(newUnit);
        
        clicks++;
        numberOfUnitsToCome--;
    }

    // Using with unit creation
    public void AddFreshUnit(Unit newUnit)
    {
        newUnit.home = this;
        garageMembers.Add(newUnit);
    }

    // Creating unit
    private void CreateUnit()
    {
        Unit unit = Instantiate(Unit.unitPrefab, transform.position, Quaternion.identity).GetComponent<Unit>();
        unit.CreateInGarage(this);

        if (isMenuOpened)
        {
            garageMenuReference.ReloadUnitManage();
        }

        GameViewMenu.Instance.ReloadMainUnitCount();
    }

    // Start process of creation
    public void StartUnitCreation()
    {
        _queue++;                    // Increments queue
        numberOfUnitsToCome--;       // Decrease number of incoming homeless units
        clicks++;                    // Clicks increment

        if (_queue == 1)
        {
            // Start courutine
            // Otherwise - no need for starting another - because it is a queue
            StartCoroutine(CreateUnitLogic());
        }
    }







    // Reload slider here because some units from garage can be on work
    public void DestroyGarage()
    {
        List<MineShaft> shaftsToReloadSliders = new List<MineShaft>();

        foreach (var unit in garageMembers)
        {
            // If we found new home at run-time - dont delete work
            bool temp = ResourceManager.Instance.SetNewHomeForUnitFromDestroyedGarage(unit, this);

            if (temp)
            {
                // Home is changed
                // WorkPlace is still the same
            }
            else
            {
                // Delete home
                // If he was working - delete work
                unit.home = null;

                if (unit.workPlace)
                {
                    MineShaft sameWorkplace = unit.workPlace;
                    unit.workPlace.RemoveUnit(unit);
                    ResourceManager.Instance.homelessUnits.Add(unit);

                    // If garage destroys but unit which was garage member was working and its shaft menu was open
                    if (sameWorkplace.isMenuOpened)
                    {
                        MineShaft.shaftMenuReference.ReloadUnitSlider();
                    }
                    else
                    {
                        shaftsToReloadSliders.Add(sameWorkplace);
                    }
                }
                else
                {
                    ResourceManager.Instance.avaliableUnits.Remove(unit);
                    ResourceManager.Instance.homelessUnits.Add(unit);
                }
            }
        }
        garageMembers.Clear();
        ResourceManager.Instance.garagesList.Remove(this);
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;


        if (isMenuOpened)
        {
            garageMenuReference.ExitMenu();
        }

        ReloadUnitManageMenuInfo(shaftsToReloadSliders);
        ReloadBuildingsManageMenuInfo();
        
        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        AstarPath.active.Scan();
    }

    private void ReloadUnitManageMenuInfo(List<MineShaft> shaftsToReloadSliders)
    {
        GameViewMenu.Instance.ReloadUnitManageMenuInfoAfterGarageDestroying(shaftsToReloadSliders);
    }

    private void ReloadBuildingsManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___AfterGarageDestroying(this);
    }
}