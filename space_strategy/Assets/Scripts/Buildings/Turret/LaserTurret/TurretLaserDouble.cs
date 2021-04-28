﻿using UnityEngine;

public class TurretLaserDouble : TurretLaser
{
    [SerializeField] private GameObject barrel;             // Init in inspector
    [SerializeField] private  GameObject barrel1;           // Init in inspector

    [SerializeField] private  GameObject firePoint;         // Init in inspector
    [SerializeField] private  GameObject firePoint1;        // Init in inspector

    [SerializeField] private  LineRenderer lineRenderer;    // Init in inspector
    [SerializeField] private  LineRenderer lineRenderer1;   // Init in inspector

    private Quaternion targetRotationForBarrel = new Quaternion();
    private Quaternion targetRotationForBarrel1 = new Quaternion();

    private bool isBarrelFacingEnemy = false;
    private bool isBarrel1FacingEnemy = false;


    public void ConstructBuildingAfterUpgrade(Turette previousTurret)
    {
        // Data initialization
        damagePoints = 10;
        int health = 0;
        int shield = 0;
        int defense = 0;
        switch (ResourceManager.Instance.shtabReference.level)
        {
            case 1:
            health = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_1;
            shield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_1;
            defense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_1;
            break;

            case 2:
            health = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_2;
            shield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_2;
            defense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_2;
            break;

            case 3:
            health = StatsManager._maxHealth_Lvl2_LaserTurret_Base_Lvl_3;
            shield = StatsManager._maxShiled_Lvl2_LaserTurret_Base_Lvl_3;
            defense = StatsManager._defensePoints_Lvl2_LaserTurret_Base_Lvl_3;
            break;
        }
        CreateGameUnit(health, shield, defense);
        


        // Init rest data from previous turret
        InitTurretDataFromPreviousTurret(previousTurret);



        // Init rest of Data
        InitData();


        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;

        // Reaplcing reference in Resource Manager class
        for (int i = 0; i < ResourceManager.Instance.laserTurretsList.Count; i++)
        {
            if (previousTurret == ResourceManager.Instance.laserTurretsList[i])
            {
                ResourceManager.Instance.laserTurretsList[i] = this;
                break;
            }
        }
    }

    public void ConstructBuildingFromFile_LaserDouble()
    {
        damagePoints = 10;

        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;

        ResourceManager.Instance.laserTurretsList.Add(this);

        
        if (upgradeTimer != 0)
        {
            StartCoroutine(UpgradeLogic());
        }
    }

    public override void Attack()
    {
        if (isFacingEnemy)
        {
            RotateBarrelTowardsEnemy();
            RotateBarrel1TowardsEnemy();

            if (isBarrelFacingEnemy && isBarrel1FacingEnemy)
            {
                if (!isAttackStart)
                {
                    isAttackStart = true;
                    base.Attack();
                }

                if (!isLasersEnabled && isAttackState)
                {
                    lineRenderer.enabled = true;
                    lineRenderer1.enabled = true;
                    isLasersEnabled = true;
                }

                lineRenderer.SetPosition(0, barrel.transform.position);
                lineRenderer1.SetPosition(0, barrel1.transform.position);

                lineRenderer.SetPosition(1, target.transform.position);
                lineRenderer1.SetPosition(1, target.transform.position);
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

    public override void ResetCombatMode()
    {
        TurnOffLaserDamage();
        isAttackStart = false;
        
        isBarrelFacingEnemy = false;
        isBarrel1FacingEnemy = false;

        isLasersEnabled = false;
        isFacingEnemy = false;
    }

    public void TurnOffLasers()
    {
        lineRenderer.enabled = false;
        lineRenderer1.enabled = false;

        isBarrelFacingEnemy = false;
        isBarrel1FacingEnemy = false;

        isLasersEnabled = false;
        isFacingEnemy = false;
    }
}