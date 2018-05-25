using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour
{

    private Dictionary<Transform, GameObject> SelectedObjects { get; set; }

    public List<GameObject> SelectedTiles { get; private set; }

    public GameObject SelectionIcon;

    private TileGeneratorTest TileGenerator { get; set; }

    ObjectPoolGenerator PoolGenerator;

    private Queue<GameObject> InactiveSelectionObjects { get; set; }
    private List<GameObject> ActiveSelectionIconObjects { get; set; }

    void Start()
    {
        if (TileGenerator == null)
            TileGenerator = GetComponent<TileGeneratorTest>();

        if (SelectedTiles == null)
            SelectedTiles = new List<GameObject>();

        if (SelectedObjects == null)
            SelectedObjects = new Dictionary<Transform, GameObject>();

        if (InactiveSelectionObjects == null)
            InactiveSelectionObjects = new Queue<GameObject>();

        if (ActiveSelectionIconObjects == null)
            ActiveSelectionIconObjects = new List<GameObject>();

        InitializeObjectPool();
    }

    void Update()
    {
        //Checks if left mouse button was pressed
        if (Input.GetMouseButtonDown(0))
        {
            //Checks if mouse is currently over ui element
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //Raycast to check for collider under mouse
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 0.1f);
                int hitx = (int)hit.transform.position.x;
                int hity = (int)hit.transform.position.y;
                if (hit.collider != null)
                {
                    if (TileGenerator.GetTileAt(hitx, hity).ActiveTower != null)
                    {
                        //Checks if object is already selected
                        if (!SelectedObjects.ContainsKey(hit.collider.transform))
                        {
                            //Checks if holding shift to select multiple
                            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                                if (SelectedTiles.Count > 0)
                                    //Clears previous selection if shift was not held down
                                    ClearSelection();
                            //Add new selection
                            AddSelection(hit.collider.transform);
                        }
                        //Object was already selected
                        else
                        {
                            //Removes selection
                            RemoveSelection(hit.collider.transform);
                            SelectedTiles.Remove(hit.collider.gameObject);
                        }
                    }
                }
            }
        }
    }

    public Tile GetTileAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit2D = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), Vector2.down);

        return TileGenerator.GetTileAt((int)hit2D.transform.position.x, (int)hit2D.transform.position.y);
    }

    /// <summary>
    /// Sets up the object spawn pool
    /// </summary>
    void InitializeObjectPool()
    {
        if (PoolGenerator == null)
            PoolGenerator = new ObjectPoolGenerator();
        PoolGenerator.SetObjectName("SelectionPoolObject");
        PoolGenerator.SetPoolCount(TileGenerator.MapWidth * TileGenerator.MapHeight);
        PoolGenerator.SetPoolPrefab(SelectionIcon);
        PoolGenerator.SetPoolParent(this.transform);
        InactiveSelectionObjects = new Queue<GameObject>(PoolGenerator.GeneratePool());

    }


    /// <summary>
    /// Adds selection icon to tile and stores reference to it
    /// </summary>
    /// <param name="selection"></param>
    void AddSelection(Transform selection)
    {
        //Dequeue inactive object from queue
        GameObject newSelectionIcon = InactiveSelectionObjects.Dequeue();
        //Set object to active in scene
        newSelectionIcon.SetActive(true);
        //Sets new object to correct position
        newSelectionIcon.transform.position = selection.position;
        //Add new object to dictionary
        SelectedObjects.Add(selection, newSelectionIcon);
        //Add to active selection list
        ActiveSelectionIconObjects.Add(newSelectionIcon);
        //Add tile to current tiles selected
        SelectedTiles.Add(selection.gameObject);
    }

    /// <summary>
    /// Removes selection icon from tile and requeues it in object pool
    /// </summary>
    /// <param name="selection"></param>
    void RemoveSelection(Transform selection)
    {
        //Queue up removed selection to inactive pool
        InactiveSelectionObjects.Enqueue(SelectedObjects[selection]);
        //Set gameobject to inactive
        SelectedObjects[selection].SetActive(false);
        //Remove selection gameobject from active objects
        ActiveSelectionIconObjects.Remove(SelectedObjects[selection]);
        //Remove from select dictionary
        SelectedObjects.Remove(selection);
    }

    /// <summary>
    /// Clear current selectin
    /// </summary>
    void ClearSelection()
    {
        foreach (GameObject obj in SelectedTiles)
        {
            RemoveSelection(obj.transform);
        }
        SelectedTiles.Clear();
    }


}
