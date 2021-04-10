using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pathfinding;



public class GameHendler : MonoBehaviour
{
    public static GameHendler Instance {get; private set;}

    #region State machine 
        public ZoomState zoomState = new ZoomState();
        public IdleState idleState = new IdleState();
        public SelectTileState selectTileState = new SelectTileState();
        public CameraMovementState cameraMovementState = new CameraMovementState();
        public BuildingSelectionState buildingSelectionState = new BuildingSelectionState();

        public BM_ZoomState BM_zoomState = new BM_ZoomState();
        public BM_IdleState BM_idleState = new BM_IdleState();
        public BM_CameraMovementState BM_cameraMovementState = new BM_CameraMovementState();
        public BM_BuildingMovementState BM_buildingMovementState = new BM_BuildingMovementState();

        public ITouchState currentState;
    #endregion

    #region Mouse and camer variables
        public GameObject redPoint; // Point for Ray Casting and finding Current Hex
        public Vector3 worldMousePosition;
        public Vector3 touchStart;
    #endregion

    #region Temp variableas and fields for DEBUG
        public Cube c = new Cube(0,0,0);// TEMP for calculating
        public Color hexColor;          // Temp
    #endregion
    

    public LayerMask idelLayerMask;           // Layer mask for idle mode
    public LayerMask BMidelLayerMask;         // Layer mask for building mode

    public GameObject CurrentHex;             // Always Hex under mouse
    public GameObject SelectedHex;            // Selected Hex at the moment
    
    public Model buildingModel = new Model(); // Building model
    public GameObject selctedBuilding = null; // Building model



    public void SaveGame()
    {

    }

    public void LoadGame()
    {
        // Init Shtab reference


        // Load all resources
        // Load electricity

        // Clear all Lists

        // 1 - Construct all buildings
        // 2 - Create all units
        // 3 - Add relations

        // 4 - Create all Enemies
        // 5 - Build pathes for enemies


        // Electricity level check - because turrets can start opening fire
        
        // Rescan map 
    }



    
























    











































    public AntenneMenu antenneMenuReference;
    public bool isAntenneOnceCreated = false;
    public GameObject antenneButtonsPanel;

    public void TurnAntenneButtonsToUnavaliable()
    {
        resourceDropButton.interactable = false;
        impusleAttackButton.interactable = false;

        antenneMenuReference.ReloadButtonManage();
    }

    public void TurnAntenneButtonsBackToLife()
    {
        if (resourceDropTimer == 0)
        {
            resourceDropButton.interactable = true;
        }
        if (impulsAttackTimer == 0)
        {
            impusleAttackButton.interactable = true;
        }
        
        antenneMenuReference.ReloadButtonManage();
    }






    public float resourceDropTimer;
    public float impulsAttackTimer;

    public Button resourceDropButton;
    public Button impusleAttackButton;

    private bool isResourceDropReady = true;
    private bool isImpusleAttackReady = true;

    [SerializeField] private Image resourceDropProgressImage;
    [SerializeField] private Image impulseAttackProgressImage;

    private float _timerStep = 0.5f;

    IEnumerator ResourceDropTimerMaintaining()
    {
        while (resourceDropTimer < 1)
        {
            resourceDropTimer += _timerStep * Time.deltaTime;
            antenneMenuReference.resourceDropProgressImage.fillAmount = resourceDropTimer;

            resourceDropProgressImage.fillAmount = resourceDropTimer;

            yield return null;
        }

        resourceDropTimer = 0f;
        isResourceDropReady = true;

        if (ResourceManager.Instance.antenneReference)
        {
            if (ResourceManager.Instance.IsPowerOn())
            {
                resourceDropButton.interactable = true;
            }

            if (ResourceManager.Instance.antenneReference.isMenuOpened)
            {
                AntenneStaticData.antenneMenuReference.ReloadButtonManage();
            }
        }
    }

    IEnumerator ImpulseAttackTimerMaintaining()
    {
        while (impulsAttackTimer < 1)
        {
            impulsAttackTimer += _timerStep * Time.deltaTime;
            antenneMenuReference.impulseAttackProgressImage.fillAmount = impulsAttackTimer;

            impulseAttackProgressImage.fillAmount = impulsAttackTimer;

            yield return null;
        }

        impulsAttackTimer = 0f;
        isImpusleAttackReady = true;

        if (ResourceManager.Instance.antenneReference)
        {
            if (ResourceManager.Instance.IsPowerOn())
            {
                impusleAttackButton.interactable = true;
            }

            if (ResourceManager.Instance.antenneReference.isMenuOpened)
            {
                AntenneStaticData.antenneMenuReference.ReloadButtonManage();
            }
        }
    }




