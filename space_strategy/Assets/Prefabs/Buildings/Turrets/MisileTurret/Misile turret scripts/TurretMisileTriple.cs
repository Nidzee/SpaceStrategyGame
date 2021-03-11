using UnityEngine;

public class TurretMisileTriple : TurretMisile
{
    private GameObject barrel;
    private GameObject barrel1;
    private GameObject barrel2;

    private GameObject firePoint;
    private GameObject firePoint1;
    private GameObject firePoint2;


    // Function for creating building
    public void Creation(TurretMisile turretMisile)
    {
        type = turretMisile.type;

        healthPoints = turretMisile.healthPoints;
        shieldPoints = turretMisile.shieldPoints;
        InitStaticsLevel_3();

        this.gameObject.name = turretMisile.name + " 3";
        this.tag = TagConstants.buildingTag;
        this.gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        this.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;


        // Reaplcing reference in Resource Manager class
        for (int i = 0; i < ResourceManager.Instance.misileTurretsList.Count; i++)
        {
            if (turretMisile == ResourceManager.Instance.misileTurretsList[i])
            {
                ResourceManager.Instance.misileTurretsList[i] = this;
                break;
            }
        }


        _tileOccupied = turretMisile._tileOccupied;

        HelperObjectInit();
        InitBarrels();
        isPowerON = ResourceManager.Instance.IsPowerOn();
    }

    // Function for diaplying info
    public override void Invoke()
    {
        base.Invoke();

        turretMenuReference.ReloadPanel(this);
    }


    private void InitBarrels()
    {
        if (gameObject.transform.childCount != 0)
        {
            barrel = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
            barrel.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);

            barrel1 = gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
            barrel1.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);

            barrel2 = gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject;
            barrel2.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);

            firePoint = barrel.transform.GetChild(0).gameObject;
            firePoint1 = barrel1.transform.GetChild(0).gameObject;
            firePoint2 = barrel2.transform.GetChild(0).gameObject;
        }
    }

    // Attack pattern
    public override void Attack()
    {
        if (!isFired)
        {
            GameObject misile = GameObject.Instantiate(misilePrefab, firePoint.transform.position, barrel.transform.rotation);
            misile.GetComponent<Rigidbody2D>().AddForce(transform.forward * 100f);
            misile.GetComponent<Misile>().target = base.target;

            GameObject misile1 = GameObject.Instantiate(misilePrefab, firePoint1.transform.position, barrel1.transform.rotation);
            misile1.GetComponent<Rigidbody2D>().AddForce(transform.forward * 100f);
            misile1.GetComponent<Misile>().target = base.target;

            GameObject misile2 = GameObject.Instantiate(misilePrefab, firePoint2.transform.position, barrel2.transform.rotation);
            misile2.GetComponent<Rigidbody2D>().AddForce(transform.forward * 100f);
            misile2.GetComponent<Misile>().target = base.target;


            Instantiate(_misileLaunchParticles, firePoint.transform.position, barrel.transform.rotation); 
            Instantiate(_misileLaunchParticles, firePoint1.transform.position, barrel1.transform.rotation); 
            Instantiate(_misileLaunchParticles, firePoint2.transform.position, barrel2.transform.rotation); 


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