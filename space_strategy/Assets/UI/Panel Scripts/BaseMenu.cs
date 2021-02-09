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

    [SerializeField] private Image level1;
    [SerializeField] private Image level2;
    [SerializeField] private Image level3;


    private void Update()
    {
        if (_base.isUpgradeInProgress) // Reload loading bar
        {
            switch(_base.level)
            {
                case 1:
                {
                    level2.fillAmount = _base.upgradeTimer;
                }
                break;

                case 2:
                {
                    level3.fillAmount = _base.upgradeTimer;
                }
                break;

                case 3:
                {
                    Debug.Log("Error");
                }
                break;
            }
        }
    }

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
        if (_base.isUpgradeInProgress)
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
        _base.Upgrade();
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