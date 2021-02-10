using UnityEngine;
using System.Collections.Generic;

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

    public Vector3 angarPosition;                      // ANgar position (for Unit FSM transitions)

    public bool isMenuOpened = false;

    public GameObject relaxPoint1;
    public GameObject relaxPoint2;
    public GameObject relaxPoint3;
    public GameObject relaxPoint4;
    public GameObject relaxPointCENTER;








    public float timerForCreatingUnit = 0f;
    public bool isCreationInProgress = false;

    public int queue = 0;
    public int clicks = 0;

    // Unit creation logic
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.name == "G1")
            {
                //TakeDamage(10);
                DestroyGarage();
            }
        }

        UnitCreationLogic();
    }

    private void UnitCreationLogic()
    {
        if (queue != 0)
        {
            if (isCreationInProgress)
            {
                timerForCreatingUnit += 0.0025f;

                if (timerForCreatingUnit > 1)
                {
                    timerForCreatingUnit = 0f;
                    queue--;
                    
                    if (queue == 0)
                    {
                        isCreationInProgress = false;
                    }

                    CreateUnit();

                    Debug.Log("Unit created!");

                    // Reload all info below
                    if (GameHendler.Instance.isUnitManageMenuOpened)
                    {
                        // Anyway reload unit counter because it is above all panels and it expands All units list
                        GameHendler.Instance.unitManageMenuReference.ReloadMainUnitCount();
                    }

                    if (isMenuOpened)
                    {
                        // No need for reloading name or HP/SP or icon
                        garageMenuReference.ReloadUnitManage();
                    }
                }
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
        if (GameHendler.Instance.isBuildingsMAnageMenuOpened)
        {
            // Drop some code here
            if (GameHendler.Instance.isIndustrialBuildingsMenuOpened)
            {
                GameHendler.Instance.buildingsManageMenuReference.ReloadGarageHPSP(this);
            }
        }
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

            
            angarPosition = gameObject.transform.GetChild(0).transform.position;
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

    private void CreateUnit()
    {
        Unit unit = Instantiate(Unit.unitPrefab, transform.position, Quaternion.identity).GetComponent<Unit>();
        unit.CreateInGarage(this);
    }

  
    // No nned for slider reload because they became free from work and they are nit involved in shaft process
    public void CreateUnitButton()
    {
        isCreationInProgress = true;
        queue++;
    }

    // No need for slider reload because they became free from work and they are not involved in shaft process
    public void AddHomelessUnit()
    {
        if (ResourceManager.Instance.homelessUnits.Count != 0)
        {
            for (int i = 0; i < garageCapacity; i++)
            {
                unitRef = ResourceManager.Instance.homelessUnits[(ResourceManager.Instance.homelessUnits.Count)-1];

                unitRef.home = this;
                garageMembers.Add(unitRef);
                ResourceManager.Instance.homelessUnits.Remove(unitRef);
                ResourceManager.Instance.avaliableUnits.Add(unitRef);

                // Debug.Log("Added homeless unit!");
                
                if (ResourceManager.Instance.homelessUnits.Count == 0)
                {
                    if (GameHendler.Instance.isUnitManageMenuOpened)
                    {
                        // Because they become avaliable and it must be shown
                        GameHendler.Instance.unitManageMenuReference.ReloadMainUnitCount();
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
    }

    // Reload slider here because some units from garage can be on work
    public void DestroyGarage()
    {
        List<MineShaft> shaftsToReloadSliders = new List<MineShaft>();

        foreach (var unit in garageMembers)
        {
            if (unit.workPlace)
            {
                // If garage destroys but unit which was garage member was working and its shaft menu was open
                if (unit.workPlace.isMenuOpened)
                {
                    unit.workPlace.RemoveUnit(unit);
                    MineShaft.shaftMenuReference.ReloadUnitSlider(); // _myShaft reference is already initilized and reloading will be correct
                }
                else // if unit was working but garage destroys
                {
                    shaftsToReloadSliders.Add(unit.workPlace);
                    unit.workPlace.RemoveUnit(unit);
                }
            }
            
            else
            {
                ResourceManager.Instance.avaliableUnits.Remove(unit);
            }

            unit.home = null;
            ResourceManager.Instance.homelessUnits.Add(unit);
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


    // Find out which type of shaft it is and reload that Slider
    private void ReloadMenuSlider()
    {
        GameHendler.Instance.unitManageMenuReference.ReloadCrystalSlider();   
        GameHendler.Instance.unitManageMenuReference.ReloadGelSlider();
        GameHendler.Instance.unitManageMenuReference.ReloadIronSlider();
    }



    private void ReloadUnitManageMenuInfo(List<MineShaft> shaftsToReloadSliders)
    {
        if (GameHendler.Instance.isUnitManageMenuOpened)
        {
            // Always need to reload because some units may be working on shafts
            GameHendler.Instance.unitManageMenuReference.ReloadMainUnitCount();
 
            if (GameHendler.Instance.isMenuAllResourcesTabOpened)
            {
                ReloadMenuSlider();
            }

            else
            {
                if (shaftsToReloadSliders.Count != 0)
                {
                    for (int i = 0; i < shaftsToReloadSliders.Count; i ++)
                    {
                        GameHendler.Instance.unitManageMenuReference.FindSLiderAndReload(shaftsToReloadSliders[i]);
                    }
                }
            }
        }
    }

    private void ReloadBuildingsManageMenuInfo()
    {
        if (GameHendler.Instance.isBuildingsMAnageMenuOpened)
        {
            if (GameHendler.Instance.isIndustrialBuildingsMenuOpened)
            {
                // Drop some code here
                GameHendler.Instance.buildingsManageMenuReference.RemoveGarageFromBuildingsMenu(this.name);
            }
        }
    }

}