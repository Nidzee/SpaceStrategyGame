using UnityEngine;
using UnityEngine.UI;

public class ShaftMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    [SerializeField] private Slider _unitSlider;

    [SerializeField] private Text _shaftName;
    [SerializeField] private Text _unitCount;

    [SerializeField] public Button _upgradeButton;
    [SerializeField] private Button _destroyBuildingButton;

    private MineShaft _myShaft = null;





    [SerializeField] private Image level1;
    [SerializeField] private Image level2;
    [SerializeField] private Image level3;




    private void Update()
    {
        if (_myShaft.isUpgradeInProgress) // Reload loading bar
        {
            switch(_myShaft.level)
            {
                case 1:
                {
                    level2.fillAmount = _myShaft.upgradeTimer;
                }
                break;

                case 2:
                {
                    level3.fillAmount = _myShaft.upgradeTimer;
                }
                break;

                case 3:
                {
                    Debug.Log("Error");
                }
                break;
            }
        }
    }

    // Button activation managment
    public void ReloadLevelManager()
    {
        // Set visual fill amount
        switch (_myShaft.level)
        {
            case 1:
            {
                level1.fillAmount = 1;
                level2.fillAmount = 0;
                level3.fillAmount = 0;
            }
            break;

            case 2:
            {
                level1.fillAmount = 1;
                level2.fillAmount = 1;
                level3.fillAmount = 0;
            }
            break;

            case 3:
            {
                level1.fillAmount = 1;
                level2.fillAmount = 1;
                level3.fillAmount = 1;
            }
            break;
        }

        // Reloads upgrade button
        if (_myShaft.isUpgradeInProgress)
        {
            _upgradeButton.interactable = false;
        }
        else if (_myShaft.level != 3)
        {
            _upgradeButton.interactable = true;
        }
        else
        {
            _upgradeButton.interactable = false;
        }
    }

    // Reload panel
    public void ReloadPanel(MineShaft shaft)
    {
        _myShaft = shaft;
        _myShaft.isMenuOpened = true;

        ReloadShaftName();
        ReloadSlidersHP_SP();
        ReloadUnitSlider();
        ReloadLevelManager();
    }


    // Reloads shaft name
    private void ReloadShaftName()
    {
        _shaftName.text = "SHAFT - " + _myShaft.name;
    }


    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myShaft.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myShaft.ShieldPoints;
    }


    // Reload Main unit slider
    public void ReloadUnitSlider()
    {
        _unitSlider.onValueChanged.RemoveAllListeners();


        _unitSlider.maxValue = _myShaft.capacity;
        _unitSlider.value = _myShaft.unitsWorkers.Count;
        _unitCount.text = _myShaft.unitsWorkers.Count.ToString() +"/"+_myShaft.capacity.ToString();


        _unitSlider.onValueChanged.AddListener( delegate{UnitManagment();} );
    }


    // Unit managment via slider - TODO
    private void UnitManagment()
    {
        if (_unitSlider.value > _myShaft.unitsWorkers.Count)
        {
            _myShaft.AddWorkerViaSlider();
        }

        if (_unitSlider.value < _myShaft.unitsWorkers.Count)
        {
            _myShaft.RemoveWorkerViaSlider();
        }

        ReloadUnitSlider();
    }


    // Upgrade - extends capacity
    public void Upgrade()
    {
        _myShaft.Upgrade();
        _upgradeButton.interactable = false;
    }


    // Destroy building logic - TODO
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building by Button!");

        _myShaft.DestroyShaft();

        ExitMenu();
    }


    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myShaft.isMenuOpened = false;
        _myShaft = null;
    }
}
