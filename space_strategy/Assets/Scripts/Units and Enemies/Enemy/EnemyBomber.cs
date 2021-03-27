using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class EnemyBomber : Enemy
{
    // public EnemyBomberData ebData;
    public BomberIdleState bomberIdleState = new BomberIdleState();
    public BomberGoToState bomberGoToState = new BomberGoToState();
    public BomberBashState bomberBashState = new BomberBashState();
    public BomberAttackState bomberAttackState = new BomberAttackState();
    public IBomberState currentState = null;
    
    public float speed;
    public float attackPoints;
    public float attackTimer;
    
    public Vector3 destination;
    public bool isPathInit = false;

    public Seeker _seeker;
    public Path _path = null;
    public int _currentWaypoint = 0;                          // Store current waypoint along path that we are targeting.


    public List<GameObject> buildingsInRange = null;
    float hexRadius = 1.3f;




    Vector3 pathEndPointPosition;
    BuildingMapInfo currentBuilding;
    BuildingMapInfo targetBuilding;

    int i;
    int specialIndex;
    int allspecialIndexes;

    List<particularPathInfo> rebuildingCurrentPathPathes = new List<particularPathInfo>();
    List<particularPathInfo> comparingPathes = new List<particularPathInfo>();
    List<particularPathInfo> pathesToShtab = new List<particularPathInfo>();


    public GameObject destinationBuilding;



    public bool isReachedTarget = false;
    public bool isBashIntersects = false;
    public bool isColidedNewBuilding = false;





    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void EnemyDestroy(AliveGameUnit gameUnit);
    public event EnemyDestroy OnEnemyDestroyed = delegate{};

    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints);

        if (healthPoints <= 0)
        {
            DestroyUnit();
            return;
        }

        OnDamageTaken(this);
    }

    private void Update()
    {
        currentState = currentState.DoState(this);

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (name == "Bomber1")
            {
                buildingsInRange.Add(GameObject.Find("G1"));
                buildingsInRange.Add(GameObject.Find("G0"));

                RebuildCurrentPath();

                // CompareRangeAndAssignCloserTarget(GameObject.Find("G1"));
                // DestroyUnit();
            }
        }
    }

    private void OnPathBuilded(Path path)
    {
        if (!path.error)
        {
            _path = path;
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

        // Reset all variables
        _currentWaypoint = 0;
    }





































    public void Creation()
    {
        _seeker = GetComponent<Seeker>();
        BomberStaticData.bomber_counter++;
        name = "Bomber" + BomberStaticData.bomber_counter;
        ResourceManager.Instance.enemies.Add(this);

        currentState = bomberGoToState;



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
        currentBuilding = destinationBuilding.GetComponent<BuildingMapInfo>();
        _seeker.StartPath(transform.position, currentBuilding.mapPoints[i].position, OnComparedPathComplete);
    }




















    private void OnInitializingPathComplete(Path path)
    {
        if (!path.error)
        {
            particularPathInfo thisPath = new particularPathInfo();




            pathEndPointPosition = path.vectorPath[path.vectorPath.Count-1];
            float distance = Vector2.Distance(currentBuilding.mapPoints[i].position, pathEndPointPosition);

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





            pathEndPointPosition = path.vectorPath[path.vectorPath.Count-1];
            float distance = Vector2.Distance(currentBuilding.mapPoints[i].position, pathEndPointPosition);
            
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





            pathEndPointPosition = path.vectorPath[path.vectorPath.Count-1];
            float distance = Vector2.Distance(currentBuilding.mapPoints[i].position, pathEndPointPosition);
            
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

    private void DestroyUnit() // Reload here because dead unit maybe was working at shaft
    {
        ResourceManager.Instance.enemies.Remove(this);
        Destroy(gameObject);
    }





    void OnTriggerEnter2D(Collider2D collider) // or ShaftRadius or SkladRadius or HomeRadius
    {
        if (collider.gameObject.tag == TagConstants.buildingTag && collider.gameObject == destinationBuilding)
        {
            // We reached destination
            isReachedTarget = true;
        }

        if (collider.gameObject.name == "Bash")
        {
            isBashIntersects = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) // For model correct placing
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision) // resource collision
    {

    }
}

    struct particularPathInfo
    {
        public bool isAccesible;
        public BuildingMapInfo currentBuilding;
        public int currentBuildingCell;
        public int currentBuildingNodes;
    };
