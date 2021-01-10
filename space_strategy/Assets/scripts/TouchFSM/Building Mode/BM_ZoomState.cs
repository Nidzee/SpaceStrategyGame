using UnityEngine;

public class BM_ZoomState : ITouchState
{
    public float zoomOutMin = 5f;
    public float zoomOutMax = 10f;

    public ITouchState DoState(GameHendler gh)
    {
        DomyState(gh);

        if (!gh.isZooming)
            return gh.BM_idleState;

        else
            return gh.BM_zoomState;
    }
    private void DomyState(GameHendler gh)
    {
        if(Input.touchCount == 2) // Zooming logic - BlackMagic
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float pervMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - pervMagnitude;

            zoom(difference * 0.01f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            gh.isZooming = false;
            gh.isFirstCollide = false;
        }
    }
    
    public void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}
