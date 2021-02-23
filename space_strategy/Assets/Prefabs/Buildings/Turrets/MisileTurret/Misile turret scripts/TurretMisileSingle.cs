using UnityEngine;

public class TurretMisileSingle : TurretMisile
{
    public GameObject barrel;
    public GameObject firePoint;




    // Function for creating building
    public void Creation(Model model)
    {
        type = 2;
        InitStaticsLevel_1();
        
        turetteMisile_counter++;
        this.gameObject.name = "TM" + TurretMisile.turetteMisile_counter;
        ResourceManager.Instance.misileTurretsList.Add(this);

        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;;


        HelperObjectInit();
        InitBarrels();
        isPowerON = ResourceManager.Instance.IsPowerOn();

        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
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
        if (!isFired)
        {
            GameObject temp = GameObject.Instantiate(misilePrefab, firePoint.transform.position, base.targetRotation);
            temp.GetComponent<Misile>().target = base.target;

            Instantiate(_misileLaunchParticles, firePoint.transform.position, barrel.transform.rotation); 

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