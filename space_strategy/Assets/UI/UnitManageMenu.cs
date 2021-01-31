using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitManageMenu : MonoBehaviour
{
    [SerializeField] private GameObject allResourcesPanel;
    [SerializeField] private GameObject crystalPanel;
    [SerializeField] private GameObject ironPanel;
    [SerializeField] private GameObject gelPanel;

    [SerializeField] private Slider crystalSlider;
    [SerializeField] private Slider ironSlider;
    [SerializeField] private Slider gelSlider;

    [SerializeField] public CrystalContentManageScript crystalScrollConten;
    [SerializeField] public IronContentManageScript ironScrollConten;
    [SerializeField] public GelContentManageScript gelScrollConten;

    
    private List<MineShaft> shaftList = new List<MineShaft>();


    public void ChangePanelToID(int i)
    {
        allResourcesPanel.SetActive(false); // 0
        crystalPanel.SetActive(false);      // 1
        ironPanel.SetActive(false);         // 2
        gelPanel.SetActive(false);          // 3

        GameHendler.Instance.isMenuAllResourcesTabOpened = false;
        GameHendler.Instance.isMenuCrystalTabOpened = false;
        GameHendler.Instance.isMenuIronTabOpened = false;
        GameHendler.Instance.isMenuGelTabOpened = false;

        switch(i)
        {
            case 0:
            GameHendler.Instance.isMenuAllResourcesTabOpened = true;
            allResourcesPanel.SetActive(true);
            ReloadCrystalTab();
            break;

            case 1:
            GameHendler.Instance.isMenuCrystalTabOpened = true;
            crystalPanel.SetActive(true);
            break;

            case 2:
            GameHendler.Instance.isMenuIronTabOpened = true;
            ironPanel.SetActive(true);
            break;

            case 3:
            GameHendler.Instance.isMenuGelTabOpened = true;
            gelPanel.SetActive(true);
            break;
        }
    }

    // Reloads all Sliders on menu
    public void ReloadPanel()
    {
        // set default all resources tab
        ReloadCrystalSlider();
        ReloadIronSlider();
        ReloadGelSlider();

        ReloadCrystalTab();
        GameHendler.Instance.isMenuCrystalTabOpened = true;
    }




    public void ReloadCrystalTab()
    {
        crystalScrollConten.ReloadScrollItems();
    }

    public void ReloadIronTab()
    {

    }

    public void ReloadGelTab()
    {

    }






#region All resources tab

    public void ReloadCrystalSlider()
    {
        crystalSlider.onValueChanged.RemoveAllListeners();


        int maxCapacity = 0; // Temp
        int fillness = 0;    // Temp
        
        for (int i = 0; i < ResourceManager.Instance.crystalShaftList.Count; i++)
        {
            maxCapacity += ResourceManager.Instance.crystalShaftList[i].capacity;
            fillness += ResourceManager.Instance.crystalShaftList[i].unitsWorkers.Count;
        }

        crystalSlider.maxValue = maxCapacity;
        crystalSlider.value = fillness;


        crystalSlider.onValueChanged.AddListener( delegate{CrystalSliderManagment();} );
    }

    public void ReloadIronSlider()
    {
        ironSlider.onValueChanged.RemoveAllListeners();


        int maxCapacity = 0; // Temp
        int fillness = 0;    // Temp
        
        for (int i = 0; i < ResourceManager.Instance.ironShaftList.Count; i++)
        {
            maxCapacity += ResourceManager.Instance.ironShaftList[i].capacity;
            fillness += ResourceManager.Instance.ironShaftList[i].unitsWorkers.Count;
        }

        ironSlider.maxValue = maxCapacity;
        ironSlider.value = fillness;


        ironSlider.onValueChanged.AddListener( delegate{IronSliderManagment();} );
    }

    public void ReloadGelSlider()
    {
        gelSlider.onValueChanged.RemoveAllListeners();


        int maxCapacity = 0; // Temp
        int fillness = 0;    // Temp
        
        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count; i++)
        {
            maxCapacity += ResourceManager.Instance.gelShaftList[i].capacity;
            fillness += ResourceManager.Instance.gelShaftList[i].unitsWorkers.Count;
        }

        gelSlider.maxValue = maxCapacity;
        gelSlider.value = fillness;


        gelSlider.onValueChanged.AddListener( delegate{GelSliderManagment();} );
    }





    private void CrystalSliderManagment()
    {
        int fillness = 0; // Temp
        for (int i = 0; i < ResourceManager.Instance.crystalShaftList.Count; i++)
        {
            fillness += ResourceManager.Instance.crystalShaftList[i].unitsWorkers.Count;
        }





        if (crystalSlider.value > fillness)
        {
            CrystalShaft crystal = FindFreeCrystalShaftForAddin();
            if (crystal)
            {
                crystal.AddWorkerViaSlider();
            }
        }

        else if (crystalSlider.value < fillness)
        {
            CrystalShaft crystal = FindFreeSCrystalhaftForDeleting();
            if (crystal)
            {
                crystal.RemoveWorkerViaSlider();
            }
        }
        




        ReloadCrystalSlider();
    }

    private CrystalShaft FindFreeSCrystalhaftForDeleting()
    {
        for (int i = 0; i < ResourceManager.Instance.crystalShaftList.Count; i++)
        {
            if (ResourceManager.Instance.crystalShaftList[i].unitsWorkers.Count != 0)
            {
                return ResourceManager.Instance.crystalShaftList[i];
            }
        }

        Debug.Log("Strange Error!");
        return null;
    }

    private CrystalShaft FindFreeCrystalShaftForAddin()
    {
        for (int i = 0; i < ResourceManager.Instance.crystalShaftList.Count; i++) // MAYBE PROBLEM HERE
        {
            if (ResourceManager.Instance.crystalShaftList[i].unitsWorkers.Count < ResourceManager.Instance.crystalShaftList[i].capacity)
            {
                //Debug.Log("Found free CrystalShaft!");
                return ResourceManager.Instance.crystalShaftList[i];
            }
        }

        Debug.Log("There is no free CrystalShaft!");
        return null;
    }






    private void IronSliderManagment()
    {
        int fillness = 0; // Temp
        for (int i = 0; i < ResourceManager.Instance.ironShaftList.Count; i++)
        {
            fillness += ResourceManager.Instance.ironShaftList[i].unitsWorkers.Count;
        }



        if (ironSlider.value > fillness)
        {
            IronShaft iron = FindFreeIronShaftForAddin();
            if (iron)
            {
                iron.AddWorkerViaSlider();
            }
        }

        else if (ironSlider.value < fillness)
        {
            IronShaft iron = FindFreeSIronhaftForDeleting();
            if (iron)
            {
                iron.RemoveWorkerViaSlider();
            }
        }
        



        ReloadIronSlider();
    }

    private IronShaft FindFreeSIronhaftForDeleting()
    {
        for (int i = 0; i < ResourceManager.Instance.ironShaftList.Count; i++)
        {
            if (ResourceManager.Instance.ironShaftList[i].unitsWorkers.Count != 0)
            {
                return ResourceManager.Instance.ironShaftList[i];
            }
        }

        Debug.Log("Strange Error!");
        return null;
    }

    private IronShaft FindFreeIronShaftForAddin()
    {
        for (int i = 0; i < ResourceManager.Instance.ironShaftList.Count; i++) // MAYBE PROBLEM HERE
        {
            if (ResourceManager.Instance.ironShaftList[i].unitsWorkers.Count < ResourceManager.Instance.ironShaftList[i].capacity)
            {
                // Debug.Log("Found free CrystalShaft!");
                return ResourceManager.Instance.ironShaftList[i];
            }
        }
        
        Debug.Log("There is no free CrystalShaft");
        return null;
    }







    private void GelSliderManagment()
    {
        int fillness = 0; // Temp
        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count; i++)
        {
            fillness += ResourceManager.Instance.gelShaftList[i].unitsWorkers.Count;
        }



        if (gelSlider.value > fillness)
        {
            GelShaft gel = FindFreeGelShaftForAddin();
            if (gel)
            {
                gel.AddWorkerViaSlider();
            }
        }

        else if (gelSlider.value < fillness)
        {
            GelShaft gel = FindFreeGelShaftForDeleting();
            if (gel)
            {
                gel.RemoveWorkerViaSlider();
            }
        }
        


        ReloadGelSlider(); // Maybe here is not neccessary as in Remove or Add via slider - we execute reloadSlider
    }

    private GelShaft FindFreeGelShaftForDeleting()
    {
        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count; i++)
        {
            if (ResourceManager.Instance.gelShaftList[i].unitsWorkers.Count != 0)
            {
                return ResourceManager.Instance.gelShaftList[i];
            }
        }

        Debug.Log("Strange Error!");
        return null;
    }

    private GelShaft FindFreeGelShaftForAddin()
    {
        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count; i++) // MAYBE PROBLEM HERE
        {
            if (ResourceManager.Instance.gelShaftList[i].unitsWorkers.Count < ResourceManager.Instance.gelShaftList[i].capacity)
            {
                // Debug.Log("Found free CrystalShaft!");
                return ResourceManager.Instance.gelShaftList[i];
            }
        }
        
        Debug.Log("There is no free CrystalShaft");
        return null;
    }

#endregion

















    // Exit to Game View Menu
    public void ExitMenu()
    {
        // ChangePanelToID(0);
        UIPannelManager.Instance.ResetPanels("GameView");
        GameHendler.Instance.isMenuAllResourcesTabOpened = false;
    }
}
