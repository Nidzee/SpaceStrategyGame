using UnityEngine;
using UnityEngine.UI;

public class ShaftMenu : MonoBehaviour
{
    private MineShaft _myShaft = null;

    [SerializeField] private Text _shaftName;

    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    [SerializeField] private Slider _unitSlider;
    [SerializeField] private Text _unitCount;

    [SerializeField] public Button _upgradeButton;

    [SerializeField] public Image level1;
    [SerializeField] public Image level2;
    [SerializeField] public Image level3;


    // Button activation managment
    public void ReloadShaftLevelVisuals()
    {
        if (_myShaft)
        {
            // Set visual fill amount
            switch (_myShaft.mineShaftData.level)
            {
                case 1:
                {
                    level1.fillAmount = 1;
                    level2.fillAmount = 0;
                    level3.fillAmount = 0;
                    _upgradeButton.interactable = true;
                }
                break;

                case 2:
                {
                    level1.fillAmount = 1;
                    level2.fillAmount = 1;
                    level3.fillAmount = 0;
                    _upgradeButton.interactable = true;
                }
                break;

                case 3:
                {
                    level1.fillAmount = 1;
                    level2.fillAmount = 1;
                    level3.fillAmount = 1;
                    _upgradeButton.interactable = false;
                }
                break;
            }

            // // Reloads upgrade button
            if (_myShaft.mineShaftData.upgradeTimer != 0)
            {
                _upgradeButton.interactable = false;
            }
            // else if (_myShaft.level != 3)
            // {
            //     _upgradeButton.interactable = true;
            // }
            // else
            // {
            //     _upgradeButton.interactable = false;
            // }
        }
    }

    // Upgrade - extends capacity
    public void Upgrade()
    {
        int crystalsNeed = 0;
        int ironNeed = 0;
        int gelNeed = 0;

        _myShaft.mineShaftData.GetResourcesNeedToExpand(out crystalsNeed, out ironNeed, out gelNeed);

        if (!ResourceManager.Instance.ChecResources(crystalsNeed, ironNeed,gelNeed))
        {
            Debug.Log("Not enough resources!");
            return;
        }

        ResourceManager.Instance.DeleteResourcesAfterAction___1PressAction(crystalsNeed, ironNeed, gelNeed);



        _myShaft.StartUpgrade();
        _upgradeButton.interactable = false;
    }



    // Reload panel
    public void ReloadPanel(MineShaft shaft)
    {
        _myShaft = shaft;
        _myShaft.mineShaftData.isMenuOpened = true;

        ReloadShaftName();
        ReloadSlidersHP_SP(_myShaft.gameUnit);
        ReloadUnitSlider();
        ReloadShaftLevelVisuals();
    }

    // Reloads shaft name
    private void ReloadShaftName()
    {
        _shaftName.text = "SHAFT - " + _myShaft.name;
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP(GameUnit gameUnit)
    {
        if (_myShaft)
        {
            _HPslider.maxValue = _myShaft.gameUnit.maxCurrentHealthPoints;
            _HPslider.value = _myShaft.gameUnit.healthPoints;

            _SPslider.maxValue = _myShaft.gameUnit.maxCurrentShieldPoints;
            _SPslider.value = _myShaft.gameUnit.shieldPoints;
        }
    }

    // Reload Main unit slider
    public void ReloadUnitSlider()
    {
        if (_myShaft)
        {
            _unitSlider.onValueChanged.RemoveAllListeners();

            _unitSlider.maxValue = _myShaft.mineShaftData.capacity;
            _unitSlider.value = _myShaft.mineShaftData.unitsWorkers.Count;
            _unitCount.text = _myShaft.mineShaftData.unitsWorkers.Count.ToString() + "/" +_myShaft.mineShaftData.capacity.ToString();

            _unitSlider.onValueChanged.AddListener( delegate{UnitManagment();} );
        }
    }

    // Unit managment via slider - TODO
    private void UnitManagment()
    {
        if (_unitSlider.value > _myShaft.mineShaftData.unitsWorkers.Count)
        {
            _myShaft.AddWorkerViaSlider();
        }

        if (_unitSlider.value < _myShaft.mineShaftData.unitsWorkers.Count)
        {
            _myShaft.RemoveWorkerViaSlider();
        }

        ReloadUnitSlider();
    }



    // Destroy building logic - TODO
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building by Button!");

        MineShaft temp = _myShaft;

        ExitMenu();

        temp.DestroyBuilding();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myShaft.mineShaftData.isMenuOpened = false;
        _myShaft = null;
    }
}
