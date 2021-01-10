using UnityEngine;

public class CameraMovementState : ITouchState
{
    public ITouchState DoState()
    {
        DomyState();

        if (GameHendler.Instance.isZooming)
        {
            GameHendler.Instance.resetInfo();
            return GameHendler.Instance.zoomState;
        }

        else if (!GameHendler.Instance.isCameraState)
            return GameHendler.Instance.idleState;

        else
            return GameHendler.Instance.cameraMovementState;
    }
    
    private void DomyState()
    {
        if (Input.touchCount == 2)
        {
            GameHendler.Instance.isZooming = true;
            return;
        }

        if (Input.GetMouseButton(0))
            CameraMovement();

        if (Input.GetMouseButtonUp(0))
            StateReset();
    }

    private void CameraMovement()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameHendler.Instance.touchStart;
        Camera.main.transform.position -= pos;
    }

    private void StateReset()
    {
        GameHendler.Instance.isCameraState = false;
        GameHendler.Instance.isFirstCollide = false;
        GameHendler.Instance.CurrentHex = null;
    }
}
