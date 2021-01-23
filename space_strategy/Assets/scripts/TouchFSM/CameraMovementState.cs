using UnityEngine;

public class CameraMovementState : ITouchState
{
    private bool isCameraMoving = true;
    private bool isZooming = false;

    public ITouchState DoState()
    {
        DomyState();

        if (isZooming)
        {
            StateReset();
            return GameHendler.Instance.zoomState;
        }

        else if (!isCameraMoving)
        {
            StateReset();
            return GameHendler.Instance.idleState;
        }

        else
            return GameHendler.Instance.cameraMovementState;
    }
    
    private void DomyState()
    {
        if (Input.touchCount == 2)
        {
            isZooming = true;
            return;
        }

        if (Input.GetMouseButton(0))
            CameraMovement();

        if (Input.GetMouseButtonUp(0))
            isCameraMoving = false;
    }

    private void CameraMovement() // Do not touch!
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameHendler.Instance.touchStart;
        Camera.main.transform.position -= pos;
    }

    private void StateReset()
    {
        isCameraMoving = true;
        isZooming = false;
    }
}
