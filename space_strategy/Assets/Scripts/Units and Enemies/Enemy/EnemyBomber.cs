using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class EnemyBomber : Enemy
{
    public EnemyBomberSavingData savingData = null;

    public BomberIdleState bomberIdleState = new BomberIdleState();
    public BomberGoToState bomberGoToState = new BomberGoToState();
    public BomberBashState bomberBashState = new BomberBashState();
    public BomberAttackState bomberAttackState = new BomberAttackState();
    public IBomberState currentState = null;
    
    public bool isReachedTarget = false;
    public bool isBashIntersects = false;
    public bool isReachedEndOfPathAndDidntIntersectTarget = false;
    public int attackPoints = 20;
    public Vector3 destination;
    public GameObject destinationBuilding = null;

    public Seeker _seeker = null;
    public Path _path = null;
    public int _currentWaypoint = 0;
    public List<GameObject> buildingsInRange = null;

    public int currentStateID = 0;


    float hexRadius = 1.3f;
    BuildingMapInfo currentBuilding;
    BuildingMapInfo targetBuilding;
    int i;
    int specialIndex;
    int allspecialIndexes;
    List<particularPathInfo> rebuildingCurrentPathPathes = new List<particularPathInfo>();
    List<particularPathInfo> comparingPathes = new List<particularPathInfo>();
    List<particularPathInfo> pathesToShtab = new List<particularPathInfo>();
    

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

        if (currentState == bomberIdleState)
        {
            savingData.currentStateID = 1;
        }
        else if (currentState == bomberGoToState)
        {
            savingData.currentStateID = 2;
        }
        else if (currentState == bomberBashState)
        {
            savingData.currentStateID = 3;
        }
        else if (currentState == bomberAttackState)
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
        _seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        currentState = bomberIdleState;


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
        _seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        currentStateID = savingData.currentStateID;


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

    private void OnPathBuilded(Path path)
    {
        if (!path.error)
        {
            _path = path;
            _currentWaypoint = 0;
        }
    }

    private void GetBestRoot(List<particularPathInfo> currentListOfPathes)
    {
        List<particularPathInfo> temp = new List<particularPathInfo>();
        particularPathInfo bestRootPath = new particularPathInfo();

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
        _seeker.StartPath(transform.position, bestRootPath.currentBuilding.mapPoints[bestRootPath.currentBuildingCell].position, OnPathBuilded);

        destinationBuilding = bestRootPath.currentBuilding.gameObject;
    }

    public void CreateStartPath()
    {
        if (buildingsInRange.Count != 0)
        {
            RebuildCurrentPath();
            return;
        }

        i = 0;
        currentBuilding = ResourceManager.Instance.shtabReference.GetComponent<BuildingMapInfo>();
        _seeker.StartPath(transform.position, currentBuilding.mapPoints[i].position, OnInitializingPathComplete);
    }

    public void RebuildCurrentPath()
    {
        rebuildingCurrentPathPathes.Clear();
        _path = null;
        allspecialIndexes = buildingsInRange.Count;
        specialIndex = 0;


        i = 0;
        currentBuilding = ResourceManager.Instance.shtabReference.GetComponent<BuildingMapInfo>();
        _seeker.StartPath(transform.position, currentBuilding.mapPoints[i].position, OnPathBuildedRebuild);
    }

    public void ComparePathesToShtabAndToTargetBuilding(GameObject building)
    {
        comparingPathes.Clear();
        _path = null;
        targetBuilding = building.GetComponent<BuildingMapInfo>();



        i = 0;
        if (destinationBuilding == null)
        {
            destinationBuilding = ResourceManager.Instance.shtabReference.gameObject;
        }
        currentBuilding = destinationBuilding.GetComponent<BuildingMapInfo>();
        _seeker.StartPath(transform.position, currentBuilding.mapPoints[i].position, OnComparedPathComplete);
    }

    private void OnInitializingPathComplete(Path path)
    {
        if (!path.error)
        {
            particularPathInfo thisPath = new particularPathInfo();

            float distance = Vector2.Distance(currentBuilding.mapPoints[i].position, path.vectorPath[path.vectorPath.Count-1]);

            if (distance <= hexRadius)
            {
                thisPath.isAccesible = true;
            }
            else
            {
                thisPath.isAccesible = false;
            }

            thisPath.currentBuilding = currentBuilding;
            thisPath.currentBuildingCell = i;
            thisPath.currentBuildingNodes = path.vectorPath.Count;

            Debug.Log("Path to: " + thisPath.currentBuilding + "    to cell: " + thisPath.currentBuildingCell + "   is: " + thisPath.currentBuildingNodes +"   and it is: " + thisPath.isAccesible);

            pathesToShtab.Add(thisPath);
            
            




            if (currentBuilding.mapPoints.Length-1 != i)
            {
                i++;
                _seeker.StartPath(transform.position, currentBuilding.mapPoints[i].position, OnInitializingPathComplete);
            }
            else
            {
                GetBestRoot(pathesToShtab);
            }
        }
    }

    private void OnPathBuildedRebuild(Path path)
    {        
        if (!path.error)
        {
            particularPathInfo thisPath = new particularPathInfo();

            float distance = Vector2.Distance(currentBuilding.mapPoints[i].position, path.vectorPath[path.vectorPath.Count-1]);
            
            if (distance <= hexRadius)
            {
                thisPath.isAccesible = true;
            }
            else
            {
                thisPath.isAccesible = false;
            }

            thisPath.currentBuilding = currentBuilding;
            thisPath.currentBuildingCell = i;
            thisPath.currentBuildingNodes = path.vectorPath.Count;

            Debug.Log("Path to: " + thisPath.currentBuilding + "    to cell: " + thisPath.currentBuildingCell + "   is: " + thisPath.currentBuildingNodes +"   and it is: " + thisPath.isAccesible);

            rebuildingCurrentPathPathes.Add(thisPath);







            if (currentBuilding.mapPoints.Length-1 != i)
            {
                i++;
                _seeker.StartPath(transform.position, currentBuilding.mapPoints[i].position, OnPathBuildedRebuild);
            }
            else
            {
                if (specialIndex < allspecialIndexes)
                {
                    // Debug.Log(currentBuilding.name);

                    currentBuilding = buildingsInRange[specialIndex].GetComponent<BuildingMapInfo>();
                    specialIndex++;
                    i = 0;
                    _seeker.StartPath(transform.position, currentBuilding.mapPoints[i].position, OnPathBuildedRebuild);
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
            particularPathInfo thisPath = new particularPathInfo();

            float distance = Vector2.Distance(currentBuilding.mapPoints[i].position, path.vectorPath[path.vectorPath.Count-1]);
            
            if (distance <= hexRadius) 
            {
                thisPath.isAccesible = true;
            }
            else 
            {
                thisPath.isAccesible = false;
            }

            thisPath.currentBuilding = currentBuilding;
            thisPath.currentBuildingCell = i;
            thisPath.currentBuildingNodes = path.vectorPath.Count;

            Debug.Log("Path to: " + thisPath.currentBuilding + "    to cell: " + thisPath.currentBuildingCell + "   is: " + thisPath.currentBuildingNodes +"   and it is: " + thisPath.isAccesible);

            comparingPathes.Add(thisPath);







            if (currentBuilding.mapPoints.Length-1 != i)
            {
                i++;
                _seeker.StartPath(transform.position, currentBuilding.mapPoints[i].position, OnComparedPathComplete);
            }
            else
            {
                if (currentBuilding != targetBuilding)
                {
                    currentBuilding = targetBuilding;
                    i = 0;
                    _seeker.StartPath(transform.position, currentBuilding.mapPoints[i].position, OnComparedPathComplete);
                }
                else
                {
                    GetBestRoot(comparingPathes);
                }
            }
        }
    }

    public void ChangeDestination(int destinationID)
    {
        GetComponent<AIDestinationSetter>().target = ResourceManager.Instance.shtabReference.GetUnitDestination();
        destination = ResourceManager.Instance.shtabReference.GetUnitDestination().position;
    }

    #endregion


    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.buildingTag && collider.gameObject == destinationBuilding)
        {
            // We reached destination
            isReachedTarget = true;
            Debug.Log("Arrived at destination!");
        }

        if (collider.gameObject.name == "Bash")
        {
            // Bash intersects us
            isBashIntersects = true;
            Debug.Log("Bash state go now!");
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
struct particularPathInfo
{
    public bool isAccesible;
    public BuildingMapInfo currentBuilding;
    public int currentBuildingCell;
    public int currentBuildingNodes;
};