    GameObject tempGarage = null;
    GameObject tempShaft = null;
    GameObject tempUnit = null;
    GameObject tempTurret = null;
    GameObject tempSG = null;
    GameObject tempAntenne = null;
    GameObject tempPowerPlant = null;
    GameObject tempShatb = null;
    public GarageSavingData garageData = new GarageSavingData();
    public UnitSavingData unitSavingData = new UnitSavingData();
    public MineShaftSavingData mineShaftSavingData = new MineShaftSavingData();
    public TurretSavingData turretSavingData = new TurretSavingData();
    public ShieldGeneratorSavingData shieldGeneratorSavingData = new ShieldGeneratorSavingData();
    public AntenneSavingData antenneSavingData = new AntenneSavingData();
    public AntenneLogicSavingData antenneLogicSavingData = new AntenneLogicSavingData();
    public PowerPlantSavingData powerPlantSavingData = new PowerPlantSavingData();
    public ShtabSavingData shtabSavingData = new ShtabSavingData();

    public SaveData saveData = new SaveData();

    private void Update()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        redPoint.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, worldMousePosition.z + 90);
        
        currentState = currentState.DoState();

        
        if (Input.GetKeyDown(KeyCode.S))
        {
            saveData = new SaveData();

            saveData.crystalResourceCount = ResourceManager.Instance.resourceCrystalCount; // Modify here to change start resource count
            saveData.ironResourceCount = ResourceManager.Instance.resourceIronCount;   // Modify here to change start resource count
            saveData.gelResourceCount = ResourceManager.Instance.resourceGelCount;     // Modify here to change start resource count

            
            saveData.electricity = ResourceManager.Instance.electricityCount;
            saveData.electricity_max = (int)GameViewMenu.Instance.wholeElectricitySlider.maxValue;

            saveData.electricityNeed = ResourceManager.Instance.electricityNeedCount;
            saveData.electricityNeed_max = (int)GameViewMenu.Instance.usingElectricitySlider.maxValue;

            saveData.IsPowerOn = ResourceManager.Instance.isPowerOn;
            saveData.isAntenneOnceCreated = isAntenneOnceCreated;



            antenneLogicSavingData = new AntenneLogicSavingData();

            antenneLogicSavingData.timerBash = impulsAttackTimer;
            antenneLogicSavingData.timerResourceDrop = resourceDropTimer;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResourceManager.Instance.LoadFromFile(saveData);

















            // tempGarage = GameObject.Instantiate(
            // GarageStaticData.BuildingPrefab, 
            // GameObject.Find(garageData._tileOccupied_name).transform.position + OffsetConstants.buildingOffset, 
            // Quaternion.Euler(0f, 0f, (garageData.rotation*60)));
            
            // tempGarage.tag = TagConstants.buildingTag;
            // tempGarage.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            // tempGarage.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
            
            // tempGarage.GetComponent<Garage>().ConstructBuildingFromFile(garageData);













            // CrystalShaft crystalShaft = null;
            // IronShaft ironShaft = null;
            // GelShaft gelShaft = null;
            
            // switch (mineShaftSavingData.type)
            // {
            //     case 1: // Crystal
            //     crystalShaft = Instantiate(
            //     CSStaticData.BuildingPrefab, 
            //     GameObject.Find(mineShaftSavingData._tileOccupiedName).transform.position + OffsetConstants.buildingOffset, 
            //     Quaternion.Euler(0f, 0f, (mineShaftSavingData.rotation*60))).GetComponent<CrystalShaft>();

            //     crystalShaft.GetComponent<MineShaft>().ConstructBuildingFromFile(mineShaftSavingData);


            //     tempShaft = crystalShaft.gameObject;
            //     break;

            //     case 2: // Iron
            //     ironShaft = GameObject.Instantiate(
            //     ISStaticData.BuildingPrefab, 
            //     GameObject.Find(mineShaftSavingData._tileOccupiedName).transform.position + OffsetConstants.buildingOffset, 
            //     Quaternion.Euler(0f, 0f, (mineShaftSavingData.rotation*60))).GetComponent<IronShaft>();

            //     ironShaft.GetComponent<MineShaft>().ConstructBuildingFromFile(mineShaftSavingData);


            //     tempShaft = ironShaft.gameObject;
            //     break;

            //     case 3: // Gel
            //     gelShaft = GameObject.Instantiate(
            //     GSStaticData.BuildingPrefab, 
            //     GameObject.Find(mineShaftSavingData._tileOccupiedName).transform.position + OffsetConstants.buildingOffset, 
            //     Quaternion.Euler(0f, 0f, (mineShaftSavingData.rotation*60))).GetComponent<GelShaft>();

            //     gelShaft.GetComponent<MineShaft>().ConstructBuildingFromFile(mineShaftSavingData);


            //     tempShaft = gelShaft.gameObject;
            //     break;
            // }

            // tempShaft.tag = TagConstants.buildingTag;
            // tempShaft.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            // tempShaft.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;











            // tempUnit = GameObject.Instantiate(
            // UnitStaticData.unitPrefab, 
            // new Vector3(unitSavingData.position_x, unitSavingData.position_y, unitSavingData.position_z) + OffsetConstants.buildingOffset, 
            // Quaternion.Euler(0f, 0f, 0f));
            // tempUnit.GetComponent<Unit>().CreateUnitFromFile(unitSavingData);









            // tempShaft.GetComponent<MineShaft>().CreateRelations();
            // tempGarage.GetComponent<Garage>().CreateRelations();












            // switch(unitSavingData.currentState_ID)
            // {
            //     case (int)UnitStates.UnitIdleState:
            //     tempUnit.GetComponent<Unit>().currentState = tempUnit.GetComponent<Unit>().unitIdleState;
            //     break;

            //     case (int)UnitStates.UnitIGoToState:
            //     tempUnit.GetComponent<Unit>().currentState = tempUnit.GetComponent<Unit>().unitIGoToState;

            //     tempUnit.GetComponent<Unit>().RebuildPath();

            //     break;

            //     case (int)UnitStates.UnitIGatherState:
            //     tempUnit.GetComponent<Unit>().currentState = tempUnit.GetComponent<Unit>().unitIGatherState;
            //     break;

            //     case (int)UnitStates.UnitResourceLeavingState:
            //     tempUnit.GetComponent<Unit>().currentState = tempUnit.GetComponent<Unit>().unitResourceLeavingState;
            //     break;

            //     case (int)UnitStates.UnitIHomelessState:
            //     tempUnit.GetComponent<Unit>().currentState = tempUnit.GetComponent<Unit>().unitIHomelessState;
            //     break;
            // }





















            // GameObject turretTile = GameObject.Find(turretSavingData.positionAndOccupationTileName);

            // switch (turretSavingData.type)
            // {
            //     case 1:
            //     switch(turretSavingData.level)
            //     {
            //         case 1:   
            //         tempTurret = GameObject.Instantiate(
            //             PrefabManager.Instance.singleTuretteLaserPrefab, 
            //             turretTile.transform.position + OffsetConstants.buildingOffset, 
            //             Quaternion.Euler(0f, 0f, 60 * turretSavingData.rotation_building));

            //         tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavingData);
            //         tempTurret.GetComponent<TurretLaserSingle>().ConstructBuildingFromFile_LaserSingle();
            //         break;

            //         case 2:     
            //         tempTurret = GameObject.Instantiate(
            //             PrefabManager.Instance.doubleTuretteLaserPrefab, 
            //             turretTile.transform.position + OffsetConstants.buildingOffset, 
            //             Quaternion.Euler(0f, 0f, 60 * turretSavingData.rotation_building));

            //         tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavingData);
            //         tempTurret.GetComponent<TurretLaserDouble>().ConstructBuildingFromFile_LaserDouble();
            //         break;

            //         case 3:    
            //         tempTurret = GameObject.Instantiate(
            //             PrefabManager.Instance.tripleTuretteLaserPrefab, 
            //             turretTile.transform.position + OffsetConstants.buildingOffset, 
            //             Quaternion.Euler(0f, 0f, 60 * turretSavingData.rotation_building));

            //         tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavingData);
            //         tempTurret.GetComponent<TurretLaserTriple>().ConstructBuildingFromFile_LaserTriple();
            //         break;
            //     }
            //     break;


            //     case 2:
            //     switch(turretSavingData.level)
            //     {
            //         case 1:   
            //         tempTurret = GameObject.Instantiate(
            //             PrefabManager.Instance.singleturetteMisilePrefab,
            //             turretTile.transform.position + OffsetConstants.buildingOffset,
            //             Quaternion.Euler(0f, 0f, 60 * turretSavingData.rotation_building));

            //         tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavingData);
            //         tempTurret.GetComponent<TurretMisileSingle>().ConstructBuildingFromFile_MisileSingle();
            //         break;

            //         case 2:     
            //         tempTurret = GameObject.Instantiate(
            //             PrefabManager.Instance.doubleturetteMisilePrefab, 
            //             turretTile.transform.position + OffsetConstants.buildingOffset, 
            //             Quaternion.Euler(0f, 0f, 60 * turretSavingData.rotation_building));

            //         tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavingData);
            //         tempTurret.GetComponent<TurretMisileDouble>().ConstructBuildingFromFile_MisileDouble();
            //         break;

            //         case 3:    
            //         tempTurret = GameObject.Instantiate(
            //             PrefabManager.Instance.truipleturetteMisilePrefab, 
            //             turretTile.transform.position + OffsetConstants.buildingOffset, 
            //             Quaternion.Euler(0f, 0f, 60 * turretSavingData.rotation_building));

            //         tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavingData);
            //         tempTurret.GetComponent<TurretMisileTriple>().ConstructBuildingFromFile_MisileTriple();
            //         break;
            //     }
            //     break;
            // }

















            // GameObject sgPlacingTile = GameObject.Find(shieldGeneratorSavingData._tileOccupied_name);

            // tempSG = GameObject.Instantiate(
            //     PrefabManager.Instance.shieldGeneratorPrefab, 
            //     sgPlacingTile.transform.position + OffsetConstants.buildingOffset, 
            //     Quaternion.Euler(0f, 0f, 60 * shieldGeneratorSavingData.rotation));

                
            // tempSG.tag = TagConstants.buildingTag;
            // tempSG.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            // tempSG.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;


            // tempSG.GetComponent<ShieldGenerator>().CreateFromFile(shieldGeneratorSavingData);














            // tempAntenne = GameObject.Instantiate(
            //     PrefabManager.Instance.antennePrefab, 
            //     GameObject.Find(antenneSavingData._tileOccupied_name).transform.position + OffsetConstants.buildingOffset, 
            //     Quaternion.Euler(0f, 0f, 60 * antenneSavingData.rotation));

            // tempAntenne.GetComponent<Antenne>().CreateFromFile(antenneSavingData);
        

            // // If antenne existed at least once
            // if (isAntenneOnceCreated)
            // {
            //     antenneButtonsPanel.SetActive(true);
            // }

            // resourceDropTimer = antenneLogicSavingData.timerResourceDrop;
            // impulsAttackTimer = antenneLogicSavingData.timerBash;

            // if (resourceDropTimer != 0)
            // {
            //     StartCoroutine(ResourceDropTimerMaintaining());
            //     resourceDropButton.interactable = false;
            // }
            // else
            // {
            //     if (ResourceManager.Instance.antenneReference)
            //     {
            //         resourceDropButton.interactable = ResourceManager.Instance.IsPowerOn();
            //     }
            //     else
            //     {
            //         resourceDropButton.interactable = false;
            //     }
            // }

            // if (impulsAttackTimer != 0)
            // {
            //     StartCoroutine(ImpulseAttackTimerMaintaining());
            //     impusleAttackButton.interactable = false;
            // }
            // else
            // {
            //     if (ResourceManager.Instance.antenneReference)
            //     {
            //         impusleAttackButton.interactable = ResourceManager.Instance.IsPowerOn();
            //     }
            //     else
            //     {
            //         impusleAttackButton.interactable = false;
            //     }
            // }










            // tempPowerPlant = GameObject.Instantiate(
            // PrefabManager.Instance.powerPlantPrefab, 
            // GameObject.Find(powerPlantSavingData._tileOccupiedName).transform.position + OffsetConstants.buildingOffset, 
            // Quaternion.Euler(0f, 0f, (powerPlantSavingData.rotation*60)));
            
            // tempPowerPlant.tag = TagConstants.buildingTag;
            // tempPowerPlant.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            // tempPowerPlant.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
            
            // tempPowerPlant.GetComponent<PowerPlant>().ConstructBuildingFromFile(powerPlantSavingData);



















            

            // tempShatb = GameObject.Instantiate(
            // PrefabManager.Instance.basePrefab, 
            // GameObject.Find(shtabSavingData._tileOccupiedName).transform.position + OffsetConstants.buildingOffset, 
            // Quaternion.Euler(0f, 0f, 0f));
            
            // tempShatb.GetComponent<Base>().ConstructBuildingFromFile(shtabSavingData);







        }
    }














































    public void ResourceDrop()
    {
        Debug.Log("Resource drop Action!");
        isResourceDropReady = false;
        resourceDropButton.interactable = false;
        StartCoroutine(ResourceDropTimerMaintaining());
    }

    public void ImpusleAttack()
    {
        Debug.Log("Impusle attack Action!");
        isImpusleAttackReady = false;
        impusleAttackButton.interactable = false;
        StartCoroutine(ImpulseAttackTimerMaintaining());
    }

    public bool CheckForResourceDropTimer()
    {
        return isResourceDropReady;
    }

    public bool CheckFromImpulseAttackTimer()
    {
        return isImpusleAttackReady;
    }




































    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        redPoint = Instantiate(redPoint, Vector3.zero, Quaternion.identity);
        currentState = idleState;

        MapGenerator.Instance.GenerateMap();




        // Shtab creation and placement - REDO!///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Base shtab = Instantiate(PrefabManager.Instance.basePrefab, new Vector3(39.83717f, 42f, 0f) + OffsetConstants.buildingOffset, Quaternion.identity).GetComponent<Base>();
        ShtabStaticData.InitStaticFields();
        shtab.ConstructBuilding(null);
        ResourceManager.Instance.shtabReference = shtab;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





        AstarData data = AstarPath.active.data;

        StartCoroutine(mapScan());
    }

    IEnumerator mapScan()
    {
        yield return null;

        AstarPath.active.Scan();
    }

    public void ResetCurrentHexAndSelectedHex() // Reset all state-machine variables for transfers
    {
        if (SelectedHex)
        {
            SelectedHex.GetComponent<SpriteRenderer>().color = hexColor;
            SelectedHex = null;
        }
        CurrentHex = null;

        GameViewMenu.Instance.TurnBuildingsCreationButtonOFF();
    }

    public void setCurrentHex() // Do not touch!
    {
        c = pixel_to_pointy_hex(redPoint.transform.position.x, redPoint.transform.position.y);
        CurrentHex = GameObject.Find(c.q + "." +c.r + "." + c.s);
    }



