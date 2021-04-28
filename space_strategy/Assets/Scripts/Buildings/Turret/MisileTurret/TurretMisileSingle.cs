using UnityEngine;

public class TurretMisileSingle : TurretMisile
{
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject firePoint;

    public override void ConstructBuilding(Model model)
    {
        // Data initialization
        type = 2;
        int health = 0;
        int shield = 0;
        int defense = 0;
        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl1_MisileTurret_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl1_MisileTurret_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl1_MisileTurret_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl1_MisileTurret_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl1_MisileTurret_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl1_MisileTurret_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl1_MisileTurret_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl1_MisileTurret_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl1_MisileTurret_Base_Lvl_3;
            break;
        }
        CreateGameUnit(health, shield, defense);
        gameObject.name = "MT" + MTStaticData.turetteMisile_counter;
        MTStaticData.turetteMisile_counter++;



        // Rest data initialization
        base.ConstructBuilding(model);


        // Add to resource manager list
        ResourceManager.Instance.misileTurretsList.Add(this);
    }
    
    public void ConstructBuildingFromFile_MisileSingle()
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
            GameObject temp = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint.transform.position, base.targetRotation);
            temp.GetComponent<Misile>().target = base.target;

            Instantiate(MTStaticData._misileLaunchParticles, firePoint.transform.position, barrel.transform.rotation); 

            isFired = true;
        }
        else
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