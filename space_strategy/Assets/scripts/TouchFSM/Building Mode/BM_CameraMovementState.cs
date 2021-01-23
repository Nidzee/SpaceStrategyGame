using UnityEngine;

public class BM_CameraMovementState : ITouchState
{
    private bool isCameraMoving = true;
    private bool isZooming = false;

    public ITouchState DoState()
    {
        DomyState();

        if (isZooming)
        {
            StateReset();
            return GameHendler.Instance.BM_zoomState;
        }

        else if (!isCameraMoving)
        {
            StateReset();
            return GameHendler.Instance.BM_idleState;
        }

        else
            return GameHendler.Instance.BM_cameraMovementState;
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

    private void StateReset()
    {
        isCameraMoving = true;
        isZooming = false;
    }

    private void CameraMovement()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameHendler.Instance.touchStart;
        Camera.main.transform.position = (Camera.main.transform.position - pos);
    }
}
