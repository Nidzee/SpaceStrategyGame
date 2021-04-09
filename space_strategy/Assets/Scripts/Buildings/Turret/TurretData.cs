// using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;

// public class TurretData 
// {
//     public int ID;

//     public int rotation_building;
//     public float rotation_center;
//     public float rotation_center_w;

//     public GameObject _tileOccupied;
//     public GameObject center;

//     public Enemy target;
//     public List<Enemy> enemiesInsideRange;

//     public bool isCreated;
//     public bool isFacingEnemy;
//     public bool attackState;
//     public bool isPowerON;
//     public bool isMenuOpened;
//     public bool isTurnedInIdleMode;

//     public Quaternion idleRotation = new Quaternion();
//     public Quaternion targetRotation = new Quaternion();

//     public float coolDownTurnTimer;
//     public float upgradeTimer;    

//     public int level;
//     public int type;

//     public TurretCombatState combatState;
//     public TurretIdleState idleState;
//     public TurretPowerOffState powerOffState;
//     public ITurretState currentState;

//     public Turette _myTurret;


//     public TurretData(Turette thisTurret)
//     {
//         _myTurret = thisTurret;

//         isPowerON = ResourceManager.Instance.IsPowerOn();
//         enemiesInsideRange = new List<Enemy>();

//         _tileOccupied = null;

//         isCreated = true;
//         isFacingEnemy = false;
//         attackState = false;
//         isMenuOpened = false;
//         isTurnedInIdleMode = true;

        
//         idleRotation = new Quaternion();
//         targetRotation = new Quaternion();

//         coolDownTurnTimer = 3f;
//         upgradeTimer = 0f;    

//         combatState = new TurretCombatState();
//         idleState = new TurretIdleState();
//         powerOffState = new TurretPowerOffState();
//         currentState = idleState;
//     }

























// }
