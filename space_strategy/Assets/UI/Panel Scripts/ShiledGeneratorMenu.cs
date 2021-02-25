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
        upgradeButton.interactable = true;

        // Set visual fill amount
        switch (_myShieldGenerator.level)
        {
            case 1:
            level1.fillAmount = 1;
            level2.fillAmount = 0;
            level3.fillAmount = 0;
            break;

            case 2:
            level1.fillAmount = 1;
            level2.fillAmount = 1;
            level3.fillAmount = 0;
            break;

            case 3:
            level1.fillAmount = 1;
            level2.fillAmount = 1;
            level3.fillAmount = 1;
            upgradeButton.interactable = false;
            break;
        }
        // Reloads upgrade button
        if (_myShieldGenerator.upgradeTimer != 0)
        {
            upgradeButton.interactable = false;
        }
        // else if (_myShieldGenerator.level != 3)
        // {
        //     upgradeButton.interactable = true;
        // }
        // else
        // {
        //     upgradeButton.interactable = false;
        // }
    }

    // Upgrade - TODO
    public void Upgrade()
    {
        int crystalsNeed = 0;
        int ironNeed = 0;
        int gelNeed = 0;

        ShieldGenerator.GetResourcesNeedToExpand(out crystalsNeed, out ironNeed, out gelNeed, _myShieldGenerator);

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


    private void Reload_ON_OFF_buttons()
    {
        if (_myShieldGenerator.shieldGeneratorRangeRef)
        {
            // If shieldRange is in progress
            if (_myShieldGenerator.isShieldCreationInProgress || _myShieldGenerator.isDisablingInProgress)
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
        _myShieldGenerator.isMenuOpened = true;

        ReloadName();
        ReloadSlidersHP_SP();
        Reload_ON_OFF_buttons();
        ReloadLevelManager();
    }

    // Reload name of Garage
    private void ReloadName()
    {
        _shieldGeneratorName.text = _myShieldGenerator.name;
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = _myShieldGenerator.maxCurrentHealthPoints;
        _HPslider.value = _myShieldGenerator.healthPoints;

        _SPslider.maxValue = _myShieldGenerator.maxCurrentShieldPoints;
        _SPslider.value = _myShieldGenerator.shieldPoints;
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

        sg.DestroyShieldGenerator();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myShieldGenerator.isMenuOpened = false;
        _myShieldGenerator = null;
    }

}
