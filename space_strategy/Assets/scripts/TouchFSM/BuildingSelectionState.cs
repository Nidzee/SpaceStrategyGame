using UnityEngine;

public class BuildingSelectionState : ITouchState
{
    public  float TouchTime = 0.2f;

    public ITouchState DoState(GameHendler gh)
    {
        DomyState(gh);

        if (gh.isZooming) // No need for ResetState cuz whole functionality is in *gh.resetInfo();*
            return gh.zoomState;

        else if (!gh.isBuildingSelected) // This case is executed only after ButtonUp -> reset is already executed
            return gh.idleState;

        else if (gh.touchStart != gh._worldMousePosition) // We moved mouse
        {
            gh.isCameraState = false;
            StateReset(gh);
            return gh.cameraMovementState;
        }
        
        else
            return gh.buildingSelectionState;
    }

    private void DomyState(GameHendler gh)
    {
        if(Input.touchCount == 2)
        {
            gh.isZooming = true;
            TouchTime = 0.2f;
            gh.resetInfo(); // In here are all *ResetState* variables
            return;
        }

        if (Input.GetMouseButton(0))
        {
            TouchTime -= Time.deltaTime;
            if (TouchTime < 0)
            {
                TouchTime = 0f;
            }
            //Building_movement();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if ((TouchTime > 0 ) && gh.touchStart == gh._worldMousePosition)
            {
                Debug.Log("Selected Building - go menu now");
            }
            StateReset(gh);
        }
    }

    private void StateReset(GameHendler gh) // Reseting all used variables
    {
        gh.isBuildingSelected = false;
            
        gh.isFirstCollide = false;
        gh.CurrentHex = null;
        TouchTime = 0.2f;
    }
}
