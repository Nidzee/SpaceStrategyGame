using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitManageMenu : MonoBehaviour
{
    public static UnitManageMenu Instance {get; private set;}
    private void Awake()
    {
        Debug.Log("Initializing unit manage menu...");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private GameObject allResourcesPanel;// Init in inspector
    [SerializeField] private GameObject crystalPanel;     // Init in inspector
    [SerializeField] private GameObject ironPanel;        // Init in inspector
    [SerializeField] private GameObject gelPanel;         // Init in inspector

    [SerializeField] private Text mainUnitCount;          // Init in inspector

    [SerializeField] private Text crystalSliderCount;     // Init in inspector
    [SerializeField] private Text ironSliderCount;        // Init in inspector
    [SerializeField] private Text gelSliderCount;         // Init in inspector

    [SerializeField] private Slider crystalSlider;        // Init in inspector
    [SerializeField] private Slider ironSlider;           // Init in inspector
    [SerializeField] private Slider gelSlider;            // Init in inspector

    [SerializeField] private GameObject scrollItemPrefab; // Init in inspector

    public List<GameObject> scrollItemsCrystal = new List<GameObject>();
    public List<GameObject> scrollItemsIron = new List<GameObject>();
    public List<GameObject> scrollItemsGel = new List<GameObject>();

    public GameObject crystalScrollContent; // Init in inspector
    public GameObject ironScrollContent;    // Init in inspector
    public GameObject gelScrollContent;     // Init in inspector


    public void ReloadPanel()
    {
        ChangePanelToID(0);

        ReloadMainUnitCount();
    }

    public void ChangePanelToID(int panelIndex)
    {
        allResourcesPanel.SetActive(false); // 0
        crystalPanel.SetActive(false);      // 1
        ironPanel.SetActive(false);         // 2
        gelPanel.SetActive(false);          // 3

        GameViewMenu.Instance.TurnOnUnitManageMenu();

        switch(panelIndex)
        {
            case 0:
            GameViewMenu.Instance.TurnOnAllResourceTab();
            allResourcesPanel.SetActive(true);

            ReloadCrystalSlider();
            ReloadIronSlider();
            ReloadGelSlider();
            break;

            case 1:
            GameViewMenu.Instance.TurnOnCrystalTab();
            crystalPanel.SetActive(true);

            ReloadCrystalScrollItems();
            break;

            case 2:
            GameViewMenu.Instance.TurnOnIronTab();
            ironPanel.SetActive(true);

            ReloadIronScrollItems();
            break;

            case 3:
            GameViewMenu.Instance.TurnOnGelTab();
            gelPanel.SetActive(true);

            ReloadGelScrollItems();
            break;
        }
    }

    public void ReloadMainUnitCount()
    {
        mainUnitCount.text = ResourceManager.Instance.UnitSittuation();
    }








































    #region All resources menu (3 sliders) managment - REDO WITH GENERICS

    private void CrystalSliderManagment()
    {
        int fillness = 0;
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

        // Reload this slider
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
                return ResourceManager.Instance.crystalShaftList[i];
            }
        }

        Debug.Log("There is no free CrystalShaft!");
        return null;
    }

    public void ReloadCrystalSlider()
    {
        if (GameViewMenu.Instance.isMenuAllResourcesTabOpened)
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
        else
        {
            Debug.Log("UNIT MANAGE MENU 3_TABS: Didnt find slider to reload!");
        }
    }



    private void IronSliderManagment()
    {
        int fillness = 0;
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

        // Reload this slider
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

    public void ReloadIronSlider()
    {
        if (GameViewMenu.Instance.isMenuAllResourcesTabOpened)
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
        else
        {
            Debug.Log("UNIT MANAGE MENU 3_TABS: Didnt find slider to reload!");
        }
    }



    private void GelSliderManagment()
    {
        int fillness = 0;
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

        // Reload this slider
        ReloadGelSlider();
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
                return ResourceManager.Instance.gelShaftList[i];
            }
        }
        
        Debug.Log("There is no free CrystalShaft");
        return null;
    }

    public void ReloadGelSlider()
    {
        if (GameViewMenu.Instance.isMenuAllResourcesTabOpened)
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
        else
        {
            Debug.Log("UNIT MANAGE MENU 3_TABS: Didnt find slider to reload!");
        }
    }

    #endregion

    #region Reloadind specific Tab with scrollitems - REDO WITH GENERICS

    public void ReloadCrystalScrollItems ()
    {
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
            temp._mySlider.onValueChanged.AddListener(delegate { ScrollItemSliderUnitManipulation(temp); });

            temp._mySlider.maxValue = temp._myShaft.capacity;
            temp._mySlider.value = temp._myShaft.unitsWorkers.Count;
            temp._unitCounter.text = temp._myShaft.unitsWorkers.Count+"/"+temp._myShaft.capacity;







            // Initializing slider variables
            scrollItemsCrystal.Add(prefab);
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
            temp._mySlider.onValueChanged.AddListener(delegate { ScrollItemSliderUnitManipulation(temp); });

            temp._mySlider.maxValue = temp._myShaft.capacity;
            temp._mySlider.value = temp._myShaft.unitsWorkers.Count;
            temp._unitCounter.text = temp._myShaft.unitsWorkers.Count+"/"+temp._myShaft.capacity;









            // Initializing slider variables
            scrollItemsIron.Add(prefab);
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
            temp._mySlider.onValueChanged.AddListener(delegate { ScrollItemSliderUnitManipulation(temp); });

            temp._mySlider.maxValue = temp._myShaft.capacity;
            temp._mySlider.value = temp._myShaft.unitsWorkers.Count;
            temp._unitCounter.text = temp._myShaft.unitsWorkers.Count+"/"+temp._myShaft.capacity;











            // Initializing slider variables
            scrollItemsGel.Add(prefab);
        }
    }

    #endregion


























    public void RemoveMineShaftFromScrollItems(MineShaft shaft)
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

        foreach (var i in scrollItemsList)
        {
            if (i.GetComponent<ScrollItemScript>()._myShaft.name == shaft.name)
            {
                scrollItemsCrystal.Remove(i);
                Destroy(i);
                return;
            }
        }
    }

    public void ScrollItemSliderUnitManipulation(ScrollItemScript scrollItem)
    {
        if (scrollItem._mySlider.value > scrollItem._myShaft.unitsWorkers.Count)
        {
            scrollItem._myShaft.AddWorkerViaSlider(); // Executes events
        }

        if (scrollItem._mySlider.value < scrollItem._myShaft.unitsWorkers.Count)
        {
            scrollItem._myShaft.RemoveWorkerViaSlider(); // Executes events
        }
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
                ScrollItemScript temp = scrollItemsList[i].GetComponent<ScrollItemScript>();

                temp._mySlider.maxValue = temp._myShaft.capacity;
                temp._mySlider.value = temp._myShaft.unitsWorkers.Count;
                temp._unitCounter.text = temp._myShaft.unitsWorkers.Count+"/"+temp._myShaft.capacity;

                return;
            }
        }

        Debug.Log("UNIT MANAGE MENU: Didnt find slider to reload! (Maybe wrong tab is opened)");
    }

    public void ExitMenu()
    {
        UIPannelManager.Instance.ResetPanels("GameView");

        GameViewMenu.Instance.TurnOffUnitManageMenu();
    }
}
