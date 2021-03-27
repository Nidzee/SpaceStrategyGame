using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    


    void Update()
    {
        
    }

    private void CreateBomber(Transform placePosition)
    {
        GameObject go = Instantiate(BomberStaticData.bomberPrefab, 
                        placePosition.position + OffsetConstants.buildingOffset, 
                        Quaternion.Euler(0f, 0f, 0f));
                
        go.GetComponent<EnemyBomber>().Creation();
    }
}
