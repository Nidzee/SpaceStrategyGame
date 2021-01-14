using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : AliveGameUnit
{
    private GameObject sprite; // FIX
    private float _moveSpeed = 5f;
    private int _capacity = 5;

    public static Vector3 sklad; // Only need COORDS

    public MineShaft workPlace = null; // Only need COORDS
    public Garage home = null; // Only need COORDS
    public GameObject ResourceCarrying; // Only need Sprite

    public Vector3 destination;


    #region State machine
        public UnitIdleState unitIdleState = new UnitIdleState();
        public UnitIGoToState unitIGoToState = new UnitIGoToState();
        public UnitIHomelessState unitIHomelessState = new UnitIHomelessState();
        public UnitResourceLeavingState unitResourceLeavingState = new UnitResourceLeavingState();
        public UnitIGatherState unitIGatherState = new UnitIGatherState();
        public IUnitState currentState;
    #endregion

    private void Awake()
    {
        //currentState = unitIdleState;
    }

    private void Update()
    {
        //currentState = currentState.DoState(this);
    }

    private void Death()
    {
        // Maybe troubles here!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (workPlace)
        {
            workPlace.RemoveUnit(this);
        }
        else
        {
            ResourceManager.Instance.avaliableUnits.Remove(this);
        }

        if (home)
        {
            home.RemoveUnit(this);
        }
        else
        {
            ResourceManager.Instance.homelessUnits.Remove(this);
        }

        ResourceManager.Instance.unitsList.Remove(this);

        // Destruction logic
    }

}
