using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BaseMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    [SerializeField] private Button _upgradeButton;

    private Base _base = null;




    private bool isUpgradeButtonInteractible = false;




    // Reload panel
    public void ReloadPanel(Base baseRef)
    {
        _base = baseRef;
        
        ReloadButtonManager();
        ReloadSlidersHP_SP();
    }


    private void ReloadButtonManager()
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
    }


    // Reload HP and SP
    private void ReloadSlidersHP_SP()
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
        UIPannelManager.Instance.ResetPanels("GameView");
    }
}