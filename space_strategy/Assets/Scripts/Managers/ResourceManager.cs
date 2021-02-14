using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance {get; private set;}

    // Resources
    private int resourceCrystalCount;
    private int resourceIronCount;
    private int resourceGelCount;


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
        resourceCrystalCount += 5;
        GameHendler.Instance.gameViewMenuReference.crystalCounter.text = resourceCrystalCount.ToString();
    }

    public void AddIronResourcePoints()
    {
        resourceIronCount += 5;
        GameHendler.Instance.gameViewMenuReference.ironCounter.text = resourceIronCount.ToString();
    }

    public void AddGelResourcePoints()
    {
        resourceGelCount += 5;
        GameHendler.Instance.gameViewMenuReference.gelCounter.text = resourceGelCount.ToString();
    }



















    private void CheckElectricityForSliderExpand()
    {
        if (electricityCount == 80 || electricityNeedCount == 80)
        {
            GameHendler.Instance.wholeElectricitySlider.maxValue += 50;
            GameHendler.Instance.usingElectricitySlider.maxValue += 50;
        }

        if (electricityCount == 120 || electricityNeedCount == 120)
        {
            GameHendler.Instance.wholeElectricitySlider.maxValue += 50;
            GameHendler.Instance.usingElectricitySlider.maxValue += 50;
        }
    }

    private int electricityCount = 20;
    private int electricityNeedCount = 0;
    private bool isPowerOn = true;

    public void CreatePPandAddElectricityWholeCount()
    {
        electricityCount += 20;
        
        if (electricityCount <= GameHendler.Instance.wholeElectricitySlider.maxValue)
        {
            GameHendler.Instance.wholeElectricitySlider.value = electricityCount;
        }
        
        CheckElectricityForSliderExpand();

        ElectricityLevelCheck();
    }

    public void DestroyPPandRemoveElectricityWholeCount()
    {
        electricityCount -= 20;

        if (electricityCount <= GameHendler.Instance.wholeElectricitySlider.maxValue)
        {
            GameHendler.Instance.wholeElectricitySlider.value = electricityCount;
        }

        ElectricityLevelCheck();
    }


    public void CreateBuildingAndAddElectricityNeedCount()
    {
        electricityNeedCount += 5;

        if (electricityNeedCount <= GameHendler.Instance.usingElectricitySlider.maxValue)
        {
            GameHendler.Instance.usingElectricitySlider.value = electricityNeedCount;
        }

        CheckElectricityForSliderExpand();

        ElectricityLevelCheck();
    }

    public void DestroyBuildingAndRemoveElectricityNeedCount()
    {
        electricityNeedCount -= 5;
        
        if (electricityNeedCount <= GameHendler.Instance.usingElectricitySlider.maxValue)
        {
            GameHendler.Instance.usingElectricitySlider.value = electricityNeedCount;
        }

        ElectricityLevelCheck();
    }

    public void CreateUnitAndAddElectricityNeedCount()
    {
        electricityNeedCount++;
        
        if (electricityNeedCount <= GameHendler.Instance.usingElectricitySlider.maxValue)
        {
            GameHendler.Instance.usingElectricitySlider.value = electricityNeedCount;
        }

        CheckElectricityForSliderExpand();

        ElectricityLevelCheck();
    }

    public void DestroyUnitAndRemoveElectricityNeedCount()
    {
        electricityNeedCount--;
        
        if (electricityNeedCount <= GameHendler.Instance.usingElectricitySlider.maxValue)
        {
            GameHendler.Instance.usingElectricitySlider.value = electricityNeedCount;
        }

        ElectricityLevelCheck();
    }



    private void ElectricityLevelCheck()
    {
        if (electricityCount < electricityNeedCount)
        {
            if (isPowerOn)
            {
                isPowerOn = false;
                TurnElectricityOFF();
            }
            // else
            // {
            //     Debug.Log("Power is still off!");
            // }
        }

        if (electricityCount > electricityNeedCount)
        {
            if (!isPowerOn)
            {
                isPowerOn = true;
                TurnElectricityON();
            }
            // else
            // {
            //     Debug.Log("Power is still on!");
            // }
        }
    }


    public bool IsPowerOn()
    {
        return isPowerOn;
    }


    private void TurnElectricityOFF()
    {
        Debug.Log("Power off!");
        // 1 - Turn off all turrets
        // 2 - Turn off buildings manage menu and unit manage menu
        // 3 - If antenne buttons on screen are active - disable them


        // 1 /////////////////////////////////////////
        foreach (var i in laserTurretsList)
        {
            i.isPowerON = false;
        }
        foreach (var i in misileTurretsList)
        {
            i.isPowerON = false;
        }



        // 2 /////////////////////////////////////////
        if (GameHendler.Instance.isUnitManageMenuOpened)
        {
            GameHendler.Instance.unitManageMenuReference.ExitMenu();
        }
        if (GameHendler.Instance.isBuildingsMAnageMenuOpened)
        {
            GameHendler.Instance.buildingsManageMenuReference.ExitMenu();
        }
        // Make buttons inactive
        GameHendler.Instance.unitManageMenuButton.interactable = false;
        GameHendler.Instance.buildingsManageMenuButton.interactable = false;



        // 3 /////////////////////////////////////////
        GameHendler.Instance.resourceDropButton.interactable = false;
        GameHendler.Instance.impusleAttackButton.interactable = false;

        Debug.Log("ReloadButoonManage");
        GameHendler.Instance.antenneMenuReference.ReloadButoonManage();
    }

    private void TurnElectricityON()
    {
        Debug.Log("Power On!");

        TurnAllTurretsON();

        GameHendler.Instance.unitManageMenuButton.interactable = true;
        GameHendler.Instance.buildingsManageMenuButton.interactable = true;

        
        if (GameHendler.Instance.resourceDropTimer == 0)
        {
            if (antenneReference)
            {
                GameHendler.Instance.resourceDropButton.interactable = true;
            }
        }
        if (GameHendler.Instance.impulsAttackTimer == 0)
        {
            if (antenneReference)
            {
                GameHendler.Instance.impusleAttackButton.interactable = true;
            }
        }

        GameHendler.Instance.antenneMenuReference.ReloadButoonManage();
    }

    private void TurnAllTurretsON()
    {
        foreach(var i in laserTurretsList)
        {
            i.isPowerON = true;
        }
        foreach(var i in misileTurretsList)
        {
            i.isPowerON = true;
        }
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
