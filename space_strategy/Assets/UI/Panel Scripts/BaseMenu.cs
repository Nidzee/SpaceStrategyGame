using UnityEngine;
using UnityEngine.UI;

public class BaseMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    [SerializeField] private Button _defenceModeButton;
    [SerializeField] private Button _attackModeButton;
    [SerializeField] public Button _buyPerksButton;

    [SerializeField] public Button _upgradeButton;

    [SerializeField] public Image level1;
    [SerializeField] public Image level2;
    [SerializeField] public Image level3;

    private Base _myBase;


    // Button activation managment
    public void ReloadBaseLevelVisuals()
    {
        _upgradeButton.interactable = true;

        // Set visual fill amount
        switch (ResourceManager.Instance.shtabReference.shtabData.level)
        {
            case 1:
            {
                level1.fillAmount = 1;
                level2.fillAmount = 0;
                level3.fillAmount = 0;
            }
            break;

            case 2:
            {
                level1.fillAmount = 1;
                level2.fillAmount = 1;
                level3.fillAmount = 0;
            }
            break;

            case 3:
            {
                level1.fillAmount = 1;
                level2.fillAmount = 1;
                level3.fillAmount = 1;
                _upgradeButton.interactable = false;
            }
            break;
        }

        // Reloads upgrade button
        if (ResourceManager.Instance.shtabReference.shtabData.upgradeTimer != 0)
        {
            _upgradeButton.interactable = false;
        }
        // else if (_base.level != 3)
        // {
        //     _upgradeButton.interactable = true;
        // }
        // else
        // {
        //     _upgradeButton.interactable = false;
        // }
    }

    // Upgrade logic - TODO
    public void Upgrade()
    {
        int crystalsNeed = 0;
        int ironNeed = 0;
        int gelNeed = 0;

        StatsManager.GetResourcesNeedToUpgrade___Shtab(out crystalsNeed, out ironNeed, out gelNeed);

        if (!ResourceManager.Instance.ChecResources(crystalsNeed, ironNeed,gelNeed))
        {
            Debug.Log("Not enough resources!");
            return;
        }

        // Delete resources here
        ResourceManager.Instance.DeleteResourcesAfterAction___1PressAction(crystalsNeed, ironNeed, gelNeed);

        ResourceManager.Instance.shtabReference.StartUpgrade();
        _upgradeButton.interactable = false;
    }

    // Reload panel
    public void ReloadPanel(Base baseRef)
    {
        _myBase = baseRef;
        ResourceManager.Instance.shtabReference.shtabData.isMenuOpened = true;
        
        ReloadSlidersHP_SP();
        ReloadBaseLevelVisuals();
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = _myBase.gameUnit.maxCurrentHealthPoints;
        _HPslider.value = _myBase.gameUnit.healthPoints;

        _SPslider.maxValue = _myBase.gameUnit.maxCurrentShieldPoints;
        _SPslider.value = _myBase.gameUnit.shieldPoints;
    }


    public void BuyPerks()
    {
        int crystalsNeed = 0;
        int ironNeed = 0;
        int gelNeed = 0;

        StatsManager.GetResourcesToBuyPerks(out crystalsNeed, out ironNeed, out gelNeed);

        if (!ResourceManager.Instance.ChecResources(crystalsNeed, ironNeed,gelNeed))
        {
            Debug.Log("Not enough resources!");
            return;
        }

        // Delete resources here
        ResourceManager.Instance.DeleteResourcesAfterAction___1PressAction(crystalsNeed, ironNeed, gelNeed);




        _defenceModeButton.interactable = true;
        _attackModeButton.interactable = true;

        _buyPerksButton.interactable = false;
    }

    public void ActivateAttackMode()
    {
        ResourceManager.Instance.shtabReference.ActivateAttackMode();
        _defenceModeButton.interactable = true;
        _attackModeButton.interactable = false;
    }

    public void ActivateDefenceMode()
    {
        ResourceManager.Instance.shtabReference.ActivateDefenceMode();
        _defenceModeButton.interactable = false;
        _attackModeButton.interactable = true;
    }


    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        ResourceManager.Instance.shtabReference.shtabData.isMenuOpened = false;
    }
}