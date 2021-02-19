using UnityEngine;
using UnityEngine.UI;

public class TurretMenu : MonoBehaviour
{
    private Turette _myTurret = null;

    [SerializeField] private Text _turretName;

    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    

















    [SerializeField] private Button _upgradeButton;

    [SerializeField] public Image level1;
    [SerializeField] public Image level2;
    [SerializeField] public Image level3;


    

    // Button activation managment
    public void ReloadLevelManager()
    {
        // Set visual fill amount
        switch (_myTurret.level)
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
        if (_myTurret.upgradeTimer != 0)
        {
            _upgradeButton.interactable = false;
        }
        else if (_myTurret.level != 3)
        {
            _upgradeButton.interactable = true;
        }
        else
        {
            _upgradeButton.interactable = false;
        }
    }

    // Upgrade - extends capacity
    public void Upgrade()
    {
        _myTurret.StartUpgrade();
        _upgradeButton.interactable = false;
    }























    // Reload panel with new turret
    public void ReloadPanel(Turette turret)
    {
        _myTurret = turret;
        _myTurret.isMenuOpened = true;
        
        ReloadInfo();
        ReloadSlidersHP_SP();
        ReloadLevelManager();
    }

    // Reloads name and info about current turret
    private void ReloadInfo()
    {
        _turretName.text = _myTurret.name;
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myTurret.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myTurret.ShieldPoints;
    }



    // Destroy building logic - TODO
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building by Button!");

        Turette temp = _myTurret;

        ExitMenu();

        temp.DestroyTurret();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myTurret.isMenuOpened = false;
        _myTurret = null;
    }
}
