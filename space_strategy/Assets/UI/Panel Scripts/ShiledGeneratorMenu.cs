using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShiledGeneratorMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    [SerializeField] private Text _shieldGeneratorName;
    [SerializeField] private Text _shieldGeneratorLevel;

    [SerializeField] private Button upgradeButton;

    private ShieldGenerator _myShieldGenerator = null;

    private bool isUpgradeButtonInteractible = true;



    // Button activation managment
    private void ReloadButtonManager()
    {
        if (_myShieldGenerator.level < 3 && !isUpgradeButtonInteractible)
        {
            upgradeButton.interactable = true;
            isUpgradeButtonInteractible = true;
        }
        else if (isUpgradeButtonInteractible && _myShieldGenerator.level == 3)
        {
            upgradeButton.interactable = false;
            isUpgradeButtonInteractible = false;
        }
    }

    // Reload panel with new info
    public void ReloadPanel(ShieldGenerator shieldGenerator)
    {
        _myShieldGenerator = shieldGenerator;
        _myShieldGenerator.isMenuOpened = true;
        
        ReloadButtonManager();
        ReloadInfo();
        ReloadSlidersHP_SP();
    }

    // Reload name of Garage
    private void ReloadInfo()
    {
        _shieldGeneratorName.text = _myShieldGenerator.name;

        _shieldGeneratorLevel.text = "Level - " + _myShieldGenerator.level;
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myShieldGenerator.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myShieldGenerator.ShieldPoints;
    }

    // Upgrade - TODO
    public void Upgrade()
    {
        Debug.Log("Upgrade logic - todo");
        _myShieldGenerator.Upgrade();
        ReloadInfo();
        ReloadButtonManager();
    }

    // Turns shield on
    public void TurnShieldOn()
    {
        _myShieldGenerator.ActivateShield();
    }

    // Turns shield off
    public void TurnShieldOff()
    {
        _myShieldGenerator.DisableShield();
    }

    // Destroy building
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building!");
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myShieldGenerator.isMenuOpened = false;
        _myShieldGenerator = null;
    }

}
