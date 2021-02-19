using UnityEngine;

public class TurretLaserSingle : TurretLaser
{
    private GameObject barrel;

    private GameObject firePoint;

    public LineRenderer lineRenderer;

    private Quaternion targetRotationForBarrel = new Quaternion();

    private bool isBarrelFacingEnemy = false;












    // Function for creating building
    public void Creation(Model model)
    {
        type = 1;
        HealthPoints = 100;
        ShieldPoints = 100;
        

        level = 1;
        turetteLaser_counter++;
        this.gameObject.name = "TL" + TurretLaser.turetteLaser_counter;
        ResourceManager.Instance.laserTurretsList.Add(this);

        tileOccupied = model.BTileZero;
        tileOccupied.GetComponent<Hex>().tile_Type = Tile_Type.ClosedTile;;


        HelperObjectInit();
        InitBarrels();
        isPowerON = ResourceManager.Instance.IsPowerOn();

        ResourceManager.Instance.CreateBuildingAndAddElectricityNeedCount();
    }

    // Function for displaying info
    public override void Invoke()
    {
        base.Invoke();

        turretMenuReference.ReloadPanel(this);
    }





    // private void Awake() // For  test
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

            lineRenderer = barrel.gameObject.GetComponent<LineRenderer>();
        }
    }



    // Attack pattern
    public override void Attack()
    {
        RotateBarrelTowardsEnemy();

        if (isBarrelFacingEnemy)
        {
            if (!isLasersEnabled)
            {
                lineRenderer.enabled = true;
                isLasersEnabled = true;
            }

            lineRenderer.SetPosition(0, barrel.transform.position);
            lineRenderer.SetPosition(1, target.transform.position);
        }
    }

    private void RotateBarrelTowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, targetRotationForBarrel, barrelTurnSpeed * Time.deltaTime);

            if ( barrel.transform.rotation == targetRotationForBarrel && !isBarrelFacingEnemy)
            {
                isBarrelFacingEnemy = true;
            }
        }
    }


    public void TurnOffLasers()
    {
        isBarrelFacingEnemy = false;
        
        if (lineRenderer)
            lineRenderer.enabled = false;

        isLasersEnabled = false;
    }
}