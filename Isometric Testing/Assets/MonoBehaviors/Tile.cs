using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public bool isWalkable = true;
	public bool isOccupied = false;

	public bool mouseOver = false;

	public GameObject[] neighbors = new GameObject[4];
	const int north = 0;
	const int east = 1;
	const int south = 2;
	const int west = 3;

	void Start () {
		CheckObstructed ();
		SetNeighbors ();
	}

	void Update () {		

	}

	void LateUpdate () {
		HighlightMouseOver ();
//		HighlightOccupied ();
	}

	public void SetNeighbors () {
		FindNeighbors (Vector3.left, north, 2f);
		FindNeighbors (Vector3.right, south, 2f);
		FindNeighbors (Vector3.back, west, 2f);
		FindNeighbors (Vector3.forward, east, 2f);
	}

	void FindNeighbors (Vector3 worldDirection, int cardinalDirection, float scanDistance) {
		RaycastHit hit;

		if (Physics.Raycast (transform.position + Vector3.up + worldDirection, Vector3.down, out hit, scanDistance)) {
			if (hit.transform.gameObject.CompareTag ("Floor"))
				neighbors [cardinalDirection] = hit.transform.gameObject;
			else
				neighbors [cardinalDirection] = null;
		}
		else
			neighbors [cardinalDirection] = null;
	}

	void CheckObstructed () {
		RaycastHit hit;
		if (Physics.Raycast (transform.position + Vector3.down, Vector3.up, out hit, 2f)) {
			GameObject go = hit.transform.gameObject;
			if (go.GetComponent<TerrainObject> () != null) {
				if (go.GetComponent<TerrainObject> ().obstructsMovement) {
					isWalkable = false;
				}
			}
		}
	}

	public void AddHighlight () {
		mouseOver = true;
	}

	public void RemoveHighlight () {
		mouseOver = false;
	}

	void HighlightMouseOver () {
		if (mouseOver) {
			if (isWalkable && !isOccupied)
				GetComponent<Renderer> ().material.color = Color.green;
			else if (isWalkable && isOccupied)
				GetComponent<Renderer> ().material.color = Color.yellow;
			else
				GetComponent<Renderer> ().material.color = Color.red;
		}
		else
			GetComponent<Renderer> ().material.color = Color.white;

		mouseOver = false;
	}

	void HighlightOccupied () {
		if (isOccupied)
			GetComponent<Renderer> ().material.color = Color.red;
		else
			GetComponent<Renderer> ().material.color = Color.white;
	}
}
