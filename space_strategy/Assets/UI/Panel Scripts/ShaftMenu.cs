using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShaftMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    [SerializeField] private Slider _unitSlider;

    [SerializeField] private Text _shaftName;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _destroyBuildingButton;

    private MineShaft _myShaft = null;

    private bool _isUpgradeButtonInteractible = true;


    // Button activation managment
    private void ReloadButtonManager()
    {
        if (_unitSlider.maxValue < 7 && !_isUpgradeButtonInteractible)
        {
            _upgradeButton.interactable = true;
            _isUpgradeButtonInteractible = true;
        }
        else if (_isUpgradeButtonInteractible && _unitSlider.maxValue == 7)
        {
            _upgradeButton.interactable = false;
            _isUpgradeButtonInteractible = false;
        }
    }


    // Reload panel
    public void ReloadPanel(MineShaft shaft)
    {
        _myShaft = shaft;
        
        ReloadButtonManager();
        ReloadShaftName();
        ReloadSliders();
        ReloadUnitSlider();
    }

    private void ReloadShaftName()
    {
        _shaftName.text = _myShaft.name;
    }


    // Reload HP and SP
    private void ReloadSliders()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myShaft.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myShaft.ShieldPoints;
    }


    // Reload Main unit slider
    private void ReloadUnitSlider()
    {
        _unitSlider.onValueChanged.RemoveAllListeners();
        _unitSlider.maxValue = _myShaft.capacity;
        _unitSlider.value = _myShaft.unitsWorkers.Count;
        _unitSlider.onValueChanged.AddListener( delegate{UnitManagment();} );
    }


    // Unit managment via slider - TODO
    private void UnitManagment()
    {
        if (_unitSlider.value > _myShaft.unitsWorkers.Count)
        {
            _myShaft.AddWorkerViaSlider();
            Debug.Log("Add Worker");
        }

        if (_unitSlider.value < _myShaft.unitsWorkers.Count)
        {
            _myShaft.RemoveWorkerViaSlider();
            Debug.Log("Remove worker");
        }

        ReloadUnitSlider();
    }


    // Upgrade - extends capacity
    public void Upgrade()
    {
        Debug.Log(_myShaft.capacity +"   -   "+ _unitSlider.maxValue);

        _myShaft.Upgrade();
        ReloadUnitSlider();

        ReloadButtonManager();
    }


    // Destroy building logic - TODO
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
