using UnityEngine;

public class TurretMisileDouble : TurretMisile
{
    public GameObject barrel;
    public GameObject barrel1;
    public GameObject firePoint;
    public GameObject firePoint1;





    public void ConstructBuildingAfterUpgrade(Turette turretMisile)
    {
        int health = 0;
        int shield = 0;
        int defense = 0;

        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl2_MisileTurret_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl2_MisileTurret_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl2_MisileTurret_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl2_MisileTurret_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl2_MisileTurret_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl2_MisileTurret_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl2_MisileTurret_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl2_MisileTurret_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl2_MisileTurret_Base_Lvl_3;
            break;
        }

        CreateGameUnit(health, shield, defense);

        InitTurretDataFromPreviousTurret_AlsoInitHelperObj_AlsoInitTurretData(turretMisile);


        isFired = false;
        coolDownTimer = 1f;




        InitBarrels();

        // Reaplcing reference in Resource Manager class
        for (int i = 0; i < ResourceManager.Instance.misileTurretsList.Count; i++)
        {
            if (turretMisile == ResourceManager.Instance.misileTurretsList[i])
            {
                ResourceManager.Instance.misileTurretsList[i] = this;
                break;
            }
        }
    }

    public void ConstructBuildingFromFile_MisileDouble()
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
            barrel = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
            barrel.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);

            barrel1 = gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
            barrel1.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);

            firePoint = barrel.transform.GetChild(0).gameObject;
            firePoint1 = barrel1.transform.GetChild(0).gameObject;
        }
    }

    public override void Attack()
    {
        if (!isFired)
        {
            GameObject misile = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint.transform.position, base.targetRotation);
            misile.GetComponent<Misile>().target = base.target;

            GameObject misile1 = GameObject.Instantiate(MTStaticData.misilePrefab, firePoint1.transform.position, base.targetRotation);
            misile1.GetComponent<Misile>().target = base.target;

            Instantiate(MTStaticData._misileLaunchParticles, firePoint.transform.position, barrel.transform.rotation); 
            Instantiate(MTStaticData._misileLaunchParticles, firePoint1.transform.position, barrel1.transform.rotation); 


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