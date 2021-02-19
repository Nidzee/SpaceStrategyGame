using UnityEngine;
using UnityEngine.UI;

public class AntenneMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;

    [SerializeField] public Button resourceDropButton;
    [SerializeField] public Button impulseAttackButton;

    [SerializeField] public Image resourceDropProgressImage;
    [SerializeField] public Image impulseAttackProgressImage;







    // // Update loading bar
    // private void Update()
    // {
    //     if (!GameHendler.Instance.CheckForResourceDropTimer()) // Reload loading bar
    //     {
    //         resourceDropProgressImage.fillAmount = GameHendler.Instance.resourceDropTimer;
    //     }

    //     if (!GameHendler.Instance.CheckFromImpulseAttackTimer()) // Reload loading bar
    //     {
    //         impulseAttackProgressImage.fillAmount = GameHendler.Instance.impulsAttackTimer;
    //     }
    // }


    // Button activation managment
    public void ReloadButoonManage()
    {
        Debug.Log("ReloadButoonManage");
        
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
        ResourceManager.Instance.antenneReference.isMenuOpened = true;
        
        ReloadSlidersHP_SP();
        ReloadButoonManage();
    }


    // Reload HP and SP
    public void ReloadSlidersHP_SP()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = ResourceManager.Instance.antenneReference.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = ResourceManager.Instance.antenneReference.ShieldPoints;
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

        ResourceManager.Instance.antenneReference.DestroyAntenne();
    }

    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
        ResourceManager.Instance.antenneReference.isMenuOpened = false;
    }

}