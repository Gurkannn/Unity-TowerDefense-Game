using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPreview : MonoBehaviour
{

    public GameObject PreviewPrefab;

    bool IsPreviewing = false;

    bool IsLocked = false;

    GameObject ActivePreview { get; set; }

    Transform PoolParent { get; set; }

    Tower TowerToPreview { get; set; }

    BuildingController BuildingController { get; set; }

    TileGeneratorTest TileGenerator { get; set; }

    ObjectPoolGenerator PoolGenerator { get; set; }

    Queue<GameObject> PreviewSpawnPool { get; set; }

    void Start()
    {
        if (TileGenerator == null)
            TileGenerator = GetComponent<TileGeneratorTest>();
        if (PoolParent == null)
            PoolParent = GameObject.FindGameObjectWithTag("Preview").transform;
        if (BuildingController == null)
        {
            BuildingController = GetComponent<BuildingController>();
            BuildingController.TowerSelectionRegisterCallback((o, s) => { SetTowerToPreview(s.SelectedTower); });
        }
        if (PoolGenerator == null)
            SetupSpawnPool();
    }

    void SetTowerToPreview(Tower tower)
    {
        TowerToPreview = tower;
        SetupPreview();
    }

    void SetupPreview()
    {
        if (TowerToPreview != null)
        {
            ActivePreview = PreviewSpawnPool.Dequeue();
            ActivePreview.GetComponent<SpriteRenderer>().sprite = TowerToPreview.TowerSprite;
            ActivePreview.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.5f);
            ActivePreview.SetActive(true);
            IsPreviewing = true;
        }
        else
        {
            ActivePreview.SetActive(false);
            PreviewSpawnPool.Enqueue(ActivePreview);
            ActivePreview = null;
            IsPreviewing = false;
        }
    }

    void Update()
    {
        if (IsPreviewing)
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), Vector2.down);
            if (hit2D.collider != null)
            {
                Tile tile = TileGenerator.GetTileAt((int)hit2D.transform.position.x, (int)hit2D.transform.position.y);
                if (tile.IsBuildable && ActivePreview.transform.position != tile.Position)
                {
                    ActivePreview.transform.position = tile.Position;
                    IsLocked = true;
                }
                else
                {
                    if (IsLocked && tile.Position != hit2D.transform.position)
                        IsLocked = false;
                }
            }

            if (!IsLocked)
                ActivePreview.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }

    void SetupSpawnPool()
    {
        PoolGenerator = new ObjectPoolGenerator();
        PoolGenerator.SetObjectName("BuildPreviewPool");
        PoolGenerator.SetPoolCount(2);
        PoolGenerator.SetPoolParent(PoolParent);
        PoolGenerator.SetPoolPrefab(PreviewPrefab);
        PreviewSpawnPool = new Queue<GameObject>(PoolGenerator.GeneratePool());
    }
}
