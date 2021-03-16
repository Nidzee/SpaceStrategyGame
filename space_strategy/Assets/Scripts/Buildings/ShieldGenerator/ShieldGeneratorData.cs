using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShieldGeneratorData
{
    private GameObject _tileOccupied = null;        // Reference to real MapTile on which building is set
    private GameObject _tileOccupied1 = null;       // Reference to real MapTile on which building is set
    private GameObject _tileOccupied2 = null;       // Reference to real MapTile on which building is set

    private GameObject shieldRangePrefab;
    public GameObject shieldGeneratorRangeRef;

    public int level = 1;
    public bool isMenuOpened = false;

    private Vector3 targetScale;

    public bool isShieldCreationInProgress = false; // Do not touch - LEGACY CODE
    public bool isDisablingInProgress = false;      // DO not touch - LEGACE CODE
    public bool isUpgradeInProgress = false;        // Do not touch - LEGACY CODE

    public float upgradeTimer = 0f;
    private float _timerStep = 0.5f;

    // public ShieldGenerator _myShieldGenerator;


    public ShieldGeneratorData()
    {
        upgradeTimer = 0;
        level = 1;
    }

    public void EnableShield(ShieldGenerator sg)
    {
        if (!shieldGeneratorRangeRef)
        {
            shieldGeneratorRangeRef = GameObject.Instantiate (shieldRangePrefab, sg.gameObject.transform.position, Quaternion.identity);
            shieldGeneratorRangeRef.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            shieldGeneratorRangeRef.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.shieldGeneratorRangeLayer;
            
            shieldGeneratorRangeRef.tag = TagConstants.shieldGeneratorRange;
            shieldGeneratorRangeRef.name = "ShieldGeneratorRange";

            // May be useless as we upgrade target scale
            switch (level)
            {
                case 1:
                targetScale = ShiledGeneratorStaticData.standartScale;
                break;

                case 2:
                targetScale = ShiledGeneratorStaticData.scaleLevel2;
                break;

                case 3:
                targetScale = ShiledGeneratorStaticData.scaleLevel3;
                break;
            }

            isShieldCreationInProgress = true;
            sg.StartCoroutine(TurningShiledON());
        }
        else
        {
            Debug.Log("Error! Shield is already On!");
        }
    }

    public void DisableShield(ShieldGenerator sg)
    {
        if (shieldGeneratorRangeRef)
        {
            isDisablingInProgress = true;
            sg.StartCoroutine(TurningShiledOFF());
        }
        else
        {
            Debug.Log("Error! Shield is already Off!");
        }
    }


    public void InitStatsAfterBaseUpgrade(ShieldGenerator sg)
    {
        int newHealth = 0;
        int newShield = 0;
        int newDefense = 0;

        switch (level)
        {
            case 1:
            newHealth = StatsManager._maxHealth_Lvl1_ShieldGenerator + StatsManager._baseUpgradeStep_ShieldGenerator;
            newShield = StatsManager._maxShiled_Lvl1_ShieldGenerator + StatsManager._baseUpgradeStep_ShieldGenerator;
            newDefense = StatsManager._defensePoints_Lvl1_ShieldGenerator;
            break;

            case 2:
            newHealth = StatsManager._maxHealth_Lvl2_ShieldGenerator + StatsManager._baseUpgradeStep_ShieldGenerator;
            newShield = StatsManager._maxShiled_Lvl2_ShieldGenerator + StatsManager._baseUpgradeStep_ShieldGenerator;
            newDefense = StatsManager._defensePoints_Lvl2_ShieldGenerator;
            break;

            case 3:
            newHealth = StatsManager._maxHealth_Lvl3_ShieldGenerator + StatsManager._baseUpgradeStep_ShieldGenerator;
            newShield = StatsManager._maxShiled_Lvl3_ShieldGenerator + StatsManager._baseUpgradeStep_ShieldGenerator;
            newDefense = StatsManager._defensePoints_Lvl3_ShieldGenerator;
            break;
        }

        sg.UpgradeStats(newHealth, newShield, newDefense);
    }


    public IEnumerator UpgradeLogic(ShieldGenerator sg)
    {
        isUpgradeInProgress = true;

        while (upgradeTimer < 1)
        {
            upgradeTimer += _timerStep * Time.deltaTime;

            if (isMenuOpened) // Reload fill circles
            {
                switch (level)
                {
                    case 1:
                    ShiledGeneratorStaticData.shieldGeneratorMenuReference.level2.fillAmount = upgradeTimer;
                    break;

                    case 2:
                    ShiledGeneratorStaticData.shieldGeneratorMenuReference.level3.fillAmount = upgradeTimer;
                    break;

                    case 3:
                    Debug.Log("Error");
                    break;
                }
            }

            yield return null;
        }

        upgradeTimer = 0;
        isUpgradeInProgress = false;

        ShiledGeneratorUpgrading(sg);
    }

    public void UpgradeToLvl2()
    {
        level = 2;
        targetScale = ShiledGeneratorStaticData.scaleLevel2;
    }

    public void UpgradeToLvl3()
    {
        level = 3;
        targetScale = ShiledGeneratorStaticData.scaleLevel3;
    }

    private void ModifyShieldRangeIfItIsActive(ShieldGenerator sg)
    {
        if (shieldGeneratorRangeRef && !isShieldCreationInProgress && !isDisablingInProgress)
        {
            isShieldCreationInProgress = true;
            sg.StartCoroutine(TurningShiledON());
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.OFFbutton.interactable = false;
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.ONbutton.interactable = false;
        }
    }

    private void ShiledGeneratorUpgrading(ShieldGenerator sg)
    {
        Debug.Log("Shield Generator levelUP!");

        if (level == 1)
        {
            sg.UpgradeToLvl2();
        }
        else if (level == 2)
        {
            sg.UpgradeToLvl3();
        }
        else
        {
            Debug.LogError("ERROR! - Invalid ShieldGenerator level!!!!!");
        }

        ModifyShieldRangeIfItIsActive(sg);

        // // Update UI
        // if (isMenuOpened)
        // {
        //     UpdateUI();
        //     // ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadSlidersHP_SP(_myShieldGenerator.gameUnit);
        //     // ShiledGeneratorStaticData.shieldGeneratorMenuReference.ReloadLevelManager();
        // }
    }

    // Need to be extracted to menu
    private void UpdateUI()
    {
        if (level == 1)
        {
            StatsManager.InitCost_ToLvl2___ShieldGenerator();
        }
        else if (level == 2)
        {
            StatsManager.InitCost_ToLvl3___ShieldGenerator();
        }
        else
        {
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
        }
    }
 

    public IEnumerator TurningShiledON()
    {
        while (shieldGeneratorRangeRef.transform.localScale != targetScale)
        {
            shieldGeneratorRangeRef.transform.localScale += new Vector3(0.5f,0.5f,0);
            yield return null;
        }

        if (isMenuOpened)
        {
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.OFFbutton.interactable = true;
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.ONbutton.interactable = false;
        }

        isShieldCreationInProgress = false;
    }

    public IEnumerator TurningShiledOFF()
    {
        while (shieldGeneratorRangeRef.transform.localScale != ShiledGeneratorStaticData.startScale)
        {
            shieldGeneratorRangeRef.transform.localScale -= new Vector3(0.5f,0.5f,0);
            yield return null;
        }

        if (isMenuOpened)
        {
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.OFFbutton.interactable = false;
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.ONbutton.interactable = true;
        }
        
        GameObject.Destroy(shieldGeneratorRangeRef);
        isDisablingInProgress = false;
    }


    public void DestroyBuilding(ShieldGenerator sg)
    {
        // Remove shield range
        if (shieldGeneratorRangeRef)
        {
            // Shield Range is destroying here
            GameObject.Destroy(shieldGeneratorRangeRef);
        }
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;
        _tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.FreeTile;

        if (isMenuOpened)
        {
            ShiledGeneratorStaticData.shieldGeneratorMenuReference.ExitMenu();
        }

        ResourceManager.Instance.shiledGeneratorsList.Remove(sg);
    }

    public void ConstructBuilding(Model model)
    {
        _tileOccupied = model.BTileZero;
        _tileOccupied1 = model.BTileOne;
        _tileOccupied2 = model.BTileTwo;
        _tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied1.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;
        _tileOccupied2.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;


        shieldRangePrefab = PrefabManager.Instance.shieldGeneratorRangePrefab;
    }

    public void Invoke()
    {
        // Can be extracted to Menu
        UpdateUI();
    }
}