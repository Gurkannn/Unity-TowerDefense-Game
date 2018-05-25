using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolGenerator
{

    private int PoolCount { get; set; }
    private string Name { get; set; }
    private Transform TargetParent { get; set; }
    private GameObject ObjectPrefab { get; set; }
    private IEnumerable<GameObject> ObjectPool { get; set; }

    public void SetPoolCount(int count)
    {
        PoolCount = count;
    }

    public void SetPoolPrefab(GameObject prefab)
    {
        ObjectPrefab = prefab;
    }

    public void SetPoolParent(Transform parent)
    {
        TargetParent = parent;
    }

    public void SetObjectName(string name)
    {
        Name = name;
    }

    public IEnumerable<GameObject> GeneratePool()
    {
        List<GameObject> objectList = new List<GameObject>();
        if (PoolCount > 0 && ObjectPrefab != null)
        {
            for (int i = 0; i < PoolCount; i++)
            {
                GameObject go = GameObject.Instantiate(ObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);

                if (TargetParent != null)
                    go.transform.SetParent(TargetParent);
                if (!string.IsNullOrEmpty(Name))                
                    go.name = Name + " " + i;
                objectList.Add(go);
                go.SetActive(false);                
            }
        }
        return objectList as IEnumerable<GameObject>;
    }
}
