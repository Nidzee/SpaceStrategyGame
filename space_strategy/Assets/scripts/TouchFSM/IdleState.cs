using UnityEngine;

public class IdleState : ITouchState
{
    private RaycastHit2D hit;

    public ITouchState DoState()
    {
        DomyState();

        if (GameHendler.Instance.isZooming) // Zooming state
        {
            GameHendler.Instance.resetInfo();
            return GameHendler.Instance.zoomState;
        }

        // TODO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // else if (gameHendler.isBuildingSelected) // if we hit Building
        // {
        //     return gameHendler.buildingSelectionState;
        // }

        else if ((!GameHendler.Instance.isBuildingSelected) && GameHendler.Instance.CurrentHex) // if we dont hit Building but we hit HEX
        {
            if (GameHendler.Instance.CurrentHex.GetComponent<Hex>().tile_Type == Tile_Type.FreeTile)
            {
                GameHendler.Instance.isTileselectState = true;
                return GameHendler.Instance.selectTileState;
            }

            else
            {
                GameHendler.Instance.isCameraState = true;
                return GameHendler.Instance.cameraMovementState;
            }
        }
        
        else return GameHendler.Instance.idleState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2) // Detects second Touch - ZoomState
        {
            GameHendler.Instance.isZooming = true;
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) // Determine next state / loop until state change
        {
            // Cashing mouse and camera position
            GameHendler.Instance.touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameHendler.Instance.resetInfo();
            GameHendler.Instance.selectTileState.setCurrentHex(); // Find HEX under mouse/touch

            //BuildingSelection(gameHendler); // If we press on Building

            GameHendler.Instance.isFirstCollide = true;
        }
    }
    
    // public void BuildingSelection()
    // {
    //     hit = Physics2D.Raycast(GameHendler.Instance.redPoint.transform.position, Vector3.forward, 10f);
            
    //     if (hit.collider != null && hit.collider.name == "Building(Clone)" && !GameHendler.Instance.isFirstCollide)
    //     {
    //         //Debug.Log("Collided   -   " + hit.collider.name);
    //         GameHendler.Instance.isBuildingSelected = true;
    //         //gameHendler.buildingColor = gameHendler.BuildingSprite.GetComponent<SpriteRenderer>().color; // TEMP
    //     }
    // }
}
