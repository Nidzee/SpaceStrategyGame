using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance {get; private set;}

    public GameObject garagePrefab;
    public GameObject turettePrefab;
    public GameObject crystalShaftPrefab;
    public GameObject gelShaftPrefab;
    public GameObject ironShaftPrefab;


    public GameObject unitPrefab;

    public GameObject crystalResourcePrefab;
    public GameObject ironResourcePrefab;
    public GameObject gelResourcePrefab;


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
        GelShaft.InitStaticFields();
        IronShaft.InitStaticFields();

        Unit.InitStaticFields();
    }
}
