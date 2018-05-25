using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    public GameObject BaseTowerPrefab;

    private SelectionController SelectionController { get; set; }

    private TileGeneratorTest TileGenerator { get; set; }

    private ObjectPoolGenerator PoolGenerator { get; set; }
    private Tower selectedTower;
    private Tower SelectedTower
    {
        get
        {
            return selectedTower;
        }
        set
        {
            selectedTower = value;
            OnTowerSelectionChanged(this, value);
        }
    }

    private List<GameObject> ActiveTowers { get; set; }

    private List<GameObject> CurrentSelection { get; set; }
    private List<GameObject> ActivePoolObjects { get; set; }
    private Queue<GameObject> InactivePoolObjects { get; set; }

    public Tower TestTower;

    Button testTowerButton;

    public delegate void TowerSelectionEventHandler(object sender, TowerSelectionEventArgs args);
    public event TowerSelectionEventHandler TowerSelectionChanged;

    protected virtual void OnTowerSelectionChanged(object sender, Tower SelectedTower)
    {
        if (TowerSelectionChanged != null)
        {
            TowerSelectionChanged.Invoke(sender, new TowerSelectionEventArgs(SelectedTower));
        }
    }

    public void TowerSelectionRegisterCallback(TowerSelectionEventHandler callback)
    {
        TowerSelectionChanged += callback;
    }
    public void TowerSelectionUnregisterCallback(TowerSelectionEventHandler callback)
    {
        TowerSelectionChanged -= callback;
    }

    void Start()
    {
        if (TileGenerator == null)
        {
            TileGenerator = GetComponent<TileGeneratorTest>();
        }
        if (testTowerButton == null)
        {
            testTowerButton = GameObject.Find("Tower1").GetComponent<Button>();
            testTowerButton.onClick.AddListener(() => { SelectedTower = TestTower; });
        }
        if (ActiveTowers == null)
            ActiveTowers = new List<GameObject>();
        if (CurrentSelection == null)
            CurrentSelection = new List<GameObject>();
        if (InactivePoolObjects == null)
            InactivePoolObjects = new Queue<GameObject>();
        if (PoolGenerator == null)
            SetupSpawnPool();
        if (SelectionController == null)
            SelectionController = GetComponent<SelectionController>();
    }

    private void Update()
    {
        //Testing out building
        if (Input.GetMouseButtonDown(0))
        {
            if (SelectedTower != null)
            {
                BuildTower(SelectionController.GetTileAtMouse(), SelectedTower);
                SelectedTower = null;
            }
        }
    }

    void SetSelectedTower(Tower tower)
    {
        SelectedTower = tower;
    }
    void ClearSelectedTower()
    {
        SelectedTower = null;
    }

    /// <summary>
    /// Sets up the spawn pool for towers
    /// </summary>
    void SetupSpawnPool()
    {
        PoolGenerator = new ObjectPoolGenerator();
        PoolGenerator.SetObjectName("TowerPoolObject");
        PoolGenerator.SetPoolCount((GetComponent<TileGeneratorTest>().MapWidth * GetComponent<TileGeneratorTest>().MapHeight) / 2);
        PoolGenerator.SetPoolParent(GameObject.FindGameObjectWithTag("Tower").transform);
        PoolGenerator.SetPoolPrefab(BaseTowerPrefab);
        InactivePoolObjects = new Queue<GameObject>(PoolGenerator.GeneratePool());
    }

    /// <summary>
    /// Builds tower at target location if conditions are met
    /// </summary>
    /// <param name="targetTileGO"></param>
    /// <param name="tower"></param>
    void BuildTower(GameObject targetTileGO, Tower tower)
    {
        if (IsTileBuildable(targetTileGO.transform.position))
        {
            if (!DoesTileContainTower(targetTileGO.transform.position))
            {
                GameObject go = InactivePoolObjects.Dequeue();
                go.GetComponent<SpriteRenderer>().sprite = tower.TowerSprite;
                go.SetActive(true);
                go.transform.position = targetTileGO.transform.position;
                ActiveTowers.Add(go);
                Debug.Log("Built tower");
            }
        }
    }
    void BuildTower(Tile targetTile, Tower tower)
    {
        if (IsTileBuildable(targetTile.Position))
        {
            if (!DoesTileContainTower(targetTile.Position))
            {
                GameObject go = InactivePoolObjects.Dequeue();
                go.GetComponent<SpriteRenderer>().sprite = tower.TowerSprite;
                go.SetActive(true);
                go.transform.position = targetTile.Position;
                ActiveTowers.Add(go);
                TileGenerator.ChangeTowerOnTile((int)targetTile.Position.x,(int)targetTile.Position.y,tower);
                Debug.Log("Built tower");
            }
        }
    }
    /// <summary>
    /// Checks if target tile already has a tower on it
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    bool DoesTileContainTower(Vector3 position)
    {
        bool towerFound = false;

        foreach (GameObject tower in ActiveTowers)
            if (tower.transform.position == position)
                towerFound = true;

        return towerFound;
    }

    /// <summary>
    /// Checks if tile is buildable
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    bool IsTileBuildable(Vector3 position)
    {
        bool tileBuildable = true;

        if (!TileGenerator.GetTileAt((int)position.x, (int)position.y).IsBuildable)
            tileBuildable = false;

        return tileBuildable;
    }
}
