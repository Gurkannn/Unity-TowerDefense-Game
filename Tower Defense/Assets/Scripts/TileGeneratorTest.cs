using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGeneratorTest : MonoBehaviour
{

    public GameObject BaseTile;
    public Sprite TestSprite;

    private Transform MapTileParent;
    public TileMap CurrentTileMap;
    public TileMap CurrenTileMapInverted;

    public int MapWidth;
    public int MapHeight;

    public Tile[,] TileArray;
    public List<Tile> Tiles;

    void Start()
    {

        if (MapTileParent == null)
            MapTileParent = GameObject.FindGameObjectWithTag("Tile").transform;

        if (Tiles == null)
            Tiles = new List<Tile>();

        Tiles = GenerateTiles(MapWidth, MapHeight);

        InstantiateTiles(TileArray);

        //Set camera to middle of map
        Camera.main.transform.position = new Vector3(MapWidth / 2, MapHeight / 2, -10);

    }

    /// <summary>
    /// Returns tile at target position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Tile GetTileAt(int x, int y)
    {
        //Tile existingTile = Tiles.First(t => t.Position.x == x && t.Position.y == y);
        return TileArray[x, y];
    }

    public void ChangeTowerOnTile(int x, int y, Tower tower)
    {
        TileArray[x, y].SetTower(tower);
    }


    /// <summary>
    /// Generate a new set of tiles
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    List<Tile> GenerateTiles(int width, int height)
    {
        List<Tile> tileList = new List<Tile>();
        TileArray = new Tile[width, height];

        for (int i = 0; i < height; i++)
        {
            for (int u = 0; u < width; u++)
            {
                TileArray[u, i] = new Tile(new Vector3(u, i));
                //tileList.Add(new Tile(new Vector3(u, i)));
            }
        }

        return tileList;
    }

    /// <summary>
    /// Instantiates tile collection into game world
    /// </summary>
    /// <param name="tileCollection"></param>
    void InstantiateTiles(Tile[,] tiles)
    {
        for (int h = 0; h < MapHeight; h++)
        {
            for (int w = 0; w < MapWidth; w++)
            {
                Transform tileObject = GameObject.Instantiate(BaseTile.transform, TileArray[h, w].Position, Quaternion.identity);
                SpriteRenderer tileSpriteRenderer = tileObject.GetComponent<SpriteRenderer>();

                if ((w == MapWidth / 2 && h > MapHeight / 2) || (w < MapWidth / 2 + 1 && h == MapHeight / 2))
                {
                    tileSpriteRenderer.sprite = TestSprite;
                }
                else if ((w == (MapWidth / 2) - 1) && h > MapHeight / 2 + 1)
                {
                    tileSpriteRenderer.sprite = CurrentTileMap.MiddleRight;
                }
                else if (w == MapWidth / 2 - 1 && h == MapHeight / 2 + 1)
                {
                    tileSpriteRenderer.sprite = CurrentTileMap.BottomRight;
                }
                else if (w == MapWidth / 2 + 1 && h == MapHeight / 2 - 1)
                {
                    tileSpriteRenderer.sprite = CurrenTileMapInverted.BottomRight;
                }
                else if ((w == (MapWidth / 2) + 1 && h > MapHeight / 2 - 1))
                {
                    tileSpriteRenderer.sprite = CurrentTileMap.MiddleLeft;
                }
                else if (h == MapHeight / 2 + 1 && w < MapWidth / 2 - 1)
                {
                    tileSpriteRenderer.sprite = CurrentTileMap.BottomMiddle;
                }
                else if (h == MapHeight / 2 - 1 && w < MapWidth / 2 + 1)
                {
                    tileSpriteRenderer.sprite = CurrentTileMap.TopMiddle;
                }
                else
                {
                    TileArray[w, h].IsBuildable = true;
                    if (tileSpriteRenderer.sprite == null)
                        tileSpriteRenderer.sprite = CurrentTileMap.Middle;
                }
                tileObject.name = "Tile: " + w + ", " + h;
                tileObject.position = TileArray[w, h].Position;
                tileObject.SetParent(MapTileParent);
            }
        }

    }
}
