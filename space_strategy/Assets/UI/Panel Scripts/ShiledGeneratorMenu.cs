﻿using UnityEngine;
using UnityEngine.UI;

public class ShiledGeneratorMenu : MonoBehaviour
{
    private ShieldGenerator _myShieldGenerator = null;

    [SerializeField] private Text _shieldGeneratorName;

    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    public Button ONbutton;
    public Button OFFbutton;















    [SerializeField] private Button upgradeButton;

    [SerializeField] private Image level1;
    [SerializeField] private Image level2;
    [SerializeField] private Image level3;


    private void Update()
    {
        if (_myShieldGenerator.isUpgradeInProgress) // Reload loading bar
        {
            switch(_myShieldGenerator.level)
            {
                case 1:
                {
                    level2.fillAmount = _myShieldGenerator.upgradeTimer;
                }
                break;

                case 2:
                {
                    level3.fillAmount = _myShieldGenerator.upgradeTimer;
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
        switch (_myShieldGenerator.level)
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
        if (_myShieldGenerator.isUpgradeInProgress)
        {
            upgradeButton.interactable = false;
        }
        else if (_myShieldGenerator.level != 3)
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }
    }

    // Upgrade - TODO
    public void Upgrade()
    {
        _myShieldGenerator.Upgrade();
        upgradeButton.interactable = false;
    }




















    private void Reload_ON_OFF_buttons()
    {
        if (_myShieldGenerator.shieldGeneratorRangeRef)
        {
            // If shieldRange is in progress
            if (_myShieldGenerator.isShieldCreationInProgress || _myShieldGenerator.isDisablingInProgress)
            {
                OFFbutton.interactable = false;
                ONbutton.interactable = false;
            }
            // If Shield is working stable
            else
            {
                OFFbutton.interactable = true;
                ONbutton.interactable = false;
            }
        }

        // The shield is turned off
        else
        {
            OFFbutton.interactable = false;
            ONbutton.interactable = true;
        }
    }

    // Reload panel with new info
    public void ReloadPanel(ShieldGenerator shieldGenerator)
    {
        _myShieldGenerator = shieldGenerator;
        _myShieldGenerator.isMenuOpened = true;

        ReloadName();
        ReloadSlidersHP_SP();
        Reload_ON_OFF_buttons();
        ReloadLevelManager();
    }

    // Reload name of Garage
    private void ReloadName()
    {
        _shieldGeneratorName.text = _myShieldGenerator.name;
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myShieldGenerator.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myShieldGenerator.ShieldPoints;
    }




    // Turns shield on
    public void TurnShieldOn()
    {
        ONbutton.interactable = false;
        OFFbutton.interactable = false;
        _myShieldGenerator.EnableShield();
    }

    // Turns shield off
    public void TurnShieldOff()
    {
        ONbutton.interactable = false;
        OFFbutton.interactable = false;
        _myShieldGenerator.DisableShield();
    }



    // Destroy building
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building!");

        ShieldGenerator sg = _myShieldGenerator;

        ExitMenu();

        sg.DestroyShieldGenerator();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myShieldGenerator.isMenuOpened = false;
        _myShieldGenerator = null;
    }

}
