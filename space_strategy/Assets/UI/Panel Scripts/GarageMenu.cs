using UnityEngine;
using UnityEngine.UI;

public class GarageMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    
    [SerializeField] private Text _garageName;

    [SerializeField] public Button createUnitButton;

    [SerializeField] private Image unitImag1;
    [SerializeField] private Image unitImag2;
    [SerializeField] private Image unitImag3;
    [SerializeField] private Image unitImag4;
    [SerializeField] private Image unitImag5;
    [SerializeField] private Image loadingBar;

    private Garage _myGarage = null;



    // Update loading bar
    private void Update()
    {
        if (_myGarage.isCreationInProgress)
        {
            loadingBar.fillAmount = _myGarage.timerForCreatingUnit;
        }
    }


    // Button activation managment
    public void ReloadUnitManage()
    {
        loadingBar.fillAmount = 0;
        
        // Unit icons managment
        switch(_myGarage.garageMembers.Count)
        {
            case 0:
            {
                unitImag1.color = Color.gray;
                unitImag2.color = Color.gray;
                unitImag3.color = Color.gray;
                unitImag4.color = Color.gray;
                unitImag5.color = Color.gray;
            }
            break;

            case 1:
            {
                unitImag1.color = Color.green;
                unitImag2.color = Color.gray;
                unitImag3.color = Color.gray;
                unitImag4.color = Color.gray;
                unitImag5.color = Color.gray;
            }
            break;

            case 2:
            {
                unitImag1.color = Color.green;
                unitImag2.color = Color.green;
                unitImag3.color = Color.gray;
                unitImag4.color = Color.gray;
                unitImag5.color = Color.gray;
            }
            break;

            case 3:
            {
                unitImag1.color = Color.green;
                unitImag2.color = Color.green;
                unitImag3.color = Color.green;
                unitImag4.color = Color.gray;
                unitImag5.color = Color.gray;
            }
            break;

            case 4:
            {
                unitImag1.color = Color.green;
                unitImag2.color = Color.green;
                unitImag3.color = Color.green;
                unitImag4.color = Color.green;
                unitImag5.color = Color.gray;
            }
            break;

            case 5:
            {
                unitImag1.color = Color.green;
                unitImag2.color = Color.green;
                unitImag3.color = Color.green;
                unitImag4.color = Color.green;
                unitImag5.color = Color.green;
            }
            break;
        }

        // Button managment
        if (_myGarage.isCreationInProgress)
        {
            createUnitButton.interactable = false;
        }
        else if (_myGarage.garageMembers.Count != 5)
        {
            createUnitButton.interactable = true;
        }
        else
        {
            createUnitButton.interactable = false;
        }
    }

    // Reload panel with new info
    public void ReloadPanel(Garage garage)
    {
        _myGarage = garage;
        _myGarage.isMenuOpened = true;

        ReloadGarageName();
        ReloadSlidersHP_SP();
        ReloadUnitManage();
    }


    // Reload name of Garage
    private void ReloadGarageName()
    {
        _garageName.text = "GARAGE - " + _myGarage.name;
    }


    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myGarage.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myGarage.ShieldPoints;
    }


    // Create unit
    public void CreateUnit()
    {
        _myGarage.CreateUnit();
        createUnitButton.interactable = false;
    }


    // Destroy building
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building!");

        _myGarage.DestroyGarage();

        ExitMenu();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myGarage.isMenuOpened = false;
        _myGarage = null;
    }

}
