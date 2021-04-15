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
        _myPowerPlant.isMenuOpened = true;
        
        ReloadInfo();
        ReloadSlidersHP_SP(_myPowerPlant);
    }

    // Reload name and text in info panel
    private void ReloadInfo()
    {
        _powerPlantName.text = "POWER PLANT - " + _myPowerPlant.name;
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP(AliveGameUnit gameUnit)
    {
        if (_myPowerPlant)
        {
            if (_myPowerPlant == gameUnit)
            {
                _HPslider.maxValue = _myPowerPlant.maxCurrentHealthPoints;
                _HPslider.value = _myPowerPlant.healthPoints;

                _SPslider.maxValue = _myPowerPlant.maxCurrentShieldPoints;
                _SPslider.value = _myPowerPlant.shieldPoints;
            }
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
        _myPowerPlant.isMenuOpened = false;
        _myPowerPlant = null;
    }
}
