﻿using UnityEngine;

public class TurretMisileSingle : TurretMisile
{
    public GameObject barrel;
    public GameObject firePoint;




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