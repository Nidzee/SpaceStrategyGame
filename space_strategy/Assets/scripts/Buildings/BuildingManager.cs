using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance {get; private set;}

    public GameObject garageSprite;
    public GameObject garagePrefab;

    public GameObject turetteSprite;
    public GameObject turettePrefab;


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

    }
}
