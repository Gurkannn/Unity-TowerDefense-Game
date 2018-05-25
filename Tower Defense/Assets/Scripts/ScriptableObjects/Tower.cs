using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "TowerDefense/Tower")]
public class Tower : ScriptableObject
{
    
    public BaseTower TowerType;
    public Sprite TowerSprite;

    public Tower(Sprite towerSprite)
    {
        TowerSprite = towerSprite;
    }
}
