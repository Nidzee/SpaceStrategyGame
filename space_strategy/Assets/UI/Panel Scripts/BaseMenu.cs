using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BaseMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _fastUnitCreationButton;

    private Base _base = null;

    private bool isUpgradeButtonInteractible = false;
    private bool isFastCreateUnitButtonInteractible = false;




    // TODO ability to create unit after menu is opened and unit is killed or garage destroy






    // Reload panel
    public void ReloadPanel(Base baseRef)
    {
        _base = baseRef;
        _base.isMenuOpened = true;
        
        ReloadButtonManager();
        ReloadSlidersHP_SP();
    }

    public void ReloadButtonManager()
    {
        if (_base.level < 3 && !isUpgradeButtonInteractible)
        {
            _upgradeButton.interactable = true;
            isUpgradeButtonInteractible = true;
        }
        else if (isUpgradeButtonInteractible && _base.level == 3)
        {
            _upgradeButton.interactable = false;
            isUpgradeButtonInteractible = false;
        }

        int maxCount = 0;
        int actualCount = 0;

        for (int i = 0; i < ResourceManager.Instance.garagesList.Count; i++)
        {
            maxCount += Garage.garageCapacity;
            actualCount += ResourceManager.Instance.garagesList[i].garageMembers.Count;
        }

        if (actualCount < maxCount && !isFastCreateUnitButtonInteractible)
        {
            isFastCreateUnitButtonInteractible = true;
            _fastUnitCreationButton.interactable = true;
        }
        else if (isFastCreateUnitButtonInteractible && actualCount == maxCount)
        {
            _fastUnitCreationButton.interactable = false;
            isFastCreateUnitButtonInteractible = false;
        }
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

    // Fast Unit creation
    public void FastUnitCreation()
    {
        Debug.Log("Fast Unit Creation! - TODO");
        
        Garage garage = ResourceManager.Instance.FindFreeGarage();

        if (garage)
        {
            garage.CreateUnit();
        }
        else
        {
            Debug.Log("Error!");
        }

        ReloadButtonManager();







        //////////////////////////////////////////////////////////////////////////////////////////////



        // Reload menu panel UNIT TEXTBOX

        // No need for reloading SLIDERS because just created units are not involved in gathering process





        //////////////////////////////////////////////////////////////////////////////////////////////





    }

    // Upgrade logic - TODO
    public void Upgrade()
    {
        _base.Upgrade();
        Debug.Log("Upgrade new age!");
        ReloadButtonManager();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        GameHendler.Instance.isBaseMenuOpened = false; // HELPER

        UIPannelManager.Instance.ResetPanels("GameView");
        _base.isMenuOpened = false;
        _base = null;
    }
}