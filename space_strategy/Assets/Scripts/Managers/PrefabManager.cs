using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance {get; private set;}

    public GameObject garagePrefab;
    public GameObject singleTuretteLaserPrefab;
    public GameObject doubleTuretteLaserPrefab;
    public GameObject tripleTuretteLaserPrefab;
    public GameObject singleturetteMisilePrefab;
    public GameObject doubleturetteMisilePrefab;
    public GameObject truipleturetteMisilePrefab;
    public GameObject crystalShaftPrefab;
    public GameObject gelShaftPrefab;
    public GameObject ironShaftPrefab;
    public GameObject powerPlantPrefab;
    public GameObject antennePrefab;
    public GameObject basePrefab;
    public GameObject shieldGeneratorPrefab;
    public GameObject unitPrefab;
    public GameObject crystalResourcePrefab;
    public GameObject ironResourcePrefab;
    public GameObject gelResourcePrefab;
    public GameObject misilePrefab;
    public GameObject shieldGeneratorRangePrefab;
    public GameObject bomberPrefab;
    public GameObject enemyAttackRange;
    public ParticleSystem enemyDeathParticles;
    public ParticleSystem misileLaunchParticles;


    public void Awake()
    {
        Debug.Log("PREFAB MANAGER START WORKING!");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartPrefabManager()
    {
        AntenneStaticData.InitStaticFields();

        GarageStaticData.InitStaticFields();

        CSStaticData.InitStaticFields();
        GSStaticData.InitStaticFields();
        ISStaticData.InitStaticFields();

        PowerPlantStaticData.InitStaticFields();

        ShiledGeneratorStaticData.InitStaticFields();

        LTStaticData.InitStaticFields();
        MTStaticData.InitStaticFields();

        UnitStaticData.InitStaticFields();
        BomberStaticData.InitStaticFields();
    }
}
