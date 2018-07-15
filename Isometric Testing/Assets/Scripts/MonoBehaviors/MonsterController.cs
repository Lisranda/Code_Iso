using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : NPCController {
	public LayerMask hostileTo;
	[SerializeField] protected float roamTime = 5f;
	[SerializeField] protected float rangeHostile = 4f;
	[SerializeField] protected GameObject targetHostile;
	[SerializeField] protected GameObject targetHostileTile;
	[SerializeField] protected GameObject attackHostileTile;

	float roamTimer = 0;
	protected List<GameObject> targetsInRange;

	protected override void Start () {
		base.Start ();
		targetsInRange = new List<GameObject> ();
	}

	protected override void Update () {
		base.Update ();
		MonsterAI ();
	}

	#region GIZMOS

	protected void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, rangeHostile);
	}

	#endregion

	protected void MonsterAI () {		
		//See if there are any hostile targets in visual range.
		targetsInRange.Clear ();
		targetsInRange = FindTargetsInRange (transform.position, rangeHostile, hostileTo);

		//Update the hostile target.
		targetHostile = null;
		targetHostile = SetHostileTarget (targetsInRange);

		//Update the tile that our hostile target is standing on.
		targetHostileTile = null;
		if (targetHostile != null) {
			targetHostileTile = GetTargetHostileTile ();
		}

		//Update the tile we want to get to in order to attack.
		attackHostileTile = null;
		if (targetHostile != null) {
			attackHostileTile = AttackFrom (targetHostile);
		}

		if (isMoving)
			return;

		if (tileMoveTarget == tileLocation) {
			tileMoveTarget = null;
		}

		//If we have a valid path to our attack tile, move on it.
		if (attackHostileTile != null) {
			tileMoveTarget = attackHostileTile;
		}

		//Roam if we have no hostile target.
		if (targetHostile == null || attackHostileTile == null) {
			MonsterRoam ();
		}

		MoveOnPath ();
		FaceTarget ();
			
	}

	protected GameObject MonsterRoam () {
		if (roamTimer < roamTime) {
			roamTimer += Time.deltaTime;
			return null;
		}
			
		GameObject neighbor = tileLocation.GetComponent<Tile> ().neighbors [Random.Range (0, 4)];

		if (neighbor == null) {
			roamTimer = 0f;
			return null;
		}

		Tile neighborTile = neighbor.GetComponent<Tile> ();

		if (!neighborTile.isWalkable || neighborTile.isOccupied) {
			roamTimer = 0f;
			return null;
		}

		tileMoveTarget = neighbor;
		roamTimer = 0f;
		return neighbor;
	}

	protected List<GameObject> FindTargetsInRange (Vector3 location, float range, LayerMask detectMask) {
		List<GameObject> inRange = new List<GameObject> ();

		Collider[] hits = Physics.OverlapSphere (location, range, detectMask);
		if (hits.Length > 0) {
			foreach (Collider c in hits) {
				inRange.Add (c.gameObject);
			}
		}
		return inRange;
	}

	protected GameObject SetHostileTarget (List<GameObject> possibleTargets) {
		if (possibleTargets.Count == 0)
			return null;
		
		foreach (GameObject hostile in possibleTargets) {
			GameObject hostileLocation = FindTileLocation (hostile.transform.position);
			Tile hostileTile = hostileLocation.GetComponent<Tile> ();
			foreach (GameObject neighbor in hostileTile.neighbors) {				

				if (neighbor == null)
					continue;
				
				Tile neighborTile = neighbor.GetComponent<Tile> ();

				if (neighbor == tileLocation)
					return hostile;
				
				if (neighborTile.isWalkable && !neighborTile.isOccupied) {
					if (Pathfinding.CalculatePathBF (tileLocation, neighbor).Count > 0)
						return hostile;
				}				
			}
		}
		return possibleTargets [0];
	}

	protected GameObject AttackFrom (GameObject hostile) {
		GameObject attackTile = null;
		GameObject hostileLocation = FindTileLocation (hostile.transform.position);
		Tile hostileTile = hostileLocation.GetComponent<Tile> ();
		int pathTracker = 1000;
		foreach (GameObject neighbor in hostileTile.neighbors) {			
			if (neighbor == null)
				continue;

			Tile neighborTile = neighbor.GetComponent<Tile> ();

			if (neighbor == tileLocation)
				return neighbor;
			
			if (neighborTile.isWalkable && !neighborTile.isOccupied) {
				int pathLength = Pathfinding.CalculatePathBF (tileLocation, neighbor).Count;

				if (pathLength == 0)
					continue;

				if (pathLength < pathTracker) {
					pathTracker = pathLength;
					attackTile = neighbor;
				}
			}				
		}
		return attackTile;		
	}

	protected GameObject GetTargetHostileTile () {		
		return FindTileLocation (targetHostile.transform.position);
	}

	protected void FaceTarget () {		
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
}
