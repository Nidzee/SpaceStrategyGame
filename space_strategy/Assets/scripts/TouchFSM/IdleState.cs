using UnityEngine;

public class IdleState : ITouchState
{
    private RaycastHit2D hit; // for building or Hex selection
    private bool isBuildingSelected = false;
    private bool isZooming = false;

    public ITouchState DoState()
    {
        DomyState();

        if (isZooming) // Zooming state
        {
            StateReset();
            GameHendler.Instance.ResetCurrentHexAndSelectedHex(); // no need for storing this data
            return GameHendler.Instance.zoomState;
        }

        else if (isBuildingSelected) // if we hit Building
        {
            StateReset();
            GameHendler.Instance.ResetCurrentHexAndSelectedHex(); // no need for storing this data
            return GameHendler.Instance.buildingSelectionState;
        }

        else if ((!isBuildingSelected) && GameHendler.Instance.CurrentHex) // if we dont hit Building but we hit HEX
        {
            if (GameHendler.Instance.CurrentHex.GetComponent<Hex>().tile_Type == Tile_Type.FreeTile || GameHendler.Instance.CurrentHex.GetComponent<Hex>().tile_Type == Tile_Type.RS1_crystal || GameHendler.Instance.CurrentHex.GetComponent<Hex>().tile_Type == Tile_Type.RS2_iron || GameHendler.Instance.CurrentHex.GetComponent<Hex>().tile_Type == Tile_Type.RS3_gel)
            {
                StateReset();
                return GameHendler.Instance.selectTileState;
            }

            else
            {
                StateReset();
                GameHendler.Instance.ResetCurrentHexAndSelectedHex(); // no need for storing this data
                return GameHendler.Instance.cameraMovementState;
            }
        }
        
        else return GameHendler.Instance.idleState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2)
        {
            isZooming = true;
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) // Determine next state / loop until state change
        {
            GameHendler.Instance.ResetCurrentHexAndSelectedHex(); // because if it was selcted Hex - after another touch we want to select another Hex

            GameHendler.Instance.touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Cashing mouse and camera position
            GameHendler.Instance.setCurrentHex(); // Find HEX under mouse/touch
            BuildingSelection(); // If we press on Building
        }
    }
    
    public void BuildingSelection()
    {
        hit = Physics2D.Raycast(GameHendler.Instance.redPoint.transform.position, Vector3.forward, 10f, GameHendler.Instance.idelLayerMask);

        Debug.Log(hit.collider.name);

        if (hit.collider != null && hit.collider.tag == TagConstants.buildingTag)
        {
            GameHendler.Instance.selctedBuilding = hit.collider.gameObject; // cashing collided building
            Debug.Log("Collided Building  -   " + hit.collider.name);
            isBuildingSelected = true;
        }
    }

    private void StateReset()
    {
        isBuildingSelected = false;
        isZooming = false;
    }
}
