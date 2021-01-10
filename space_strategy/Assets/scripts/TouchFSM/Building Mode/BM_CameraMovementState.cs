using UnityEngine;

public class BM_CameraMovementState : ITouchState
{
    public ITouchState DoState()
    {
        DomyState();

        if (GameHendler.Instance.isZooming) // No nned for ResetState func cuz before Zoom we reset everything
        {
            GameHendler.Instance.resetInfo();
            return GameHendler.Instance.BM_zoomState;
        }

        else if (!GameHendler.Instance.isCameraState) // We can get into this case only with gbUP
            return GameHendler.Instance.BM_idleState;

        else
            return GameHendler.Instance.BM_cameraMovementState;
    }
    
    private void DomyState()
    {
        if (Input.touchCount == 2)
        {
            GameHendler.Instance.isZooming = true;
            //GameHendler.Instance.isCameraState = false;
            return;
        }

        if (Input.GetMouseButton(0))
            CameraMovement();

        if(Input.GetMouseButtonUp(0))
            StateReset();
            
    }

    private void CameraMovement()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameHendler.Instance.touchStart;
        Camera.main.transform.position = (Camera.main.transform.position - pos);
    }

    private void StateReset()
    {
        GameHendler.Instance.isCameraState = false;
        GameHendler.Instance.isFirstCollide = false;
        GameHendler.Instance.CurrentHex = null;
    }
}
