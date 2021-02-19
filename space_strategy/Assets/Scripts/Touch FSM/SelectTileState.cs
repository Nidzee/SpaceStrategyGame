using UnityEngine;

public class SelectTileState : ITouchState
{
    private float touchTime = 0.2f;
    private bool isTileselectState = true;
    private bool isZooming = false;

    public ITouchState DoState()
    {
        DomyState();

        if (isZooming) // No need for ResetState cuz whole functionality is in *gh.resetInfo();*
        {
            StateReset();
            GameHendler.Instance.ResetCurrentHexAndSelectedHex();
            return GameHendler.Instance.zoomState;
        }

        else if (!isTileselectState) // we do have selected hex or dont - that is why no "ResetCurrentHexAndSelectedHex()"
        {
            StateReset();
            return GameHendler.Instance.idleState;
        }

        else if (GameHendler.Instance.touchStart != GameHendler.Instance.worldMousePosition) // We moved mouse - CameraMovementState
        {
            StateReset();
            GameHendler.Instance.ResetCurrentHexAndSelectedHex();
            return GameHendler.Instance.cameraMovementState;
        }
        
        else
            return GameHendler.Instance.selectTileState;
    }
    
    private void DomyState()
    {
        if (Input.touchCount == 2) // Detects second Touch
        {
            isZooming = true;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            touchTime -= Time.deltaTime;
            if (touchTime < 0)
                touchTime = 0f;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            if (touchTime > 0 && GameHendler.Instance.touchStart == GameHendler.Instance.worldMousePosition) // if all conditions are correct -> set Selected Tile
            {
                GameHendler.Instance.hexColor = GameHendler.Instance.CurrentHex.GetComponent<SpriteRenderer>().color; // TEMP
                GameHendler.Instance.SelectedHex = GameHendler.Instance.CurrentHex;
                GameHendler.Instance.SelectedHex.GetComponent<SpriteRenderer>().color = Color.yellow; // TEMP

                GameViewMenu.Instance.TurnBuildingsCreationButtonON();
            }
            
            isTileselectState = false;
        }
    }

    private void StateReset()
    {
        isTileselectState = true;
        touchTime = 0.2f;
        isZooming = false;

        GameHendler.Instance.CurrentHex = null; // it is using for FSM transitions and in this state we can have or not have selected hex
                                                // but current hex is using further so we need to set it to null
    }
}   