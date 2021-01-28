using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShaftMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    [SerializeField] private Slider _unitSlider;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _destroyBuildingButton;

    private MineShaft _myShaft = null;

    private bool _isUpgradeButtonInteractible = true;


    // Button activation managment
    private void Update()
    {
        if (_myShaft)
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
    }


    // Reload panel
    public void ReloadPanel(MineShaft shaft)
    {
        _myShaft = shaft;
        
        ReloadSliders();
        ReloadUnitSlider();
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
        _unitSlider.value = _myShaft.test; // unitsWorkers.Count
        _unitSlider.onValueChanged.AddListener( delegate{UnitManagment();} );
    }


    // Unit managment via slider - TODO
    private void UnitManagment()
    {
        if (_unitSlider.value > _myShaft.test) // unitsWorkers.Count
        {
            Debug.Log("Add Worker"); // _myShaft.RemoveWorkerViaSlider();
            _myShaft.test++;
        }

        if (_unitSlider.value < _myShaft.test) // unitsWorkers.Count
        {
            Debug.Log("Remove worker"); // _myShaft.AddWorkerViaSlider();
            _myShaft.test--;
        }

        ReloadUnitSlider();
    }


    // Upgrade - extends capacity
    public void Upgrade()
    {
        _myShaft.Upgrade();
        ReloadUnitSlider();
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
