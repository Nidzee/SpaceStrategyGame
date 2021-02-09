using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}

    // Resources
    private int resourceCrystalCount;
    private int resourceMetalCount;
    private int resourceGelCount;
    private int electricityWholeCount;

    private int electricityNeedCount;

    // Unit Resources
    public List<Unit> unitsList;
    public List<Unit> avaliableUnits;
    public List<Unit> homelessUnits;

    // Shafts list
    public List<CrystalShaft> crystalShaftList;
    public List<IronShaft> ironShaftList;
    public List<GelShaft> gelShaftList;
    public List<Garage> garagesList;
    public List<PowerPlant> powerPlantsList;
    public List<TurretLaser> laserTurretsList;
    public List<TurretMisile> misileTurretsList;
    public List<ShieldGenerator> shiledGeneratorsList;
    public Antenne antenneReference;
    public Base shtabReference;



    public bool isGlobalPowerON = false; 


    public Unit SetAvaliableUnitToWork(Unit workerRef)
    {
        if (avaliableUnits.Count == 0)
        {
            Debug.Log("There is no Avaliable Unit at the moment!");
            return null;
        }
        else
        {
            workerRef = avaliableUnits[(avaliableUnits.Count) - 1];

            avaliableUnits.Remove(workerRef);
            
            Debug.Log("Avaliable Unit is assigned succesfully!");
            
            return workerRef;
        }
    }





    public void AddCrystalResourcePoints()
    {
        resourceCrystalCount++;
    }

    public void AddIronResourcePoints()
    {
        resourceMetalCount++;
    }

    public void AddGelResourcePoints()
    {
        resourceGelCount++;
    }























    public void CreatePPandAddElectricityWholeCount()
    {
        electricityWholeCount += 30;
    }


    public void DestroyPPandRemoveElectricityWholeCount()
    {
        electricityWholeCount -= 30;
    }

    public void CreateBuildingAndAddElectricityNeedCount()
    {
        electricityNeedCount += 12;
    }

    public void DestroyBuildingAndRemoveElectricityNeedCount()
    {
        electricityNeedCount -= 12;
    }

    public void CreateUnitAndAddElectricityNeedCount()
    {
        electricityNeedCount++;
    }

    public void DestroyUnitAndRemoveElectricityNeedCount()
    {
        electricityNeedCount--;
    }





    private void ElectricityLevelCheck()
    {
        if (electricityWholeCount > electricityNeedCount)
        {
            if (!isGlobalPowerON)
            {
                Debug.Log("Turn electricity back ON!");
                isGlobalPowerON = true;
                TurnElectricityON();
            }

            else
            {
                Debug.Log("Everything is still fine with electricity!");
            }
        }

        else
        {
            if (isGlobalPowerON)
            {
                Debug.Log("Turn electricity OFF!");
                isGlobalPowerON = false;
                TurnElectricityOFF();
            }

            else
            {
                Debug.Log("Still electricity OFF!");
            }
        }
    }




    private void TurnElectricityOFF()
    {
        // See my journal
        GameHendler.Instance.resourceDropButton.interactable = false;
        GameHendler.Instance.impusleAttackButton.interactable = false;
    }

    private void TurnElectricityON()
    {
        // See my journal



        // Antenne buttons managment
        if (GameHendler.Instance.CheckForResourceDropTimer())
        {
            GameHendler.Instance.resourceDropButton.interactable = true;
        }

        if (GameHendler.Instance.CheckFromImpulseAttackTimer())
        {
            GameHendler.Instance.impusleAttackButton.interactable = true;
        }
        // TODO Antenne menu
    }














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

        crystalShaftList = new List<CrystalShaft>();
        ironShaftList = new List<IronShaft>();
        gelShaftList = new List<GelShaft>();
        garagesList = new List<Garage>();

        unitsList = new List<Unit>();
        avaliableUnits = new List<Unit>();
        homelessUnits = new List<Unit>();


        Base shtab = Instantiate(PrefabManager.Instance.basePrefab, new Vector3(8.660254f, 6f, 0f) + OffsetConstants.buildingOffset, Quaternion.identity).GetComponent<Base>();
        shtab.InitStaticFields();
        shtab.Creation();
        ResourceManager.Instance.shtabReference = shtab;
    }
}
