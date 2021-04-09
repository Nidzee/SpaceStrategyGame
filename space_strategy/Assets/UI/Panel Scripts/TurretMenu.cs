using UnityEngine;
using UnityEngine.UI;

public class TurretMenu : MonoBehaviour
{
    private Turette _myTurret = null;

    [SerializeField] private Text _turretName;

    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    

















    [SerializeField] public Button _upgradeButton;

    [SerializeField] public Image level1;
    [SerializeField] public Image level2;
    [SerializeField] public Image level3;


    

    // Button activation managment
    public void ReloadLevelManager()
    {
        if (_myTurret)
        {
            // Set visual fill amount
            switch (_myTurret.level)
            {
                case 1:
                {
                    StatsManager.InitCost_ToLvl2___MisileTurret();
                    level1.fillAmount = 1;
                    level2.fillAmount = 0;
                    level3.fillAmount = 0;
                }
                break;

                case 2:
                {
                    StatsManager.InitCost_ToLvl3___MisileTurret();
                    level1.fillAmount = 1;
                    level2.fillAmount = 1;
                    level3.fillAmount = 0;
                }
                break;

                case 3:
                {
                    TurretStaticData.turretMenuReference._upgradeButton.GetComponentInChildren<Text>().text = "Maximum level reached.";
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
    }

    // Upgrade - extends capacity
    public void Upgrade()
    {
        int crystalsNeed = 0;
        int ironNeed = 0;
        int gelNeed = 0;

        if (_myTurret.type == 1) // Laser
        {
            StatsManager.GetResourcesNeedToExpand___LaserTurret(out crystalsNeed, out ironNeed, out gelNeed, (TurretLaser)_myTurret);
        }
        else if (_myTurret.type == 2) // Misile
        {
            StatsManager.GetResourcesNeedToExpand___MisileTurret(out crystalsNeed, out ironNeed, out gelNeed, (TurretMisile)_myTurret);
        }
        else
        {
            Debug.Log("Invalid turret type");
            return;
        }

        if (!ResourceManager.Instance.ChecResources(crystalsNeed, ironNeed, gelNeed))
        {
            Debug.Log("Not enough resources!");
            return;
        }

        // Delete resources here
        ResourceManager.Instance.DeleteResourcesAfterAction___1PressAction(crystalsNeed, ironNeed, gelNeed);


        _myTurret.StartUpgrade();
        _upgradeButton.interactable = false;
    }


















    public void ReloadTurretLevelVisuals(Turette newTurret)
    {
        _myTurret = newTurret;
        _myTurret.isMenuOpened = true;
        
        Debug.Log("Reloading visuals");
        ReloadInfo();
        ReloadSlidersHP_SP(_myTurret);
        ReloadLevelManager();
    }




    // Reload panel with new turret
    public void ReloadPanel(Turette turret)
    {
        _myTurret = turret;
        _myTurret.isMenuOpened = true;
        
        ReloadInfo();
        ReloadSlidersHP_SP(_myTurret);
        ReloadLevelManager();
    }

    // Reloads name and info about current turret
    private void ReloadInfo()
    {
        if (_myTurret)
        {
            _turretName.text = _myTurret.name;
        }
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP(AliveGameUnit gameUnit)
    {
        if (_myTurret)
        {
            if (_myTurret == gameUnit)
            {
                _HPslider.maxValue = _myTurret.maxCurrentHealthPoints;
                _HPslider.value = _myTurret.healthPoints;

                _SPslider.maxValue = _myTurret.maxCurrentShieldPoints;
                _SPslider.value = _myTurret.shieldPoints;
            }
        }
    }



    // Destroy building logic - TODO
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building by Button!");

        Turette temp = _myTurret;

        ExitMenu();

        temp.DestroyBuilding();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myTurret.isMenuOpened = false;
        _myTurret = null;
    }
}
