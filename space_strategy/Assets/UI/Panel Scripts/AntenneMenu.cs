﻿using UnityEngine;
using UnityEngine.UI;

public class AntenneMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    [SerializeField] public Button resourceDropButton;
    [SerializeField] public Button impulseAttackButton;

    [SerializeField] public Image resourceDropProgressImage;
    [SerializeField] public Image impulseAttackProgressImage;


    public Antenne _myAntenne;

    // Button activation managment
    public void ReloadButtonManage()
    {
        Debug.Log("ReloadButtonManage");
        
        if (GameHendler.Instance.CheckForResourceDropTimer())
        {
            Debug.Log("Interactible");
            resourceDropButton.interactable = ResourceManager.Instance.IsPowerOn();
            resourceDropProgressImage.fillAmount = 1;
        }
        else
        {
            resourceDropButton.interactable = false;
        }

        if (GameHendler.Instance.CheckFromImpulseAttackTimer())
        {
            impulseAttackButton.interactable = ResourceManager.Instance.IsPowerOn();
            impulseAttackProgressImage.fillAmount = 1;
        }
        else
        {
            impulseAttackButton.interactable = false;
        }
    }

    // Reload panel with new info
    public void ReloadPanel()
    {
        _myAntenne = ResourceManager.Instance.antenneReference;
        _myAntenne.isMenuOpened = true;
        
        ReloadSlidersHP_SP();
        ReloadButtonManage();
    }

    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = ResourceManager.Instance.antenneReference.maxCurrentHealthPoints;
        _HPslider.value = ResourceManager.Instance.antenneReference.healthPoints;

        _SPslider.maxValue = ResourceManager.Instance.antenneReference.maxCurrentShieldPoints;
        _SPslider.value = ResourceManager.Instance.antenneReference.shieldPoints;
    }
    

    public void ResourceDrop()
    {
        resourceDropButton.interactable = false;
        GameHendler.Instance.ResourceDrop();
    }

    public void ImpusleAttack()
    {
        impulseAttackButton.interactable = false;
        GameHendler.Instance.ImpusleAttack();
    }


    // Destroy building
    public void DestroyBuilding()
    {
        Debug.Log("Destroy building!");

        ExitMenu();

        ResourceManager.Instance.antenneReference.DestroyBuilding();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        _myAntenne.isMenuOpened = false;
    }

}