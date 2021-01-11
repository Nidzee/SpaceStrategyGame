using UnityEngine;

public class BM_BuildingMovementState : ITouchState
{
    public ITouchState DoState()
    {
        DomyState();


        // Transitions
        if (GameHendler.Instance.isZooming)
        {
            GameHendler.Instance.resetInfo();
            return GameHendler.Instance.BM_zoomState;
        }

        else if (!GameHendler.Instance.isBuildingSelected)
            return GameHendler.Instance.BM_idleState;

        else
            return GameHendler.Instance.BM_buildingMovementState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2)
        {
            GameHendler.Instance.isZooming = true;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            GameHendler.Instance.buildingModel.MoveModel();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StateReset();
        }
    }

    private void StateReset() // Reseting all used variables
    {
        GameHendler.Instance.buildingModel.BSelectedTileIndex = 0; // For understanding, which Tile is Selected(BuildingTile)

        GameHendler.Instance.isBuildingSelected = false;
        GameHendler.Instance.isFirstCollide = false;
        GameHendler.Instance.CurrentHex = null;
    }

}