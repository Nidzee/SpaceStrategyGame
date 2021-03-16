using UnityEngine;
using UnityEngine.UI;

public class PowerPlantMenu : MonoBehaviour
{
    private PowerPlant _myPowerPlant = null;

    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    
    [SerializeField] private Text _powerPlantName;

    [SerializeField] private Button _destroyBuildingButton;


    // Reload panel
    public void ReloadPanel(PowerPlant powerPlant)
    {
        _myPowerPlant = powerPlant;
        _myPowerPlant.OnDamageTaken += ReloadSlidersHP_SP;
        _myPowerPlant.powerPlantData.isMenuOpened = true;
        
        ReloadInfo();
        ReloadSlidersHP_SP(_myPowerPlant.gameUnit);
    }

    // Reload name and text in info panel
    private void ReloadInfo()
    {
        _powerPlantName.text = _myPowerPlant.name;
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP(GameUnit gameUnit)
    {
        if (_myPowerPlant.gameUnit == gameUnit)
        {
            _HPslider.maxValue = _myPowerPlant.gameUnit.maxCurrentHealthPoints;
            _HPslider.value = _myPowerPlant.gameUnit.healthPoints;

            _SPslider.maxValue = _myPowerPlant.gameUnit.maxCurrentShieldPoints;
            _SPslider.value = _myPowerPlant.gameUnit.shieldPoints;
        }
    }


    // Destruction logic - TODO
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building!");

        PowerPlant pp = _myPowerPlant;

        ExitMenu();

        pp.DestroyBuilding();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        _myPowerPlant.OnDamageTaken -= ReloadSlidersHP_SP;

        UIPannelManager.Instance.ResetPanels("GameView");
        _myPowerPlant.powerPlantData.isMenuOpened = false;
        _myPowerPlant = null;
    }
}
