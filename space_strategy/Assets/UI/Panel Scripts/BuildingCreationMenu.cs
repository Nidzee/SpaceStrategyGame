using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreationMenu : MonoBehaviour
{
    [SerializeField] private GameObject industrialPanel;
    [SerializeField] private GameObject militaryPanel;


    // Turn off Military Panel and activate Industrial
    public void ChangePanelToIndustrial()
    {
        militaryPanel.SetActive(false);
        industrialPanel.SetActive(true);
    }


    // Turn off Industrial Panel and activate Military
    public void ChangePanelToMilitary()
    {
        industrialPanel.SetActive(false);
        militaryPanel.SetActive(true);
    }


    // Initiate model - start creating model process
    public void InitModelViaButton(int buildingID)
    {
        GameHendler.Instance.buildingModel.InitModel(buildingID); // Refer to UI button, ID(1/2/3) will change (switch)
        GameHendler.Instance.ResetCurrentHexAndSelectedHex();
        GameHendler.Instance.currentState = GameHendler.Instance.BM_idleState;
        Debug.Log("Building_MODE");

        UIPannelManager.Instance.ResetPanels("ModelMenu");
    }


    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
    }
}
