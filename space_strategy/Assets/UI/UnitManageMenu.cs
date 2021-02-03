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

    [SerializeField] private Text mainUnitCount;

    [SerializeField] private Text crystalSliderCount;
    [SerializeField] private Text ironSliderCount;
    [SerializeField] private Text gelSliderCount;


    [SerializeField] private GameObject scrollItemPrefab;

    public List<GameObject> scrollItemsCrystal = new List<GameObject>();
    public List<GameObject> scrollItemsIron = new List<GameObject>();
    public List<GameObject> scrollItemsGel = new List<GameObject>();

    public GameObject crystalScrollWievPlacerPlace;
    public GameObject ironScrollWievPlacerPlace;
    public GameObject gelScrollWievPlacerPlace;


#region Resource sliders functions

    public void ReloadCrystalScrollItems ()
    {
        foreach(var i in scrollItemsCrystal)
        {
            Destroy(i);
        }
        scrollItemsCrystal.Clear();

        for (int i = 0; i < ResourceManager.Instance.crystalShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollItemPrefab);
            prefab.gameObject.transform.SetParent(crystalScrollWievPlacerPlace.transform, false);


            // Refering particular shaft to its slider
            prefab.GetComponent<ScrollItemScript>()._myShaft = ResourceManager.Instance.crystalShaftList[i];

            prefab.GetComponent<ScrollItemScript>()._name.text = ResourceManager.Instance.crystalShaftList[i].name;

            prefab.GetComponent<ScrollItemScript>()._unitCounter.text = ResourceManager.Instance.crystalShaftList[i].capacity +"/"+ ResourceManager.Instance.crystalShaftList[i].unitsWorkers.Count;

            // Determine changing of slider value to be possible to reload this slider with shaft info
            prefab.GetComponent<ScrollItemScript>()._mySlider.onValueChanged.AddListener(delegate 
            { 
                UnitManage(prefab.GetComponent<ScrollItemScript>()._mySlider, prefab.GetComponent<ScrollItemScript>()._myShaft, prefab.GetComponent<ScrollItemScript>()._unitCounter); 
            });

            ReloadCurrentSlider(prefab.GetComponent<ScrollItemScript>()._mySlider, ResourceManager.Instance.crystalShaftList[i], prefab.GetComponent<ScrollItemScript>()._unitCounter);

            scrollItemsCrystal.Add(prefab);
        }
    }

    public void ReloadIronScrollItems ()
    {
        foreach(var i in scrollItemsIron)
        {
            Destroy(i);
        }
        scrollItemsIron.Clear();

        for (int i = 0; i < ResourceManager.Instance.ironShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollItemPrefab);
            prefab.gameObject.transform.SetParent(ironScrollWievPlacerPlace.transform, false);


            // Refering particular shaft to its slider
            prefab.GetComponent<ScrollItemScript>()._myShaft = ResourceManager.Instance.ironShaftList[i];

            prefab.GetComponent<ScrollItemScript>()._name.text = ResourceManager.Instance.ironShaftList[i].name;

            prefab.GetComponent<ScrollItemScript>()._unitCounter.text = ResourceManager.Instance.ironShaftList[i].capacity +"/"+ ResourceManager.Instance.ironShaftList[i].unitsWorkers.Count;


            // Determine changing of slider value to be possible to reload this slider with shaft info
            prefab.GetComponent<ScrollItemScript>()._mySlider.onValueChanged.AddListener(delegate 
            { 
                UnitManage(prefab.GetComponent<ScrollItemScript>()._mySlider, prefab.GetComponent<ScrollItemScript>()._myShaft, prefab.GetComponent<ScrollItemScript>()._unitCounter); 
            });

            ReloadCurrentSlider(prefab.GetComponent<ScrollItemScript>()._mySlider, ResourceManager.Instance.ironShaftList[i], prefab.GetComponent<ScrollItemScript>()._unitCounter);

            scrollItemsIron.Add(prefab);
        }
    }

    public void ReloadGelScrollItems ()
    {
        foreach(var i in scrollItemsGel)
        {
            Destroy(i);
        }
        scrollItemsGel.Clear();

        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollItemPrefab);
            prefab.gameObject.transform.SetParent(gelScrollWievPlacerPlace.transform, false);


            // Refering particular shaft to its slider
            prefab.GetComponent<ScrollItemScript>()._myShaft = ResourceManager.Instance.gelShaftList[i];

            prefab.GetComponent<ScrollItemScript>()._name.text = ResourceManager.Instance.gelShaftList[i].name;

            prefab.GetComponent<ScrollItemScript>()._unitCounter.text = ResourceManager.Instance.gelShaftList[i].capacity +"/"+ ResourceManager.Instance.gelShaftList[i].unitsWorkers.Count;


            // Determine changing of slider value to be possible to reload this slider with shaft info
            prefab.GetComponent<ScrollItemScript>()._mySlider.onValueChanged.AddListener(delegate 
            { 
                UnitManage(prefab.GetComponent<ScrollItemScript>()._mySlider, prefab.GetComponent<ScrollItemScript>()._myShaft, prefab.GetComponent<ScrollItemScript>()._unitCounter); 
            });

            ReloadCurrentSlider(prefab.GetComponent<ScrollItemScript>()._mySlider, ResourceManager.Instance.gelShaftList[i], prefab.GetComponent<ScrollItemScript>()._unitCounter);

            scrollItemsGel.Add(prefab);
        }
    }






    public void UnitManage(Slider slider, MineShaft shaft, Text unitCounter)
    {
        if (slider.value > shaft.unitsWorkers.Count)
        {
            shaft.AddWorkerViaSlider();
        }

        if (slider.value < shaft.unitsWorkers.Count)
        {
            shaft.RemoveWorkerViaSlider();
        }

        ReloadCurrentSlider(slider, shaft, unitCounter);
        ReloadMainUnitCount();
    }

    public void ReloadCurrentSlider(Slider slider, MineShaft shaft, Text unitCounter)
    {
        slider.maxValue = shaft.capacity;
        slider.value = shaft.unitsWorkers.Count;
        unitCounter.text = shaft.unitsWorkers.Count+"/"+shaft.capacity;
    }

    public void FindSLiderAndReload(MineShaft shaft, int shaftIndex)
    {
        switch(shaftIndex)
        {
            case 1:
            {
                for (int i = 0; i < scrollItemsCrystal.Count; i++)
                {
                    //Debug.Log(scrollItemsCrystal[i].GetComponent<ScrollItemScript>()._myShaft +"    -    "+ shaft);
                    if (scrollItemsCrystal[i].GetComponent<ScrollItemScript>()._myShaft == shaft)
                    {
                        ReloadCurrentSlider(scrollItemsCrystal[i].GetComponent<ScrollItemScript>()._mySlider, shaft, scrollItemsCrystal[i].GetComponent<ScrollItemScript>()._unitCounter);
                        return;
                    }
                }
                Debug.Log("STRANGE ERROR!");
            }
            break;

            case 2:
            {
                for (int i = 0; i < scrollItemsIron.Count; i++)
                {
                    //Debug.Log(scrollItemsIron[i].GetComponent<ScrollItemScript>()._myShaft +"    -    "+ shaft);
                    if (scrollItemsIron[i].GetComponent<ScrollItemScript>()._myShaft == shaft)
                    {
                        ReloadCurrentSlider(scrollItemsIron[i].GetComponent<ScrollItemScript>()._mySlider, shaft, scrollItemsIron[i].GetComponent<ScrollItemScript>()._unitCounter);
                        return;
                    }
                }
                Debug.Log("STRANGE ERROR!");
            }
            break;

            case 3:
            {
                for (int i = 0; i < scrollItemsGel.Count; i++)
                {
                    //Debug.Log(scrollItemsGel[i].GetComponent<ScrollItemScript>()._myShaft +"    -    "+ shaft);
                    if (scrollItemsGel[i].GetComponent<ScrollItemScript>()._myShaft == shaft)
                    {
                        ReloadCurrentSlider(scrollItemsGel[i].GetComponent<ScrollItemScript>()._mySlider, shaft, scrollItemsGel[i].GetComponent<ScrollItemScript>()._unitCounter);
                        return;
                    }
                }
                Debug.Log("STRANGE ERROR!");
            }
            break;
        }        
    }

