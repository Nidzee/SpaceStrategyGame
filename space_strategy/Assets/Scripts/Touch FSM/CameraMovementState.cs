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
        Vector3 pos = GameHendler.Instance.cam.ScreenToWorldPoint(Input.mousePosition) - GameHendler.Instance.touchStart;
        GameHendler.Instance.cam.transform.position -= pos;


        float camHeight = GameHendler.Instance.cam.orthographicSize;
        float camWidth  = GameHendler.Instance.cam.orthographicSize * GameHendler.Instance.cam.aspect;

        float minX = MapGenerator.Instance.leftLimit + camWidth;
        float maxX = MapGenerator.Instance.rightLimit - camWidth;

        float minY = MapGenerator.Instance.botLimit + camHeight;
        float maxY = MapGenerator.Instance.topLimit - camHeight;

        float newX = Mathf.Clamp(GameHendler.Instance.cam.transform.position.x , minX, maxX);
        float newY = Mathf.Clamp(GameHendler.Instance.cam.transform.position.y , minY, maxY);

        GameHendler.Instance.cam.transform.position = new Vector3(newX, newY, GameHendler.Instance.cam.transform.position.z);
    }

    private void StateReset()
    {
        isCameraMoving = true;
        isZooming = false;
    }
}
