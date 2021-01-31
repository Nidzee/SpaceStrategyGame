using UnityEngine;

public class TurretMisileDouble : TurretMisile
{
    public GameObject barrel;
    public GameObject barrel1;
    public GameObject firePoint;
    public GameObject firePoint1;


    // Function for creating building
    public override void Creation(Model model)
    {
        base.Creation(model);

        InitBarrels();
    }

    // Function for diaplying info
    public override void Invoke()
    {
        base.Invoke();

        turretMenuReference.ReloadPanel(this);
    }



    private void Awake() // For prefab test
    {
        isCreated = true;
        InitBarrels();
    }

    private void InitBarrels()
    {
        if (gameObject.transform.childCount != 0)
        {
            barrel = gameObject.transform.GetChild(1).gameObject;
            barrel.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            barrel.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.buildingLayer;

            barrel1 = gameObject.transform.GetChild(2).gameObject;
            barrel1.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
            barrel1.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.buildingLayer;


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