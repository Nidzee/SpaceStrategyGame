using UnityEngine;

public class BM_ZoomState : ITouchState
{
    private float zoomOutMin = 5f;
    private float zoomOutMax = 10f;

    public ITouchState DoState()
    {
        DomyState();

        if (!GameHendler.Instance.isZooming)
            return GameHendler.Instance.BM_idleState;

        else
            return GameHendler.Instance.BM_zoomState;
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
            StateReset();

    }
    
    public void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    private void StateReset()
    {
        GameHendler.Instance.isZooming = false;
        GameHendler.Instance.isFirstCollide = false;
    }
}
