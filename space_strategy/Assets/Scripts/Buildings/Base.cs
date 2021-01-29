using UnityEngine;

public class Base : AliveGameUnit, IBuilding
{
    private static BaseMenu baseMenuReference; // Reference to UI panel (same field for all Garages)
    private static GameObject basePrefab;      // Static prefab for creating base

    public GameObject resourceRef;             // Reference to Unit resource object (for creating copy and consuming)
    public Vector3 storageConsumerPosition;    // Place for resource consuming and dissappearing
    public int level = 1;                      // Determin upgrade level of rest buildings

    public bool isMenuOpened = false;



    public override void TakeDamage(float DamagePoints)
    {
        base.TakeDamage(DamagePoints);

        if (isMenuOpened)
        {
            baseMenuReference.ReloadSlidersHP_SP();
        }
    }

    public static void InitStaticFields()
    {
        basePrefab = PrefabManager.Instance.basePrefab;
    }

    public void Creation()
    {   
        transform.tag = TagConstants.buildingTag;
        gameObject.layer = LayerMask.NameToLayer(LayerConstants.buildingLayer);
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = LayerConstants.buildingLayer;

        HelperObjectInit();
    }

    public void Upgrade()
    {
        Debug.Log("TODO!");
        level++;
    }

    public void FastUnitCreation()
    {
        Debug.Log("Fast unit creation! - TODO");
    }

    private void HelperObjectInit()
    {
        if (gameObject.transform.childCount != 0)
        {
            gameObject.transform.GetChild(0).tag = TagConstants.baseStorageTag;
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer(LayerConstants.nonInteractibleLayer);
            
            storageConsumerPosition = gameObject.transform.GetChild(0).transform.position;
        }
        else
        {
            Debug.LogError("No child object (For range) in shaft!     Cannot get dispenser coords!");
        }
    }

    public void Invoke()
    {
        UIPannelManager.Instance.ResetPanels("BaseMenu");
        
        if (!baseMenuReference) // executes once
        {
            baseMenuReference = GameObject.Find("BaseMenu").GetComponent<BaseMenu>();
        }

        baseMenuReference.ReloadPanel(this);
    }
}
