using UnityEngine;
using UnityEngine.UI;

public class ShaftPanel : MonoBehaviour
{
    [SerializeField]private Text shaftName;

    [SerializeField]private Image shaftIcon;
    [SerializeField]private Image crystalShaftIcon;
    [SerializeField]private Image gelShaftIcon;
    [SerializeField]private Image ironShaftIcon;
    
    [SerializeField]private Slider shaftHP;
    [SerializeField]private Slider shaftSP;
    [SerializeField]private Slider unitSlider;

    [SerializeField]private Button upgradeButton;
    [SerializeField]private Button destroyBuildingButton;

    public MineShaft shaftRef;


    void Update()
    {
        // if (shaftHP.value != shaftRef.HealthPoints || shaftSP.value != shaftRef.ShieldPoints)
        // {
        //     HPSPSlidersReset();
        // }
    }

    public void ReloadPanel(MineShaft shaft) // Reload all UI elements aquoting to garage object taken as parametr
    {
        shaftRef = shaft;
        Debug.Log(shaftRef + "   -   " + shaft);

        shaftName.text = shaft.gameObject.name;
        HPSPSlidersReset();
        UnitSliderReload();
        Debug.Log(shaftName.text + "   -   " + shaft.gameObject.name);
    }

    private void HPSPSlidersReset()
    {
        shaftHP.value = shaftRef.HealthPoints;
        shaftSP.value = shaftRef.ShieldPoints;
        Debug.Log(shaftHP.value + "  -  " + shaftRef.HealthPoints + "       " + shaftSP.value + "  -  " + shaftRef.ShieldPoints);
    }

    private void UnitSliderReload()
    {
        unitSlider.onValueChanged.RemoveAllListeners();
        unitSlider.maxValue = shaftRef.capacity;
        unitSlider.value = shaftRef.unitsWorkers.Count;
        unitSlider.onValueChanged.AddListener( delegate{AddWorker();} );
        Debug.Log(unitSlider.value + "   -   " + unitSlider.maxValue + "          shaftRef - " + shaftRef);
    }

    public void AddWorker()
    {
        Debug.Log("AddWorker");

        if (unitSlider.value > shaftRef.unitsWorkers.Count)
        {
            Debug.Log(shaftRef.name + "Add Worker");
        }
        if (unitSlider.value < shaftRef.unitsWorkers.Count)
        {
            Debug.Log(shaftRef.name + "Remove worker");
        }
        UnitSliderReload();
    }
}



    // private void ShaftIconInit()
    // {
    //     if (shaftRef.GetComponent<CrystalShaft>())
    //     {
    //         shaftIcon = crystalShaftIcon;
    //     }
    //     else if (shaftRef.GetComponent<GelShaft>())
    //     {
    //         shaftIcon = gelShaftIcon;
    //     }
    //     else if (shaftRef.GetComponent<IronShaft>())
    //     {
    //         shaftIcon = ironShaftIcon;
    //     }
    // }
