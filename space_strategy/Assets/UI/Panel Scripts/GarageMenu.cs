using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GarageMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    [SerializeField] private Text _unitCounter;

    [SerializeField] private Button createUnitButton;
    [SerializeField] private Button DestroyBuildingButton;


    private Garage _myGarage = null;



    private bool isCreateUnitButtonInteractible = true;


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



    public void ReloadPanel(Garage garage)
    {
        _myGarage = garage;
        
        ReloadSlidersHP_SP();
        ReloadUnitImage();
    }

    private void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myGarage.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myGarage.ShieldPoints;
    }

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
                isCreateUnitButtonInteractible = false;
            break;
        }
    }

    public void CreateUnit()
    {
        _myGarage.CreateUnit();
        ReloadUnitImage();
    }

    public void DestroyBuilding()
    {
        Debug.Log("Destroy building!");
    }

    public void ExitMenu()
    {
        Debug.Log("Exit Menu");
        UIPannelManager.Instance.ResetPanels("GameView");
    }

}
