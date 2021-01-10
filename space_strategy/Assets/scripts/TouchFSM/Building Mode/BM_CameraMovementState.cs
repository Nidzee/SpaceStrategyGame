using UnityEngine;

public class BM_CameraMovementState : ITouchState
{
    public ITouchState DoState(GameHendler gh)
    {
        DomyState(gh);

        if (gh.isZooming) // No nned for ResetState func cuz before Zoom we reset everything
            return gh.BM_zoomState;

        else if(gh.isCameraState) // We can get into this case only with gbUP
            return gh.BM_idleState;

        else
            return gh.BM_cameraMovementState;
    }
    
    private void DomyState(GameHendler gh)
    {
        if(Input.touchCount == 2)
        {
            gh.isZooming = true;
            gh.isCameraState = true;
            gh.resetInfo();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            //Debug.Log("Moving camera!");
            Camera_Movement(gh);
        }

        if(Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Camera STOP!");
            gh.isCameraState = true;

            gh.isFirstCollide = false;
            gh.CurrentHex = null;
        }
    }

    public void Camera_Movement(GameHendler gh)
    {
        Vector3 pos = gh._worldMousePosition - gh.touchStart;
        Camera.main.transform.position = (Camera.main.transform.position - pos);
    }
}
