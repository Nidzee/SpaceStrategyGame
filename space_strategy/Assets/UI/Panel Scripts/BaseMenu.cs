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



    public void UpdateUIAfterBaseUpgrade()
    {
        if (_myBase)
        {
            ReloadSlidersHP_SP(_myBase);
            ReloadBaseLevelVisuals();
        }
    }

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
        ResourceManager.Instance.DeleteResourcesAfterAction___1PressAction(crystalsNeed, ironNeed, gelNeed);

        ResourceManager.Instance.shtabReference.StartUpgrade();
        _upgradeButton.interactable = false;
    }


    public void ReloadPanel(Base baseRef)
    {
        _myBase = baseRef;
        _myBase.isMenuOpened = true;
        
        ReloadSlidersHP_SP(_myBase);
        ReloadBaseLevelVisuals();
    }

    public void ReloadSlidersHP_SP(AliveGameUnit aliveGameUnit)
    {
        if (_myBase)
        {
            if (_myBase == aliveGameUnit)
            {
                _HPslider.maxValue = aliveGameUnit.maxCurrentHealthPoints;
                _HPslider.value = aliveGameUnit.healthPoints;

                _SPslider.maxValue = aliveGameUnit.maxCurrentShieldPoints;
                _SPslider.value = aliveGameUnit.shieldPoints;
            }
        }
    }

    public void ReloadBaseLevelVisuals()
    {
        if (_myBase)
        {
            switch (ResourceManager.Instance.shtabReference.level)
            {
                case 1:
                {
                    level1.fillAmount = 1;
                    level2.fillAmount = 0;
                    level3.fillAmount = 0;
                    _upgradeButton.interactable = true;
                }
                break;

                case 2:
                {
                    level1.fillAmount = 1;
                    level2.fillAmount = 1;
                    level3.fillAmount = 0;
                    _upgradeButton.interactable = true;
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

            if (ResourceManager.Instance.shtabReference.upgradeTimer != 0)
            {
                _upgradeButton.interactable = false;
            }
        }
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
        _myBase.isMenuOpened = false;
        _myBase = null;
    }
}