using UnityEngine;

public class ZoomState : ITouchState
{
    private float zoomOutMin = 5f;
    private float zoomOutMax = 15f;
    private bool isZooming = true;

    public ITouchState DoState()
    {
        DomyState();

        if (!isZooming)
        {
            StateReset();
            return GameHendler.Instance.idleState;
        }

        else
            return GameHendler.Instance.zoomState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2) // Zooming logic - BlackMagic
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float pervMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - pervMagnitude;
            Zoom(difference * 0.01f);
        }

        if (Input.GetMouseButtonUp(0))
            isZooming = false;
    }
    
    private void Zoom(float increment)
    {
        GameHendler.Instance.cam.orthographicSize = Mathf.Clamp(GameHendler.Instance.cam.orthographicSize - increment, zoomOutMin, zoomOutMax);
        
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
        isZooming = true;
    }
}
