using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower  {

    public BaseTower(string name, int damage, int range, float attackSpeed, int splashRadius, bool slowing, int cost)
    {
        Name = name;
        Damage = damage;
        Range = range;
        AttackSpeed = attackSpeed;
        SplashRadius = splashRadius;
        Slowing = slowing;
        Cost = cost;
    }
    public string Name { get; set; }
    public int Cost { get; set; }
    public int Damage { get; set; }
    public int Range { get; set; }
    public float AttackSpeed { get; set; }
    public int SplashRadius { get; set; }
    public bool Slowing { get; set; }

}
