using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreepSpawnTesting : MonoBehaviour
{

    public int SpawnCount { get; set; }
    public float SpawnDelay = 300f;
    private float SpawnTimer;
    public GameObject CreepPrefab;

    //Pathing
    private Transform CreepParent { get; set; }
    private Transform PathParent { get; set; }
    private Vector3 PathStart { get; set; }
    private List<Vector3> PathPoints { get; set; }
    private PathingTesting PathingComponent { get; set; }

    //Spawning pool
    public Queue<BaseWave> WaveQueue { get; set; }
    public Queue<GameObject> SpawnPool { get; set; }
    public Queue<BaseCreep> CreepQueue { get; set; }
    public int PoolCount { get; private set; }


    void Start()
    {
        PoolCount = 5;
        SpawnCount = 9;
        SpawnTimer = SpawnDelay;

        if (SpawnPool == null)        
            SpawnPool = new Queue<GameObject>();        
        if (CreepQueue == null)
            CreepQueue = new Queue<BaseCreep>();

        //Testing out creepqueue
        for (int i = 0; i < SpawnCount; i++)
        {
            CreepQueue.Enqueue(new TestCreep());
        }

        //Finds pathing parent gameobject
        PathParent = GameObject.FindGameObjectWithTag("Path").transform;

        //Finds creep parent gameobject
        CreepParent = GameObject.FindGameObjectWithTag("Creep").transform;

        //Finds pathing component on attached gameobject
        PathingComponent = GetComponent<PathingTesting>();

        //Sets first child of pathing parent as start point
        PathStart = PathParent.GetChild(0).position;

        //Instantiate Spawn Pool
        for (int i = 0; i < PoolCount; i++)
        {
            //Create and instantiate gameobject
            GameObject newSpawn = GameObject.Instantiate(CreepPrefab, PathStart, Quaternion.identity);
            //Set Creep object as parent
            newSpawn.transform.SetParent(CreepParent);
            //Create movement component
            CreepMovementTesting spawnMovement = newSpawn.AddComponent<CreepMovementTesting>();
            //Add path queue to component
            spawnMovement.PathQueue = PathingComponent.GetPathQueue();
            //Set goal action to rejoin spawn pool when goal reached
            spawnMovement.GoalAction = () =>
            {
                SpawnPool.Enqueue(newSpawn);
                Debug.Log("Readded creep to spawnpool queue");
            };
            //Set current creep on component. Test for now
            spawnMovement.CurrentCreep = new TestCreep();
            //Deactivate gameobject
            newSpawn.SetActive(false);
            //Add new object to SpawnPool
            SpawnPool.Enqueue(newSpawn);
        }
    }
    int SpawnCounter = 0;
    void Update()
    {
        if (CreepQueue != null)
        {
            if (CreepQueue.Count > 0 && SpawnPool.Count > 0)
            {
                //Counts down to spawn
                SpawnTimer -= Time.deltaTime;
                if (SpawnTimer <= 0)
                {
                    //Gets next creep in queue
                    BaseCreep creep = CreepQueue.Dequeue();
                    //Gets next SpawnPool object
                    GameObject poolObject = SpawnPool.Dequeue();
                    //Sets object to active
                    poolObject.SetActive(true);
                    //Gets component for creep handling and sets correct creep
                    poolObject.GetComponent<CreepMovementTesting>().CurrentCreep = creep;
                    Debug.Log("Spawning Creep");
                    //Reset timer
                    SpawnTimer = SpawnDelay;
                    SpawnCounter++;
                    Debug.Log(SpawnCounter);
                }
            }
        }
    }

}
