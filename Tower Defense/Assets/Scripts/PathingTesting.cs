using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingTesting : MonoBehaviour
{

    private Transform PathParent { get; set; }
    private Vector3 PathStart { get; set; }
    private List<Vector3> PathPoints { get; set; }

    // Use this for initialization
    void Start()
    {
        InitializePath();
    }

    void InitializePath()
    {
        PathParent = GameObject.FindGameObjectWithTag("Path").transform;
        PathPoints = new List<Vector3>();
        Transform[] pathNodes = GetChildren(PathParent);
        foreach (Transform transform in pathNodes)
        {
            PathPoints.Add(transform.position);
        }
    }

    public Queue<Vector3> GetPathQueue()
    {
        Queue<Vector3> pathQueue = new Queue<Vector3>();

        if (PathPoints == null)
        {
            InitializePath();
        }
        foreach (Vector3 point in PathPoints)
        {
            pathQueue.Enqueue(point);
        }

        return pathQueue;
    }


    #region HelperMethods

    Transform[] GetChildren(Transform parent)
    {
        Transform[] children = new Transform[parent.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = parent.GetChild(i);
        }
        return children;
    }

    #endregion
}
