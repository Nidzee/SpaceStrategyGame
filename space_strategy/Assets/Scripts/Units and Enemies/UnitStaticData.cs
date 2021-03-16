using UnityEngine;

public class UnitStaticData
{
    public static int unit_counter = 0; // Unit number in ibspector
    public static GameObject unitPrefab; // Unit prefab
    public static float moveSpeed = 3f;  // Const speed of all units

    public static void InitStaticFields()
    {
        unitPrefab = PrefabManager.Instance.unitPrefab;
    }
}
