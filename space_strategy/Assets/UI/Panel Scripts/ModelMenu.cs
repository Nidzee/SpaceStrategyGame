using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ModelMenu : MonoBehaviour
{
    [SerializeField] private Button placeButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button cancelButton;

    private bool helper = false;


    void Update() // REDO
    {
        if (!GameHendler.Instance.buildingModel.isModelPlacable && !helper)
        {
            helper = true;
            placeButton.interactable = false;
        }
        else if (GameHendler.Instance.buildingModel.isModelPlacable && helper)
        {
            helper = false;
            placeButton.interactable = true;
        }
    }

    public void RotateModel()
    {
        Debug.Log("Rotate");
        GameHendler.Instance.buildingModel.RotateModel();
    }

    public void PlaceBuilding()
    {
        GameHendler.Instance.buildingModel.CreateBuildingFromModel();
        GameHendler.Instance.ResetCurrentHexAndSelectedHex();

        GameHendler.Instance.currentState = GameHendler.Instance.idleState;

        Debug.Log("Create Building");
        UIPannelManager.Instance.ResetPanels("GameView");
    }

    public void Cancel()
    {
        GameHendler.Instance.currentState = GameHendler.Instance.idleState;
        GameHendler.Instance.buildingModel.ResetModel();

        Debug.Log("Cancel");
        UIPannelManager.Instance.ResetPanels("GameView");
    }
}