#region  Calculating functions
    public Point pointy_hex_to_pixel(Hex hex)
    {
        var x = (Mathf.Sqrt(3) * hex.Q  +  Mathf.Sqrt(3)/2 * hex.R);
        var y = (                         3f/2 * hex.R);
        return new Point(x, y);
    }

    public Cube pixel_to_pointy_hex(float point_x, float point_y)
    {
        var q = (Mathf.Sqrt(3)/3 * point_x  -  1f/3 * point_y);
        var r = (                              2f/3 * point_y);
        return cube_round(new Cube(q, r, -(q+r)));
    }

    private Cube cube_round(Cube cube)
    {
        var rx = Mathf.Round(cube.q);
        var ry = Mathf.Round(cube.r);
        var rz = Mathf.Round(cube.s);

        var x_diff = Mathf.Abs(rx - cube.q);
        var y_diff = Mathf.Abs(ry - cube.r);
        var z_diff = Mathf.Abs(rz - cube.s);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry-rz;
        else if (y_diff > z_diff)
            ry = -rx-rz;
        else
            rz = -rx-ry;

        return new Cube(rx, ry, rz);
    }
#endregion
}


public class Point
{
    public float x;
    public float y;

    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Cube
{
    public float q;
    public float r;
    public float s;

    public int c_q_arr_pos;
    public int c_r_arr_pos;
    

    public Cube(float q, float r, float s)
    {
        this.q = q;
        this.r = r;
        this.s = s;

        this.c_q_arr_pos = (int)q + ((int)r/2);
        this.c_r_arr_pos = (int)r;
    }
}