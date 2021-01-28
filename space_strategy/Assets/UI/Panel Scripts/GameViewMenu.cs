using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameViewMenu : MonoBehaviour
{
    [SerializeField] private Button buildingCreationMenuButton;

    private bool isInteractible = false;


    // Button activation managment
    private void Update()
    {
        if (GameHendler.Instance.SelectedHex != null && !isInteractible)
        {
            buildingCreationMenuButton.interactable = true;
            isInteractible = true;
        }
        else if (!GameHendler.Instance.SelectedHex && isInteractible)
        {
            buildingCreationMenuButton.interactable = false;
            isInteractible = false;
        }
    }


    // Opens Building Creation Menu
    public void BuildingCreationMenu()
    {
        UIPannelManager.Instance.ResetPanels("BuildingCreationMenu");
    }


    // Opens Units Menu - TODO
    public void UnitMenu()
    {
        Debug.Log("UnitMenu - NOT READY YET!");
    }


    // Opens Buildings Menu - TODO
    public void BuildingsMenu()
    {
        Debug.Log("BuildingsMenu - NOT READY YET!");
    }
}
