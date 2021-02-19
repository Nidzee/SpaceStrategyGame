using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BaseMenu : MonoBehaviour
{
    private Base _base = null;

    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    [SerializeField] public Button _defenceModeButton;
    [SerializeField] public Button _attackModeButton;
    [SerializeField] public Button _buyPerksButton;

















    [SerializeField] public Button _upgradeButton;

    [SerializeField] public Image level1;
    [SerializeField] public Image level2;
    [SerializeField] public Image level3;


    // Button activation managment
    public void ReloadLevelManager()
    {
        // Set visual fill amount
        switch (_base.level)
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
            }
            break;
        }

        // Reloads upgrade button
        if (_base.upgradeTimer != 0)
        {
            _upgradeButton.interactable = false;
        }
        else if (_base.level != 3)
        {
            _upgradeButton.interactable = true;
        }
        else
        {
            _upgradeButton.interactable = false;
        }
    }

    // Upgrade logic - TODO
    public void Upgrade()
    {
        _base.StartUpgrade();
        _upgradeButton.interactable = false;
    }



















    




    // Reload panel
    public void ReloadPanel(Base baseRef)
    {
        _base = baseRef;
        _base.isMenuOpened = true;
        
        ReloadSlidersHP_SP();
        ReloadLevelManager();
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _base.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _base.ShieldPoints;
    }



    public void BuyPerks()
    {
        _defenceModeButton.interactable = true;
        _attackModeButton.interactable = true;

        _buyPerksButton.interactable = false;
    }

    public void ActivateAttackMode()
    {
        _base.ActivateAttackMode();
        _defenceModeButton.interactable = true;
        _attackModeButton.interactable = false;
    }

    public void ActivateDefenceMode()
    {
        _base.ActivateDefenceMode();
        _defenceModeButton.interactable = false;
        _attackModeButton.interactable = true;
    }



    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _base.isMenuOpened = false;
        _base = null;
    }
}