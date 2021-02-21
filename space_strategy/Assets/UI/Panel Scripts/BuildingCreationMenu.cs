using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCreationMenu : MonoBehaviour
{
    [SerializeField] private GameObject industrialPanel;
    [SerializeField] private GameObject militaryPanel;


    [SerializeField] private Button antenneCreationButton;
    [SerializeField] private Button crsytalShaftCreationButton;
    [SerializeField] private Button ironShaftCreationButton;
    [SerializeField] private Button gelShaftCreationButton;
    [SerializeField] private Button garageCreationButton;
    [SerializeField] private Button powerPlantCreationButton;
    [SerializeField] private Button shieldGeneratorCreationButton;
    [SerializeField] private Button laserTurretCreationButton;
    [SerializeField] private Button misileTurretCreationButton;


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

    public void InitBuildingsCosts()
    {
        antenneCreationButton.GetComponentInChildren<Text>().text = "Antenne   " + Antenne.GetResourcesNeedToBuildAsText();

        crsytalShaftCreationButton.GetComponentInChildren<Text>().text = "Crystal Shaft   " + MineShaft.GetResourcesNeedToBuildAsText();

        ironShaftCreationButton.GetComponentInChildren<Text>().text = "Iron Shaft   " + MineShaft.GetResourcesNeedToBuildAsText();

        gelShaftCreationButton.GetComponentInChildren<Text>().text = "Gel Shaft   " + MineShaft.GetResourcesNeedToBuildAsText();

        garageCreationButton.GetComponentInChildren<Text>().text = "Garage   " + Garage.GetResourcesNeedToBuildAsText();

        powerPlantCreationButton.GetComponentInChildren<Text>().text = "PowerPlant   " + PowerPlant.GetResourcesNeedToBuildAsText();

        shieldGeneratorCreationButton.GetComponentInChildren<Text>().text = "ShieldGenerator   " + ShieldGenerator.GetResourcesNeedToBuildAsText();

        laserTurretCreationButton.GetComponentInChildren<Text>().text = "TurretLaser   " + TurretLaser.GetResourcesNeedToBuildAsText();

        misileTurretCreationButton.GetComponentInChildren<Text>().text = "TurretMisile   " + TurretMisile.GetResourcesNeedToBuildAsText();
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
