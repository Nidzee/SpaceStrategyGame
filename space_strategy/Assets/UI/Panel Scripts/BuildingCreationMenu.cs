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


    public void ChangePanelToIndustrial()
    {
        militaryPanel.SetActive(false);
        industrialPanel.SetActive(true);
    }

    public void ChangePanelToMilitary()
    {
        industrialPanel.SetActive(false);
        militaryPanel.SetActive(true);
    }

    public void InitBuildingsCosts()
    {
        antenneCreationButton.GetComponentInChildren<Text>().text = "Antenne   " + StatsManager.GetResourcesNeedToBuildAsText___Antenne();

        crsytalShaftCreationButton.GetComponentInChildren<Text>().text = "Crystal Shaft   " + StatsManager.GetResourcesNeedToBuildAsText___MineShaft();

        ironShaftCreationButton.GetComponentInChildren<Text>().text = "Iron Shaft   " + StatsManager.GetResourcesNeedToBuildAsText___MineShaft();

        gelShaftCreationButton.GetComponentInChildren<Text>().text = "Gel Shaft   " + StatsManager.GetResourcesNeedToBuildAsText___MineShaft();

        garageCreationButton.GetComponentInChildren<Text>().text = "Garage   " + StatsManager.GetResourcesNeedToBuildAsText___Garage();

        powerPlantCreationButton.GetComponentInChildren<Text>().text = "PowerPlant   " + StatsManager.GetResourcesNeedToBuildAsText___PowerPlant();

        shieldGeneratorCreationButton.GetComponentInChildren<Text>().text = "ShieldGenerator   " + StatsManager.GetResourcesNeedToBuildAsText___ShieldGenerator();

        laserTurretCreationButton.GetComponentInChildren<Text>().text = "TurretLaser   " + StatsManager.GetResourcesNeedToBuildAsText___LaserTurret();

        misileTurretCreationButton.GetComponentInChildren<Text>().text = "TurretMisile   " + StatsManager.GetResourcesNeedToBuildAsText___MisileTurret();
    }

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
                StatsManager.GetResourcesNeedToBuild___Garage(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDturretLaser: // Turette
                StatsManager.GetResourcesNeedToBuild___LaserTurret(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDturretMisile: // Turette
                StatsManager.GetResourcesNeedToBuild___MisileTurret(out crystalsNeed, out ironNeed, out gelNeed);
            break;
                        
            case (int)IDconstants.IDgelShaft: // GelShaft
                StatsManager.GetResourcesNeedToBuild___MineShaft(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDcrystalShaft: // CrystalShaft
                StatsManager.GetResourcesNeedToBuild___MineShaft(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDironShaft: // IronShaft
                StatsManager.GetResourcesNeedToBuild___MineShaft(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDshieldGenerator: // Shield Generator
                StatsManager.GetResourcesNeedToBuild___ShieldGenerator(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDantenne: // Antenne
                StatsManager.GetResourcesNeedToBuild___Antenne(out crystalsNeed, out ironNeed, out gelNeed);
            break;

            case (int)IDconstants.IDpowerPlant: // Power plant
                StatsManager.GetResourcesNeedToBuild___PowerPlant(out crystalsNeed, out ironNeed, out gelNeed);
            break;
        }

        if (ResourceManager.Instance.ChecResources(crystalsNeed, ironNeed, gelNeed))
        {
            ResourceManager.Instance.StoreResourceNeed(crystalsNeed, ironNeed, gelNeed);

            GameHendler.Instance.buildingModel.InitModel(buildingID);
            GameHendler.Instance.ResetCurrentHexAndSelectedHex();
            GameHendler.Instance.currentState = GameHendler.Instance.BM_idleState;

            UIPannelManager.Instance.ResetPanels("ModelMenu");
        }

        else
        {
            Debug.Log("Not enogh resources!");
        }
    }

    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");
    }
}