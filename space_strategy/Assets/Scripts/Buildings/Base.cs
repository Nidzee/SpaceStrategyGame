using UnityEngine;
using System.Collections;

public class Base : AliveGameUnit, IBuilding
{
    private BaseMenu baseMenuReference;        // Reference to UI panel (same field for all Garages)
    private GameObject basePrefab;      // Static prefab for creating base

    public GameObject resourceRef;             // Reference to Unit resource object (for creating copy and consuming)
    public GameObject storageConsumer;    // Place for resource consuming and dissappearing
    public int level = 1;                      // Determin upgrade level of rest buildings

    public bool isMenuOpened = false;


    public float upgradeTimer = 0;
    private float _timerStep = 0.5f;


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
        level++;                     // Increments level

        Debug.Log("BASE LEVELE UP!");

        if (isMenuOpened)            // Update menu if it is opened
        {
            // No need for reloading name
            // No need for reloading HP/SP because it is TakeDamage buisness

            baseMenuReference.ReloadLevelManager(); // update buttons and vizuals
        }

        // No need for reloading UnitManageMenu - unitCounter - because no new units created or died or else
        // Only need to reload sliders or specific slider tab
    }


    public void StartUpgrade()
    {
        StartCoroutine(UpgradeLogic());
    }


    public override void TakeDamage(float DamagePoints)
    {
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
        HealthPoints = 100;
        ShieldPoints = 100;

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
