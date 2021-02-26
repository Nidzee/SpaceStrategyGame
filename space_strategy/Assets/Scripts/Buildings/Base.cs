using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Base : AliveGameUnit, IBuilding
{
    private static BaseMenu baseMenuReference;        // Reference to UI panel (same field for all Garages)
    private GameObject basePrefab;      // Static prefab for creating base

    public GameObject resourceRef;             // Reference to Unit resource object (for creating copy and consuming)
    public GameObject storageConsumer;    // Place for resource consuming and dissappearing
    public int level;                      // Determin upgrade level of rest buildings

    public bool isMenuOpened = false;

    public float upgradeTimer = 0;
    private float _timerStep = 0.5f;

    private static int _crystalNeedForExpand_ForPerks;
    private static int _ironNeedForForExpand_ForPerks;
    private static int _gelNeedForForExpand_ForPerks;

    private static int _crystalNeedForExpand_ToLvl2;
    private static int _ironNeedForForExpand_ToLvl2;
    private static int _gelNeedForForExpand_ToLvl2;

    private static int _crystalNeedForExpand_ToLvl3;
    private static int _ironNeedForForExpand_ToLvl3;
    private static int _gelNeedForForExpand_ToLvl3;

    private static int _maxHealth_Lvl1; 
    private static int _maxHealth_Lvl2; 
    private static int _maxHealth_Lvl3;

    private static int _maxShiled_Lvl1; 
    private static int _maxShiled_Lvl2; 
    private static int _maxShiled_Lvl3;

    private static int _deffencePoints_Lvl1; 
    private static int _deffencePoints_Lvl2; 
    private static int _deffencePoints_Lvl3;


    public static void GetResourcesNeedToUpgrade(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        if (ResourceManager.Instance.shtabReference.level == 1)
        {
            crystalNeed = _crystalNeedForExpand_ToLvl2;
            ironNeed = _ironNeedForForExpand_ToLvl2;
            gelNeed = _gelNeedForForExpand_ToLvl2;
        }
        else
        {
            crystalNeed = _crystalNeedForExpand_ToLvl3;
            ironNeed = _ironNeedForForExpand_ToLvl3;
            gelNeed = _gelNeedForForExpand_ToLvl3;
        }
    }

    public static void GetResourcesToBuyPerks(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        crystalNeed = _crystalNeedForExpand_ForPerks;
        ironNeed = _ironNeedForForExpand_ForPerks;
        gelNeed = _gelNeedForForExpand_ForPerks;
    }

    public static void InitCost_ForPerks()
    {
        baseMenuReference._buyPerksButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ForPerks.ToString() + " " + _ironNeedForForExpand_ForPerks.ToString() +" "+_gelNeedForForExpand_ForPerks.ToString();
    }

    public static void InitCost_ToLvl2()
    {
        baseMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl2.ToString() + " " + _ironNeedForForExpand_ToLvl2.ToString() +" "+_gelNeedForForExpand_ToLvl2.ToString();
    }

    public static void InitCost_ToLvl3()
    {
        baseMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl3.ToString() + " " + _ironNeedForForExpand_ToLvl3.ToString() +" "+_gelNeedForForExpand_ToLvl3.ToString();
    }


    IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += _timerStep * Time.deltaTime;

            if (isMenuOpened)
            {
                // Reload fill circles
                switch(level)
                {
                    case 1:
                    {
                        baseMenuReference.level2.fillAmount = upgradeTimer;
                    }
                    break;

                    case 2:
                    {
                        baseMenuReference.level3.fillAmount = upgradeTimer;
                    }
                    break;

                    case 3:
                    {
                        Debug.Log("Error");
                    }
                    break;
                }
            }

            yield return null;
        }

        upgradeTimer = 0;

        BaseUpgrading();
    }

    private void BaseUpgrading()
    {
        upgradeTimer = 0f;           // Reset timer

        if (level == 1)
        {
            InitStaticsLevel_2();
            Debug.Log("BASE LEVELE UP!");
        }
        else if (level == 2)
        {
            InitStaticsLevel_3();
            Debug.Log("BASE LEVELE UP!");
        }
        else
        {
            Debug.LogError("ERROR! - Invalid base level!!!!!");
        }


        ResourceManager.Instance.UpgradeStatisticsAfterBaseUpgrade();


        if (isMenuOpened)            // Update menu if it is opened
        {
            if (level == 1)
            {
                InitCost_ToLvl2();
            }
            else if (level == 2)
            {
                InitCost_ToLvl3();
            }
            else
            {
                baseMenuReference._upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
            }

            baseMenuReference.ReloadBaseLevelVisuals(); // update buttons and visuals
            baseMenuReference.ReloadSlidersHP_SP();
        }

        ReloadHP_SPAfterDamage();
    }


    public void StartUpgrade()
    {
        StartCoroutine(UpgradeLogic());
    }


    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            baseMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        ReloadHP_SPAfterDamage();
    }

    public void InitStaticsLevel_1()
    {
        level = 1;

        healthPoints = _maxHealth_Lvl1;
        maxCurrentHealthPoints = _maxHealth_Lvl1;

        shieldPoints = _maxShiled_Lvl1;
        maxCurrentShieldPoints = _maxShiled_Lvl1;

        deffencePoints = _deffencePoints_Lvl1;
    }

    public void InitStaticsLevel_2()
    {
        level = 2;

        healthPoints = (_maxHealth_Lvl2 * healthPoints) / _maxHealth_Lvl1;
        maxCurrentHealthPoints = _maxHealth_Lvl2;

        shieldPoints = (_maxShiled_Lvl2 * shieldPoints) / _maxShiled_Lvl1;
        maxCurrentShieldPoints = _maxShiled_Lvl2;

        deffencePoints = _deffencePoints_Lvl2;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            baseMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        ReloadHP_SPAfterDamage();
    }

    public void InitStaticsLevel_3()
    {
        level = 3;

        healthPoints = (_maxHealth_Lvl3 * healthPoints) / _maxHealth_Lvl2;
        maxCurrentHealthPoints = _maxHealth_Lvl3;

        shieldPoints = (_maxShiled_Lvl3 * shieldPoints) / _maxShiled_Lvl2;
        maxCurrentShieldPoints = _maxShiled_Lvl3;

        deffencePoints = _deffencePoints_Lvl3;

        // Reload Sliders
        // If mineshaft menu was opened
        // If UI small panel above building was active
        // If buildings manage menu was opened

        if (isMenuOpened)
        {
            baseMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        ReloadHP_SPAfterDamage();
    }

    private void ReloadHP_SPAfterDamage()
    {
        GameViewMenu.Instance.ReloadBaseHPSP();
    }

    public void InitStaticFields()
    {
        basePrefab = PrefabManager.Instance.basePrefab;

        _crystalNeedForExpand_ForPerks = 20;
        _ironNeedForForExpand_ForPerks = 20;
        _gelNeedForForExpand_ForPerks = 20;

        _crystalNeedForExpand_ToLvl2 = 20;
        _ironNeedForForExpand_ToLvl2 = 20;
        _gelNeedForForExpand_ToLvl2 = 20;

        _crystalNeedForExpand_ToLvl3 = 30;
        _ironNeedForForExpand_ToLvl3 = 30;
        _gelNeedForForExpand_ToLvl3 = 30;

        _maxHealth_Lvl1 = 200; 
        _maxHealth_Lvl2 = 300; 
        _maxHealth_Lvl3 = 400;

        _maxShiled_Lvl1 = 200; 
        _maxShiled_Lvl2 = 300; 
        _maxShiled_Lvl3 = 400;

        _deffencePoints_Lvl1 = 10; 
        _deffencePoints_Lvl2 = 15; 
        _deffencePoints_Lvl3 = 20;
    }

    public void Creation()
    {   
        InitStaticsLevel_1();

        this.gameObject.name = "BASE";
        ResourceManager.Instance.shtabReference = this;

        transform.tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;

        HelperObjectInit();
    }

    private void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            storageConsumer = gameObject.transform.GetChild(0).gameObject;

            storageConsumer.tag = TagConstants.baseStorageTag;
            storageConsumer.gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);            
        }
        else
        {
            Debug.LogError("ERROR!     No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("BaseMenu");
        
        if (!baseMenuReference) // executes once
        {
            baseMenuReference = GameObject.Find("BaseMenu").GetComponent<BaseMenu>();
            
            InitCost_ForPerks();
        }

        if (level == 1)
        {
            InitCost_ToLvl2();
        }
        else if (level == 2)
        {
            InitCost_ToLvl3();
        }
        else
        {
            baseMenuReference._upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
        }

        baseMenuReference.ReloadPanel(this);
    }



    public void DestroyBase()
    {
        Debug.Log("END OF THE GAME!");
    }


    public void ActivateDefenceMode()
    {
        Debug.Log("Defence MODE!");
    }

    public void ActivateAttackMode()
    {
        Debug.Log("Attack MODE!");
    }
}