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
        // Initialize resource need variables;
        int crystalsNeed = 0;
        int ironNeed = 0;
        int gelNeed = 0;

        // Set resource need variables
        switch (buildingID)
        {
            case (int)IDconstants.IDgarage: // Garage
                Garage.GetResourcesNeedToBuild(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDturretLaser: // Turette
                TurretLaser.GetResourcesNeedToBuild(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDturretMisile: // Turette
                TurretMisile.GetResourcesNeedToBuild(out crystalsNeed, out ironNeed, out gelNeed);
            break;
                        
            case (int)IDconstants.IDgelShaft: // GelShaft
                MineShaft.GetResourcesNeedToBuild(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDcrystalShaft: // CrystalShaft
                MineShaft.GetResourcesNeedToBuild(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDironShaft: // IronShaft
                MineShaft.GetResourcesNeedToBuild(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDshieldGenerator: // Shield Generator
                ShieldGenerator.GetResourcesNeedToBuild(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDantenne: // Antenne
                Antenne.GetResourcesNeedToBuild(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDpowerPlant: // Power plant
                PowerPlant.GetResourcesNeedToBuild(out crystalsNeed, out ironNeed, out gelNeed);
            break;
        }



        


        // Check and than delete resource need variables
        if (ResourceManager.Instance.ChecResources(crystalsNeed, ironNeed, gelNeed))
        {
            ResourceManager.Instance.StoreResourceNeed(crystalsNeed, ironNeed, gelNeed);



            GameHendler.Instance.buildingModel.InitModel(buildingID); // Refer to UI button, ID(1/2/3) will change (switch)
            GameHendler.Instance.ResetCurrentHexAndSelectedHex();
            GameHendler.Instance.currentState = GameHendler.Instance.BM_idleState;
            // Debug.Log("Building_MODE");

            UIPannelManager.Instance.ResetPanels("ModelMenu");
        }

        else
        {
            Debug.Log("Not enogh resources!");
        }
    }


    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
    }
}
