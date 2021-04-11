using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class IdleState : ITouchState
{
    private RaycastHit2D hit; // for building or Hex selection
    private bool isBuildingSelected = false;
    private bool isZooming = false;


    private bool IsPointerOverUIObject() 
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


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
            GameHendler.Instance.touchStart = GameHendler.Instance.cam.ScreenToWorldPoint(Input.mousePosition);

            // GameHendler.Instance.ResetCurrentHexAndSelectedHex(); // because if it was selcted Hex - after another touch we want to select another Hex

            GameHendler.Instance.worldMousePosition = GameHendler.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
            GameHendler.Instance.redPoint.transform.position = new Vector3(GameHendler.Instance.worldMousePosition.x, GameHendler.Instance.worldMousePosition.y, GameHendler.Instance.worldMousePosition.z + 90);
            
            if (IsPointerOverUIObject())
            {
                return;
            }

            GameHendler.Instance.ResetCurrentHexAndSelectedHex(); // because if it was selcted Hex - after another touch we want to select another Hex
            
            GameHendler.Instance.setCurrentHex(); // Find HEX under mouse/touch
            BuildingSelection(); // If we press on Building
        }
    }
    
    public void BuildingSelection()
    {
        hit = Physics2D.Raycast(GameHendler.Instance.redPoint.transform.position, Vector3.forward, 10f, GameHendler.Instance.idelLayerMask);

        if (hit.collider != null)
        {
            // Debug.Log(hit.collider.name);
        
            if (hit.collider.tag == TagConstants.buildingTag)
            {
                GameHendler.Instance.selctedBuilding = hit.collider.gameObject; // cashing collided building
                // Debug.Log("Collided Building  -   " + hit.collider.name);
                isBuildingSelected = true;
            }
        }
    }

    private void StateReset()
    {
        isBuildingSelected = false;
        isZooming = false;
    }
}
