using UnityEngine;
using UnityEngine.UI;

public class GarageMenu : MonoBehaviour
{
    private Garage _myGarage = null;

    [SerializeField] private Text _garageName;

    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;    

    [SerializeField] private Button createUnitButton;

    [SerializeField] private Image unitImag1;
    [SerializeField] private Image unitImag2;
    [SerializeField] private Image unitImag3;
    [SerializeField] private Image unitImag4;
    [SerializeField] private Image unitImag5;

    [SerializeField] public Image loadingBar;


    public void InitUnitCostButton(int crystalNeed, int ironNeed, int gelNeed)
    {
        createUnitButton.GetComponentInChildren<Text>().text = crystalNeed.ToString() + " " + ironNeed.ToString() + " " + gelNeed.ToString();
    }

    public void CreateUnit()
    {
        int crystalsNeed = 0;
        int ironNeed = 0;
        int gelNeed = 0;

        StatsManager.GetResourcesNeedToCreateUnit___Garage(out crystalsNeed, out ironNeed, out gelNeed);

        if (!ResourceManager.Instance.ChecResources(crystalsNeed, ironNeed, gelNeed))
        {
            Debug.Log("Not enough resources!");
            return;
        }

        ResourceManager.Instance.DeleteResourcesAfterAction___1PressAction(crystalsNeed, ironNeed, gelNeed);



        _myGarage.StartUnitCreation();

        switch(_myGarage.garageData._clicks)
        {
            case 1:
                unitImag1.color = Color.yellow;
            break;

            case 2:
                unitImag2.color = Color.yellow;
            break;

            case 3:
                unitImag3.color = Color.yellow;
            break;

            case 4:
                unitImag4.color = Color.yellow;
            break;

            case 5:
                unitImag5.color = Color.yellow;
                createUnitButton.interactable = false;
            break;
        }
    }

    public void ReloadUnitManage()
    {
        if (_myGarage)
        {
            loadingBar.fillAmount = 0;

            unitImag1.color = Color.gray;
            unitImag2.color = Color.gray;
            unitImag3.color = Color.gray;
            unitImag4.color = Color.gray;
            unitImag5.color = Color.gray;

            switch (_myGarage.garageData._clicks)
            {
                case 1:
                unitImag1.color = Color.yellow;
                break;

                case 2:
                unitImag1.color = Color.yellow;
                unitImag2.color = Color.yellow;
                break;

                case 3:
                unitImag1.color = Color.yellow;
                unitImag2.color = Color.yellow;
                unitImag3.color = Color.yellow;
                break;

                case 4:
                unitImag1.color = Color.yellow;
                unitImag2.color = Color.yellow;
                unitImag3.color = Color.yellow;
                unitImag4.color = Color.yellow;
                break;
                
                case 5:
                unitImag1.color = Color.yellow;
                unitImag2.color = Color.yellow;
                unitImag3.color = Color.yellow;
                unitImag4.color = Color.yellow;
                unitImag5.color = Color.yellow;
                createUnitButton.interactable = false;
                break;
            }

            // Unit icons managment
            switch(_myGarage.garageData._garageMembers.Count)
            {
                // case 0:
                // break;

                case 1:
                unitImag1.color = Color.green;
                break;

                case 2:
                unitImag1.color = Color.green;
                unitImag2.color = Color.green;
                break;

                case 3:
                unitImag1.color = Color.green;
                unitImag2.color = Color.green;
                unitImag3.color = Color.green;
                break;

                case 4:
                unitImag1.color = Color.green;
                unitImag2.color = Color.green;
                unitImag3.color = Color.green;
                unitImag4.color = Color.green;
                break;

                case 5:
                unitImag1.color = Color.green;
                unitImag2.color = Color.green;
                unitImag3.color = Color.green;
                unitImag4.color = Color.green;
                unitImag5.color = Color.green;
                break;
            }

            // Button managment
            if (_myGarage.garageData._clicks != 5)
            {
                createUnitButton.interactable = true;
            }
            else
            {
                createUnitButton.interactable = false;
            }
        }
    }

    public void ReloadPanel(Garage garage)
    {
        _myGarage = garage;
        _myGarage.garageData._isMenuOpened = true;

        ReloadGarageName();
        ReloadSlidersHP_SP(_myGarage);
        ReloadUnitManage();
    }

    private void ReloadGarageName()
    {
        _garageName.text = "GARAGE - " + _myGarage.name;
    }

    public void ReloadSlidersHP_SP(AliveGameUnit gameUnit)
    {
        if (_myGarage)
        {
            if (_myGarage == gameUnit)
            {
                _HPslider.maxValue = _myGarage.maxCurrentHealthPoints;
                _HPslider.value = _myGarage.healthPoints;

                _SPslider.maxValue = _myGarage.maxCurrentShieldPoints;
                _SPslider.value = _myGarage.shieldPoints;
            }
        }
    }

    public void DestroyBuilding()
    {
        Garage garage = _myGarage;

        ExitMenu();

        garage.DestroyBuilding();
    }

    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myGarage.garageData._isMenuOpened = false;
        _myGarage = null;
    }
}