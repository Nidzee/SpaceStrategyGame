using UnityEngine;

public class IdleState : ITouchState
{
    public RaycastHit2D hit;

    public ITouchState DoState(GameHendler gh)
    {
        DomyState(gh);

        if (gh.isZooming) // Zooming state
        {
            return gh.zoomState;
        }

        else if (gh.isBuildingSelected) // if we hit Building
        {
            return gh.buildingSelectionState;
        }

        else if((!gh.isBuildingSelected) && gh.CurrentHex) // if we dont hit Building but we hit HEX
        {
            if (gh.CurrentHex.GetComponent<Hex>().tile_Type == Tile_Type.FreeTile)
            {
                return gh.selectTileState;
            }
            else
            {
                return gh.cameraMovementState;
            }
        }
        
        else return gh.idleState; // Loop IdleState
    }

    private void DomyState(GameHendler gh)
    {
        if (Input.touchCount == 2) // Detects second Touch - ZoomState
        {
            gh.isZooming = true;
            gh.resetInfo();
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) // Determine next state / loop until state change
        {
            // Cashing mouse and camera position
            gh.touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            gh.resetInfo();

            gh.selectTileState.setCurrentHex(gh); // Find HEX under mouse/touch

            //BuildingSelection(gh); // If we press on Building

            gh.isFirstCollide = true;
        }
    }
    
    public void BuildingSelection(GameHendler gh)
    {
        hit = Physics2D.Raycast(gh.redPoint.transform.position, Vector3.forward, 10f);
            
        if (hit.collider != null && hit.collider.name == "Building(Clone)" && !gh.isFirstCollide)
        {
            //Debug.Log("Collided   -   " + hit.collider.name);
            gh.isBuildingSelected = true;
            //gh.buildingColor = gh.BuildingSprite.GetComponent<SpriteRenderer>().color; // TEMP
        }
    }
}
