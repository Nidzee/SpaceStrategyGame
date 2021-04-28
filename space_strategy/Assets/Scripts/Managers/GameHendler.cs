using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;


public class GameHendler : MonoBehaviour
{
    public static GameHendler Instance {get; private set;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("CAMERA")]
    public Camera cam;

    [Header("STATE MACHINE")]
    public ZoomState zoomState = new ZoomState();
    public IdleState idleState = new IdleState();
    public SelectTileState selectTileState = new SelectTileState();
    public CameraMovementState cameraMovementState = new CameraMovementState();
    public BuildingSelectionState buildingSelectionState = new BuildingSelectionState();
    public BM_ZoomState BM_zoomState = new BM_ZoomState();
    public BM_IdleState BM_idleState = new BM_IdleState();
    public BM_CameraMovementState BM_cameraMovementState = new BM_CameraMovementState();
    public BM_BuildingMovementState BM_buildingMovementState = new BM_BuildingMovementState();
    public ITouchState currentState = null;

    [Header("TOUCH VARIABLES")]
    public GameObject redPoint;
    public Vector3 worldMousePosition;
    public Vector3 touchStart;

    [Header("TEMP VARIABLES FOR CALCULATING")]
    private Cube c = new Cube(0,0,0);
    public Color hexColor;
    
    [Header("VARIABLES")]
    public LayerMask idelLayerMask;           // Layer mask for idle mode
    public LayerMask BMidelLayerMask;         // Layer mask for building mode
    public GameObject CurrentHex;             // Always Hex under mouse
    public GameObject SelectedHex;            // Selected Hex at the moment
    public Model buildingModel = new Model(); // Building model
    public GameObject selctedBuilding = null; // Building model
    
    [Header("ANTENNE LOGIC VARIABLES")]
    public float resourceDropTimer    = 0f;
    public float impulsAttackTimer    = 0f;
    private float _timerStep          = 0.5f;
    private bool isResourceDropReady  = true;
    private bool isImpusleAttackReady = true;
    public GameObject antenneButtonsPanel;                     // Init in inspector
    [SerializeField] private Image resourceDropProgressImage;  // Init in inspector
    [SerializeField] private Image impulseAttackProgressImage; // Init in inspector
    public Button resourceDropButton;                          // Init in inspector
    public Button impusleAttackButton;                         // Init in inspector

    [Header("SAVING DATA")]
    public ResourcesSavingData saveData                          = null;    
    public ShtabSavingData shtabSavingData                       = null;
    public List<PowerPlantSavingData> powerPlantsSaved           = new List<PowerPlantSavingData>();
    public List<GarageSavingData> garagesSaved                   = new List<GarageSavingData>();
    public List<MineShaftSavingData> mineShaftsSaved             = new List<MineShaftSavingData>();
    public List<ShieldGeneratorSavingData> shieldGeneratorsSaved = new List<ShieldGeneratorSavingData>();
    public List<TurretSavingData> turretsSaved                   = new List<TurretSavingData>();
    public AntenneSavingData antenneSavingData                   = null;
    public AntenneLogicSavingData antenneLogicSavingData         = null;
    public List<UnitSavingData> unitsSaved                       = new List<UnitSavingData>();
    public EnemySpawnerSavingData spawnerSavingData              = new EnemySpawnerSavingData();
    public List<EnemyBomberSavingData> bombersSaved              = new List<EnemyBomberSavingData>();

    GameObject tempGarage     = null;
    GameObject tempShaft      = null;
    GameObject tempUnit       = null;
    GameObject tempTurret     = null;
    GameObject tempSG         = null;
    GameObject tempAntenne    = null;
    GameObject tempPowerPlant = null;
    GameObject tempShatb      = null;
    GameObject tempBomber     = null;

    public int particularLevelNumber = 0;


