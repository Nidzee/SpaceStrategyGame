using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GelContentManageScript : MonoBehaviour
{
    [SerializeField] private GameObject scrollItemPrefab;

    public List<GameObject> scrollItems = new List<GameObject>();


    // private void Update()
    // {
    //     // if (Input.GetKeyDown(KeyCode.Space))
    //     // {
    //     //     ReloadScrollItems();
    //     // }
    // }


    public void ReloadScrollItems()
    {
        for (int i = 0; i < ResourceManager.Instance.gelShaftList.Count; i++)
        {
            // Instantiating scrollItem prefab
            GameObject prefab = Instantiate(scrollItemPrefab);
            prefab.gameObject.transform.SetParent(gameObject.transform, false);


            // Refering particular shaft to its slider
            prefab.GetComponent<ScrollItemScript>()._myShaft = ResourceManager.Instance.gelShaftList[i];

            // Determine changing of slider value to be possible to reload this slider with shaft info
            prefab.GetComponent<ScrollItemScript>()._mySlider.onValueChanged.AddListener(delegate 
            { 
                UnitManage(prefab.GetComponent<ScrollItemScript>()._mySlider, prefab.GetComponent<ScrollItemScript>()._myShaft); 
            });


            scrollItems.Add(prefab);
        }
    }

    public void UnitManage(Slider slider, MineShaft shaft)
    {
        if (slider.value > shaft.unitsWorkers.Count)
        {
            shaft.AddWorkerViaSlider();
        }

        if (slider.value < shaft.unitsWorkers.Count)
        {
            shaft.RemoveWorkerViaSlider();
        }

        ReloadCurrentSlider(slider, shaft);
    }

    public void ReloadCurrentSlider(Slider slider, MineShaft shaft)
    {
        slider.maxValue = shaft.capacity;
        slider.value = shaft.unitsWorkers.Count;
    }

    public void FindSLiderAndReload(MineShaft shaft)
    {
        for (int i = 0; i < scrollItems.Count; i++)
        {
            if (scrollItems[i].GetComponent<ScrollItemScript>()._myShaft == shaft)
            {
                ReloadCurrentSlider(scrollItems[i].GetComponent<ScrollItemScript>()._mySlider, shaft);
                return;
            }
        }

        Debug.Log("STRANGE ERROR!");
    }
}
