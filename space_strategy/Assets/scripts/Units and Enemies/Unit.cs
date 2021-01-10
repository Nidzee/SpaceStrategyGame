using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : AliveGameUnit
{
    public Sprite sprite;
    public float MoveSpeed = 5f;
    public int capacity = 5;

    public static GameObject Base; // Only need COORDS

    public GameObject WorkPlace = null; // Only need COORDS
    public GameObject Home = null; // Only need COORDS
    public GameObject ResourceCarrying; // Only need Sprite

    // State machine
    public UnitIdleState unitIdleState = new UnitIdleState();
    public UnitGatherState unitGatherState = new UnitGatherState();
    public UnitApproachState unitApproachState = new UnitApproachState();
    public UnitLeavingResourceState unitLeavingResourceState = new UnitLeavingResourceState();
    public IUnitState currentState;

    void Awake()
    {
        currentState = unitIdleState;
    }
    void Update()
    {
        currentState = currentState.DoState(this);
    }

    void WorkPlaceInit()
    {

    }

    void WorkPlaceDestroying()
    {

    }

}
