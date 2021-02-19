using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Garage :  AliveGameUnit, IBuilding
{
    public static GarageMenu garageMenuReference;                    // Reference to UI panel (same field for all Garages)
    
    public static Tile_Type PlacingTileType {get; private set;}      // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}      // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}      // Static field - Specific prefab for creating building
    
    private GameObject tileOccupied = null;            // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null;           // Reference to real MapTile on which building is set
    
    private static int garage_counter = 0;             // For understanding which building number is this    
    public const int garageCapacity = 5;               // Constant field - All garages have same capacity
    
    private Unit unitRef = null;                       // Reference tu some Unit for algorithms
    public List<Unit> garageMembers = new List<Unit>();// Units that are living here

    public GameObject angar;                      // ANgar position (for Unit FSM transitions)

    public bool isMenuOpened = false;

    // Points where unit is chilling at ANGAR
    public GameObject relaxPoint1;
    public GameObject relaxPoint2;
    public GameObject relaxPoint3;
    public GameObject relaxPoint4;
    public GameObject relaxPointCENTER;

    public float timerForCreatingUnit = 0f;
    // public bool isCreationInProgress = false;

    private int _queue = 0;
    public int clicks = 0;
    public int numberOfUnitsToCome = garageCapacity;

    private float _timerStep = 0.5f;

    // // Unit creation logic
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.F))
    //     {
    //         if (gameObject.name == "G1")
    //         {
    //             //TakeDamage(10);
    //             DestroyGarage();
    //         }
    //     }
    //     // UpgradeLogic();
    // }


    IEnumerator UpgradeLogic()
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
            ReloadLogicAfterUnitCreation();

            _queue--;

            if (_queue == 0)
            {
                // Stop courutine
                yield break;
            }
        }
    }


    public override void TakeDamage(float damagePoints)
    {
        HealthPoints -= damagePoints;
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            garageMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadGarageHP_SPAfterDamage(this);
    }

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.garagePrefab;
    }

    // Function for creating building
    public void Creation(Model model)
    {
        HealthPoints = 100;
        ShieldPoints = 100;

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
            gameObject.transform.GetChild(0).tag = TagConstants.garageAngarTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            gameObject.transform.GetChild(0).transform.position = tileOccupied1.transform.position;

            relaxPoint1 = gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
            relaxPoint2 = gameObject.transform.GetChild(0).transform.GetChild(1).gameObject;
            relaxPoint3 = gameObject.transform.GetChild(0).transform.GetChild(2).gameObject;
            relaxPoint4 = gameObject.transform.GetChild(0).transform.GetChild(3).gameObject;
            relaxPointCENTER = gameObject.transform.GetChild(0).transform.GetChild(4).gameObject;

            
            angar = gameObject.transform.GetChild(0).gameObject;
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
        }

        garageMenuReference.ReloadPanel(this);
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

                // Reload unit images because we add new unit
                if (isMenuOpened)
                {
                    garageMenuReference.ReloadPanel(this);
                }
                
                // Check there are still homeless units (decrements above!)
                if (ResourceManager.Instance.homelessUnits.Count == 0)
                {
                    if (GameViewMenu.Instance.CheckForUnitManageMenuOpened())
                    {
                        // Because they become avaliable and it must be shown
                        GameViewMenu.Instance.ReloadMainUnitCount();
                    }

                    return;
                }
            }
        }

        else
        {
            // Debug.Log("No homeless units!");
            return;
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
    }

    // Start process of creation
    public void StartUnitCreation()
    {
        // Timer will call CreateUnit - CreateInGarage - AddFreshUnit (and there is no need for clicks or number... modifying)
        // isCreationInProgress = true; // Bool leaver for starting timer
        _queue++;                     // Increments queue
        numberOfUnitsToCome--;       // Decrease number of incoming homeless units
        clicks++;                    // Clicks increment

        if (_queue == 1)
        {
            // Start courutine
            // Otherwise - no need for starting another - because it is a queue
            StartCoroutine(UpgradeLogic());
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
    }


    private void ReloadLogicAfterUnitCreation()
    {
        if (GameViewMenu.Instance.CheckForUnitManageMenuOpened())
        { 
            // Anyway reload unit counter because it is above all panels and it expands All units list
            GameViewMenu.Instance.ReloadMainUnitCount();
        }

        if (isMenuOpened)
        {
            // No need for reloading name or HP/SP or icon
            garageMenuReference.ReloadUnitManage();
        }
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



    // private void UnitCreationLogic()
    // {
    //     if (queue != 0)
    //     {
    //         if (isCreationInProgress)
    //         {
    //             timerForCreatingUnit += 0.005f;

    //             if (timerForCreatingUnit > 1)
    //             {
    //                 timerForCreatingUnit = 0f;
    //                 queue--;
                    
    //                 if (queue == 0)
    //                 {
    //                     isCreationInProgress = false;
    //                 }

    //                 CreateUnit();


    //                 // Reload all info below
    //                 if (GameViewMenu.Instance.CheckForUnitManageMenuOpened())
    //                 {
    //                     // Anyway reload unit counter because it is above all panels and it expands All units list
    //                     GameViewMenu.Instance.ReloadMainUnitCount();
    //                 }

    //                 if (isMenuOpened)
    //                 {
    //                     // No need for reloading name or HP/SP or icon
    //                     garageMenuReference.ReloadUnitManage();
    //                 }
    //             }
    //         }
    //     }
    // }
