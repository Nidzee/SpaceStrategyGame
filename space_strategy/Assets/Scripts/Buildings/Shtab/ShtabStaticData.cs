using UnityEngine;

public class ShtabStaticData
{
    public static BaseMenu baseMenuReference; // Reference to UI panel (same field for all Garages)
    public static GameObject basePrefab;      // Static prefab for creating base

    // Static info about building - determins all info about every object of this building class
    public static void InitStaticFields()
    {
        basePrefab = PrefabManager.Instance.basePrefab;
    }
}
