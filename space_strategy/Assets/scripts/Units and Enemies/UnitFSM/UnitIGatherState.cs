using UnityEngine;

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
                unit.destination = unit.home.angarPosition;
                return unit.unitIHomelessState;
            }
            else
            {
                GameObject.Destroy(unit.resource);
                return unit.unitIHomelessState;
            }
        }

        else if (!unit.workPlace) // if we lost job - destroy resource and go home at any time
        {
            // Destroy resource
            GameObject.Destroy(unit.resource);

            unit.isGatheringComplete = false; 

            unit.destination = unit.home.angarPosition;
            return unit.unitIGoToState;
        }

        else if (unit.isGatheringComplete)
        {
            unit.isGatheringComplete = false;  

            unit.destination = unit.storage.storageConsumerPosition;
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
                    unit.resource = GameObject.Instantiate(CrystalShaft.crystalShaftResourcePrefab, unit.workPlace.dispenserPosition, Quaternion.identity);
                    break;

                    case 2:
                    unit.resource = GameObject.Instantiate(IronShaft.ironShaftResourcePrefab, unit.workPlace.dispenserPosition, Quaternion.identity);
                    break;

                    case 3:
                    unit.resource = GameObject.Instantiate(GelShaft.gelShaftResourcePrefab, unit.workPlace.dispenserPosition, Quaternion.identity);
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

