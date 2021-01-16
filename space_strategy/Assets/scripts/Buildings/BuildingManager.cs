using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance {get; private set;}

    public GameObject garagePrefab;
    public GameObject turettePrefab;
    public GameObject crystalShaftPrefab;


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


        Garage.InitStaticFields();
        Turette.InitStaticFields();
        CrystalShaft.InitStaticFields();

    }
}
