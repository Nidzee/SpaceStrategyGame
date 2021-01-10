using UnityEngine;

public class CameraMovementState : ITouchState
{
    public ITouchState DoState(GameHendler gh)
    {
        DomyState(gh);

        if (gh.isZooming) // No nned for ResetState func cuz before Zoom we reset everything
            return gh.zoomState;

        else if(gh.isCameraState) // We can get into this case only with gbUP
        {
            //gh.isCameraState = false; // idk if it is necessary here
            return gh.idleState;
        }
        else
            return gh.cameraMovementState;
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
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gh.touchStart;
        //Debug.Log(pos);
        Camera.main.transform.position -= pos;
    }
}
