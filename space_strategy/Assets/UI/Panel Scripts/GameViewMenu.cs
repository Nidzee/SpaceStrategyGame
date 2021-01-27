using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameViewMenu : MonoBehaviour
{
    [SerializeField] private Button unitMenuButton;
    [SerializeField] private Button buildingsMenuButton;
    [SerializeField] private Button buildingCreationMenuButton;

    private bool isInteractible = false;


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

    public void BuildingCreationMenu()
    {
        Debug.Log("BuildingCreationMenu");
        UIPannelManager.Instance.ResetPanels("BuildingCreationMenu");
    }

    public void UnitMenu()
    {
        Debug.Log("UnitMenu");
        Debug.Log("NOT READY YET!");
    }

    public void BuildingsMenu()
    {
        Debug.Log("BuildingsMenu");
        Debug.Log("NOT READY YET!");
    }
}