    // Main functions
    public void StartGame(int levelNumber, bool isGameLoad)
    {
        Debug.Log("..........[ GAME HENDLER START ]..........");

        // Start all managers with info from inspector
        PrefabManager.Instance.StartPrefabManager();
        UIPannelManager.Instance.InitAllPanelsReferences();
        StatsManager.Instance.InitAllStatistic();
        ResourceManager.Instance.InitStartData();
        MapGenerator.Instance.GenerateMap(levelNumber);
        GameViewMenu.Instance.InitData();

        // Sets level number (Used to build particular map)
        particularLevelNumber = levelNumber;

        // Some start initializations
        redPoint = Instantiate(redPoint, Vector3.zero, Quaternion.identity);
        currentState = idleState;
        


        float posX = 0;
        float posy = 0;
        if (isGameLoad)
        {
            // Sets camera to face SHTAB
            switch(levelNumber)
            {
                case 1:
                posX = 39.83717f;
                posy = 42f;
                break;
                
                case 2:
                posX = 31.17691f;
                posy = 54f;
                break;
                
                case 3:
                posX = 24.24871f;
                posy = 33f;
                break;
                
                case 4:
                posX = 24.24871f;
                posy = 33f;
                break;
                
                case 5:
                posX = 24.24871f;
                posy = 45f;
                break;
            }
            

            // Load all info from file and initialize it
            LoadGameWithPreviouslyInitializedData(); // Enemy spawner starts here
        }
        else
        {
            // Temp variables - references to tiles on which SHTAB is placed
            string tileName1 = "";
            string tileName2 = "";
            string tileName3 = "";
            string tileName4 = "";


            // Resources are already initialized above - in "ResourceManager.Instance.InitStartData();" line of code
            // Creating "SHTAB"
            switch(levelNumber)
            {
                case 1:
                posX = 39.83717f;
                posy = 42f;
                tileName1 = "9.28.-37";
                tileName2 = "9.29.-38";
                tileName3 = "10.28.-38";
                tileName4 = "10.29.-39";
                break;
                
                case 2:
                posX = 31.17691f;
                posy = 54f;
                tileName1 = "0.36.-36";
                tileName2 = "1.36.-37";
                tileName3 = "0.37.-37";
                tileName4 = "1.37.-38";
                break;
                
                case 3:
                posX = 24.24871f;
                posy = 33f;
                tileName1 = "3.22.-25";
                tileName2 = "4.22.-26";
                tileName3 = "3.23.-26";
                tileName4 = "4.23.-27";
                break;
                
                case 4:
                posX = 24.24871f;
                posy = 33f;
                tileName1 = "3.22.-25";
                tileName2 = "4.22.-26";
                tileName3 = "3.23.-26";
                tileName4 = "4.23.-27";
                break;
                
                case 5:
                posX = 24.24871f;
                posy = 45f;
                tileName1 = "-1.30.-29";
                tileName2 = "0.30.-30";
                tileName3 = "-1.31.-30";
                tileName4 = "0.31.-31";
                break;
            }
            Base shtab = Instantiate(PrefabManager.Instance.basePrefab, new Vector3(posX, posy, 0f) + OffsetConstants.buildingOffset, Quaternion.identity).GetComponent<Base>();
            ShtabStaticData.InitStaticFields();
            shtab._tileOccupied = GameObject.Find(tileName1);
            shtab._tileOccupied1 = GameObject.Find(tileName2);
            shtab._tileOccupied2 = GameObject.Find(tileName3);
            shtab._tileOccupied3 = GameObject.Find(tileName4);
            shtab.ConstructBuilding(null);
            shtab.InitEventsBuildingMapInfoResourceManagerReference();
            ResourceManager.Instance.shtabReference = shtab;


            // Enemy spawner start here
            EnemySpawner.Instance.StartEnemySpawnTimer();
        }


        // Sets camera position to always face "SHTAB"
        cam.transform.position = new Vector3(posX, posy, cam.transform.position.z);



        // Map mesh scanning
        AstarData data = AstarPath.active.data;
        AstarPath.active.Scan();                // KOSTUL' - idk why it doesn't find closed tiles with first try
        StartCoroutine(MapScan(isGameLoad));    // KOSTUL' - because if we init unit/enemies states before - they will build their pathes ON closed tiles
    }

