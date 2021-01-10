using UnityEngine;
using UnityEngine.UI;

public class GameHendler : MonoBehaviour
{
    #region State machine 
        public IdleState idleState = new IdleState();
        public BuildingSelectionState buildingSelectionState = new BuildingSelectionState();
        public SelectTileState selectTileState = new SelectTileState();
        public CameraMovementState cameraMovementState = new CameraMovementState();
        public ZoomState zoomState = new ZoomState();

        public BM_IdleState BM_idleState = new BM_IdleState();
        public BM_CameraMovementState BM_cameraMovementState = new BM_CameraMovementState();
        public BM_ZoomState BM_zoomState = new BM_ZoomState();
        public BM_BuildingMovementState BM_buildingMovementState = new BM_BuildingMovementState();

        private ITouchState currentState;
    #endregion

    // For states transitions
    [SerializeField] public bool isBuildingSelected = false;
    [SerializeField] public bool isFirstCollide = false;
    [SerializeField] public bool isZooming = false;
    [SerializeField] public bool isTileselectState = false;
    [SerializeField] public bool isCameraState = false;
    [SerializeField] public bool BM_BuildingIsSet = false;

    #region Mouse and camer variables
        public GameObject redPoint; // Point for Ray Casting and finding Current Hex
        public Vector3 _worldMousePosition;
        public Vector3 touchStart;
    #endregion

    [SerializeField] public  GameObject BuildingSprite; // TEMP - For Debugging for showing Color
    [SerializeField] public  GameObject BuildingSprite1; // TEMP - For Debugging for showing Color
    [SerializeField] public  GameObject BuildingSprite2; // TEMP - For Debugging for showing Color
    
    public GameObject CurrentHex;
    public GameObject SelectedHex;
    public Cube c; // TEMP for calculating

    public Model buildingModel;

    public Color hexColor;





    public void ResetDebugTilesPosition() // For Debug
    {
        BuildingSprite.transform.position = BuildingSprite1.transform.
            position = BuildingSprite2.transform.position = Vector3.zero;
    }

    private void Start()
    {
        //currentState = BM_idleState;
        currentState = idleState; // initializing carrent state 
        buildingModel = new Model();
        redPoint = Instantiate(redPoint, Vector3.zero, Quaternion.identity);

        BuildingSprite = Instantiate(BuildingSprite, Vector3.zero, Quaternion.identity);  // TEMP for Debug
        BuildingSprite1 = Instantiate(BuildingSprite1, Vector3.zero, Quaternion.identity);// TEMP for Debug
        BuildingSprite2 = Instantiate(BuildingSprite2, Vector3.zero, Quaternion.identity);// TEMP for Debug
        
        c = new Cube(0,0,0); // Temp for calculating
    }
    
    private void FixedUpdate()
    {
        _worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        redPoint.transform.position = 
        new Vector3(_worldMousePosition.x,_worldMousePosition.y,_worldMousePosition.z + 20);
    }

    private void Update()
    {
        currentState = currentState.DoState(this);
        
        // Seting Model to start calculating future building position
        if (Input.GetKeyDown(KeyCode.Space) && SelectedHex)
        {
            // Refer to UI button, ID(1/2/3) will change (switch)
            buildingModel.InitModel((int)IDconstants.IDturette);

            // Cashing Selected Hex info
            int q = (SelectedHex.GetComponent<Hex>().Q);
            int r = (SelectedHex.GetComponent<Hex>().R);
            int s = (SelectedHex.GetComponent<Hex>().S);

            // Findind positions for Tiles aquoting to Building Type
            switch(buildingModel.buildingType)
            {
                case BuildingType.SingleTileBuilding:
                    buildingModel.BTileZero = SelectedHex;
                    buildingModel.BTileOne = null;
                    buildingModel.BTileTwo = null;
                break;
                
                case BuildingType.DoubleTileBuilding:
                    buildingModel.BTileZero = SelectedHex;
                    buildingModel.BTileOne = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                    buildingModel.BTileTwo = null;
                break;
                
                case BuildingType.TripleTileBuilding:
                    buildingModel.BTileZero = SelectedHex;
                    buildingModel.BTileOne = GameObject.Find(q + "." + (r+1) + "." + (s-1));
                    buildingModel.BTileTwo = GameObject.Find(q + "." + (r-1) + "." + (s+1));
                break;

                // If i want to add Another Shape of TripleTile Building
                // Then Add *NewTripleTileBuilding* to BuildingType.cs
                // And add another case with cubic coords offset (див. с.Вулик)
            }

            ResetBuildingSpritePositions();
            BM_buildingMovementState.ChechForCorrectPlacement(this);

            currentState = BM_idleState;
        }
    }

    public void resetInfo()
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

    //Only for DEBUG (Need access to buildingModel)
    public void ResetBuildingSpritePositions()
    {
        switch(buildingModel.buildingType)
        {
            case BuildingType.SingleTileBuilding:
                BuildingSprite.transform.position = buildingModel.BTileZero.transform.position;
            break;

            case BuildingType.DoubleTileBuilding:
                BuildingSprite.transform.position = buildingModel.BTileZero.transform.position;
                BuildingSprite1.transform.position = buildingModel.BTileOne.transform.position;
            break;
            
            case BuildingType.TripleTileBuilding:
                BuildingSprite.transform.position = buildingModel.BTileZero.transform.position;
                BuildingSprite1.transform.position = buildingModel.BTileOne.transform.position;
                BuildingSprite2.transform.position = buildingModel.BTileTwo.transform.position;
            break;
        }
    }
}




















































































    

