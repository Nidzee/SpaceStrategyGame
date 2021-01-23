using UnityEngine;

public class BuildingSelectionState : ITouchState
{
    private float touchTime = 0.2f;
    private bool isBuildingSelected = true;
    private bool isZooming = false;

    public ITouchState DoState()
    {
        DomyState();

        if (isZooming) // No need for ResetState cuz whole functionality is in *gameHendler.resetInfo();*
        {
            StateReset();
            return GameHendler.Instance.zoomState;
        }

        else if (!isBuildingSelected) // This case is executed only after ButtonUp -> reset is already executed
        {
            StateReset();
            return GameHendler.Instance.idleState;
        }
        
        else if (GameHendler.Instance.touchStart != GameHendler.Instance.worldMousePosition) // We moved mouse
        {
            StateReset();
            return GameHendler.Instance.cameraMovementState;
        }
        
        else
            return GameHendler.Instance.buildingSelectionState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2)
        {
            isZooming = true;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            touchTime -= Time.deltaTime;
            if (touchTime < 0)
                touchTime = 0f;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            if ((touchTime > 0 ) && GameHendler.Instance.touchStart == GameHendler.Instance.worldMousePosition)
            {
                GameHendler.Instance.selctedBuilding.GetComponent<IBuilding>().Invoke();
            }
            isBuildingSelected = false;
        }
    }

    private void StateReset() // Reseting all used variables
    {
        isBuildingSelected = true;
        isZooming = false;
        touchTime = 0.2f;
        GameHendler.Instance.selctedBuilding = null;
    }
}
