using UnityEngine;

public class BuildingSelectionState : ITouchState
{
    private float touchTime = 0.2f;

    public ITouchState DoState()
    {
        DomyState();

        if (GameHendler.Instance.isZooming) // No need for ResetState cuz whole functionality is in *gameHendler.resetInfo();*
        {
            GameHendler.Instance.resetInfo();
            touchTime = 0.2f;
            return GameHendler.Instance.zoomState;
        }

        else if (!GameHendler.Instance.isBuildingSelected) // This case is executed only after ButtonUp -> reset is already executed
        {
            StateReset(); // Продубльоване виконання функції (тільки для симетрії)
            return GameHendler.Instance.idleState;
        }
        
        else if (GameHendler.Instance.touchStart != GameHendler.Instance.worldMousePosition) // We moved mouse
        {
            StateReset();
            GameHendler.Instance.isCameraState = true;
            return GameHendler.Instance.cameraMovementState;
        }
        
        else
            return GameHendler.Instance.buildingSelectionState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2)
        {
            GameHendler.Instance.isZooming = true;
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
                // Building selection logic
                Debug.Log("Selected Building - go menu now");
            }
            StateReset();
        }
    }

    private void StateReset() // Reseting all used variables
    {
        GameHendler.Instance.isBuildingSelected = false;
        GameHendler.Instance.isFirstCollide = false;
        GameHendler.Instance.CurrentHex = null;
        touchTime = 0.2f;
    }
}
