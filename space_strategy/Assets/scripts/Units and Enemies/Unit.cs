﻿using UnityEngine;

public class Unit : AliveGameUnit
{
    private static int unit_counter = 0;
    public static GameObject unitPrefab;  // Unit sprite
    public static float moveSpeed = 1f;  // Const speed of all units

    #region Key waypoints
        public Base storage = null;       // static for all units
        public Garage home = null;        // Garage reference
        public MineShaft workPlace = null;   // Shaft reference
        public Vector3 destination;
    #endregion

    #region State machine variables
        public bool isApproachShaft = false;
        public bool isApproachStorage = false;
        public bool isApproachHome = false;
        public bool isGatheringComplete = false;

        public UnitIdleState unitIdleState = new UnitIdleState();
        public UnitIGoToState unitIGoToState = new UnitIGoToState();
        public UnitIHomelessState unitIHomelessState = new UnitIHomelessState();
        public UnitResourceLeavingState unitResourceLeavingState = new UnitResourceLeavingState();
        public UnitIGatherState unitIGatherState = new UnitIGatherState();
        public IUnitState currentState;
    #endregion

    public GameObject resourcePrefab;     // workplace.prefab - need for holding 
    public GameObject resource;           // reference for calculating
    
    private Rigidbody2D rb;






    private void Update()
    {        
        currentState = currentState.DoState(this);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.name == "Unit1")
            {
                Death();
            }
        }
    }

    public static void InitStaticFields()
    {
        unitPrefab = PrefabManager.Instance.unitPrefab;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void CreateInGarage(Garage garage) // no need to reload sliders here or text field - everything is done in GARAGE function
    {
        unit_counter++;

        gameObject.name = "Unit" + unit_counter;

        tag = TagConstants.unitTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
        GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.unitEnemiesResourcesBulletsLayer;

        GetComponent<Unit>().currentState = GetComponent<Unit>().unitIdleState;
        GetComponent<Unit>().storage = PrefabManager.Instance.shtab;
        GetComponent<Unit>().home = garage;

        garage.garageMembers.Add(GetComponent<Unit>());

        ResourceManager.Instance.unitsList.Add(GetComponent<Unit>());
        ResourceManager.Instance.avaliableUnits.Add(GetComponent<Unit>());
    }











    private void Death() // Reload here because dead unit maybe was working at shaft
    {
        if (home)
        {
            if (home.isMenuOpened)
            {
                home.RemoveUnit(this);
                Garage.garageMenuReference.ReloadUnitImage();
                Garage.garageMenuReference.ReloadButtonManager();
            }
            else
            {
                home.RemoveUnit(this);
            }

            if (workPlace)
            {
                if (workPlace.isMenuOpened)
                {
                    workPlace.RemoveUnit(this);
                    MineShaft.shaftMenuReference.ReloadUnitSlider();
                }
                else
                {
                    switch (workPlace.type)
                    {
                        case 1:
                        if (GameHendler.Instance.isMenuCrystalTabOpened)
                        {
                            MineShaft temp = workPlace;
                            workPlace.RemoveUnit(this);
                            GameHendler.Instance.unitManageMenuReference.crystalScrollConten.FindSLiderAndReload(temp);
                        }
                        break;

                        case 2:
                        if (GameHendler.Instance.isMenuIronTabOpened)
                        {
                            MineShaft temp = workPlace;
                            workPlace.RemoveUnit(this); 
                            GameHendler.Instance.unitManageMenuReference.ironScrollConten.FindSLiderAndReload(temp);
                        }
                        break;

                        case 3:
                        if (GameHendler.Instance.isMenuGelTabOpened)
                        {
                            MineShaft temp = workPlace;
                            workPlace.RemoveUnit(this); 
                            GameHendler.Instance.unitManageMenuReference.gelScrollConten.FindSLiderAndReload(temp);
                        }
                        break;
                    }
                }

                ReloadMenuSlider(); // if he was working and if menu is opened than reload because of death

            }

            else
                ResourceManager.Instance.avaliableUnits.Remove(this);
        }

        else 
            ResourceManager.Instance.homelessUnits.Remove(this);

        ResourceManager.Instance.unitsList.Remove(this);

        if (resource)
        {
            Destroy(resource.gameObject);
        }







        /////////////////////////////////////////////////////////////////////////////////////////////////
  
        // Reload unit TEXTBOX because unit dies
        // Reload SLIDERS because unit maybe was worker

        //ReloadMenuSlider();
         
        if (GameHendler.Instance.isBaseMenuOpened) // Reload base menu button if it is opened and unit dies
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






    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.shaftDispenserTag && destination == collider.gameObject.transform.position)
        {
            //Debug.Log("I am near shaft!");
            isApproachShaft = true;
        }

        if (collider.gameObject.tag == TagConstants.baseStorageTag && destination == collider.gameObject.transform.position)
        {
            //Debug.Log("I am near storage!");
            isApproachStorage = true;
        }

        if (collider.gameObject.tag == TagConstants.garageAngarTag && destination == collider.gameObject.transform.position)
        {
            //Debug.Log("I am near home!");
            isApproachHome = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // resource collision
    {
        if (collision.gameObject.tag == TagConstants.resourceTag && collision.gameObject == resource.gameObject) // correct resource
        {
            // Joint Logic
            Vector3 myVector = transform.position - collision.transform.position;
            collision.gameObject.AddComponent<HingeJoint2D>();
            collision.gameObject.GetComponent<HingeJoint2D>().connectedBody = rb;
            collision.gameObject.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
            collision.gameObject.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0,0);
            collision.gameObject.GetComponent<HingeJoint2D>().anchor = new Vector2(myVector.x*4, myVector.y*4);

            //Debug.Log("Resource is attached!");
            isGatheringComplete = true;

            collision.gameObject.GetComponent<CircleCollider2D>().isTrigger = true; // to make resource go through other units
        }
    }
}