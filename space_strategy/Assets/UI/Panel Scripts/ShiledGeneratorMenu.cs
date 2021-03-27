using UnityEngine;
using UnityEngine.UI;

public class ShiledGeneratorMenu : MonoBehaviour
{
    private ShieldGenerator _myShieldGenerator = null;

    [SerializeField] private Text _shieldGeneratorName;

    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    public Button ONbutton;
    public Button OFFbutton;


    [SerializeField] public Button upgradeButton;

    [SerializeField] public Image level1;
    [SerializeField] public Image level2;
    [SerializeField] public Image level3;



    // Button activation managment
    public void ReloadLevelManager()
    {
        if (_myShieldGenerator)
        {
            upgradeButton.interactable = true;

            // Set visual fill amount
            switch (_myShieldGenerator.shieldGeneratorData.level)
            {
                case 1:
                StatsManager.InitCost_ToLvl2___ShieldGenerator();
                level1.fillAmount = 1;
                level2.fillAmount = 0;
                level3.fillAmount = 0;
                break;

                case 2:
                StatsManager.InitCost_ToLvl3___ShieldGenerator();
                level1.fillAmount = 1;
                level2.fillAmount = 1;
                level3.fillAmount = 0;
                break;

                case 3:
                ShiledGeneratorStaticData.shieldGeneratorMenuReference.upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
                level1.fillAmount = 1;
                level2.fillAmount = 1;
                level3.fillAmount = 1;
                upgradeButton.interactable = false;
                break;
            }
            // Reloads upgrade button
            if (_myShieldGenerator.shieldGeneratorData.upgradeTimer != 0)
            {
                upgradeButton.interactable = false;
            }
            else if (_myShieldGenerator.shieldGeneratorData.level != 3)
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }
        }
    }

    // Upgrade - TODO
    public void Upgrade()
    {
        int crystalsNeed = 0;
        int ironNeed = 0;
        int gelNeed = 0;

        StatsManager.GetResourcesNeedToExpand___ShieldGenerator(out crystalsNeed, out ironNeed, out gelNeed, _myShieldGenerator);

        if (!ResourceManager.Instance.ChecResources(crystalsNeed, ironNeed,gelNeed))
        {
            Debug.Log("Not enough resources!");
            return;
        }

        // Delete resources here
        ResourceManager.Instance.DeleteResourcesAfterAction___1PressAction(crystalsNeed, ironNeed, gelNeed);


        _myShieldGenerator.StartUpgrade();
        upgradeButton.interactable = false;
    }




    public void UpgradeVisuals()
    {
        if (_myShieldGenerator)
        {
            ReloadName();
            ReloadSlidersHP_SP(_myShieldGenerator);
            Reload_ON_OFF_buttons();
            ReloadLevelManager();
        }
    }






    public void Reload_ON_OFF_buttons()
    {
        if (_myShieldGenerator.shieldGeneratorData.shieldGeneratorRangeRef)
        {
            // If shieldRange is in progress
            if (_myShieldGenerator.shieldGeneratorData.isShieldCreationInProgress || _myShieldGenerator.shieldGeneratorData.isDisablingInProgress)
            {
                OFFbutton.interactable = false;
                ONbutton.interactable = false;
            }
            // If Shield is working stable
            else
            {
                OFFbutton.interactable = true;
                ONbutton.interactable = false;
            }
        }

        // The shield is turned off
        else
        {
            OFFbutton.interactable = false;
            ONbutton.interactable = true;
        }
    }

    // Reload panel with new info
    public void ReloadPanel(ShieldGenerator shieldGenerator)
    {
        _myShieldGenerator = shieldGenerator;
        _myShieldGenerator.shieldGeneratorData.isMenuOpened = true;

        ReloadName();
        ReloadSlidersHP_SP(_myShieldGenerator);
        Reload_ON_OFF_buttons();
        ReloadLevelManager();
    }

    // Reload name of Garage
    private void ReloadName()
    {
        _shieldGeneratorName.text = _myShieldGenerator.name;
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP(AliveGameUnit gameUnit)
    {
        if (_myShieldGenerator)
        {
            if (_myShieldGenerator == gameUnit)
            {
                _HPslider.maxValue = _myShieldGenerator.maxCurrentHealthPoints;
                _HPslider.value = _myShieldGenerator.healthPoints;

                _SPslider.maxValue = _myShieldGenerator.maxCurrentShieldPoints;
                _SPslider.value = _myShieldGenerator.shieldPoints;
            }
        }
    }



    // Turns shield on
    public void TurnShieldOn()
    {
        ONbutton.interactable = false;
        OFFbutton.interactable = false;
        _myShieldGenerator.EnableShield();
    }

    // Turns shield off
    public void TurnShieldOff()
    {
        ONbutton.interactable = false;
        OFFbutton.interactable = false;
        _myShieldGenerator.DisableShield();
    }


    // Destroy building
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building!");

        ShieldGenerator sg = _myShieldGenerator;

        ExitMenu();

        sg.DestroyBuilding();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myShieldGenerator.shieldGeneratorData.isMenuOpened = false;
        _myShieldGenerator = null;
    }

}
