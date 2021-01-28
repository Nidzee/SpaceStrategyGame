using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GarageMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    [SerializeField] private Text _unitCounter;
    [SerializeField] private Text _garageName;

    [SerializeField] private Button createUnitButton;

    private Garage _myGarage = null;

    private bool isCreateUnitButtonInteractible = false;


    // Button activation managment
    private void Update()
    {
        if (_myGarage)
        {
            if (_myGarage.test < 5 && !isCreateUnitButtonInteractible)
            {
                createUnitButton.interactable = true;
                isCreateUnitButtonInteractible = true;
            }
            else if (isCreateUnitButtonInteractible && _myGarage.test == 5)
            {
                createUnitButton.interactable = false;
                isCreateUnitButtonInteractible = false;
            }
        }
    }


    // Reload panel with new info
    public void ReloadPanel(Garage garage)
    {
        _myGarage = garage;
        
        ReloadInfo();
        ReloadSlidersHP_SP();
        ReloadUnitImage();
    }


    // Reload name of Garage
    private void ReloadInfo()
    {
        _garageName.text = _myGarage.name;
    }


    // Reload HP and SP
    private void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myGarage.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myGarage.ShieldPoints;
    }


    // Reload image aquoting to capacity of garage
    private void ReloadUnitImage()
    {
        Debug.Log("Reload Units Image 1/2/3/4/5 !");
        switch(_myGarage.test)
        {
            case 0:
                _unitCounter.text = "No units!";
            break;

            case 1:
                _unitCounter.text = "Unit - 1";
            break;

            case 2:
                _unitCounter.text = "Units - 2";
            break;

            case 3:
                _unitCounter.text = "Units - 3";
            break;

            case 4:
                _unitCounter.text = "Units - 4";
            break;

            case 5:
                _unitCounter.text = "Units - 5";
            break;
        }
    }


    // Create unit
    public void CreateUnit()
    {
        _myGarage.CreateUnit();
        ReloadUnitImage();
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
