using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour {
	public bool isWalkable = true;
	public bool isOccupied = false;

	public GameObject[] neighbors = new GameObject[4];
	const int north = 0;
	const int east = 1;
	const int south = 2;
	const int west = 3;

	void Start () {
		SetNeighbors ();
	}

	public void SetNeighbors () {
		FindNeighbors (Vector3.left, north, 1f);
		FindNeighbors (Vector3.right, south, 1f);
		FindNeighbors (Vector3.back, west, 1f);
		FindNeighbors (Vector3.forward, east, 1f);
	}

	void FindNeighbors (Vector3 worldDirection, int cardinalDirection, float scanDistance) {
		RaycastHit hit;

		if (Physics.Raycast (transform.position, worldDirection, out hit, scanDistance)) {
			if (hit.transform.gameObject.CompareTag ("Terrain"))
				neighbors [cardinalDirection] = hit.transform.gameObject;
			else
				neighbors [cardinalDirection] = null;
		}
		else
			neighbors [cardinalDirection] = null;
	}
}
