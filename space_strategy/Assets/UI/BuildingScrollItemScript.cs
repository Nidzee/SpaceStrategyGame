using UnityEngine;
using UnityEngine.UI;


public class BuildingScrollItemScript : MonoBehaviour
{
    public Image buildingIcon;

    public Text buildingName;

    public Slider buildingHPslider;
    public Slider buildingSPslider;

    public IBuilding buildingRef = null;
}
