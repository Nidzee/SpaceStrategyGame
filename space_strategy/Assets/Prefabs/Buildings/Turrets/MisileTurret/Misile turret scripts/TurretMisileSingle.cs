using UnityEngine;

public class TurretMisileSingle : TurretMisile
{
    private GameObject barrel;
    private GameObject firePoint;


    public override void ConstructBuilding(Model model)
    {
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

        type = 2;

        CreateGameUnit(health, shield, defense);


        isFired = false;
        coolDownTimer = 1f;

        base.ConstructBuilding(model);

        MTStaticData.turetteMisile_counter++;
        gameObject.name = "TM" + MTStaticData.turetteMisile_counter;
        ResourceManager.Instance.misileTurretsList.Add(this);  //GetComponent<TurretMisile>()

        InitBarrels();
    }
    
    public void ConstructBuildingFromFile_MisileSingle()
    {
        ResourceManager.Instance.misileTurretsList.Add(this);

        InitBarrels();

        
        if (upgradeTimer != 0)
        {
            StartCoroutine(UpgradeLogic());
        }
    }



















    private void InitBarrels()
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;
            gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;

            barrel = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
            barrel.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            barrel.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;
            barrel.GetComponent<SpriteRenderer>().sortingOrder = 3;

            firePoint = barrel.transform.GetChild(0).gameObject;
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