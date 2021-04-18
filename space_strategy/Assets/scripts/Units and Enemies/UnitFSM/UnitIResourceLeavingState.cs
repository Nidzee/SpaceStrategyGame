using UnityEngine;

public class UnitResourceLeavingState : IUnitState
{
    private bool isCoolDownOver = false;
    private float coolDownTimer = 2f;
    private bool isResourceDropped = false;

    private void StateReset()
    {
        coolDownTimer = 2f;
        isCoolDownOver = false;
        isResourceDropped = false;
    }

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (!unit.Home) // means that we dont have job and home
        {
            StateReset();

            unit.ChangeDestination((int)UnitDestinationID.Null);

            return unit.unitIHomelessState;
        }

        if (isCoolDownOver)
        {
            StateReset();

            // UNIT still have job - go to work
            if (unit.WorkPlace)
            {
                unit.ChangeDestination((int)UnitDestinationID.WorkPlace);
            }
            // UNIT dont have job - go home
            else 
            {
                unit.ChangeDestination((int)UnitDestinationID.Home);
            }

            unit.RebuildPath();

            return unit.unitIGoToState;
        }

        else 
            return unit.unitResourceLeavingState;
    }

    private void DoMyState(Unit unit)
    {
        // Resource destruction
        if (unit.resource)
        {
            GameObject.Destroy(unit.resource);
        }

        if (!isResourceDropped)
        {
            switch (unit.resourceType)
            {
                case 1:
                Debug.Log("We got CRYSTAL resource!");
                ResourceManager.Instance.AddCrystalResourcePoints();
                break;

                case 2:
                Debug.Log("We got IRON resource!");
                ResourceManager.Instance.AddIronResourcePoints();
                break;

                case 3:
                Debug.Log("We got GEL resource!");
                ResourceManager.Instance.AddGelResourcePoints();
                break;
            }
            isResourceDropped = true;
        }

        
        coolDownTimer -= Time.deltaTime;
        if (coolDownTimer <= 0)
        {
            isCoolDownOver = true;
        }
    }
}
