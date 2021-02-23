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
        _crystalNeedForExpand_ForPerks = 20;
        _ironNeedForForExpand_ForPerks = 20;
        _gelNeedForForExpand_ForPerks = 20;

        baseMenuReference._buyPerksButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ForPerks.ToString() + " " + _ironNeedForForExpand_ForPerks.ToString() +" "+_gelNeedForForExpand_ForPerks.ToString();
    }

    public static void InitCost_ToLvl2()
    {
        _crystalNeedForExpand_ToLvl2 = 5;
        _ironNeedForForExpand_ToLvl2 = 5;
        _gelNeedForForExpand_ToLvl2 = 5;

        baseMenuReference._upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl2.ToString() + " " + _ironNeedForForExpand_ToLvl2.ToString() +" "+_gelNeedForForExpand_ToLvl2.ToString();
    }

    public static void InitCost_ToLvl3()
    {
        _crystalNeedForExpand_ToLvl3 = 10;
        _ironNeedForForExpand_ToLvl3 = 10;
        _gelNeedForForExpand_ToLvl3 = 10;

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
            Debug.Log("ERROR! - Invalid base level!!!!!");
        }

        ResourceManager.Instance.UpgradeStatisticsAfterBaseUpgrade();

        if (isMenuOpened)            // Update menu if it is opened
        {
            // No need for reloading name
            // No need for reloading HP/SP because it is TakeDamage buisness
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


            baseMenuReference.ReloadLevelManager(); // update buttons and visuals
            baseMenuReference.ReloadSlidersHP_SP();
        }

        // No need for reloading UnitManageMenu - unitCounter - because no new units created or died or else
        // Only need to reload sliders or specific slider tab
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



    private static int maxHealth_Lvl1 = 300; 
    private static int maxHealth_Lvl2 = 400; 
    private static int maxHealth_Lvl3 = 500;

    private static int maxShiled_Lvl1 = 200; 
    private static int maxShiled_Lvl2 = 300; 
    private static int maxShiled_Lvl3 = 400;

    private static int deffencePoints_Lvl1 = 20; 
    private static int deffencePoints_Lvl2 = 30; 
    private static int deffencePoints_Lvl3 = 40;



    public void InitStaticsLevel_1()
    {
        level = 1;

        healthPoints = maxHealth_Lvl1;
        maxCurrentHealthPoints = maxHealth_Lvl1;

        shieldPoints = maxShiled_Lvl1;
        maxCurrentShieldPoints = maxShiled_Lvl1;

        deffencePoints = deffencePoints_Lvl1;
    }

    public void InitStaticsLevel_2()
    {
        level = 2;

        healthPoints = (maxHealth_Lvl2 * healthPoints) / maxHealth_Lvl1;
        maxCurrentHealthPoints = maxHealth_Lvl2;

        shieldPoints = (maxShiled_Lvl2 * shieldPoints) / maxShiled_Lvl1;
        maxCurrentShieldPoints = maxShiled_Lvl2;

        deffencePoints = deffencePoints_Lvl2;

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

        healthPoints = (maxHealth_Lvl3 * healthPoints) / maxHealth_Lvl2;
        maxCurrentHealthPoints = maxHealth_Lvl3;

        shieldPoints = (maxShiled_Lvl3 * shieldPoints) / maxShiled_Lvl2;
        maxCurrentShieldPoints = maxShiled_Lvl3;

        deffencePoints = deffencePoints_Lvl3;

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
        GameViewMenu.Instance.ReloadBaseHP_SPAfterDamage();
    }

    public void InitStaticFields()
    {
        basePrefab = PrefabManager.Instance.basePrefab;
    }

    public void Creation()
    {   
        InitStaticsLevel_1();

        this.gameObject.name = "BASE";
        // ResourceManager.Instance.shtabReference = this;

        transform.tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;

        HelperObjectInit();

        // ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    private void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.baseStorageTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            
            storageConsumer = gameObject.transform.GetChild(0).gameObject;
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




    // private void UpgradeLogic()
    // {
    //     if (isUpgradeInProgress)
    //     {
    //         upgradeTimer += 0.005f;

    //         if (upgradeTimer > 1)
    //         {
    //             upgradeTimer = 0f;           // Reset timer
    //             level++;                     // Increments level

    //             Debug.Log("BASE LEVELE UP!");

    //             if (isMenuOpened)            // Update menu if it is opened
    //             {
    //                 // No need for reloading name
    //                 // No need for reloading HP/SP because it is TakeDamage buisness

    //                 baseMenuReference.ReloadLevelManager(); // update buttons and vizuals
    //             }

    //             // No need for reloading UnitManageMenu - unitCounter - because no new units created or died or else
    //             // Only need to reload sliders or specific slider tab
    //         }
    //     }
    // }
