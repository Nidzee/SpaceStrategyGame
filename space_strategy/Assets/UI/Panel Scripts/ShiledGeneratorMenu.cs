using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShiledGeneratorMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    [SerializeField] private Text _shieldGeneratorName;

    [SerializeField] private Button createUnitButton;

    private ShieldGenerator _myShieldGenerator = null;

    private bool isUpgradeButtonInteractible = true;


    // Button activation managment
    private void Update()
    {
        if (_myShieldGenerator)
        {
            if (_myShieldGenerator.level < 3 && !isUpgradeButtonInteractible)
            {
                createUnitButton.interactable = true;
                isUpgradeButtonInteractible = true;
            }
            else if (isUpgradeButtonInteractible && _myShieldGenerator.level == 3)
            {
                createUnitButton.interactable = false;
                isUpgradeButtonInteractible = false;
            }
        }
    }


    // Reload panel with new info
    public void ReloadPanel(ShieldGenerator shieldGenerator)
    {
        _myShieldGenerator = shieldGenerator;
        
        ReloadInfo();
        ReloadSlidersHP_SP();
    }


    // Reload name of Garage
    private void ReloadInfo()
    {
        _shieldGeneratorName.text = _myShieldGenerator.name;
    }


    // Reload HP and SP
    private void ReloadSlidersHP_SP()
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
    }

}
