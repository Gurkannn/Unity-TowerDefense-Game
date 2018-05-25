using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGenerator : EditorWindow {

    [MenuItem("Map/MapGenerator")]
    public static void ShowWindow()
    {
        GetWindow<MapGenerator>("MapGenerator");
    }

	// Use this for initialization
	void OnGui () {
	    
	}
	
}
