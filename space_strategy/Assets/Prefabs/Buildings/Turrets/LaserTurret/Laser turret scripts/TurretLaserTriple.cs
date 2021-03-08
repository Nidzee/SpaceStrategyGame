using UnityEngine;

public class TurretLaserTriple : TurretLaser
{
    private GameObject barrel;
    private GameObject barrel1;
    private GameObject barrel2;

    private GameObject firePoint;
    private GameObject firePoint1;
    private GameObject firePoint2;

    private LineRenderer lineRenderer;
    private LineRenderer lineRenderer1;
    private LineRenderer lineRenderer2;

    private Quaternion targetRotationForBarrel = new Quaternion();
    private Quaternion targetRotationForBarrel1 = new Quaternion();
    private Quaternion targetRotationForBarrel2 = new Quaternion();

    private bool isBarrelFacingEnemy = false;
    private bool isBarrel1FacingEnemy = false;
    private bool isBarrel2FacingEnemy = false;


    // Function for creating building
    public void Creation(TurretLaser turretLaser)
    {
        type = turretLaser.type;

        healthPoints = turretLaser.healthPoints;
        shieldPoints = turretLaser.shieldPoints;
        InitStaticsLevel_3();

        this.gameObject.name = turretLaser.name + " 3";
        this.tag = TagConstants.buildingTag;
        this.gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        this.GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretLayer;


        // Reaplcing reference in Resource Manager class
        for (int i = 0; i < ResourceManager.Instance.laserTurretsList.Count; i++)
        {
            if (turretLaser == ResourceManager.Instance.laserTurretsList[i])
            {
                ResourceManager.Instance.laserTurretsList[i] = this;
                break;
            }
        }


        tileOccupied = turretLaser.tileOccupied;

        HelperObjectInit();
        InitBarrels();
        isPowerON = ResourceManager.Instance.IsPowerOn();
    }

    // Function for displaying info
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

            lineRenderer = barrel.gameObject.GetComponent<LineRenderer>();
            lineRenderer1 = barrel1.gameObject.GetComponent<LineRenderer>();
            lineRenderer2 = barrel2.gameObject.GetComponent<LineRenderer>();
        }
    }

    // Attack pattern
    public override void Attack()
    {
        if (isFacingEnemy)
        {
            RotateBarrelTowardsEnemy();
            RotateBarrel1TowardsEnemy();
            RotateBarrel2TowardsEnemy();

            if (isBarrelFacingEnemy && isBarrel1FacingEnemy && isBarrel2FacingEnemy)
            {
                if (!isLasersEnabled && attackState)
                {
                    lineRenderer.enabled = true;
                    lineRenderer1.enabled = true;
                    lineRenderer2.enabled = true;

                    isLasersEnabled = true;
                }

                lineRenderer.SetPosition(0, barrel.transform.position);
                lineRenderer1.SetPosition(0, barrel1.transform.position);
                lineRenderer2.SetPosition(0, barrel2.transform.position);

                lineRenderer.SetPosition(1, target.transform.position);
                lineRenderer1.SetPosition(1, target.transform.position);
                lineRenderer2.SetPosition(1, target.transform.position);
            }
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

    private void RotateBarrel1TowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel1.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel1 = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel1.transform.rotation = Quaternion.RotateTowards(barrel1.transform.rotation, targetRotationForBarrel1, barrelTurnSpeed * Time.deltaTime);

            if (barrel1.transform.rotation == targetRotationForBarrel1 && !isBarrel1FacingEnemy)
            {
                isBarrel1FacingEnemy = true;
            }
        }
    }

    private void RotateBarrel2TowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = barrel2.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotationForBarrel2 = Quaternion.Euler(new Vector3(0, 0, angle));
            barrel2.transform.rotation = Quaternion.RotateTowards(barrel2.transform.rotation, targetRotationForBarrel2, barrelTurnSpeed * Time.deltaTime);

            if (barrel2.transform.rotation == targetRotationForBarrel2 && !isBarrel2FacingEnemy)
            {
                isBarrel2FacingEnemy = true;
            }
        }
    }


    public override void ResetCombatMode()
    {
        isBarrelFacingEnemy = false;
        isBarrel1FacingEnemy = false;
        isBarrel2FacingEnemy = false;

        isLasersEnabled = false;
        isFacingEnemy = false;
    }

    public void TurnOffLasers()
    {
        Debug.Log("Turn off lasers");
        isBarrelFacingEnemy = false;
        isBarrel1FacingEnemy = false;
        isBarrel2FacingEnemy = false;
        
        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;
        lineRenderer2.enabled = false; 

        isFacingEnemy = false;
        isLasersEnabled = false;
    }
}