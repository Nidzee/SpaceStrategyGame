using UnityEngine;
using UnityEngine.UI;

public class GameHendler : MonoBehaviour
{
    public static GameHendler Instance {get;private set;}

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

        private ITouchState currentState;
    #endregion

    #region State machine transition variables
        [SerializeField] public bool isBuildingSelected = false;
        [SerializeField] public bool isFirstCollide = false;
        [SerializeField] public bool isZooming = false;
        [SerializeField] public bool isTileselectState = false;
        [SerializeField] public bool isCameraState = false;
        //[SerializeField] public bool BM_BuildingIsSet = false;
    #endregion

    #region Mouse and camer variables
        public GameObject redPoint; // Point for Ray Casting and finding Current Hex
        public Vector3 worldMousePosition;
        public Vector3 touchStart;
    #endregion

    #region Temp variableas and fields for DEBUG
        //public  GameObject buildingSprite; // TEMP - For Debugging for showing Color
        //public  GameObject buildingSprite1; // TEMP - For Debugging for showing Color
        //public  GameObject buildingSprite2; // TEMP - For Debugging for showing Color
        public Cube c; // TEMP for calculating
        public Color hexColor; // Temp
    #endregion
    
    public GameObject CurrentHex; // Always Hex under mouse
    public GameObject SelectedHex; // Selected Hex at the moment
    public Model buildingModel; // Building model



    private void Awake()
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

        buildingModel = new Model();
        redPoint = Instantiate(redPoint, Vector3.zero, Quaternion.identity);
        c = new Cube(0,0,0); // Temp for calculating
    }

    private void Start()
    {
        currentState = idleState; // initializing carrent state 
    }
    
    private void FixedUpdate()
    {
        redPoint.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, worldMousePosition.z + 90);
    }

    private void LateUpdate()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        currentState = currentState.DoState();
        
        // Seting Model to start calculating future building position
        if (Input.GetKeyDown(KeyCode.Space) && SelectedHex)
        {
            Debug.Log("Building_MODE");
            buildingModel.InitModel((int)IDconstants.IDgelShaft); // Refer to UI button, ID(1/2/3) will change (switch)
            currentState = BM_idleState;
        }
    }

    public void resetInfo() // Reset all state-machine variables for transfers
    {
        if (SelectedHex)
        {
            SelectedHex.GetComponent<SpriteRenderer>().color = hexColor;
            SelectedHex = null;
        }

        isFirstCollide = false;
        isBuildingSelected = false;
        isTileselectState = false;
        isCameraState = false;
        CurrentHex = null;
    }



    public void ResetBuildingSpritePositions() //Only for DEBUG
    {
        // switch (buildingModel.buildingType)
        // {
        //     case BuildingType.SingleTileBuilding:
        //         buildingSprite.transform.position = buildingModel.BTileZero.transform.position + new Vector3 (0,0,-0.1f);
        //     break;

        //     case BuildingType.DoubleTileBuilding:
        //         buildingSprite.transform.position = buildingModel.BTileZero.transform.position + new Vector3 (0,0,-0.1f);
        //         buildingSprite1.transform.position = buildingModel.BTileOne.transform.position + new Vector3 (0,0,-0.1f);
        //     break;
            
        //     case BuildingType.TripleTileBuilding:
        //         buildingSprite.transform.position = buildingModel.BTileZero.transform.position + new Vector3 (0,0,-0.1f);
        //         buildingSprite1.transform.position = buildingModel.BTileOne.transform.position + new Vector3 (0,0,-0.1f);
        //         buildingSprite2.transform.position = buildingModel.BTileTwo.transform.position + new Vector3 (0,0,-0.1f);
        //     break;
        // }
    }

    public void ResetDebugTilesPosition() //Only for DEBUG
    {
        // buildingSprite.transform.position = buildingSprite1.transform.
        //     position = buildingSprite2.transform.position = Vector3.zero;
    }
}