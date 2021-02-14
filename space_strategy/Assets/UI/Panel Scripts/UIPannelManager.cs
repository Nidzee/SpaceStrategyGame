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

        ResetPanels("GameView");
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