using UnityEngine;

public class TurretMisileSingle : TurretMisile
{
    private GameObject barrel;
    private GameObject firePoint;


    public override void ConstructBuilding(Model model)
    {
        CreateGameUnit(StatsManager._maxHealth_Lvl1_MisileTurret, StatsManager._maxShiled_Lvl1_MisileTurret, StatsManager._defensePoints_Lvl1_MisileTurret);
        turretData = new TurretData(this);
        misileTurretData = new MTData();

        base.ConstructBuilding(model);
        turretData.ConstructBuilding_MT();

        MTStaticData.turetteMisile_counter++;
        gameObject.name = "TM" + MTStaticData.turetteMisile_counter;
        // myName = gameObject.name;
        ResourceManager.Instance.misileTurretsList.Add(this);  //GetComponent<TurretMisile>()

        InitBarrels();
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