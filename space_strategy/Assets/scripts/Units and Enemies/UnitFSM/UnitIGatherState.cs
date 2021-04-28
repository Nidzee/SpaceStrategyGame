using UnityEngine;

public class UnitExtractingState : IUnitState
{
    private float gatheringSpeed = 1f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (!unit.Home) // means that we dont have job and home
        {
            if (unit.resource)
            {
                GameObject.Destroy(unit.resource);
            }

            unit.isGatheringComplete = false;
            unit.isApproachShaft = false;
            unit.isApproachStorage = false;
            // unit.isApproachHome = false;

            unit.ChangeDestination((int)UnitDestinationID.Null);

            return unit.noSignalState;
        }

        else if (!unit.WorkPlace) // if we lost job - destroy resource and go home at any time
        {
            if (unit.resource)
            {
                GameObject.Destroy(unit.resource);
            }

            unit.isGatheringComplete = false;
            unit.isApproachShaft = false;
            unit.isApproachStorage = false;
            // unit.isApproachHome = false;

            unit.ChangeDestination((int)UnitDestinationID.Home);
            unit.RebuildPath();

            return unit.movingState;
        }

        else if (unit.isGatheringComplete)
        {
            unit.isGatheringComplete = false;
            unit.isApproachShaft = false;
            unit.isApproachStorage = false;
            // unit.isApproachHome = false;
            
            unit.ChangeDestination((int)UnitDestinationID.Storage);
            unit.RebuildPath();

            return unit.movingState;
        }

        else
            return unit.extractingState;
    }

    private void DoMyState(Unit unit)
    {
        // Magical cure
        if (unit.WorkPlace)
        {
            // Creates unit-resource object inside shaft
            if (!unit.resource)
            {
                switch (unit.WorkPlace.type)
                {
                    case 1:
                    unit.resource = GameObject.Instantiate(CSStaticData.crystalShaftResourcePrefab, unit.WorkPlace.dispenser.transform.position, Quaternion.identity);
                    unit.resourceType = 1;
                    break;

                    case 2:
                    unit.resource = GameObject.Instantiate(ISStaticData.ironShaftResourcePrefab, unit.WorkPlace.dispenser.transform.position, Quaternion.identity);
                    unit.resourceType = 2;
                    break;

                    case 3:
                    unit.resource = GameObject.Instantiate(GSStaticData.gelShaftResourcePrefab, unit.WorkPlace.dispenser.transform.position, Quaternion.identity);
                    unit.resourceType = 3;
                    break;
                }
            } 

            // Move resource object towards unit
            if (!unit.isGatheringComplete)
            {
                unit.resource.transform.position = 
                Vector3.MoveTowards(unit.resource.transform.position, 
                unit.transform.position, gatheringSpeed * Time.deltaTime);
            }      
        }
    }
}