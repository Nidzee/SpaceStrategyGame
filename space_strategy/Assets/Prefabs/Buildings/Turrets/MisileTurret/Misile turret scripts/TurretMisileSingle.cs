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

        CreateGameUnit(health, shield, defense);

        turretData = new TurretData(this);
        misileTurretData = new MTData();

        base.ConstructBuilding(model);
        turretData.type = 2;

        MTStaticData.turetteMisile_counter++;
        gameObject.name = "TM" + MTStaticData.turetteMisile_counter;
        // myName = gameObject.name;
        ResourceManager.Instance.misileTurretsList.Add(this);  //GetComponent<TurretMisile>()

        InitBarrels();
    }
    
    public void ConstructBuildingFromFile_MisileSingle()
    {
        misileTurretData = new MTData();

        ResourceManager.Instance.misileTurretsList.Add(this);

        InitBarrels();

        
        if (turretData.upgradeTimer != 0)
        {
            StartCoroutine(turretData.UpgradeLogic());
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

    // Attack pattern
    public override void Attack()
    {
        if (!misileTurretData.isFired)
        {
            GameObject temp = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint.transform.position, base.turretData.targetRotation);
            temp.GetComponent<Misile>().target = base.turretData.target;

            Instantiate(MTStaticData._misileLaunchParticles, firePoint.transform.position, barrel.transform.rotation); 

            misileTurretData.isFired = true;
        }
        else
        {
            misileTurretData.coolDownTimer -= Time.deltaTime;
            if (misileTurretData.coolDownTimer < 0)
            {
                misileTurretData.coolDownTimer = 1f;
                misileTurretData.isFired = false;
            }
        }
    }
}