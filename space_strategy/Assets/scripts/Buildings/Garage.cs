using UnityEngine;
using System.Collections.Generic;

public class Garage :  AliveGameUnit, IBuilding
{
    public static GarageMenu garageMenuReference;// Reference to UI panel (same field for all Garages)
    
    public static Tile_Type PlacingTileType {get; private set;}      // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}      // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}      // Static field - Specific prefab for creating building

    public static GameObject UnitPrefab {get; private set;}          // Static field - Unit prefab
    
    private static int garage_counter = 0;        // For understanding which building number is this    
    public const int garageCapacity = 5;          // Constant field - All garages have same capacity
    
    private GameObject tileOccupied = null;       // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null;      // Reference to real MapTile on which building is set

    private Unit unitRef = null;                  // Reference tu some Unit for algorithms
    public List<Unit> garageMembers;              // Units that are living here
    
    public Vector3 angarPosition;                 // ANgar position (for Unit FSM transitions)

    public bool isMenuOpened = false;






    private void Update() // TEST ONLY
    {
        // if (Input.GetKeyDown(KeyCode.V))
        // {
        //     DestroyGarage();
        // }
    }

    public override void TakeDamage(float damagePoints)
    {
        base.TakeDamage(damagePoints);
        HealthPoints -= damagePoints;

        if (isMenuOpened)
        {
            garageMenuReference.ReloadPanel(this);
        }
    }

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.DoubleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.garagePrefab;

        UnitPrefab = PrefabManager.Instance.unitPrefab;
    }

    // Function for creating building
    public void Creation(Model model)
    {
        HealthPoints = 100;
        ShieldPoints = 100;

        ResourceManager.Instance.garagesList.Add(this);

        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;

        garage_counter++;

        this.gameObject.name = "Garage" + Garage.garage_counter;

        garageMembers = new List<Unit>();
        HelperObjectInit();
        AddHomelessUnit();
    }

    // Initializing helper GameObject - Angar or throwing ERROR if it is impossible
    private void HelperObjectInit()                     
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.garageAngarTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);

            gameObject.transform.GetChild(0).transform.position = tileOccupied1.transform.position;
            
            angarPosition = gameObject.transform.GetChild(0).transform.position;
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
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






#region Garage logic funsctions
    








    public void CreateUnit() // No nned for slider reload because they became free from work and they are nit involved in shaft process
    {
        Unit unit = Instantiate(UnitPrefab, angarPosition, Quaternion.identity).GetComponent<Unit>();
        
        unit.CreateInGarage(this);

        Debug.Log("UnitCreated!");



        
        /////////////////////////////////////////////////////////////////////////////////////////////////


        // Reload Unit Manage Menu - text box with Unit info


        /////////////////////////////////////////////////////////////////////////////////////////////////







    }


    public void AddHomelessUnit() // No need for slider reload because they became free from work and they are nit involved in shaft process
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


                Debug.Log("Added homeless unit!");
                

                if (ResourceManager.Instance.homelessUnits.Count == 0)
                {

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////



                    // Reload Unit manage menu  - unit text Box vecause some became avaliable




                    //////////////////////////////////////////////////////////////////////////////////////////////////////////

                    
                    return;
                }
            }
        }
        else
        {
            Debug.Log("No homeless units!");
            return;
        }
    }







    public void RemoveUnit(Unit deadUnit) // Formal function
    {
        deadUnit.home = null;
        garageMembers.Remove(deadUnit);
    }









    public void DestroyGarage() // Reload slider here because some units from garage can be on work
    {
        foreach (var unit in garageMembers)
        {
            if (unit.workPlace)
            {
                if (unit.workPlace.isMenuOpened)
                {
                    unit.workPlace.RemoveUnit(unit);
                    MineShaft.shaftMenuReference.ReloadUnitSlider();
                }
                else
                {
                    switch(unit.workPlace.type)
                    {
                        case 1:
                        if (GameHendler.Instance.isMenuCrystalTabOpened)
                        {
                            MineShaft temp = unit.workPlace;
                            unit.workPlace.RemoveUnit(unit);
                            GameHendler.Instance.unitManageMenuReference.crystalScrollConten.FindSLiderAndReload(temp);
                        }
                        break;

                        case 2:
                        if (GameHendler.Instance.isMenuIronTabOpened)
                        {
                            MineShaft temp = unit.workPlace;
                            unit.workPlace.RemoveUnit(unit); 
                            GameHendler.Instance.unitManageMenuReference.ironScrollConten.FindSLiderAndReload(temp);
                        }
                        break;

                        case 3:
                        if (GameHendler.Instance.isMenuGelTabOpened)
                        {
                            MineShaft temp = unit.workPlace;
                            unit.workPlace.RemoveUnit(unit); 
                            GameHendler.Instance.unitManageMenuReference.gelScrollConten.FindSLiderAndReload(temp);
                        }
                        break;
                    }
                }
                //ReloadMenuSlider(); // if he was working and if menu is opened than reload because of destruction
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






        /////////////////////////////////////////////////////////////////////////////////////////////////

        // RELOAD main menu text box with units - because some became homeless
        // Reload Unit Manage Menu SLIDERS because unit can be workers 
        ReloadMenuSlider();




        if (GameHendler.Instance.isBaseMenuOpened) // Reload base menu button if garage destroys while menu is opened
        {
            GameHendler.Instance.baseMenuReference.ReloadButtonManager();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////








        
        Destroy(gameObject);
    }




















    // Find out which type of shaft it is and reload that Slider
    public void ReloadMenuSlider()
    {
        if (GameHendler.Instance.isMenuAllResourcesTabOpened) // FIX!!!!!!! Problem is we dont know if this unit was working or where he was working
        {
            GameHendler.Instance.unitManageMenuReference.ReloadCrystalSlider();   
            GameHendler.Instance.unitManageMenuReference.ReloadGelSlider();
            GameHendler.Instance.unitManageMenuReference.ReloadIronSlider();
        }
    }






#endregion

}