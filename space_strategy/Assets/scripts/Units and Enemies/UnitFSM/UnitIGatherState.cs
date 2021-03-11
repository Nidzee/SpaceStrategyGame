using UnityEngine;
using Pathfinding;


public class UnitIGatherState : IUnitState
{
    private float gatheringSpeed = 1f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);

        if (!unit.home) // means that we dont have job and home
        {
            if (unit.isGatheringComplete)
            {
                unit.isGatheringComplete = false;

                unit.GetComponent<AIDestinationSetter>().target = unit.home.angar.transform;

                unit.destination = unit.home.angar.transform.position;
            }
            
            else
            {
                GameObject.Destroy(unit.resource);
            }

            return unit.unitIHomelessState;
        }

        else if (!unit.workPlace) // if we lost job - destroy resource and go home at any time
        {
            // Destroy resource
            GameObject.Destroy(unit.resource);

            unit.isGatheringComplete = false; 

            unit.GetComponent<AIDestinationSetter>().target = unit.home.angar.transform;

            unit.destination = unit.home.angar.transform.position;
            return unit.unitIGoToState;
        }

        else if (unit.isGatheringComplete)
        {
            unit.isGatheringComplete = false;  

            unit.GetComponent<AIDestinationSetter>().target = unit.storage.storageConsumer.transform;

            unit.destination = unit.storage.storageConsumer.transform.position;
            return unit.unitIGoToState;
        }

        else
            return unit.unitIGatherState;
    }

    private void DoMyState(Unit unit)
    {
        if (unit.workPlace) // Magical cure
        {
            if (!unit.resource) // Creates unit-resource object inside shaft
            {
                switch (unit.workPlace.type)
                {
                    case 1:
                    unit.resource = GameObject.Instantiate(CrystalShaft.crystalShaftResourcePrefab, unit.workPlace.dispenser.transform.position, Quaternion.identity);
                    unit.resourceType = 1;
                    break;

                    case 2:
                    unit.resource = GameObject.Instantiate(IronShaft.ironShaftResourcePrefab, unit.workPlace.dispenser.transform.position, Quaternion.identity);
                    unit.resourceType = 2;
                    break;

                    case 3:
                    unit.resource = GameObject.Instantiate(GelShaft.gelShaftResourcePrefab, unit.workPlace.dispenser.transform.position, Quaternion.identity);
                    unit.resourceType = 3;
                    break;
                }
            } 

            if (!unit.isGatheringComplete) // move resource object towards unit
            {
                unit.resource.transform.position = Vector3.MoveTowards(unit.resource.transform.position, 
                                                    unit.transform.position, gatheringSpeed*Time.deltaTime);
            }      
        }
    }
}

