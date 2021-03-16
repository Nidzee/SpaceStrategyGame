using UnityEngine;

public class UnitIGatherState : IUnitState
{
    private float gatheringSpeed = 1f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (!unit.Home) // means that we dont have job and home
        {
            if (unit.unitData.isGatheringComplete)
            {
                unit.unitData.isGatheringComplete = false; // unit.destination = unit.home.GetUnitDestination().position;
            }
            
            else
            {
                GameObject.Destroy(unit.unitData.resource);
            }

            unit.ChangeDestination((int)UnitDestinationID.Null);

            return unit.unitData.unitIHomelessState;
        }

        else if (!unit.WorkPlace) // if we lost job - destroy resource and go home at any time
        {
            GameObject.Destroy(unit.unitData.resource);

            unit.unitData.isGatheringComplete = false; 
            unit.ChangeDestination((int)UnitDestinationID.Home);// unit.GetComponent<AIDestinationSetter>().target = unit.home.GetUnitDestination();// unit.destination = unit.home.GetUnitDestination().position;

            return unit.unitData.unitIGoToState;
        }

        else if (unit.unitData.isGatheringComplete)
        {
            unit.unitData.isGatheringComplete = false;
            unit.ChangeDestination((int)UnitDestinationID.Storage);// unit.GetComponent<AIDestinationSetter>().target = unit.storage.GetUnitDestination();// unit.destination = unit.storage.GetUnitDestination().position;
            
            return unit.unitData.unitIGoToState;
        }

        else
            return unit.unitData.unitIGatherState;
    }

    private void DoMyState(Unit unit)
    {
        if (unit.WorkPlace) // Magical cure
        {
            if (!unit.unitData.resource) // Creates unit-resource object inside shaft
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

            if (!unit.unitData.isGatheringComplete) // move resource object towards unit
            {
                unit.unitData.resource.transform.position = Vector3.MoveTowards(unit.unitData.resource.transform.position, 
                                                    unit.transform.position, gatheringSpeed*Time.deltaTime);
            }      
        }
    }

}