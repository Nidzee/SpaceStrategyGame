using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance {get; private set;}

    public GameObject garagePrefab;
    public GameObject turetteLaserPrefab;
    public GameObject turetteBulletPrefab;
    public GameObject turetteMisilePrefab;
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
    public GameObject bulletPrefab;

    public GameObject shieldGeneratorRangePrefab;


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
        TurretBullet.InitStaticFields();
        TurretLaser.InitStaticFields();
        TurretMisile.InitStaticFields();
        CrystalShaft.InitStaticFields();
        GelShaft.InitStaticFields();
        IronShaft.InitStaticFields();
        PowerPlant.InitStaticFields();
        Antenne.InitStaticFields();
        ShieldGenerator.InitStaticFields();

        Unit.InitStaticFields();
    }
}
