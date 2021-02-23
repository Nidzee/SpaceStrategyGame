using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShieldGenerator :  AliveGameUnit, IBuilding
{
    private static ShiledGeneratorMenu shieldGeneratorMenuReference; // Reference to UI panel
    
    private static int shieldGenerator_counter = 0; // For understanding which building number is this

    public static Tile_Type PlacingTileType {get; private set;}       // Static field - Tile type on whic building need to be placed
    public static BuildingType BuildingType {get; private set;}       // Static field - Building type (1-Tile / 2-Tiles / 3-Tiles)
    public static GameObject BuildingPrefab {get; private set;}       // Static field - Specific prefab for creating building
    
    private GameObject tileOccupied = null;        // Reference to real MapTile on which building is set
    private GameObject tileOccupied1 = null;       // Reference to real MapTile on which building is set
    private GameObject tileOccupied2 = null;       // Reference to real MapTile on which building is set

    private GameObject shieldRangePrefab;
    public GameObject shieldGeneratorRangeRef;

    public int level = 1;
    public bool isMenuOpened = false;



















    private static Vector3 startScale  = new Vector3(1,1,1);
    private static Vector3 standartScale = new Vector3(15,15,1);
    private static Vector3 scaleLevel2 = new Vector3(20,20,1);
    private static Vector3 scaleLevel3 = new Vector3(25,25,1);
    private Vector3 targetScale;

    public bool isShieldCreationInProgress = false; // Do not touch - LEGACY CODE
    public bool isDisablingInProgress = false;      // DO not touch - LEGACE CODE
    public bool isUpgradeInProgress = false;        // Do not touch - LEGACY CODE

    public float upgradeTimer = 0f;
    private float _timerStep = 0.5f;














    private static int _crystalNeedForBuilding = 0;
    private static int _ironNeedForBuilding = 0;
    private static int _gelNeedForBuilding = 0;

    private static int _crystalNeedForExpand_ToLvl2 = 0;
    private static int _ironNeedForForExpand_ToLvl2 = 0;
    private static int _gelNeedForForExpand_ToLvl2 = 0;

    private static int _crystalNeedForExpand_ToLvl3 = 100;
    private static int _ironNeedForForExpand_ToLvl3 = 100;
    private static int _gelNeedForForExpand_ToLvl3 = 100;


    public static string GetResourcesNeedToBuildAsText()
    {
        return _crystalNeedForBuilding.ToString() + " " + _ironNeedForBuilding.ToString() +" "+_gelNeedForBuilding.ToString();
    }


    public static void GetResourcesNeedToBuild(out int crystalNeed, out int ironNeed, out int gelNeed)
    {
        crystalNeed = _crystalNeedForBuilding;
        ironNeed = _ironNeedForBuilding;
        gelNeed = _gelNeedForBuilding;
    }

    public static void GetResourcesNeedToExpand(out int crystalNeed, out int ironNeed, out int gelNeed, ShieldGenerator sg)
    {
        if (sg.level == 1)
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

    public static void InitCost_ToLvl2()
    {
        _crystalNeedForExpand_ToLvl2 = 5;
        _ironNeedForForExpand_ToLvl2 = 5;
        _gelNeedForForExpand_ToLvl2 = 5;

        shieldGeneratorMenuReference.upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl2.ToString() + " " + _ironNeedForForExpand_ToLvl2.ToString() +" "+_gelNeedForForExpand_ToLvl2.ToString();
    }

    public static void InitCost_ToLvl3()
    {
        _crystalNeedForExpand_ToLvl3 = 10;
        _ironNeedForForExpand_ToLvl3 = 10;
        _gelNeedForForExpand_ToLvl3 = 10;

        shieldGeneratorMenuReference.upgradeButton.GetComponentInChildren<Text>().text = _crystalNeedForExpand_ToLvl3.ToString() + " " + _ironNeedForForExpand_ToLvl3.ToString() +" "+_gelNeedForForExpand_ToLvl3.ToString();
    }























    IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += _timerStep * Time.deltaTime;

            if (isMenuOpened)
            {
                // Reload fill circles
                switch (level)
                {
                    case 1:
                    {
                        shieldGeneratorMenuReference.level2.fillAmount = upgradeTimer;
                    }
                    break;

                    case 2:
                    {
                        shieldGeneratorMenuReference.level3.fillAmount = upgradeTimer;
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
        isUpgradeInProgress = false;

        ShiledGeneratorUpgrading();
    }

    IEnumerator TurningShiledON()
    {
        while (shieldGeneratorRangeRef.transform.localScale != targetScale)
        {
            shieldGeneratorRangeRef.transform.localScale += new Vector3(0.5f,0.5f,0);
            yield return null;
        }

        if (isMenuOpened)
        {
            shieldGeneratorMenuReference.OFFbutton.interactable = true;
            shieldGeneratorMenuReference.ONbutton.interactable = false;
        }

        isShieldCreationInProgress = false;
    }

    IEnumerator TurningShiledOFF()
    {
        while (shieldGeneratorRangeRef.transform.localScale != startScale)
        {
            shieldGeneratorRangeRef.transform.localScale -= new Vector3(0.5f,0.5f,0);
            yield return null;
        }

        if (isMenuOpened)
        {
            shieldGeneratorMenuReference.OFFbutton.interactable = false;
            shieldGeneratorMenuReference.ONbutton.interactable = true;
        }
        
        Destroy(shieldGeneratorRangeRef);
        isDisablingInProgress = false;
    }


    public void StartUpgrade()
    {
        isUpgradeInProgress = true;
        StartCoroutine(UpgradeLogic());
    }

    public void EnableShield()
    {
        if (!shieldGeneratorRangeRef)
        {
            shieldGeneratorRangeRef = GameObject.Instantiate (shieldRangePrefab, gameObject.transform.position, Quaternion.identity);
            shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
            
            shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            switch (level)
            {
                case 1:
                targetScale = standartScale;
                break;

                case 2:
                targetScale = scaleLevel2;
                break;

                case 3:
                targetScale = scaleLevel3;
                break;
            }

            isShieldCreationInProgress = true;
            StartCoroutine(TurningShiledON());
        }
        else
        {
            Debug.Log("Error! Shield is already On!");
        }
    }

    public void DisableShield()
    {
        if (shieldGeneratorRangeRef)
        {
            isDisablingInProgress = true;
            StartCoroutine(TurningShiledOFF());
        }
        else
        {
            Debug.Log("Error! Shield is already Off!");
        }
    }

    private void ShiledGeneratorUpgrading()
    {
        // level++;

        if (level == 1)
        {
            InitStaticsLevel_2();
        }
        else if (level == 2)
        {
            InitStaticsLevel_3();
        }
        else
        {
            Debug.Log("ERROR! - Invalid ShieldGenerator level!!!!!");
        }







        if (level == 2)
        {
            targetScale = scaleLevel2;
        }
        else
        {
            targetScale = scaleLevel3;
        }

        if (shieldGeneratorRangeRef && !isShieldCreationInProgress && !isDisablingInProgress)
        {
            StartCoroutine(TurningShiledON());
            isShieldCreationInProgress = true;
            shieldGeneratorMenuReference.OFFbutton.interactable = false;
            shieldGeneratorMenuReference.ONbutton.interactable = false;
        }

        Debug.Log("Shield Generator levelUP!");

        if (isMenuOpened)
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
                shieldGeneratorMenuReference.upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
            }
            
            shieldGeneratorMenuReference.ReloadSlidersHP_SP();
            shieldGeneratorMenuReference.ReloadLevelManager();
        }
        
        GameViewMenu.Instance.ReloadShieldGeneratorHP_SPAfterDamage(this);
    }
 



















    // Reloads sliders if Turret Menu is opened
    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);
        ///////////////////////////////
        ///// Damage logic HERE ///////
        ///////////////////////////////


        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            shieldGeneratorMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadShieldGeneratorHP_SPAfterDamage(this);
    }

    private static int maxHealth_Lvl1 = 100; 
    private static int maxHealth_Lvl2 = 200; 
    private static int maxHealth_Lvl3 = 300;

    private static int maxShiled_Lvl1 = 100; 
    private static int maxShiled_Lvl2 = 200; 
    private static int maxShiled_Lvl3 = 300;

    private static int deffencePoints_Lvl1 = 10; 
    private static int deffencePoints_Lvl2 = 15; 
    private static int deffencePoints_Lvl3 = 20;



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

        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            shieldGeneratorMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadShieldGeneratorHP_SPAfterDamage(this);
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

        // Reloads HP/SP sliders if menu is opened
        if (isMenuOpened)
        {
            shieldGeneratorMenuReference.ReloadSlidersHP_SP();
        }

        // Reloads HP_SP sliders if buildings manage menu opened
        GameViewMenu.Instance.ReloadShieldGeneratorHP_SPAfterDamage(this);
    }
























    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        PlacingTileType = Tile_Type.FreeTile;
        BuildingType = BuildingType.TripleTileBuilding;
        BuildingPrefab = PrefabManager.Instance.shieldGeneratorPrefab;
    }

    // Function for creating building
    public void Creation(Model model)
    {
        InitStaticsLevel_1();


        shieldGenerator_counter++;
        this.gameObject.name = "SG" + ShieldGenerator.shieldGenerator_counter;
        ResourceManager.Instance.shiledGeneratorsList.Add(this);

        tileOccupied = model.BTileZero;
        tileOccupied1 = model.BTileOne;
        tileOccupied2 = model.BTileTwo;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;


        shieldRangePrefab = PrefabManager.Instance.shieldGeneratorRangePrefab;


        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    // Function for displaying info
    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("ShieldGeneratorMenu");
        
        if (!shieldGeneratorMenuReference) // executes once
        {
            shieldGeneratorMenuReference = GameObject.Find("ShieldGeneratorMenu").GetComponent<ShiledGeneratorMenu>();
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
            shieldGeneratorMenuReference.upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
        }

        shieldGeneratorMenuReference.ReloadPanel(this);
    }








    public void DestroyShieldGenerator()
    {
        // Remove shield range
        if (shieldGeneratorRangeRef)
        {
            // Shield Range is destroying here
            Destroy(shieldGeneratorRangeRef);
        }

        ResourceManager.Instance.shiledGeneratorsList.Remove(this);

        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;


        if (isMenuOpened)
        {
            shieldGeneratorMenuReference.ExitMenu();
        }


        // No need for reloading unit manage menu
        ReloadBuildingsManageMenuInfo();
        
        
        Destroy(gameObject);
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();
        AstarPath.active.Scan();
    }

    private void ReloadBuildingsManageMenuInfo()
    {
        GameViewMenu.Instance.ReloadBuildingsManageMenuInfo___AfterShieldGeneratorDestroying(this);
    }
}




    // private void UpgradeAndMaintainLogic()
    // {
    //     if (isShieldCreationInProgress)
    //     {
    //         shieldGeneratorRangeRef.transform.localScale += new Vector3(0.5f,0.5f,0);

    //         if (shieldGeneratorRangeRef.transform.localScale == targetScale)
    //         {
    //             isShieldCreationInProgress = false;
                
    //             if (isMenuOpened)
    //             {
    //                 shieldGeneratorMenuReference.OFFbutton.interactable = true;
    //                 shieldGeneratorMenuReference.ONbutton.interactable = false;
    //             }
    //         }
    //     }

        // if (isDisablingInProgress)
        // {
        //     shieldGeneratorRangeRef.transform.localScale -= new Vector3(0.5f,0.5f,0);
            
        //     if (shieldGeneratorRangeRef.transform.localScale == startScale)
        //     {
        //         isDisablingInProgress = false;

        //         if (isMenuOpened)
        //         {
        //             shieldGeneratorMenuReference.OFFbutton.interactable = false;
        //             shieldGeneratorMenuReference.ONbutton.interactable = true;
        //         }
                
        //         Destroy(shieldGeneratorRangeRef);   
        //     }
        // }
    // }
