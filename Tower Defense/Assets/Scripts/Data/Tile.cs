using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile  {

    public Vector3 Position { get; set; }
    public bool IsBuildable { get; set; }
    public bool IsPath { get; set; }
    public Tower ActiveTower { get; private set; }

    public void SetTower(Tower tower)
    {
        ActiveTower = tower;
    }

    public Tile(Vector3 pos)
    {
        IsBuildable = false;
        IsPath = false;
        Position = pos;
        ActiveTower = null;
    }

}
