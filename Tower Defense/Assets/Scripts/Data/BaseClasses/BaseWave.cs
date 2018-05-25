using System;
using System.Collections;
using System.Collections.Generic;

public class BaseWave  {

    public int CreepCount { get; set; }
    public BaseCreep CreepType { get; set; }
    public Func<BaseCreep> CreepModification { get; set; }

    public BaseWave(int CreepCount, BaseCreep CreepType)
    {
        this.CreepCount = CreepCount;
        this.CreepType = CreepType;
    }

}
