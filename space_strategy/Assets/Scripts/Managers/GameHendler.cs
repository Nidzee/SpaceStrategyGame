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

            if (ResourceManager.Instance.antenneReference.antenneData.isMenuOpened)
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

            if (ResourceManager.Instance.antenneReference.antenneData.isMenuOpened)
            {
                AntenneStaticData.antenneMenuReference.ReloadButtonManage();
            }
        }
    }






    private void Update()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        redPoint.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, worldMousePosition.z + 90);
        
        currentState = currentState.DoState();

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     GameObject goo;
        //     goo = Instantiate(PrefabManager.Instance.bomberPrefab, new Vector3(20,10,0), Quaternion.identity);
        //     goo.GetComponent<EnemyBomber>().Creation();
        // }
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
        Base shtab = Instantiate(PrefabManager.Instance.basePrefab, new Vector3(8.660254f, 6f, 0f) + OffsetConstants.buildingOffset, Quaternion.identity).GetComponent<Base>();
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