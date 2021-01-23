using UnityEngine;

public class BM_BuildingMovementState : ITouchState
{
    private bool isBuildingSelected = true;
    private bool isZooming = false;

    public ITouchState DoState()
    {
        DomyState();

        if (isZooming)
        {
            StateReset();
            return GameHendler.Instance.BM_zoomState;
        }

        else if (!isBuildingSelected)
        {
            StateReset();
            return GameHendler.Instance.BM_idleState;
        }

        else
            return GameHendler.Instance.BM_buildingMovementState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2)
        {
            isZooming = true;
            return;
        }

        if (Input.GetMouseButton(0))
            GameHendler.Instance.buildingModel.MoveModel();

        if (Input.GetMouseButtonUp(0))
            isBuildingSelected = false;
    }

    private void StateReset()
    {
        isZooming = false;
        isBuildingSelected = true;
        GameHendler.Instance.ResetCurrentHexAndSelectedHex();
        //GameHendler.Instance.CurrentHex = null;
        GameHendler.Instance.buildingModel.BSelectedTileIndex = 0; // For understanding, which Tile is Selected(BuildingTile)
    }

}