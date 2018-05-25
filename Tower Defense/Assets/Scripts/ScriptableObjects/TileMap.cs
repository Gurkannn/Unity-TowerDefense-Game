using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TileMap",menuName = "TowerDefense/TileMap")]
public class TileMap : ScriptableObject
{
    public Sprite TopLeft;
    public Sprite TopMiddle;
    public Sprite TopRight;
    public Sprite MiddleLeft;
    public Sprite Middle;
    public Sprite MiddleRight;
    public Sprite BottomLeft;
    public Sprite BottomMiddle;
    public Sprite BottomRight;
}
