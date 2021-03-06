﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public bool isWalkable = true;
	public bool isOccupied = false;
	public bool isReserved = false;

	public bool mouseOver = false;
	public bool movementTarget = false;

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
		ReleaseOccupied ();
	}

	void LateUpdate () {
		OccupyReserves ();
		HighlightTile ();
	}

	void OccupyReserves () {
		if (isReserved)
			isOccupied = true;
	}
		
	void ReleaseOccupied () {		
		RaycastHit hit;
		Vector3 mod = new Vector3 (0f, -0.5f, 0f);
		if (Physics.Raycast (transform.position + mod, Vector3.up, out hit, 5f)) {
			GameObject go = hit.transform.gameObject;
			if (go.GetComponent<PawnController> () != null) {
				return;
			} else if (go.GetComponent<PawnController> () == null && !isReserved) {
				isOccupied = false;
			}
		} else if (!isReserved) {
			isOccupied = false;
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
		Vector3 mod = new Vector3 (0f, -0.5f, 0f);
		if (Physics.Raycast (transform.position + mod, Vector3.up, out hit, 1f)) {
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

	public void AddMovementTarget () {
		movementTarget = true;
	}

	void HighlightTile () {
		if (movementTarget) {
			GetComponent<Renderer> ().materials [1].color = Color.magenta;
		} else if (mouseOver) {
			if (isWalkable && !isOccupied)
				GetComponent<Renderer> ().materials [1].color = Color.green;
			else if (isWalkable && isOccupied)
				GetComponent<Renderer> ().materials [1].color = Color.yellow;
			else
				GetComponent<Renderer> ().materials [1].color = Color.red;
		} else
			GetComponent<Renderer> ().materials [1].color = Color.white;

		mouseOver = false;
		movementTarget = false;
	}
}
