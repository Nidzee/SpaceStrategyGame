using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance {get; private set;}


    // GARAGE
    public static int _crystalNeedForBuilding_Garage;
    public static int _ironNeedForBuilding_Garage;
    public static int _gelNeedForBuilding_Garage;

    public static int _crystalNeedForUnitCreation;
    public static int _ironNeedForForUnitCreation;
    public static int _gelNeedForForUnitCreation;

    public static int _maxHealth_Garage; 
    public static int _maxShiled_Garage; 
    public static int _maxDefensePoints_Garage; 

    public static int _baseUpgradeStep_Garage;


    private void InitGarageStats()
    {
        _maxHealth_Garage = 120; 
        _maxShiled_Garage = 100; 
        _maxDefensePoints_Garage = 10; 

        _crystalNeedForUnitCreation = 5;
        _ironNeedForForUnitCreation = 5;
        _gelNeedForForUnitCreation = 5;

        _crystalNeedForBuilding_Garage = 10;
        _ironNeedForBuilding_Garage = 10;
        _gelNeedForBuilding_Garage = 10;

        _baseUpgradeStep_Garage = 25;
    }















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
    }
}
