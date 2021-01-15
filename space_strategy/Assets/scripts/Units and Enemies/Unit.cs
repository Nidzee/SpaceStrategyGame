using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : AliveGameUnit
{
    public static GameObject unitPrefab;  // Unit sprite
    public static float _moveSpeed = 1f;  // Const speed of all units

    public GameObject sklad = null;       // static for all units
    public GameObject home = null;        // Garage reference
    public GameObject workPlace = null;   // Shaft reference
    public Vector3 destination;

    public GameObject resourcePrefab;     // = workplace.prefab
    public GameObject resource;           // reference for calculating
    
    public Rigidbody2D rb;


    #region State machine variable
        public bool isApproachShaft = false;
        public bool isApproachSklad = false;
        public bool isApproachHome = false;
        public bool isGatheringComplete = false;

        public UnitIdleState unitIdleState = new UnitIdleState();
        public UnitIGoToState unitIGoToState = new UnitIGoToState();
        public UnitIHomelessState unitIHomelessState = new UnitIHomelessState();
        public UnitResourceLeavingState unitResourceLeavingState = new UnitResourceLeavingState();
        public UnitIGatherState unitIGatherState = new UnitIGatherState();
        public IUnitState currentState;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = unitIdleState;
    }

    private void Update()
    {        
        currentState = currentState.DoState(this);
    }

    private void Death()
    {
        // Maybe troubles here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (workPlace)
        {
            workPlace.GetComponent<MineShaft>().RemoveUnit(this);
        }
        else
        {
            ResourceManager.Instance.avaliableUnits.Remove(this);
        }

        if (home)
        {
            home.GetComponent<Garage>().RemoveUnit(this);
        }
        else
        {
            ResourceManager.Instance.homelessUnits.Remove(this);
        }

        ResourceManager.Instance.unitsList.Remove(this);

        // Destruction logic
    }



    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        Debug.Log("OnTriggerEnter2D");
        
        if (collider.gameObject.tag == "ShaftRadius" && destination == collider.gameObject.transform.position)
        {
            Debug.Log("I am near shaft!");
            isApproachShaft = true;
        }

        if (collider.gameObject.tag == "SkladRadius" && destination == collider.gameObject.transform.position)
        {
            Debug.Log("I am near sklad!");
            isApproachSklad = true;
        }

        if (collider.gameObject.tag == "HomeRadius" && destination == collider.gameObject.transform.position)
        {
            Debug.Log("I am near home!");
            isApproachHome = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // resource collision
    {
        if (collision.gameObject.tag == "Resource" && collision.gameObject == resource.gameObject) // correct resource
        {
            // Joint Logic
            Vector3 myVector = transform.position - collision.transform.position;
            collision.gameObject.AddComponent<HingeJoint2D>();
            collision.gameObject.GetComponent<HingeJoint2D>().connectedBody = rb;
            collision.gameObject.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
            collision.gameObject.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0,0);
            collision.gameObject.GetComponent<HingeJoint2D>().anchor = new Vector2(myVector.x*4, myVector.y*4);

            Debug.Log("Resource ready to go!");
            isGatheringComplete = true;
        }
    }

}
