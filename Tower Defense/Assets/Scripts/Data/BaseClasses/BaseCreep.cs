using System.Collections;
using System.Collections.Generic;

public class BaseCreep  {

    public BaseCreep(int Health, float MovementSpeed)
    {
        this.Health = Health;
        this.MovementSpeed = MovementSpeed;
    }

    public int Health { get; set; }
    public float MovementSpeed { get; set; }
    public int Armor { get; set; }


}
