﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Turette : AliveGameUnit, IBuilding
{
    public static TurretMenu turretMenuReference; // Reference to UI panel

    public GameObject tileOccupied = null;                      // Reference to real MapTile on which building is set

    public List<Enemy> enemiesInsideRange;
    public Enemy target;

    public bool isFacingEnemy = false;
    private bool isTurnedInIdleMode = true;
    public bool attackState = false;
    public bool isCreated = false;

    private float turnSpeed = 200f;
    private float coolDownTurnTimer = 3f;

    private Quaternion idleRotation = new Quaternion();
    public Quaternion targetRotation = new Quaternion();

    public int level = 1;
    public int type = 0;

    public bool isMenuOpened = false;

    public bool isPowerON = true;

    // Upgrade logic
    public float upgradeTimer = 0f;
    





    private void Update()
    {
        if (isCreated)
        {
            if (isPowerON)
            {
                if (attackState)
                {
                    CombatMode();
                }
                else
                {
                    IdleMode();
                }
            }

            else
            {
                NoElectricityMode();
            }
        }
    }




    // Upgrade logic in update
    public void StartUpgrade()
    {
        StartCoroutine(UpgradeLogic());
    }

    private float _timerStep = 0.5f;

    IEnumerator UpgradeLogic()
    {
        while (upgradeTimer < 1)
        {
            upgradeTimer += _timerStep * Time.deltaTime;

            if (isMenuOpened)
            {
                switch(level)
                {
                    case 1:
                    {
                        turretMenuReference.level2.fillAmount = upgradeTimer;
                    }
                    break;

                    case 2:
                    {
                        turretMenuReference.level3.fillAmount = upgradeTimer;
                    }
                    break;

                    case 3:
                    {
                        Debug.Log("Error");
                    }
                    break;
                }
            }

            yield return null;
        }

        upgradeTimer = 0;

        TurretUpgrading();
    }


    private void TurretUpgrading()
    {
        upgradeTimer = 0f;           // Reset timer

        Debug.Log("TURRET LEVEL UP!");

        level++;

        Turette temp = null;

        // Replace old turret with new turret HERE
        switch (type)
        {
            // Upgrade laser turret
            case 1:
            {
                TurretLaser turretLaser = null;
                switch (level)
                {
                    case 1:
                    Debug.Log("Noone will never be here ;c ");
                    break;

                    case 2:
                    {
                        turretLaser = GameObject.Instantiate(PrefabManager.Instance.doubleTuretteLaserPrefab, this.transform.position, this.transform.rotation).GetComponent<TurretLaserDouble>();
                        turretLaser.GetComponent<TurretLaserDouble>().Creation(this.GetComponent<TurretLaser>());
                        temp = turretLaser;
                    }
                    break;

                    case 3:
                    {
                        turretLaser = GameObject.Instantiate(PrefabManager.Instance.tripleTuretteLaserPrefab, this.transform.position, this.transform.rotation).GetComponent<TurretLaserTriple>();
                        turretLaser.GetComponent<TurretLaserTriple>().Creation(this.GetComponent<TurretLaser>());
                        temp = turretLaser;
                    }
                    break;
                }

                GameViewMenu.Instance.ReloadMisileTurretHPSP_Laser(turretLaser);
            }
            break;


            // Upgrade misile turret
            case 2:
            {
                TurretMisile turretMisile = null;
                switch (level)
                {
                    case 1:
                    Debug.Log("Noone will never be here ;c ");
                    break;

                    case 2:
                    {
                        turretMisile = GameObject.Instantiate(PrefabManager.Instance.doubleturetteMisilePrefab, this.transform.position, this.transform.rotation).GetComponent<TurretMisileDouble>();
                        turretMisile.GetComponent<TurretMisileDouble>().Creation(this.GetComponent<TurretMisile>());
                        temp = turretMisile;
                    }
                    break;

                    case 3:
                    {
                        turretMisile = GameObject.Instantiate(PrefabManager.Instance.truipleturetteMisilePrefab, this.transform.position, this.transform.rotation).GetComponent<TurretMisileTriple>();
                        turretMisile.GetComponent<TurretMisileTriple>().Creation(this.GetComponent<TurretMisile>());
                        temp = turretMisile;
                    }
                    break;
                }

                GameViewMenu.Instance.ReloadMisileTurretHPSP_Misile(turretMisile);
            }
            break;
        }

        // Destroy old turret
        // Chaek if menu is opened
        // Check if BuildingManageMenu is opened - isnide cases above!

        if (isMenuOpened)
        {
            turretMenuReference.ReloadPanel(temp);
            // turretMenuReference.ReloadLevelManager(); // update buttons and visuals
        }

        Destroy(this.gameObject);
    }























    // Function for displaying info
    public virtual void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("TurretMenu");
        
        if (!turretMenuReference) // executes once
        {
            turretMenuReference = GameObject.Find("TurretMenu").GetComponent<TurretMenu>();
        }
    }

    public virtual void DestroyTurret()
    {
        if (isMenuOpened)
        {
            turretMenuReference.ExitMenu();
        }
                
        ResourceManager.Instance.DestroyBuildingAndRemoveElectricityNeedCount();

        // Rest in child classes
    }

#region  Terret function

    // Initializing helper GameObject - Dispenser
    public void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.turretRange;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer); // Means that it is noninteractible
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerConstants.turretRangeLayer;
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }

        isCreated = true;
    }

    private void IdleMode()
    {
        RandomIdleTurn();
    }

    private void CombatMode()
    {
        TurnTowardsEnemy();
        
        if (isFacingEnemy)
        {
            Attack();
        }
    }

    private void NoElectricityMode()
    {
        Debug.Log("I have no power!");
    }

    private void RandomIdleTurn()
    {
        if (isTurnedInIdleMode)
        {
            coolDownTurnTimer -= Time.deltaTime;
            if (coolDownTurnTimer < 0)
            {
                coolDownTurnTimer = 5f;
                isTurnedInIdleMode = false;
                idleRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, -360f)));
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, idleRotation, turnSpeed * Time.deltaTime);

            if (transform.rotation == idleRotation)
            {
                isTurnedInIdleMode = true;
            }
        }
    }

    // Turning turret logic - correct!
    private void TurnTowardsEnemy()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            if (transform.rotation == targetRotation && !isFacingEnemy)
            {
                isFacingEnemy = true;
            }
        }
    }

    // Every turret has its own attack pattern
    public virtual void Attack(){}

#endregion

}