#endregion

    public void ReloadMainUnitCount()
    {
        mainUnitCount.text = ResourceManager.Instance.avaliableUnits.Count +"/"+ ResourceManager.Instance.unitsList.Count;
    }


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
            ReloadPanel();
            break;

            case 1:
            GameHendler.Instance.isMenuCrystalTabOpened = true;
            crystalPanel.SetActive(true);
            ReloadCrystalTab();
            break;

            case 2:
            GameHendler.Instance.isMenuIronTabOpened = true;
            ironPanel.SetActive(true);
            ReloadIronTab();
            break;

            case 3:
            GameHendler.Instance.isMenuGelTabOpened = true;
            gelPanel.SetActive(true);
            ReloadGelTab();
            break;
        }
    }

    // Reloads all Sliders on menu
    public void ReloadPanel()
    {
        allResourcesPanel.SetActive(true); // 0
        crystalPanel.SetActive(false);      // 1
        ironPanel.SetActive(false);         // 2
        gelPanel.SetActive(false);          // 3

        GameHendler.Instance.isMenuAllResourcesTabOpened = true;
        GameHendler.Instance.isMenuCrystalTabOpened = false;
        GameHendler.Instance.isMenuIronTabOpened = false;
        GameHendler.Instance.isMenuGelTabOpened = false;

        // set default all resources tab
        ReloadCrystalSlider();
        ReloadIronSlider();
        ReloadGelSlider();

        ReloadCrystalTab();
        ReloadIronTab();
        ReloadGelTab();

        
        ReloadMainUnitCount();

        GameHendler.Instance.isMenuCrystalTabOpened = true;
    }




    public void ReloadCrystalTab()
    {
        ReloadCrystalScrollItems();
    }

    public void ReloadIronTab()
    {
        ReloadIronScrollItems();
    }

    public void ReloadGelTab()
    {
        ReloadGelScrollItems();
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
        crystalSliderCount.text = crystalSlider.value +"/"+crystalSlider.maxValue;


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
        ironSliderCount.text = ironSlider.value +"/"+ironSlider.maxValue;


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
        gelSliderCount.text = gelSlider.value +"/"+gelSlider.maxValue;


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
        ReloadMainUnitCount();
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
        ReloadMainUnitCount();
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
        ReloadMainUnitCount();
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
        GameHendler.Instance.isMenuCrystalTabOpened = false;
        GameHendler.Instance.isMenuGelTabOpened = false;
        GameHendler.Instance.isMenuIronTabOpened = false;

        GameHendler.Instance.isUnitManageMenuOpened = false;
    }
}
