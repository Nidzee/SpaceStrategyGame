using UnityEngine;

public class TurretMisileDouble : TurretMisile
{
    public GameObject barrel;
    public GameObject barrel1;
    public GameObject firePoint;
    public GameObject firePoint1;


    // Function for creating building
    public void Creation(TurretMisile turretMisile)
    {
        type = turretMisile.type;

        healthPoints = turretMisile.healthPoints;
        shieldPoints = turretMisile.shieldPoints;
        InitStaticsLevel_2();

        this.gameObject.name = turretMisile.name + " 2";
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


        tileOccupied = turretMisile.tileOccupied;

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



    // private void Awake() // For prefab test
    // {
    //     isCreated = true;
    //     InitBarrels();
    // }

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



    // Attack pattern
    public override void Attack()
    {
        if (!isFired)
        {
            GameObject misile = GameObject.Instantiate(misilePrefab, firePoint.transform.position, base.targetRotation);
            misile.GetComponent<Misile>().target = base.target;

            GameObject misile1 = GameObject.Instantiate(misilePrefab, firePoint1.transform.position, base.targetRotation);
            misile1.GetComponent<Misile>().target = base.target;

            Instantiate(_misileLaunchParticles, firePoint.transform.position, barrel.transform.rotation); 
            Instantiate(_misileLaunchParticles, firePoint1.transform.position, barrel1.transform.rotation); 


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