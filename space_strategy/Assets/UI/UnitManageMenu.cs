using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitManageMenu : MonoBehaviour
{
    [SerializeField] private GameObject allResourcesPanel;
    [SerializeField] private GameObject crystalPanel;
    [SerializeField] private GameObject ironPanel;
    [SerializeField] private GameObject gelPanel;

    [SerializeField] private Text mainUnitCount;

    [SerializeField] private Text crystalSliderCount;
    [SerializeField] private Text ironSliderCount;
    [SerializeField] private Text gelSliderCount;

    [SerializeField] private Slider crystalSlider;
    [SerializeField] private Slider ironSlider;
    [SerializeField] private Slider gelSlider;

    [SerializeField] private GameObject scrollItemPrefab;

    public List<GameObject> scrollItemsCrystal = new List<GameObject>();
    public List<GameObject> scrollItemsIron = new List<GameObject>();
    public List<GameObject> scrollItemsGel = new List<GameObject>();

    public GameObject crystalScrollContent;
    public GameObject ironScrollContent;
    public GameObject gelScrollContent;





    // Reloads all Sliders on menu
    public void ReloadPanel()
    {
        GameHendler.Instance.isUnitManageMenuOpened = true;

        GameHendler.Instance.isMenuAllResourcesTabOpened = true;
        GameHendler.Instance.isMenuCrystalTabOpened = false;
        GameHendler.Instance.isMenuIronTabOpened = false;
        GameHendler.Instance.isMenuGelTabOpened = false;

        allResourcesPanel.SetActive(true);  // 0
        crystalPanel.SetActive(false);      // 1
        ironPanel.SetActive(false);         // 2
        gelPanel.SetActive(false);          // 3

        // set default all resources tab
        ReloadCrystalSlider();
        ReloadIronSlider();
        ReloadGelSlider();
        
        ReloadMainUnitCount();

        GameHendler.Instance.isMenuCrystalTabOpened = true;
    }

    public void ChangePanelToID(int panelIndex)
    {
        allResourcesPanel.SetActive(false); // 0
        crystalPanel.SetActive(false);      // 1
        ironPanel.SetActive(false);         // 2
        gelPanel.SetActive(false);          // 3

        GameHendler.Instance.isMenuAllResourcesTabOpened = false;
        GameHendler.Instance.isMenuCrystalTabOpened = false;
        GameHendler.Instance.isMenuIronTabOpened = false;
        GameHendler.Instance.isMenuGelTabOpened = false;

        switch(panelIndex)
        {
            case 0:
            GameHendler.Instance.isMenuAllResourcesTabOpened = true;
            allResourcesPanel.SetActive(true);
            ReloadPanel();
            break;

            case 1:
            GameHendler.Instance.isMenuCrystalTabOpened = true;
            crystalPanel.SetActive(true);
            ReloadCrystalScrollItems();
            break;

            case 2:
            GameHendler.Instance.isMenuIronTabOpened = true;
            ironPanel.SetActive(true);
            ReloadIronScrollItems();
            break;

            case 3:
            GameHendler.Instance.isMenuGelTabOpened = true;
            gelPanel.SetActive(true);
            ReloadGelScrollItems();
            break;
        }
    }


    public void ReloadMainUnitCount()
    {
        mainUnitCount.text = ResourceManager.Instance.avaliableUnits.Count +"/"+ ResourceManager.Instance.unitsList.Count;
    }


    #region All resources menu (3 sliders) managment

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
                    return ResourceManager.Instance.crystalShaftList[i];
                }
            }

            Debug.Log("There is no free CrystalShaft!");
            return null;
        }

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

    #endregion







    public void RemoveCrystalScrollItem(CrystalShaft crystalShaft)
    {
        foreach (var i in scrollItemsCrystal)
        {
            if (i.GetComponent<ScrollItemScript>()._myShaft == crystalShaft)
            {
                scrollItemsCrystal.Remove(i);
                Destroy(i);
                return;
            }
        }
    }

    public void RemoveIronScrollItem(IronShaft ironShaft)
    {
        foreach (var i in scrollItemsIron)
        {
            if (i.GetComponent<ScrollItemScript>()._myShaft == ironShaft)
            {
                scrollItemsIron.Remove(i);
                Destroy(i);
                return;
            }
        }
    }

    public void RemoveGelScrollItem(GelShaft gelShaft)
    {
        foreach (var i in scrollItemsGel)
        {
            if (i.GetComponent<ScrollItemScript>()._myShaft == gelShaft)
            {
                scrollItemsGel.Remove(i);
                Destroy(i);
                return;
            }
        }
    }



    public void ReloadCrystalScrollItems ()
    {
        // It is easier to destroy and reload all of them either to find every scrollItem and check whetheir it is on panel or not
        foreach (var i in scrollItemsCrystal)
        {
            Destroy(i);
        }
        scrollItemsCrystal.Clear();

        // Reloading all scrollItems on crystal tab
        for (int i = 0; i < ResourceManager.Instance.crystalShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollItemPrefab);
            prefab.gameObject.transform.SetParent(crystalScrollContent.transform, false);
            ScrollItemScript temp = prefab.GetComponent<ScrollItemScript>();


            // Refering particular shaft to its slider
            temp._myShaft = ResourceManager.Instance.crystalShaftList[i];
            temp._name.text = ResourceManager.Instance.crystalShaftList[i].name;
            temp._unitCounter.text = ResourceManager.Instance.crystalShaftList[i].capacity +"/"+ ResourceManager.Instance.crystalShaftList[i].unitsWorkers.Count;
            temp._mySlider.onValueChanged.AddListener(delegate { ScrollItemSliderUnitManage(temp); });


            // Initializing slider variables
            scrollItemsCrystal.Add(prefab);
            ReloadCurrentScrollItem(temp);
        }
    }

    public void ReloadIronScrollItems ()
    {
        // It is easier to destroy and reload all of them either to find every scrollItem and check whetheir it is on panel or not
        foreach(var i in scrollItemsIron)
        {
            Destroy(i);
        }
        scrollItemsIron.Clear();

        // Reloading all scrollItems on iron tab
        for (int i = 0; i < ResourceManager.Instance.ironShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollItemPrefab);
            prefab.gameObject.transform.SetParent(ironScrollContent.transform, false);
            ScrollItemScript temp = prefab.GetComponent<ScrollItemScript>();


            // Refering particular shaft to its slider
            temp._myShaft = ResourceManager.Instance.ironShaftList[i];
            temp._name.text = ResourceManager.Instance.ironShaftList[i].name;
            temp._unitCounter.text = ResourceManager.Instance.ironShaftList[i].capacity +"/"+ ResourceManager.Instance.ironShaftList[i].unitsWorkers.Count;
            temp._mySlider.onValueChanged.AddListener(delegate { ScrollItemSliderUnitManage(temp); });


            // Initializing slider variables
            scrollItemsIron.Add(prefab);
            ReloadCurrentScrollItem(temp);
        }
    }

    public void ReloadGelScrollItems ()
    {
        // It is easier to destroy and reload all of them either to find every scrollItem and check whetheir it is on panel or not
        foreach(var i in scrollItemsGel)
        {
            Destroy(i);
        }
        scrollItemsGel.Clear();

        // Reloading all scrollItems on iron tab
        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollItemPrefab);
            prefab.gameObject.transform.SetParent(gelScrollContent.transform, false);
            ScrollItemScript temp = prefab.GetComponent<ScrollItemScript>();


            // Refering particular shaft to its slider
            temp._myShaft = ResourceManager.Instance.gelShaftList[i];
            temp._name.text = ResourceManager.Instance.gelShaftList[i].name;
            temp._unitCounter.text = ResourceManager.Instance.gelShaftList[i].capacity +"/"+ ResourceManager.Instance.gelShaftList[i].unitsWorkers.Count;
            temp._mySlider.onValueChanged.AddListener(delegate { ScrollItemSliderUnitManage(temp); });


            // Initializing slider variables
            scrollItemsGel.Add(prefab);
            ReloadCurrentScrollItem(temp);
        }
    }



    public void ScrollItemSliderUnitManage(ScrollItemScript scrollItem)
    {
        if (scrollItem._mySlider.value > scrollItem._myShaft.unitsWorkers.Count)
        {
            scrollItem._myShaft.AddWorkerViaSlider();
        }

        if (scrollItem._mySlider.value < scrollItem._myShaft.unitsWorkers.Count)
        {
            scrollItem._myShaft.RemoveWorkerViaSlider();
        }

        ReloadCurrentScrollItem(scrollItem);
        ReloadMainUnitCount();
    }

    public void ReloadCurrentScrollItem(ScrollItemScript scrollItem)
    {
        scrollItem._mySlider.maxValue = scrollItem._myShaft.capacity;
        scrollItem._mySlider.value = scrollItem._myShaft.unitsWorkers.Count;

        scrollItem._unitCounter.text = scrollItem._myShaft.unitsWorkers.Count+"/"+scrollItem._myShaft.capacity;
    }

    public void FindSLiderAndReload(MineShaft shaft)
    {
        List<GameObject> scrollItemsList = new List<GameObject>();

        switch(shaft.type)
        {
            case 1:
            scrollItemsList = scrollItemsCrystal;
            break;

            case 2:
            scrollItemsList = scrollItemsIron;
            break;

            case 3:
            scrollItemsList = scrollItemsGel;
            break;
        }

        for (int i = 0; i < scrollItemsList.Count; i++)
        {
            if (scrollItemsList[i].GetComponent<ScrollItemScript>()._myShaft == shaft)
            {
                ReloadCurrentScrollItem (scrollItemsList[i].GetComponent<ScrollItemScript>());
                return;
            }
        }
        Debug.Log("STRANGE ERROR!");       
    }









    // Exit to Game View Menu
    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");

        GameHendler.Instance.isMenuAllResourcesTabOpened = false;
        GameHendler.Instance.isMenuCrystalTabOpened = false;
        GameHendler.Instance.isMenuGelTabOpened = false;
        GameHendler.Instance.isMenuIronTabOpened = false;

        GameHendler.Instance.isUnitManageMenuOpened = false;
    }
}
