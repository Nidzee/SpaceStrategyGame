using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameViewMenu : MonoBehaviour
{
    [SerializeField] private Button buildingCreationMenuButton;

    private bool isCreateBuildingButtonInteractible = false;


    // Button activation managment
    private void Update()
    {
        if (GameHendler.Instance.SelectedHex != null && !isCreateBuildingButtonInteractible)
        {
            buildingCreationMenuButton.interactable = true;
            isCreateBuildingButtonInteractible = true;
        }
        else if (!GameHendler.Instance.SelectedHex && isCreateBuildingButtonInteractible)
        {
            buildingCreationMenuButton.interactable = false;
            isCreateBuildingButtonInteractible = false;
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
        UIPannelManager.Instance.ResetPanels("UnitManageMenu");
        
        GameHendler.Instance.unitManageMenuReference.ReloadPanel();
    }

    // Opens Buildings Menu - TODO
    public void BuildingsManagmentMenu()
    {
        UIPannelManager.Instance.ResetPanels("BuildingsManageMenu");
        
        GameHendler.Instance.buildingsManageMenuReference.ReloadPanel();
    }
}
