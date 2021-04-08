using UnityEngine;

public class UnitIGatherState : IUnitState
{
    private float gatheringSpeed = 1f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (!unit.Home) // means that we dont have job and home
        {
            if (unit.unitData.resource)
            {
                GameObject.Destroy(unit.unitData.resource);
            }

            unit.unitData.isGatheringComplete = false;
            unit.unitData.isApproachShaft = false;
            unit.unitData.isApproachStorage = false;
            unit.unitData.isApproachHome = false;

            unit.ChangeDestination((int)UnitDestinationID.Null);

            return unit.unitData.unitIHomelessState;
        }

        else if (!unit.WorkPlace) // if we lost job - destroy resource and go home at any time
        {
            if (unit.unitData.resource)
            {
                GameObject.Destroy(unit.unitData.resource);
            }

            unit.unitData.isGatheringComplete = false;
            unit.unitData.isApproachShaft = false;
            unit.unitData.isApproachStorage = false;
            unit.unitData.isApproachHome = false;

            unit.ChangeDestination((int)UnitDestinationID.Home);
            unit.RebuildPath();

            return unit.unitData.unitIGoToState;
        }

        else if (unit.unitData.isGatheringComplete)
        {
            unit.unitData.isGatheringComplete = false;
            unit.unitData.isApproachShaft = false;
            unit.unitData.isApproachStorage = false;
            unit.unitData.isApproachHome = false;
            
            unit.ChangeDestination((int)UnitDestinationID.Storage);
            unit.RebuildPath();

            return unit.unitData.unitIGoToState;
        }

        else
            return unit.unitData.unitIGatherState;
    }

    private void DoMyState(Unit unit)
    {
        // Magical cure
        if (unit.WorkPlace)
        {
            // Creates unit-resource object inside shaft
            if (!unit.unitData.resource)
            {
                switch (unit.WorkPlace.mineShaftData.type)
                {
                    case 1:
                    unit.unitData.resource = GameObject.Instantiate(CSStaticData.crystalShaftResourcePrefab, unit.unitData.workPlace.mineShaftData.dispenser.transform.position, Quaternion.identity);
                    unit.unitData.resourceType = 1;
                    break;

                    case 2:
                    unit.unitData.resource = GameObject.Instantiate(ISStaticData.ironShaftResourcePrefab, unit.unitData.workPlace.mineShaftData.dispenser.transform.position, Quaternion.identity);
                    unit.unitData.resourceType = 2;
                    break;

                    case 3:
                    unit.unitData.resource = GameObject.Instantiate(GSStaticData.gelShaftResourcePrefab, unit.unitData.workPlace.mineShaftData.dispenser.transform.position, Quaternion.identity);
                    unit.unitData.resourceType = 3;
                    break;
                }
            } 

            // Move resource object towards unit
            if (!unit.unitData.isGatheringComplete)
            {
                unit.unitData.resource.transform.position = 
                Vector3.MoveTowards(unit.unitData.resource.transform.position, 
                unit.transform.position, gatheringSpeed * Time.deltaTime);
            }      
        }
    }

}