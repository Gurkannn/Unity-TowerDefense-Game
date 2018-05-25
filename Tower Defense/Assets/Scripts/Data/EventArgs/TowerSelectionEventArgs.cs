using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionEventArgs : EventArgs {

    public Tower SelectedTower { get; set; }
    public TowerSelectionEventArgs(Tower Selection)
    {
        SelectedTower = Selection;
    }
}
