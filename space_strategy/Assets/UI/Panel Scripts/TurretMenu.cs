using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TurretMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    
    [SerializeField] private Text _turretName;
    [SerializeField] private Text _infoPanelText;

    [SerializeField] private Button _upgradeButton;

    private Turette _myTurret = null;

    private bool _isUpgradeButtonInteractible = true;


    // Button activation managment
    private void Update()
    {
        if (_myTurret)
        {
            if (_myTurret.level < 3 && !_isUpgradeButtonInteractible)
            {
                _upgradeButton.interactable = true;
                _isUpgradeButtonInteractible = true;
            }
            else if (_isUpgradeButtonInteractible && _myTurret.level == 3)
            {
                _upgradeButton.interactable = false;
                _isUpgradeButtonInteractible = false;
            }
        }
    }


    // Reload panel with new turret
    public void ReloadPanel(Turette turret)
    {
        _myTurret = turret;
        
        ReloadInfo();
        ReloadSlidersHP_SP();
    }


    // Reloads name and info about current turret
    private void ReloadInfo()
    {
        _turretName.text = _myTurret.name;
        _infoPanelText.text = "Turret level - " + _myTurret.level;
    }


    // Reload HP and SP
    private void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myTurret.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myTurret.ShieldPoints;
    }


    // Upgrade turret logic
    public void UpgradeTurret()
    {
        _myTurret.Upgrade();

        _infoPanelText.text = "Turret level - " + _myTurret.level;  
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