    IEnumerator MapScan(bool isLoadGame)
    {
        yield return null;

        AstarPath.active.Scan();

        // Sets all states for units and enemies KOSTUL'
        if (isLoadGame)
        {  
            foreach (var unit in ResourceManager.Instance.unitsList)
            {
                switch(unit.currentStateID)
                {
                    case (int)UnitStates.UnitIdleState:
                    unit.isAtGarage = true;
                    unit.currentState = unit.idleState;
                    break;

                    case (int)UnitStates.UnitMovingState:
                    unit.currentState = unit.movingState;

                    unit.RebuildPath();

                    break;

                    case (int)UnitStates.UnitExtractingState:
                    unit.currentState = unit.extractingState;
                    break;

                    case (int)UnitStates.UnitResourceLeavingState:
                    unit.currentState = unit.resourceLeavingState;
                    break;

                    case (int)UnitStates.UnitNoSignalState:
                    unit.currentState = unit.noSignalState;
                    break;
                }
            }
            foreach (var bomber in ResourceManager.Instance.enemiesBombers)
            {
                switch (bomber.currentStateID)
                {
                    case 1: // Idle
                    bomber.currentState = bomber.idleState;
                    break;

                    case 2: // Go To
                    bomber.currentState = bomber.idleState;
                    break;

                    case 3: // Bash
                    bomber.currentState = bomber.bashState;
                    break;

                    case 4: // Attack
                    bomber.currentState = bomber.attackState;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        worldMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        redPoint.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, worldMousePosition.z + 90);
        
        if (currentState != null)
        {
            currentState = currentState.DoState();
        }
    }



    // Saving - loading functions
    public void SaveCurrentSceneData()
    {
        // Init saving variables
        saveData = new ResourcesSavingData();
        shtabSavingData = null;
        powerPlantsSaved = new List<PowerPlantSavingData>();
        garagesSaved = new List<GarageSavingData>();
        mineShaftsSaved = new List<MineShaftSavingData>();
        shieldGeneratorsSaved = new List<ShieldGeneratorSavingData>();
        turretsSaved = new List<TurretSavingData>();
        antenneSavingData = null;
        antenneLogicSavingData = new AntenneLogicSavingData();
        unitsSaved = new List<UnitSavingData>();
        spawnerSavingData = new EnemySpawnerSavingData();
        bombersSaved = new List<EnemyBomberSavingData>();


        // Save resource data
        saveData.crystalResourceCount = ResourceManager.Instance.resourceCrystalCount; 
        saveData.ironResourceCount = ResourceManager.Instance.resourceIronCount; 
        saveData.gelResourceCount = ResourceManager.Instance.resourceGelCount;
        // Save electricity data        
        saveData.electricity = ResourceManager.Instance.electricityCount;
        saveData.electricity_max = (int)GameViewMenu.Instance.electricityCountSlider.maxValue;
        saveData.electricityNeed = ResourceManager.Instance.electricityNeedCount;
        saveData.electricityNeed_max = (int)GameViewMenu.Instance.electricityNeedCountSlider.maxValue;
        // Save global electricity state
        saveData.IsPowerOn = ResourceManager.Instance.isPowerOn;
        // Save antenne panel
        saveData.isAntenneOnceCreated = ResourceManager.Instance.isAntenneOnceCreated;
        // Save all game units static counters
        saveData.unitCounter = UnitStaticData.unit_counter;
        saveData.gagareCounter = GarageStaticData.garage_counter;
        saveData.crystalShaftCounter = CSStaticData.crystalShaft_counter;
        saveData.ironShaftCounter = ISStaticData.ironShaft_counter;
        saveData.gelShaftCounter = GSStaticData.gelShaft_counter;
        saveData.ppCounter = PowerPlantStaticData.powerPlant_counter;
        saveData.sgCounter = ShiledGeneratorStaticData.shieldGenerator_counter;
        saveData.ltCounter = LTStaticData.turetteLaser_counter;
        saveData.mtCounter = MTStaticData.turetteMisile_counter;
        // Save game waves
        saveData.currentWave = ResourceManager.currentWave;
        saveData.winWaveCounter = ResourceManager.winWaveCounter;


        // Save enemy spawner timer//////////////////////////////////////////////////////////////////////////////
        // Also need to save wave sittuation - becaiuse after loading timer will start but we dont know 
        // if enemies are spawned already
        spawnerSavingData._enemyTimer = EnemySpawner.Instance._enemyTimer;


        // Save SHTAB data
        ResourceManager.Instance.shtabReference.SaveData();

        // Save enemies data
        foreach (var bomber in ResourceManager.Instance.enemiesBombers)
        {
            bomber.SaveData();
        }
        
        // Save power plant data
        foreach (var pp in ResourceManager.Instance.powerPlantsList)
        {
            pp.SaveData();
        }
        
        // Save garage data
        foreach (var garage in ResourceManager.Instance.garagesList)
        {
            garage.SaveData();
        }
        
        // Save mine shafts data
        foreach (var crystalShaft in ResourceManager.Instance.crystalShaftList)
        {
            crystalShaft.GetComponent<MineShaft>().SaveData();
        }    
        foreach (var ironShaft in ResourceManager.Instance.ironShaftList)
        {
            ironShaft.GetComponent<MineShaft>().SaveData();
        }      
        foreach (var gelShaft in ResourceManager.Instance.gelShaftList)
        {
            gelShaft.GetComponent<MineShaft>().SaveData();
        }
        
        // Save shield generators data
        foreach (var sg in ResourceManager.Instance.shiledGeneratorsList)
        {
            sg.SaveData();
        }
        
        // Save turret data
        foreach (var turretMisile in ResourceManager.Instance.misileTurretsList)
        {
            turretMisile.SaveData();
        }
        foreach (var turretLaser in ResourceManager.Instance.laserTurretsList)
        {
            turretLaser.SaveData();
        }
        
        // Save antenne data
        if (ResourceManager.Instance.antenneReference)
        {
            ResourceManager.Instance.antenneReference.SaveData();
        }

        // Save unit data
        foreach (var unit in ResourceManager.Instance.unitsList)
        {
            unit.SaveData();
        }
    
        // Save antenne logic data
        antenneLogicSavingData.timerBash = impulsAttackTimer;
        antenneLogicSavingData.timerResourceDrop = resourceDropTimer;
    }

    public void LoadGameWithPreviouslyInitializedData()
    {
        ///////////////////////////////////// ENEMY SPAWNER DATA ////////////////////////////////////////////////////////
        EnemySpawner.Instance.LoadData(spawnerSavingData);
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////// RESOURCES ///////////////////////////////////////////////////////////////////
        if (saveData != null)
        {
            ResourceManager.Instance.LoadFromFile(saveData);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////// SHTAB ///////////////////////////////////////////////////////////////////
        if (shtabSavingData != null)
        {
            GameObject shtabPlacingTile = GameObject.Find(shtabSavingData._tileOccupiedName); 
            
            tempShatb = GameObject.Instantiate(
            PrefabManager.Instance.basePrefab, 
            shtabPlacingTile.transform.position + OffsetConstants.buildingOffset, 
            Quaternion.Euler(0f, 0f, 0f));
            
            tempShatb.GetComponent<Base>().ConstructBuildingFromFile(shtabSavingData);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////// POWER PLANT ///////////////////////////////////////////////////////////////////
        foreach (var powerPlantSavedData in powerPlantsSaved)
        {
            GameObject powerPlantPlacingTile = GameObject.Find(powerPlantSavedData._tileOccupiedName);
            
            tempPowerPlant = GameObject.Instantiate(
            PrefabManager.Instance.powerPlantPrefab, 
            powerPlantPlacingTile.transform.position + OffsetConstants.buildingOffset, 
            Quaternion.Euler(0f, 0f, (powerPlantSavedData.rotation*60)));
            
            tempPowerPlant.GetComponent<PowerPlant>().ConstructBuildingFromFile(powerPlantSavedData);
        }            
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////// GARAGE /////////////////////////////////////////////////////////////
        foreach (var garageSavedData in garagesSaved)
        {
            GameObject garagePlacingTile = GameObject.Find(garageSavedData._tileOccupied_name);

            tempGarage = GameObject.Instantiate(
            GarageStaticData.BuildingPrefab, 
            garagePlacingTile.transform.position + OffsetConstants.buildingOffset, 
            Quaternion.Euler(0f, 0f, (garageSavedData.rotation*60)));
            
            tempGarage.GetComponent<Garage>().ConstructBuildingFromFile(garageSavedData);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////// MINE SHAFTS /////////////////////////////////////////////////////////
        foreach (var mineShaftSavedData in mineShaftsSaved)
        {
            GameObject mineShaftPlacingTile = GameObject.Find(mineShaftSavedData._tileOccupiedName);
            
            switch (mineShaftSavedData.type)
            {
                // Crystal
                case 1:
                tempShaft = Instantiate(
                PrefabManager.Instance.crystalShaftPrefab, 
                mineShaftPlacingTile.transform.position + OffsetConstants.buildingOffset, 
                Quaternion.Euler(0f, 0f, (mineShaftSavedData.rotation*60)));
                break;


                // Iron
                case 2:
                tempShaft = GameObject.Instantiate(
                PrefabManager.Instance.ironShaftPrefab, 
                GameObject.Find(mineShaftSavedData._tileOccupiedName).transform.position + OffsetConstants.buildingOffset, 
                Quaternion.Euler(0f, 0f, (mineShaftSavedData.rotation*60)));
                break;


                // Gel
                case 3: 
                tempShaft = GameObject.Instantiate(
                PrefabManager.Instance.gelShaftPrefab, 
                GameObject.Find(mineShaftSavedData._tileOccupiedName).transform.position + OffsetConstants.buildingOffset, 
                Quaternion.Euler(0f, 0f, (mineShaftSavedData.rotation*60)));
                break;
            }

            tempShaft.GetComponent<MineShaft>().ConstructBuildingFromFile(mineShaftSavedData);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////// SHIELD GENERATOR ////////////////////////////////////////////////////////////
        foreach (var shieldGeneratorSavedData in shieldGeneratorsSaved)
        {
            GameObject sgPlacingTile = GameObject.Find(shieldGeneratorSavedData._tileOccupied_name);

            tempSG = GameObject.Instantiate(
            PrefabManager.Instance.shieldGeneratorPrefab, 
            sgPlacingTile.transform.position + OffsetConstants.buildingOffset, 
            Quaternion.Euler(0f, 0f, 60 * shieldGeneratorSavedData.rotation));

            
            tempSG.GetComponent<ShieldGenerator>().CreateFromFile(shieldGeneratorSavedData);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////// ANTENNE ////////////////////////////////////////////////////////////
        if (antenneSavingData != null)
        {
            GameObject antennePlacingTile = GameObject.Find(antenneSavingData._tileOccupied_name);

            tempAntenne = GameObject.Instantiate(
            PrefabManager.Instance.antennePrefab, 
            antennePlacingTile.transform.position + OffsetConstants.buildingOffset, 
            Quaternion.Euler(0f, 0f, 60 * antenneSavingData.rotation));

            tempAntenne.GetComponent<Antenne>().CreateFromFile(antenneSavingData);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////// TURRET ///////////////////////////////////////////////////////////
        foreach (var turretSavedData in turretsSaved)
        {
            GameObject turretTile = GameObject.Find(turretSavedData.positionAndOccupationTileName);

            switch (turretSavedData.type)
            {
                case 1:
                switch (turretSavedData.level)
                {
                    case 1: // LASER SINGLE TURRET
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.singleTuretteLaserPrefab, 
                    turretTile.transform.position + OffsetConstants.buildingOffset, 
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotationBuilding));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretLaserSingle>().ConstructBuildingFromFile_LaserSingle();
                    break;

                    case 2: // LASER DOUBLE TURRET 
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.doubleTuretteLaserPrefab, 
                    turretTile.transform.position + OffsetConstants.buildingOffset, 
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotationBuilding));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretLaserDouble>().ConstructBuildingFromFile_LaserDouble();
                    break;

                    case 3: // LASER TRIPPLE TURRET 
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.tripleTuretteLaserPrefab, 
                    turretTile.transform.position + OffsetConstants.buildingOffset, 
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotationBuilding));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretLaserTriple>().ConstructBuildingFromFile_LaserTriple();
                    break;
                }
                break;


                case 2:
                switch (turretSavedData.level)
                {
                    case 1: // MISILE SINGLE TURRET 
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.singleturetteMisilePrefab,
                    turretTile.transform.position + OffsetConstants.buildingOffset,
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotationBuilding));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretMisileSingle>().ConstructBuildingFromFile_MisileSingle();
                    break;

                    case 2: // MISILE DOUBLE TURRET   
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.doubleturetteMisilePrefab, 
                    turretTile.transform.position + OffsetConstants.buildingOffset, 
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotationBuilding));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretMisileDouble>().ConstructBuildingFromFile_MisileDouble();
                    break;

                    case 3: // MISILE TRIPPLE TURRET
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.truipleturetteMisilePrefab, 
                    turretTile.transform.position + OffsetConstants.buildingOffset, 
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotationBuilding));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretMisileTriple>().ConstructBuildingFromFile_MisileTriple();
                    break;
                }
                break;
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////// ANTENNE LOGIC DATA /////////////////////////////////////////////////////////
        // Needs to be loaded after resources
        // Because in resources we save "isAntenneOnceCreated"
        // And after loading it inits above and here - we check and set panel to "true" or "false"

        // Also buttons on panel are initialized from "Antenne" - this means we need to load "Antenne saving data"
        // Before this loading call
        if (antenneLogicSavingData != null)
        {
            if (ResourceManager.Instance.isAntenneOnceCreated)
            {
                antenneButtonsPanel.SetActive(true);
            }

            resourceDropTimer = antenneLogicSavingData.timerResourceDrop;
            impulsAttackTimer = antenneLogicSavingData.timerBash;

            if (resourceDropTimer != 0)
            {
                StartCoroutine(ResourceDropTimerMaintaining());
                resourceDropButton.interactable = false;
            }
            else
            {
                if (ResourceManager.Instance.antenneReference)
                {
                    resourceDropButton.interactable = ResourceManager.Instance.IsPowerOn();
                }
                else
                {
                    resourceDropButton.interactable = false;
                }
            }

            if (impulsAttackTimer != 0)
            {
                StartCoroutine(ImpulseAttackTimerMaintaining());
                impusleAttackButton.interactable = false;
            }
            else
            {
                if (ResourceManager.Instance.antenneReference)
                {
                    impusleAttackButton.interactable = ResourceManager.Instance.IsPowerOn();
                }
                else
                {
                    impusleAttackButton.interactable = false;
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////// ENEMIES /////////////////////////////////////////////////////////////////////
        foreach (var bomber in bombersSaved)
        {
            Vector3 bomberPosition = new Vector3 (bomber.pos_x, bomber.pos_y, bomber.pos_z);

            tempBomber = GameObject.Instantiate(
            PrefabManager.Instance.bomberPrefab, 
            bomberPosition + OffsetConstants.buildingOffset, 
            Quaternion.Euler(0f, 0f, 0f));

            tempBomber.GetComponent<EnemyBomber>().CreateFromFile(bomber);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////// UNIT /////////////////////////////////////////////////////////////////////
        foreach (var unitSavedData in unitsSaved)
        {
            Vector3 unitPosition = new Vector3(unitSavedData.position_x, unitSavedData.position_y, unitSavedData.position_z);
            
            tempUnit = GameObject.Instantiate(
            PrefabManager.Instance.unitPrefab, 
            unitPosition + OffsetConstants.buildingOffset, 
            Quaternion.Euler(0f, 0f, 0f));

            tempUnit.GetComponent<Unit>().CreateUnitFromFile(unitSavedData);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////// CREATE RELATIONS BETWEEN "GARAGES" "SHAFTS" AND "UNIT"//////////////////////////////////////
        foreach (var shaft in ResourceManager.Instance.crystalShaftList)
        {
            shaft.CreateRelations();
        }
        foreach (var shaft in ResourceManager.Instance.ironShaftList)
        {
            shaft.CreateRelations();
        }
        foreach (var shaft in ResourceManager.Instance.gelShaftList)
        {
            shaft.CreateRelations();
        }
        foreach (var garage in ResourceManager.Instance.garagesList)
        {
            garage.AddUnitsFromFileToGarageFromFile();
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }



    // Antenne logic functions
    public void ResourceDrop()
    {
        Debug.Log("Resource drop Action!");
        isResourceDropReady = false;
        resourceDropButton.interactable = false;

        ResourceManager.Instance.AddResourceDrop();

        StartCoroutine(ResourceDropTimerMaintaining());
    }

    public void ImpusleAttack()
    {
        Debug.Log("Impusle attack Action!");
        isImpusleAttackReady = false;
        impusleAttackButton.interactable = false;

        ResourceManager.Instance.BashAllEnemies();

        StartCoroutine(ImpulseAttackTimerMaintaining());
    }

    IEnumerator ResourceDropTimerMaintaining()
    {
        while (resourceDropTimer < 1)
        {
            resourceDropTimer += _timerStep * Time.deltaTime;
            AntenneStaticData.antenneMenuReference.resourceDropProgressImage.fillAmount = resourceDropTimer;

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
            AntenneStaticData.antenneMenuReference.impulseAttackProgressImage.fillAmount = impulsAttackTimer;

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

    public void TurnAntenneButtonsToUnavaliable()
    {
        resourceDropButton.interactable = false;
        impusleAttackButton.interactable = false;

        AntenneStaticData.antenneMenuReference.ReloadButtonManage();
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
        
        AntenneStaticData.antenneMenuReference.ReloadButtonManage();
    }

    public bool CheckForResourceDropTimer()
    {
        return isResourceDropReady;
    }

    public bool CheckFromImpulseAttackTimer()
    {
        return isImpusleAttackReady;
    }



    // Functions for Touch FSM
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