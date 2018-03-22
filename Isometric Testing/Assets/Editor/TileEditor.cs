using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
[CanEditMultipleObjects]
public class TileEditor : Editor {

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();

		if(GUILayout.Button("Find Neighbors"))
		{
			foreach (Object obj in targets) {
				Tile tileData = (Tile)obj;
				tileData.SetNeighbors ();
			}
		}
	}
}
