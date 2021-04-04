using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance {get; private set;}

    public GameObject[] enemyTMapTiles = null;
    public List<GameObject> pointsToSpawnEnemies;
    
    public Image timeToSpawnEnemiesImage;

    private float _enemyTimerStep = 0.05f;
    private float _enemyTimer = 0f;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        StartCoroutine(EnemyTimerMaintaining());
    }

    bool isEnemyTilesInitialized = false;

    public void SpawnEnemies()
    {
        if (!isEnemyTilesInitialized)
        {
            isEnemyTilesInitialized = true;
            
            enemyTMapTiles = GameObject.FindGameObjectsWithTag("EnemyTiles");
            
            foreach(var item in enemyTMapTiles)
            {
                pointsToSpawnEnemies.Add(item.transform.GetChild(0).gameObject);
                pointsToSpawnEnemies.Add(item.transform.GetChild(1).gameObject);
                pointsToSpawnEnemies.Add(item.transform.GetChild(2).gameObject);
            }
        }

        foreach(var spawnPoint in pointsToSpawnEnemies)
        {
            GameObject enemy = Instantiate
            (BomberStaticData.bomberPrefab,
            spawnPoint.transform.position + OffsetConstants.buildingOffset,
            Quaternion.identity);
                
            enemy.GetComponent<EnemyBomber>().Creation();
        }
    }

    public void RestartEnemySpawnTimer()
    {
        StartCoroutine(EnemyTimerMaintaining());
    }

    IEnumerator EnemyTimerMaintaining()
    {
        while (_enemyTimer < 1)
        {
            _enemyTimer += _enemyTimerStep * Time.deltaTime;
            timeToSpawnEnemiesImage.fillAmount = _enemyTimer;

            yield return null;
        }

        _enemyTimer = 0f;

        SpawnEnemies();
    }
}