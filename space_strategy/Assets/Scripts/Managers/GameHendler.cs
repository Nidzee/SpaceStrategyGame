using UnityEngine;
using UnityEngine.UI;

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

        private ITouchState currentState;
    #endregion

    #region Mouse and camer variables
        public GameObject redPoint; // Point for Ray Casting and finding Current Hex
        public Vector3 worldMousePosition;
        public Vector3 touchStart;
    #endregion

    #region Temp variableas and fields for DEBUG
        public Cube c;         // TEMP for calculating
        public Color hexColor; // Temp
    #endregion
    

    public LayerMask idelLayerMask;
    public LayerMask BMidelLayerMask;


    public GameObject CurrentHex;             // Always Hex under mouse
    public GameObject SelectedHex;            // Selected Hex at the moment
    public Model buildingModel;               // Building model

    public GameObject selctedBuilding = null; // Building model



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
            buildingModel.InitModel((int)IDconstants.IDgarage); // Refer to UI button, ID(1/2/3) will change (switch)
            ResetCurrentHexAndSelectedHex();
            currentState = BM_idleState;
            Debug.Log("Building_MODE");
        }
    }


    public void ResetCurrentHexAndSelectedHex() // Reset all state-machine variables for transfers
    {
        if (SelectedHex)
        {
            SelectedHex.GetComponent<SpriteRenderer>().color = hexColor;
            SelectedHex = null;
        }
        CurrentHex = null;
    }

    public void setCurrentHex() // Do not touch!
    {
        GameHendler.Instance.c = pixel_to_pointy_hex(GameHendler.Instance.redPoint.transform.position.x, GameHendler.Instance.redPoint.transform.position.y);
        GameHendler.Instance.CurrentHex = GameObject.Find(GameHendler.Instance.c.q + "." + GameHendler.Instance.c.r + "." + GameHendler.Instance.c.s);
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



    // public void ResetBuildingSpritePositions() //Only for DEBUG
    // {
    //     // switch (buildingModel.buildingType)
    //     // {
    //     //     case BuildingType.SingleTileBuilding:
    //     //         buildingSprite.transform.position = buildingModel.BTileZero.transform.position + new Vector3 (0,0,-0.1f);
    //     //     break;

    //     //     case BuildingType.DoubleTileBuilding:
    //     //         buildingSprite.transform.position = buildingModel.BTileZero.transform.position + new Vector3 (0,0,-0.1f);
    //     //         buildingSprite1.transform.position = buildingModel.BTileOne.transform.position + new Vector3 (0,0,-0.1f);
    //     //     break;
            
    //     //     case BuildingType.TripleTileBuilding:
    //     //         buildingSprite.transform.position = buildingModel.BTileZero.transform.position + new Vector3 (0,0,-0.1f);
    //     //         buildingSprite1.transform.position = buildingModel.BTileOne.transform.position + new Vector3 (0,0,-0.1f);
    //     //         buildingSprite2.transform.position = buildingModel.BTileTwo.transform.position + new Vector3 (0,0,-0.1f);
    //     //     break;
    //     // }
    // }

    // public void ResetDebugTilesPosition() //Only for DEBUG
    // {
    //     // buildingSprite.transform.position = buildingSprite1.transform.
    //     //     position = buildingSprite2.transform.position = Vector3.zero;
    // }
