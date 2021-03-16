﻿using UnityEngine;


public class Unit : MonoBehaviour, IAliveGameUnit
{
    public GameUnit gameUnit;
    public UnitData unitData;
    public UnitSavingData unitSavingData;

    public delegate void DamageTaken(GameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void UnitDestroy(GameUnit gameUnit);
    public event UnitDestroy OnUnitDestroyed = delegate{};


    public Vector3 Destination
    {
        get
        {
            return unitData.destination;
        }
    }

    public Garage Home
    {
        get
        {
            return unitData.home;
        }
        set
        {
            unitData.home = value;
        }
    }

    public MineShaft WorkPlace
    {
        get
        {
            return unitData.workPlace;
        }
        set
        {
            unitData.workPlace = value;
        }
    }












    // public void SetWorkPlaceToNull()
    // {
    //     unitData.SetWorkPlaceToNull();
    // }

    // public void SetNewWorkPlace(MineShaft workplace)
    // {
    //     unitData.SetNewWorkPlace(workplace);
    // }

    // public void SetHomeToNull()
    // {
    //     unitData.SetHomeToNull();
    // }

    // public MineShaft GetWorkPlace()
    // {
    //     return unitData.GetWorkPlace();
    // }

    // public void SetNewHome(Garage newHome)
    // {
    //     unitData.SetNewHome(newHome);
    // }



















    public void TakeDamage(int damagePoints)
    {
        if (!gameUnit.TakeDamage(damagePoints))
        {
            DestroyUnit();
            return;
        }

        OnDamageTaken(gameUnit);
    }

    // Unit life cycle
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (name == "Unit - 1")
            {
                DestroyUnit();
            }
        }

        // Debug.Log(unitData.currentState);

        unitData.LifeCycle(this);
    }

    public void ChangeDestination(int destinationID)
    {
        unitData.ChangeDestination(destinationID);
    }







    public void CreateInGarage(Garage garage) // no need to reload sliders here or text field - everything is done in GARAGE function
    {
        gameUnit = new GameUnit(StatsManager.maxUnit_Health, StatsManager.maxUnit_Shield, StatsManager.maxUnit_defense);
        unitData = new UnitData();

        unitData.CreateUnit(garage, this);

        ResourceManager.Instance.unitsList.Add(this);
        ResourceManager.Instance.avaliableUnits.Add(this);

        ResourceManager.Instance.CreateUnitAndAddElectricityNeedCount();
    }









    // Add intermidiary calculations - unit work and home = null

    private void DestroyUnit() // Reload here because dead unit maybe was working at shaft
    {
        unitData.DestroyUnit();


        Destroy(gameObject);
        ResourceManager.Instance.DestroyUnitAndRemoveElectricityNeedCount();
    }
 










    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        unitData.OnTriggerEnter(collider);
    }

    void OnTriggerExit2D(Collider2D collider) // For model correct placing
    {
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
            // Debug.Log("Unit leaves model!");
            GameHendler.Instance.buildingModel.isModelPlacable = true;
            GameHendler.Instance.buildingModel.ChechForCorrectPlacement();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // resource collision
    {
        unitData.OnCollisionEnter(collision);
    }
}