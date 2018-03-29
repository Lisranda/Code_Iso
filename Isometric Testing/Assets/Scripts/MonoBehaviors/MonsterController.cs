using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterController : NPCController {
	public LayerMask hostileTo;
	[SerializeField] protected float rangeHostile = 4f;
	[SerializeField] protected GameObject targetHostile;
	[SerializeField] protected GameObject targetHostileTile;
	[SerializeField] protected GameObject attackHostileTile;

	protected List<GameObject> targetsInRange;

	protected override void Start () {
		base.Start ();
		targetsInRange = new List<GameObject> ();
	}

	protected override void Update () {
		if (!isServer) {
			return;
		}

		base.Update ();
		DetectHostiles ();
		GetTargetHostileTile ();
		FindAttackHostileTile ();
		SetTileTarget ();
		MoveOnPath ();
		FaceTarget ();
		GetKey ();
		MoveOnPath ();
	}

	protected void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, rangeHostile);
	}

	protected void DetectHostiles () {
		if (isMoving)
			return;
		
		targetsInRange.Clear ();

		Collider[] hits = Physics.OverlapSphere (transform.position, rangeHostile, hostileTo);
		if (hits.Length > 0) {
			foreach (Collider c in hits) {
				targetsInRange.Add (c.gameObject);
			}
		}

		if (targetHostile != null) {
			if (!targetsInRange.Contains (targetHostile)) {
				targetHostile = null;
			}
		}

		if (targetsInRange.Count > 0) {
			if (targetHostile == null) {
				targetHostile = targetsInRange [0];
			}
		}
	}

	protected void GetTargetHostileTile () {
		//TODO: EXPLORE USING THE TARGET'S STORED tileLocation FOR THIS
		// DOES THE SERVER KNOW THIS?  CAN THE SERVER KNOW THIS?
		if (isMoving)
			return;
		
		targetHostileTile = null;

		if (targetHostile != null) {
			targetHostileTile = FindTileLocation (targetHostile.transform.position);
		}
	}

	protected void FindAttackHostileTile () {
		if (isMoving)
			return;
		
		attackHostileTile = null;

		if (targetHostile == null)
			return;

		int pathLength;
		int trackerLength = 1000;

		foreach (GameObject go in targetHostileTile.GetComponent<Tile>().neighbors) {
			if (go == null)
				continue;

			if (go == tileLocation) {
				attackHostileTile = tileLocation;
				break;
			}
				

			Tile t = go.GetComponent<Tile> ();
			if (t.isWalkable && !t.isOccupied) {
				pathLength = Pathfinding.CalculatePathBF (tileLocation, go).Count;

				if (pathLength < trackerLength) {
					trackerLength = pathLength;
					attackHostileTile = go;
				}
			}
		}
	}

	protected void SetTileTarget () {
		if (isMoving)
			return;
		
		tileTarget = null;

		if (attackHostileTile != null)
			tileTarget = attackHostileTile;
	}

	protected void FaceTarget () {
		if (isMoving)
			return;
		
		if (attackHostileTile != null) {
			if (attackHostileTile == tileLocation) {
				Vector3 direction = PathfindingGetDirectionForMove (targetHostileTile);
				Facing face = PathfindingGetFacingForMove (targetHostileTile);
				if (facingDirection != face) {
					InitiateRotate (face, direction);
				}
			}
		}
	}

	protected void GetKey () {		

		if (Input.GetKeyDown ("m")) {
			int locX = Random.Range (0, 10);
			int locY = 0;
			int locZ = Random.Range (0, 10);

			Vector3 location = new Vector3 ((float)locX, (float)locY, (float)locZ);
//			Vector3 location = new Vector3 (0f, 0f, 0f);
			Debug.Log (location);
			if (FindTileLocation (location) != null) {
				if (FindTileLocation (location).GetComponent<Tile> ().isWalkable && !FindTileLocation (location).GetComponent<Tile> ().isOccupied) {
					tileTarget = FindTileLocation (location);
				}
			}
		}
	}

}
