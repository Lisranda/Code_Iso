using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tile : NetworkBehaviour {
	public bool isWalkable = true;
	[SyncVar] public bool isOccupied = false;
	[SyncVar] public bool isReserved = false;

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
		OccupyReserves ();
		ReleaseOccupied ();
		HighlightMouseOver ();
	}

	void OccupyReserves () {
		if (isReserved)
			isOccupied = true;
	}

	public GameObject testing;

	void ReleaseOccupied () {
		if (isServer) {
			if (isOccupied) {
				RaycastHit hit;
				if (Physics.Raycast (transform.position, Vector3.up, out hit, 1f)) {
					GameObject go = hit.transform.gameObject;
					testing = go;
					if (go.GetComponent<PawnController> () != null) {
						return;
					} else if (go.GetComponent<PawnController> () == null && !isReserved) {
						isOccupied = false;
					}
				} else if (!isReserved) {
					isOccupied = false;
				}
			}
		}
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
		if (Physics.Raycast (transform.position, Vector3.up, out hit, 1f)) {
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
}
