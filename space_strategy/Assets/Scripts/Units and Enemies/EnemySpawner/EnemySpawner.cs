using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance {get; private set;}

    [SerializeField] private Image enemySpawnerTimerImage; // Init in inspector

    private GameObject[] enemyTMapTiles           = null;
    private List<GameObject> pointsToSpawnEnemies = new List<GameObject>();
    public float _enemyTimer                      = 0f;
    public float _enemyTimerStep                  = 0.5f;


    private void Awake()
    {
        Debug.Log("ENEMY SPAWNER MANAGER START WORKING");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
        StartCoroutine("EnemyTimerMaintaining");
    }








    public IEnumerator EnemyTimerMaintaining()
    {        
        while (_enemyTimer < 1)
        {
            _enemyTimer += _enemyTimerStep * Time.deltaTime;
            enemySpawnerTimerImage.fillAmount = _enemyTimer;

            yield return null;
        }

        _enemyTimer = 0f;


        SpawnEnemies();
    }

    public void StopEnemySpawnTimer()
    {
        StopCoroutine("EnemyTimerMaintaining");
    }

    public void StartEnemySpawnTimer()
    {
        StartCoroutine("EnemyTimerMaintaining");
    }

    
    public void LoadData(EnemySpawnerSavingData savingData)
    {
        StopCoroutine("EnemyTimerMaintaining");///////////////////////////////////////////////////////////////

        _enemyTimer = savingData._enemyTimer;

        if (_enemyTimer != 0)
        {
            StartCoroutine("EnemyTimerMaintaining");
        }
    }
}