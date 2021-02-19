using UnityEngine;
using UnityEngine.EventSystems;

public class BM_IdleState : ITouchState
{
    private RaycastHit2D hit;
    private bool isZooming = false;
    private bool isBuildingSelected = false;

    public ITouchState DoState()
    {
        DomyState();

        if (isZooming) 
        {
            StateReset();
            return GameHendler.Instance.BM_zoomState;
        }

        else if (isBuildingSelected) 
        {
            StateReset();
            return GameHendler.Instance.BM_buildingMovementState;
        }

        else if ((!isBuildingSelected) && GameHendler.Instance.CurrentHex)
        {
            StateReset();
            return GameHendler.Instance.BM_cameraMovementState;
        }
        
        else 
            return GameHendler.Instance.BM_idleState;
    }

    private void DomyState()
    {
        if (Input.touchCount == 2) // Detects second Touch - ZoomState
        {
            isZooming = true;
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) // Determine next state / loop until state change
        {
            GameHendler.Instance.touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameHendler.Instance.ResetCurrentHexAndSelectedHex(); // because if it was selcted Hex - after another touch we want to select another Hex

            GameHendler.Instance.worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameHendler.Instance.redPoint.transform.position = new Vector3(GameHendler.Instance.worldMousePosition.x, GameHendler.Instance.worldMousePosition.y, GameHendler.Instance.worldMousePosition.z + 90);
            
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            GameHendler.Instance.touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Cashing mouse and camera position
            GameHendler.Instance.setCurrentHex(); // Find HEX under mouse/touch
            ModelSelection(); // If we press on Building
        }
    }
    
    private void ModelSelection()
    {
        hit = Physics2D.Raycast(GameHendler.Instance.redPoint.transform.position, Vector3.forward, 10f, GameHendler.Instance.BMidelLayerMask);
        
        if (hit.collider != null)
        {
            // Debug.Log(hit.collider.name);
                
            // if we hit sth that is model
            if (hit.collider.tag == TagConstants.modelTag)
            {
                isBuildingSelected = true;

                // Here we found 2 references. 
                // 1 - CurrentHex - it is a reference to hex on map (found by algorithm (GameObject.Finf()))
                // 2 - BTileX - it is a reference to hex on map but with mathematic founding methodic

                if (GameHendler.Instance.CurrentHex == GameHendler.Instance.buildingModel.BTileZero)
                    GameHendler.Instance.buildingModel.BSelectedTileIndex = 0;

                else if (GameHendler.Instance.CurrentHex == GameHendler.Instance.buildingModel.BTileOne)
                    GameHendler.Instance.buildingModel.BSelectedTileIndex = 1;

                else if (GameHendler.Instance.CurrentHex == GameHendler.Instance.buildingModel.BTileTwo)
                    GameHendler.Instance.buildingModel.BSelectedTileIndex = 2;

                else
                    Debug.Log("Error!");
            }
        }
    }

    private void StateReset()
    {
        GameHendler.Instance.ResetCurrentHexAndSelectedHex();
        isBuildingSelected = false;
        isZooming = false;
    }
}

