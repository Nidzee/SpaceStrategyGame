using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class EnemyBomber : Enemy
{
    public EnemyBomberSavingData savingData  = null;
    public bool isReachedTarget              = false;
    public bool isBashIntersects             = false;
    public int attackPoints                  = 20;
    public Vector3 destination;
    public GameObject destinationBuilding    = null;
    public Seeker seeker                     = null;
    public Path path                         = null;
    public int currentWaypoint               = 0;
    public List<GameObject> buildingsInRange = null;
    public int currentStateID                = 0;
    private float hexRadius                  = 1.3f;

    BuildingMapInfo currentBuilding;
    BuildingMapInfo targetBuilding;
    int indexOfBuildingTile;
    int currentBuildingFromRange;
    int buildingsInRangeCount;
    List<ParticularPathInfo> rebuildingCurrentPathPathes    = new List<ParticularPathInfo>();
    List<ParticularPathInfo> comparingPathes                = new List<ParticularPathInfo>();
    List<ParticularPathInfo> pathesToBase                   = new List<ParticularPathInfo>();
    
    public BomberIdleState idleState      = new BomberIdleState();
    public BomberMovingState movingState  = new BomberMovingState();
    public BomberBashState bashState      = new BomberBashState();
    public BomberAttackState attackState  = new BomberAttackState();
    public IBomberState currentState      = null;
    

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints + bashAdditionalDamage);

        if (healthPoints <= 0)
        {
            DestroyEnemy();
            return;
        }

        bars.SetActive(true);

        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        StopCoroutine("UICanvasmaintaining");
        uiCanvasDissapearingTimer = 0f;
        StartCoroutine("UICanvasmaintaining");
    }

    public void SaveData()
    {
        savingData = new EnemyBomberSavingData();

        savingData.name = name;

        savingData.healthPoints = healthPoints;
        savingData.shieldPoints = shieldPoints;
        savingData.maxCurrentHealthPoints = maxCurrentHealthPoints;  // For correct percentage recalculation
        savingData.maxCurrentShieldPoints = maxCurrentShieldPoints;  // For correct percentage recalculation
        savingData.deffencePoints = deffencePoints;
        savingData.isShieldOn = isShieldOn;
        savingData.shieldGeneratorInfluencers = shieldGeneratorInfluencers;

        if (currentState == idleState)
        {
            savingData.currentStateID = 1;
        }
        else if (currentState == movingState)
        {
            savingData.currentStateID = 2;
        }
        else if (currentState == bashState)
        {
            savingData.currentStateID = 3;
        }
        else if (currentState == attackState)
        {
            savingData.currentStateID = 4;
        }

        savingData.pos_x = transform.position.x;
        savingData.pos_y = transform.position.y;
        savingData.pos_z = transform.position.z;

        GameHendler.Instance.bombersSaved.Add(savingData);

        // Destroy(gameObject);
    }

    private void Update()
    {
        if (currentState!=null)
        currentState = currentState.DoState(this);
    }

    public void Attack()
    {
        GameObject damageRadius = Instantiate(PrefabManager.Instance.enemyAttackRange, transform.position, transform.rotation);
        damageRadius.GetComponent<EnemyAttackRange>().damagePoints = attackPoints;

        DestroyEnemy();
    }


    #region Creation and destruction logic
    public void Creation()
    {        
        // Data initialization
        CreateGameUnit(40, 40, 5);
        name = "Bomber" + BomberStaticData.bomber_counter;
        BomberStaticData.bomber_counter++;
        seeker = GetComponent<Seeker>();
        rigidBodyRef = GetComponent<Rigidbody2D>();
        currentState = idleState;
        attackPoints = 20;


        // UI
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(true);
        powerOffIndicator.SetActive(false);
        bars.SetActive(false);


        // Resource manager lists maintaining
        ResourceManager.Instance.enemiesBombers.Add(this);
    }

    public void CreateFromFile(EnemyBomberSavingData savingData)
    {
        // Data initialization
        InitGameUnitFromFile(
        savingData.healthPoints, 
        savingData.maxCurrentHealthPoints,
        savingData.shieldPoints,
        savingData.maxCurrentShieldPoints,
        savingData.deffencePoints,
        savingData.isShieldOn,
        savingData.shieldGeneratorInfluencers);
        name = savingData.name;
        seeker = GetComponent<Seeker>();
        rigidBodyRef = GetComponent<Rigidbody2D>();
        currentStateID = savingData.currentStateID;
        attackPoints = 20;


        // UI
        canvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;
        canvas.SetActive(true);
        powerOffIndicator.SetActive(false);
        bars.SetActive(false);


        // Resource manager lists maintaining
        ResourceManager.Instance.enemiesBombers.Add(this);
    }

    public override void DestroyEnemy() // Reload here because dead unit maybe was working at shaft
    {
        ResourceManager.Instance.enemiesBombers.Remove(this);

        ResourceManager.Instance.CheckForEndOfWave();

        base.DestroyEnemy();
    }
    #endregion

    #region A* path manipulating
    
    private void OnPathBuilded(Path newPath)/////////////////////////////////////////////////////////////////////////
    {
        if (!newPath.error)
        {
            path = newPath;
            currentWaypoint = 0;
        }
    }

    private void GetBestRoot(List<ParticularPathInfo> currentListOfPathes)
    {
        List<ParticularPathInfo> temp = new List<ParticularPathInfo>();
        ParticularPathInfo bestRootPath = new ParticularPathInfo();

        foreach (var item in currentListOfPathes)
        {
            if (item.isAccesible)
            {
                temp.Add(item);
            }
        }

        // Means all pathes are inaccessible
        if (temp.Count == 0)
        {
            temp = currentListOfPathes;
        }


        int bestRootNodes = -1;
        foreach(var item in temp)
        {
            if (bestRootNodes == -1)
            {
                bestRootNodes = item.currentBuildingNodes;
                bestRootPath = item;
            }
            else
            {
                if (item.currentBuildingNodes < bestRootNodes)
                {
                    bestRootPath = item;
                }
            }
        }

        temp.Clear();
        

        Debug.Log("Best path to: " + bestRootPath.currentBuilding + "   is to cell: " + bestRootPath.currentBuildingCell + "   number of waypoints: " + bestRootPath.currentBuildingNodes +"   and it is: " + bestRootPath.isAccesible);

        GetComponent<AIDestinationSetter>().target = bestRootPath.currentBuilding.mapPoints[bestRootPath.currentBuildingCell];
        destination = bestRootPath.currentBuilding.mapPoints[bestRootPath.currentBuildingCell].position;
        seeker.StartPath(transform.position, bestRootPath.currentBuilding.mapPoints[bestRootPath.currentBuildingCell].position, OnPathBuilded);

        destinationBuilding = bestRootPath.currentBuilding.gameObject;
    }

    public void CreateStartPath()
    {
        if (buildingsInRange.Count != 0)
        {
            RebuildCurrentPath();
            return;
        }

        indexOfBuildingTile = 0;
        currentBuilding = ResourceManager.Instance.shtabReference.GetComponent<BuildingMapInfo>();
        seeker.StartPath(transform.position, currentBuilding.mapPoints[indexOfBuildingTile].position, OnInitializingPathComplete);
    }

    public void RebuildCurrentPath()
    {
        rebuildingCurrentPathPathes.Clear();
        path = null;
        buildingsInRangeCount = buildingsInRange.Count;
        currentBuildingFromRange = 0;


        indexOfBuildingTile = 0;
        currentBuilding = ResourceManager.Instance.shtabReference.GetComponent<BuildingMapInfo>();
        seeker.StartPath(transform.position, currentBuilding.mapPoints[indexOfBuildingTile].position, OnRebuildPathBuilded);
    }

    public void ComparePathesToCurrentBuildingAndToTargetBuilding(GameObject building)
    {
        comparingPathes.Clear();
        path = null;
        targetBuilding = building.GetComponent<BuildingMapInfo>();



        indexOfBuildingTile = 0;
        if (destinationBuilding == null)
        {
            destinationBuilding = ResourceManager.Instance.shtabReference.gameObject;
        }
        currentBuilding = destinationBuilding.GetComponent<BuildingMapInfo>();
        seeker.StartPath(transform.position, currentBuilding.mapPoints[indexOfBuildingTile].position, OnComparedPathComplete);
    }

    private void OnInitializingPathComplete(Path path)
    {
        if (!path.error)
        {
            ParticularPathInfo thisPath = new ParticularPathInfo();

            float distance = Vector2.Distance(currentBuilding.mapPoints[indexOfBuildingTile].position, path.vectorPath[path.vectorPath.Count-1]);

            if (distance <= hexRadius)
            {
                thisPath.isAccesible = true;
            }
            else
            {
                thisPath.isAccesible = false;
            }

            thisPath.currentBuilding = currentBuilding;
            thisPath.currentBuildingCell = indexOfBuildingTile;
            thisPath.currentBuildingNodes = path.vectorPath.Count;

            Debug.Log("Path to: " + thisPath.currentBuilding + "    to cell: " + thisPath.currentBuildingCell + "   is: " + thisPath.currentBuildingNodes +"   and it is: " + thisPath.isAccesible);

            pathesToBase.Add(thisPath);
            
            




            if (currentBuilding.mapPoints.Length-1 != indexOfBuildingTile)
            {
                indexOfBuildingTile++;
                seeker.StartPath(transform.position, currentBuilding.mapPoints[indexOfBuildingTile].position, OnInitializingPathComplete);
            }
            else
            {
                GetBestRoot(pathesToBase);
            }
        }
    }

    private void OnRebuildPathBuilded(Path path)
    {        
        if (!path.error)
        {
            ParticularPathInfo thisPath = new ParticularPathInfo();

            float distance = Vector2.Distance(currentBuilding.mapPoints[indexOfBuildingTile].position, path.vectorPath[path.vectorPath.Count-1]);
            
            if (distance <= hexRadius)
            {
                thisPath.isAccesible = true;
            }
            else
            {
                thisPath.isAccesible = false;
            }

            thisPath.currentBuilding = currentBuilding;
            thisPath.currentBuildingCell = indexOfBuildingTile;
            thisPath.currentBuildingNodes = path.vectorPath.Count;

            Debug.Log("Path to: " + thisPath.currentBuilding + "    to cell: " + thisPath.currentBuildingCell + "   is: " + thisPath.currentBuildingNodes +"   and it is: " + thisPath.isAccesible);

            rebuildingCurrentPathPathes.Add(thisPath);







            if (currentBuilding.mapPoints.Length-1 != indexOfBuildingTile)
            {
                indexOfBuildingTile++;
                seeker.StartPath(transform.position, currentBuilding.mapPoints[indexOfBuildingTile].position, OnRebuildPathBuilded);
            }
            else
            {
                if (currentBuildingFromRange < buildingsInRangeCount)
                {
                    // Debug.Log(currentBuilding.name);

                    currentBuilding = buildingsInRange[currentBuildingFromRange].GetComponent<BuildingMapInfo>();
                    currentBuildingFromRange++;
                    indexOfBuildingTile = 0;
                    seeker.StartPath(transform.position, currentBuilding.mapPoints[indexOfBuildingTile].position, OnRebuildPathBuilded);
                }
                else
                {
                    GetBestRoot(rebuildingCurrentPathPathes);
                }
            }
        }
    }

    private void OnComparedPathComplete(Path path)
    {
        if (!path.error)
        {
            ParticularPathInfo thisPath = new ParticularPathInfo();

            float distance = Vector2.Distance(currentBuilding.mapPoints[indexOfBuildingTile].position, path.vectorPath[path.vectorPath.Count-1]);
            
            if (distance <= hexRadius) 
            {
                thisPath.isAccesible = true;
            }
            else 
            {
                thisPath.isAccesible = false;
            }

            thisPath.currentBuilding = currentBuilding;
            thisPath.currentBuildingCell = indexOfBuildingTile;
            thisPath.currentBuildingNodes = path.vectorPath.Count;

            Debug.Log("Path to: " + thisPath.currentBuilding + "    to cell: " + thisPath.currentBuildingCell + "   is: " + thisPath.currentBuildingNodes +"   and it is: " + thisPath.isAccesible);

            comparingPathes.Add(thisPath);







            if (currentBuilding.mapPoints.Length-1 != indexOfBuildingTile)
            {
                indexOfBuildingTile++;
                seeker.StartPath(transform.position, currentBuilding.mapPoints[indexOfBuildingTile].position, OnComparedPathComplete);
            }
            else
            {
                if (currentBuilding != targetBuilding)
                {
                    currentBuilding = targetBuilding;
                    indexOfBuildingTile = 0;
                    seeker.StartPath(transform.position, currentBuilding.mapPoints[indexOfBuildingTile].position, OnComparedPathComplete);
                }
                else
                {
                    GetBestRoot(comparingPathes);
                }
            }
        }
    }

    // public void ChangeDestination(int destinationID)
    // {
    //     GetComponent<AIDestinationSetter>().target = ResourceManager.Instance.shtabReference.GetUnitDestination();
    //     destination = ResourceManager.Instance.shtabReference.GetUnitDestination().position;
    // }
    #endregion


    void OnTriggerEnter2D(Collider2D collider) // or Destination or Model
    {
        if (collider.gameObject.tag == TagConstants.buildingTag && collider.gameObject == destinationBuilding)
        {
            // We reached destination
            isReachedTarget = true;
            Debug.Log("Arrived at destination!");
        }

        // Sets model unplacable
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
            GameHendler.Instance.buildingModel.isModelPlacable = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider) // For model correct placing
    {
        if (collider.gameObject.tag == TagConstants.modelTag)
        {
            GameHendler.Instance.buildingModel.isModelPlacable = true;
            GameHendler.Instance.buildingModel.ChechForCorrectPlacement();
        }
    }
}

struct ParticularPathInfo
{
    public bool isAccesible;
    public BuildingMapInfo currentBuilding;
    public int currentBuildingCell;
    public int currentBuildingNodes;
};