using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Newtonsoft.Json;



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
    
    public Camera cam;




    /////////////////////////// /ANTENNE LOGIC /////////////////////////////////    
    public AntenneMenu antenneMenuReference;
    public GameObject antenneButtonsPanel;

    public float resourceDropTimer;
    public float impulsAttackTimer;

    public Button resourceDropButton;
    public Button impusleAttackButton;

    private bool isResourceDropReady = true;
    private bool isImpusleAttackReady = true;

    [SerializeField] private Image resourceDropProgressImage;
    [SerializeField] private Image impulseAttackProgressImage;

    private float _timerStep = 0.5f;
    /////////////////////////////////////////////////////////////////////


    


    GameObject tempGarage = null;
    GameObject tempShaft = null;
    GameObject tempUnit = null;
    GameObject tempTurret = null;
    GameObject tempSG = null;
    GameObject tempAntenne = null;
    GameObject tempPowerPlant = null;
    GameObject tempShatb = null;
    GameObject tempBomber = null;


    public ResourcesSavingData saveData = null;    
    public ShtabSavingData shtabSavingData = null;
    public List<PowerPlantSavingData> powerPlantsSaved = new List<PowerPlantSavingData>();
    public List<GarageSavingData> garagesSaved = new List<GarageSavingData>();
    public List<MineShaftSavingData> mineShaftsSaved = new List<MineShaftSavingData>();
    public List<ShieldGeneratorSavingData> shieldGeneratorsSaved = new List<ShieldGeneratorSavingData>();
    public List<TurretSavingData> turretsSaved = new List<TurretSavingData>();
    public AntenneSavingData antenneSavingData = null;
    public AntenneLogicSavingData antenneLogicSavingData = null;
    public List<UnitSavingData> unitsSaved = new List<UnitSavingData>();
    public EnemySpawnerSavingData spawnerSavingData = new EnemySpawnerSavingData();
    public List<EnemyBomberSavingData> bombersSaved = new List<EnemyBomberSavingData>();


    private void Start()
    {
        Debug.Log("Game hendler start");
        if (Instance == null)
        {
            Instance = this;
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

        // Globas saving system
        // 0 - Static variables
        // 1 - Resources
        // 2 - Shtab
        // 3 - Power Plants
        // 4 - Garages
        // 5 - Shaftas
        // 6 - Shield Generators
        // 7 - Turrets
        // 8 - Antenne
        // 9 - Antenne menu panel
        // 10 - Enemies
        // 11 - Units

    public void SaveCurrentSceneData()
    {
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




        spawnerSavingData._enemyTimer = EnemySpawner.Instance._enemyTimer;


        foreach (var bomber in ResourceManager.Instance.enemiesBombers)
        {
            bomber.SaveData();
        }
        









        saveData.crystalResourceCount = ResourceManager.Instance.resourceCrystalCount; 
        saveData.ironResourceCount = ResourceManager.Instance.resourceIronCount; 
        saveData.gelResourceCount = ResourceManager.Instance.resourceGelCount;
        saveData.electricity = ResourceManager.Instance.electricityCount;
        saveData.electricity_max = (int)GameViewMenu.Instance.wholeElectricitySlider.maxValue;
        saveData.electricityNeed = ResourceManager.Instance.electricityNeedCount;
        saveData.electricityNeed_max = (int)GameViewMenu.Instance.usingElectricitySlider.maxValue;
        saveData.IsPowerOn = ResourceManager.Instance.isPowerOn;
        saveData.isAntenneOnceCreated = ResourceManager.Instance.isAntenneOnceCreated;
        saveData.unitCounter = UnitStaticData.unit_counter;
        saveData.gagareCounter = GarageStaticData.garage_counter;
        saveData.crystalShaftCounter = CSStaticData.crystalShaft_counter;
        saveData.ironShaftCounter = ISStaticData.ironShaft_counter;
        saveData.gelShaftCounter = GSStaticData.gelShaft_counter;
        saveData.ppCounter = PowerPlantStaticData.powerPlant_counter;
        saveData.sgCounter = ShiledGeneratorStaticData.shieldGenerator_counter;
        saveData.ltCounter = LTStaticData.turetteLaser_counter;
        saveData.mtCounter = MTStaticData.turetteMisile_counter;
        saveData.currentWave = ResourceManager.currentWave;
        saveData.winWaveCounter = ResourceManager.winWaveCounter;







        ResourceManager.Instance.shtabReference.SaveData();







        foreach (var pp in ResourceManager.Instance.powerPlantsList)
        {
            pp.SaveData();
        }
        






        
        foreach (var garage in ResourceManager.Instance.garagesList)
        {
            garage.SaveData();
        }
        







        
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
        








        foreach (var sg in ResourceManager.Instance.shiledGeneratorsList)
        {
            sg.SaveData();
        }
        




        





        foreach (var turretMisile in ResourceManager.Instance.misileTurretsList)
        {
            turretMisile.SaveData();
        }
        
        foreach (var turretLaser in ResourceManager.Instance.laserTurretsList)
        {
            turretLaser.SaveData();
        }
        





        if (ResourceManager.Instance.antenneReference)
        {
            ResourceManager.Instance.antenneReference.SaveData();
        }



        

        
        antenneLogicSavingData.timerBash = impulsAttackTimer;
        antenneLogicSavingData.timerResourceDrop = resourceDropTimer;






        foreach (var unit in ResourceManager.Instance.unitsList)
        {
            unit.SaveData();
        }
    }

    public void LoadGameWithPreviouslyInitializedData()
    {
        EnemySpawner.Instance.StopTimer();


        /////////////////////////////////////////// ENEMY SPAWNER DATA ////////////////////////////////////////////////////////
        EnemySpawner.Instance.LoadData(spawnerSavingData);
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////// RESOURCES ///////////////////////////////////////////////////////////////////
        if (saveData != null)
        {
            ResourceManager.Instance.LoadFromFile(saveData);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////












        ///////////////////////////////////////////// SHTAB ///////////////////////////////////////////////////////////////////
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

        
        ////////////////////////////////////// POWER PLANT ///////////////////////////////////////////////////////////////////
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
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotation_building));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretLaserSingle>().ConstructBuildingFromFile_LaserSingle();
                    break;

                    case 2: // LASER DOUBLE TURRET 
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.doubleTuretteLaserPrefab, 
                    turretTile.transform.position + OffsetConstants.buildingOffset, 
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotation_building));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretLaserDouble>().ConstructBuildingFromFile_LaserDouble();
                    break;

                    case 3: // LASER TRIPPLE TURRET 
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.tripleTuretteLaserPrefab, 
                    turretTile.transform.position + OffsetConstants.buildingOffset, 
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotation_building));

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
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotation_building));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretMisileSingle>().ConstructBuildingFromFile_MisileSingle();
                    break;

                    case 2: // MISILE DOUBLE TURRET   
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.doubleturetteMisilePrefab, 
                    turretTile.transform.position + OffsetConstants.buildingOffset, 
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotation_building));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretMisileDouble>().ConstructBuildingFromFile_MisileDouble();
                    break;

                    case 3: // MISILE TRIPPLE TURRET
                    tempTurret = GameObject.Instantiate(
                    PrefabManager.Instance.truipleturetteMisilePrefab, 
                    turretTile.transform.position + OffsetConstants.buildingOffset, 
                    Quaternion.Euler(0f, 0f, 60 * turretSavedData.rotation_building));

                    tempTurret.GetComponent<Turette>().ConstructBuildingFromFile(turretSavedData);
                    tempTurret.GetComponent<TurretMisileTriple>().ConstructBuildingFromFile_MisileTriple();
                    break;
                }
                break;
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////// ANTENNE LOGIC DATA /////////////////////////////////////////////////////////
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
        foreach (var unit in ResourceManager.Instance.unitsList)
        {
            switch(unit.currentState_ID)
            {
                case (int)UnitStates.UnitIdleState:
                unit.currentState = unit.unitIdleState;
                break;

                case (int)UnitStates.UnitIGoToState:
                unit.currentState = unit.unitIGoToState;

                unit.RebuildPath();

                break;

                case (int)UnitStates.UnitIGatherState:
                unit.currentState = unit.unitIGatherState;
                break;

                case (int)UnitStates.UnitResourceLeavingState:
                unit.currentState = unit.unitResourceLeavingState;
                break;

                case (int)UnitStates.UnitIHomelessState:
                unit.currentState = unit.unitIHomelessState;
                break;
            }
        }
        foreach (var bomber in ResourceManager.Instance.enemiesBombers)
        {
            switch (bomber.currentStateID)
            {
                case 1: // Idle
                bomber.currentState = bomber.bomberIdleState;
                break;

                case 2: // Go To
                bomber.currentState = bomber.bomberIdleState;
                break;

                case 3: // Bash
                bomber.currentState = bomber.bomberBashState;
                break;

                case 4: // Attack
                bomber.currentState = bomber.bomberAttackState;
                break;
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }





















    private void Update()
    {
        worldMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        redPoint.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, worldMousePosition.z + 90);
        
        currentState = currentState.DoState();
    }

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

    public bool CheckForResourceDropTimer()
    {
        return isResourceDropReady;
    }

    public bool CheckFromImpulseAttackTimer()
    {
        return isImpusleAttackReady;
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