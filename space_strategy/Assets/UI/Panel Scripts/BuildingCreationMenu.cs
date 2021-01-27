using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreationMenu : MonoBehaviour
{
    [SerializeField] private GameObject industrialPanel;
    [SerializeField] private GameObject militaryPanel;

    public void ChangePanelToIndustrial()
    {
        // Turn off Military Panel and activate Industrial
        militaryPanel.SetActive(false);
        industrialPanel.SetActive(true);
    }

    public void ChangePanelToMilitary()
    {
        // Turn off Industrial Panel and activate Military
        industrialPanel.SetActive(false);
        militaryPanel.SetActive(true);
    }

    public void InitModelViaButton(int buildingID)
    {
        Debug.Log("Selected building ID - " + buildingID);

        if (GameHendler.Instance.SelectedHex) // this pannel can be only opened if Selected hex != null
        {
            GameHendler.Instance.buildingModel.InitModel(buildingID); // Refer to UI button, ID(1/2/3) will change (switch)
            GameHendler.Instance.ResetCurrentHexAndSelectedHex();
            GameHendler.Instance.currentState = GameHendler.Instance.BM_idleState;
            Debug.Log("Building_MODE");
        }
    
        // turn off all panels and open model Panel
        Debug.Log("ModelMenu");
        UIPannelManager.Instance.ResetPanels("ModelMenu");
    }

    public void ExitMenu()
    {
        Debug.Log("Exit Menu");
        UIPannelManager.Instance.ResetPanels("GameView");
    }
}
