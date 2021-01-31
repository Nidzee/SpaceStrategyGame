using UnityEngine;

public class TurretLaserSingle : TurretLaser
{
    private GameObject barrel;

    private GameObject firePoint;

    private LineRenderer lineRenderer;

    private Quaternion targetRotationForBarrel = new Quaternion();

    private bool isBarrelFacingEnemy = false;












    // Function for creating building
    public override void Creation(Model model)
    {
        base.Creation(model);

        InitBarrels();
    }

    // Function for displaying info
    public override void Invoke()
    {
        base.Invoke();

        turretMenuReference.ReloadPanel(this);
    }





    private void Awake() // For  test
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
        
        lineRenderer.enabled = false;

        isLasersEnabled = false;
    }
}