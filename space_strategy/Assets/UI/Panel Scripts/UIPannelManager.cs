using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPannelManager : MonoBehaviour
{
    public static UIPannelManager Instance {get;private set;}

    [SerializeField] private List<GameObject> pannels;
    

    // Initialize start panel - Game View
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

        GarageStaticData.garageMenuReference = GameObject.Find("GarageMenu").GetComponent<GarageMenu>();

        MineShaftStaticData.shaftMenuReference = GameObject.Find("ShaftMenu").GetComponent<ShaftMenu>();

        PowerPlantStaticData.powerPlantMenuReference = GameObject.Find("PowerPlantMenu").GetComponent<PowerPlantMenu>();

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
        pannels[2].SetActive(true);
    }
}