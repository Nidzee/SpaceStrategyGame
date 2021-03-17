using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShtabData
{
    public GameObject resourceRef;        // Reference to Unit resource object (for creating copy and consuming)
    public GameObject storageConsumer;    // Place for resource consuming and dissappearing
    public int level;                     // Determin upgrade level of rest buildings

    public bool isMenuOpened = false;

    public float upgradeTimer = 0;
    private float _timerStep = 0.25f;

    public Base _myShtab;

    public ShtabData(Base thisShtab)
    {
        _myShtab = thisShtab;
        _timerStep = 0.25f;
        upgradeTimer = 0;
        isMenuOpened = false;

        level = 1;
    }


    public void HelperObjectInit()
    {
        if (_myShtab.gameObject.transform.childCount != 0)
        {
            storageConsumer = _myShtab.gameObject.transform.GetChild(0).gameObject;

            storageConsumer.tag = TagConstants.baseStorageTag;
            storageConsumer.gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);            
        }
        else
        {
            Debug.LogError("ERROR!     No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }

    public Transform GetUnitDestination()
    {
        return storageConsumer.transform;
    }


    public void UpgradeToLvl2()
    {
        level = 2;
    }

    public void UpgradeToLvl3()
    {
        level = 3;
    }

    public void StartUpgrade()
    {
        _myShtab.StartCoroutine(UpgradeLogic());
    }

    IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += _timerStep * Time.deltaTime;

            if (isMenuOpened) // Reload fill circles
            {
                switch(level)
                {
                    case 1:
                    ShtabStaticData.baseMenuReference.level2.fillAmount = upgradeTimer;
                    break;

                    case 2:
                    ShtabStaticData.baseMenuReference.level3.fillAmount = upgradeTimer;
                    break;

                    case 3:
                    Debug.LogError("Error! Invalid Circle filling");
                    break;
                }
            }
            yield return null;
        }

        upgradeTimer = 0;

        _myShtab.Upgrade();
    }

    public void Upgrade()
    {
        if (level == 1)
        {
            _myShtab.InitStaticsLevel_2();
            StatsManager.InitCost_ToLvl3___Shtab();
        }
        else if (level == 2)
        {
            _myShtab.InitStaticsLevel_3();
            ShtabStaticData.baseMenuReference._upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
        }
        else
        {
            Debug.LogError("ERROR! - Invalid base level!!!!!");
        }
    }


    public void CreateBuilding(Model model)
    {
        level = 1;

        // _myShtab.name = "BASE";
        // _myShtab.transform.tag = TagConstants.buildingTag;
        // _myShtab.gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        // _myShtab.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;
    }

    public void DestroyBuilding()
    {
        Debug.Log("Deleta shtab data here!");
    }


    public void Invoke()
    {
        ShtabStaticData.baseMenuReference.ReloadPanel(ResourceManager.Instance.shtabReference);
    }

}