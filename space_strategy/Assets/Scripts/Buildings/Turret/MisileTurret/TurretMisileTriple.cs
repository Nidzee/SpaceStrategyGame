using UnityEngine;

public class TurretMisileTriple : TurretMisile
{
    [SerializeField] private  GameObject barrel;
    [SerializeField] private  GameObject barrel1;
    [SerializeField] private  GameObject barrel2;

    [SerializeField] private  GameObject firePoint;
    [SerializeField] private  GameObject firePoint1;
    [SerializeField] private  GameObject firePoint2;

    public void ConstructBuildingAfterUpgrade(Turette previousTurret)
    {
        // Data initialization
        int health = 0;
        int shield = 0;
        int defense = 0;
        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl3_MisileTurret_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl3_MisileTurret_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl3_MisileTurret_Base_Lvl_3;
            break;
        }
        CreateGameUnit(health, shield, defense);
        

        // Rest data initialization from previous turret
        InitTurretDataFromPreviousTurret(previousTurret);


        // Init rest of Data
        InitData();


        // Reaplcing reference in Resource Manager class
        for (int i = 0; i < ResourceManager.Instance.misileTurretsList.Count; i++)
        {
            if (previousTurret == ResourceManager.Instance.misileTurretsList[i])
            {
                ResourceManager.Instance.misileTurretsList[i] = this;
                break;
            }
        }
    }

    public void ConstructBuildingFromFile_MisileTriple()
    {
        ResourceManager.Instance.misileTurretsList.Add(this);


        if (upgradeTimer != 0)
        {
            StartCoroutine(UpgradeLogic());
        }
    }

    public override void Attack()
    {
        if (!isFired)
        {
            GameObject misile = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint.transform.position, barrel.transform.rotation);
            misile.GetComponent<Rigidbody2D>().AddForce(transform.forward * 100f);
            misile.GetComponent<Misile>().target = base.target;

            GameObject misile1 = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint1.transform.position, barrel1.transform.rotation);
            misile1.GetComponent<Rigidbody2D>().AddForce(transform.forward * 100f);
            misile1.GetComponent<Misile>().target = base.target;

            GameObject misile2 = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint2.transform.position, barrel2.transform.rotation);
            misile2.GetComponent<Rigidbody2D>().AddForce(transform.forward * 100f);
            misile2.GetComponent<Misile>().target = base.target;


            Instantiate(MTStaticData._misileLaunchParticles, firePoint.transform.position, barrel.transform.rotation); 
            Instantiate(MTStaticData._misileLaunchParticles, firePoint1.transform.position, barrel1.transform.rotation); 
            Instantiate(MTStaticData._misileLaunchParticles, firePoint2.transform.position, barrel2.transform.rotation); 


            isFired = true;
        }
        else // Cooldown
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer < 0)
            {
                coolDownTimer = 1f;
                isFired = false;
            }
        }
    }
}