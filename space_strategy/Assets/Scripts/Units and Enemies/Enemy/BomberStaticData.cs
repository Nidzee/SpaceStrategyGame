using UnityEngine;

public class BomberStaticData : MonoBehaviour
{
    public static int bomber_counter = 0; // Unit number in ibspector
    public static GameObject bomberPrefab; // Unit prefab
    public static float moveSpeed = 3f;  // Const speed of all units

    public static void InitStaticFields()
    {
        bomberPrefab = PrefabManager.Instance.bomberPrefab;
    }
}
