using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PowerPlantMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    
    [SerializeField] private Text _powerPlantName;

    [SerializeField] private Button _destroyBuildingButton;

    private PowerPlant _myPowerPlant = null;



    // Reload panel
    public void ReloadPanel(PowerPlant powerPlant)
    {
        _myPowerPlant = powerPlant;
        _myPowerPlant.isMenuOpened = true;
        
        ReloadInfo();
        ReloadSlidersHP_SP();
    }

    // Reload name and text in info panel
    private void ReloadInfo()
    {
        _powerPlantName.text = _myPowerPlant.name;
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = _myPowerPlant.maxCurrentHealthPoints;
        _HPslider.value = _myPowerPlant.healthPoints;

        _SPslider.maxValue = _myPowerPlant.maxCurrentShieldPoints;
        _SPslider.value = _myPowerPlant.shieldPoints;
    }


    // Destruction logic - TODO
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building!");

        PowerPlant pp = _myPowerPlant;

        ExitMenu();

        pp.DestroyPP();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myPowerPlant.isMenuOpened = false;
        _myPowerPlant = null;
    }
}
