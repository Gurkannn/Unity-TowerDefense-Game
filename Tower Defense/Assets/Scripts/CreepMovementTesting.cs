using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepMovementTesting : MonoBehaviour
{
    public Action GoalAction { get; set; }
    public BaseCreep CurrentCreep { get; set; }
    public Queue<Vector3> PathQueue { get; set; }
    public Queue<Vector3> PathCache { get; set; }
    public Vector3 StartPosition { get; set; }
    private Vector3 TargetNode { get; set; }
    private float NodeMargin = 0.1f;

    void Start()
    {
        if (PathCache == null)
            PathCache = new Queue<Vector3>();
        if (PathQueue.Count > 0)
        {
            TargetNode = PathQueue.Dequeue();
            StartPosition = TargetNode;
        }
        else
        {
            Debug.Log("PathQueue is empty");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = TargetNode - transform.position;

        dir.Normalize();

        transform.Translate(dir * Time.deltaTime * CurrentCreep.MovementSpeed);

        if (Vector3.Distance(transform.position, TargetNode) < NodeMargin)
        {
            Debug.Log("Creep reached target node");
            if (PathQueue.Count > 0)
            {
                PathCache.Enqueue(TargetNode);
                TargetNode = PathQueue.Dequeue();
            }
            else
                ReachedGoal(GoalAction);
        }
    }

    void ReachedGoal(Action goalAction)
    {
        if (goalAction != null)
        {
            goalAction();
            Debug.Log("Performed Goal Action");
        }
        else
            Debug.Log("Goal actions is empty");
        foreach (Vector3 node in PathCache)
        {
            PathQueue.Enqueue(node);
        }
        this.transform.position = StartPosition;
        this.gameObject.SetActive(false);
    }
}
