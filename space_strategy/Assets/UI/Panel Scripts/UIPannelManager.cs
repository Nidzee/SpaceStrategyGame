using System.Collections.Generic;
using UnityEngine;

public class UIPannelManager : MonoBehaviour
{
    public static UIPannelManager Instance {get;private set;}

    [SerializeField] private List<GameObject> pannels;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitAllPanels();

        ResetPanels("GameView");
    }

    private void InitAllPanels()
    {
        for (int i = 0; i < pannels.Count; i++)
        {
            pannels[i].SetActive(true);
        }

        ShtabStaticData.baseMenuReference = GameObject.Find("BaseMenu").GetComponent<BaseMenu>();

        AntenneStaticData.antenneMenuReference = GameObject.Find("AntenneMenu").GetComponent<AntenneMenu>();

        PowerPlantStaticData.powerPlantMenuReference = GameObject.Find("PowerPlantMenu").GetComponent<PowerPlantMenu>();

        MineShaftStaticData.shaftMenuReference = GameObject.Find("ShaftMenu").GetComponent<ShaftMenu>();

        GarageStaticData.garageMenuReference = GameObject.Find("GarageMenu").GetComponent<GarageMenu>();


        TurretStaticData.turretMenuReference = GameObject.Find("TurretMenu").GetComponent<TurretMenu>();

        ShiledGeneratorStaticData.shieldGeneratorMenuReference = GameObject.Find("ShieldGeneratorMenu").GetComponent<ShiledGeneratorMenu>();
    }


    // Resets all panels to notactive except needed one
    public void ResetPanels(string menuName)
    {
        for (int i = 0; i < pannels.Count; i++)
        {
            if (pannels[i].name == menuName)
            {
                pannels[i].SetActive(true);
            }
            else
            {
                pannels[i].SetActive(false);
            }
        }
    }
}