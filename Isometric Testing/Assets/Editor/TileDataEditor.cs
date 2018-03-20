using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileData))]
[CanEditMultipleObjects]
public class TileDataEditor : Editor {

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();

		if(GUILayout.Button("Find Neighbors"))
		{
			foreach (Object obj in targets) {
				TileData tileData = (TileData)obj;
				tileData.SetNeighbors ();
			}
		}
	}
}
