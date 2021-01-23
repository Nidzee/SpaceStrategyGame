using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPannelManager : MonoBehaviour
{
    public static UIPannelManager Instance {get;private set;}

    public List<GameObject> pannels;
    
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
        ResetPanels(0);
    }

    void Update()
    {
        
    }

    public void ResetPanels(int paneliIndex)
    {
        for (int i = 0; i < pannels.Count; i++)
        {
            if (i == paneliIndex)
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

public enum InitPannelIndex
{
    gameViewPanel = 0,
    menuPanel = 1,
    unitMenuPanel = 7,
    buildingManagerPanel = 3,
    buildingCreationPanel = 4,
    basePanel = 5,
    garagePanel = 6,
    shaftPanel = 2,
    powerPlantPanel = 8,
    shieldGeneratorPanel = 9,
    turretePanel = 10,
    antennePael = 11
}