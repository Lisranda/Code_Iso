using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour {
	[SerializeField] protected bool showForwardRay = false;
	protected float cardinalSpeed = 5f;
	protected float rotateSpeed = 3f;
	protected bool isMoving = false;
	protected enum Facing {North, East, South, West};
	[SerializeField] protected Facing facingDirection = new Facing ();
	[SerializeField] protected GameObject tileLocation;
	[SerializeField] protected GameObject tileTarget;

	Dictionary<GameObject,GameObject> goParents;

	protected void MoveOnPath () {
		if (tileLocation == tileTarget) {
			tileTarget = null;
		}
		if (tileTarget != null && !isMoving) {
			IList<GameObject> path = CalculatePath (tileLocation, tileTarget);
			if (path.Count > 0) {
				Vector3 direction = GetDirectionForMove (path [path.Count - 1]);
				Facing face = GetFacingForMove (path [path.Count - 1]);
				MoveOrRotate(face, direction);
			}
		}
	}

	protected IList<GameObject> CalculatePath (GameObject goStart, GameObject goTarget) {
		goParents = new Dictionary<GameObject, GameObject> ();
		IList<GameObject> path = new List<GameObject> ();

		GameObject pathTarget = BFS (goStart, goTarget);

		if (pathTarget == goStart) {
			//We failed to find a path, more error checking is needed.
			return null;
		}

		GameObject pathCurrent = pathTarget;
		while (pathCurrent != goStart) {
			path.Add (pathCurrent);
			pathCurrent = goParents [pathCurrent];
		}

		return path;
	}

	protected GameObject BFS (GameObject goStart, GameObject goTarget){
		Queue<GameObject> goQueue = new Queue<GameObject> ();
		HashSet<GameObject> explored = new HashSet<GameObject> ();
		goQueue.Enqueue (goStart);

		while (goQueue.Count != 0) {
			GameObject goCurrent = goQueue.Dequeue ();
			if (goCurrent == goTarget) {
				return goCurrent;
			}

			IList<GameObject> goNeighbors = GetWalkableNeighbors (goCurrent);

			foreach (GameObject go in goNeighbors) {
				if (!explored.Contains (go)) {
					explored.Add (go);
					goParents.Add (go, goCurrent);
					goQueue.Enqueue (go);
				}
			}
		}
		return goStart;
	}

	protected List<GameObject> GetWalkableNeighbors (GameObject tileGO){
		List<GameObject> goList = new List<GameObject> ();
		foreach (GameObject go in tileGO.GetComponent<Tile> ().neighbors) {
			if (go != null && go.GetComponent<Tile> ().isWalkable) {
				goList.Add (go);
			}
		}
		return goList;
	}

	#region UTILITY FUNCTIONS

	protected void DrawForwardRay () {
		if (showForwardRay) {
			Vector3 fwd = transform.TransformDirection (Vector3.forward) * 10;
			Debug.DrawRay (transform.position, fwd, Color.green);
		}
	}

	protected void SetInitialPosition () {
		tileLocation = FindTileLocation (transform.position);
		tileLocation.GetComponent<Tile> ().isOccupied = true;
		transform.position = new Vector3 (tileLocation.transform.position.x, tileLocation.transform.position.y - 0.5f + 1.25f, tileLocation.transform.position.z);
	}

	protected void SetInitialFacing () {
		if (transform.forward == Vector3.left)
			facingDirection = Facing.North;
		if (transform.forward == Vector3.right)
			facingDirection = Facing.South;
		if (transform.forward == Vector3.back)
			facingDirection = Facing.West;
		if (transform.forward == Vector3.forward)
			facingDirection = Facing.East;
	}

	protected GameObject FindTileLocation (Vector3 location) {
		RaycastHit hit;

		if (Physics.Raycast (location, Vector3.down, out hit, 1f)) {
			return hit.transform.gameObject;
		}
		return null;
	}

	#endregion

	#region MOVEMENT & ROTATION

	protected bool PathClear (Vector3 direction) {
		Vector3 target = transform.position + direction;
		RaycastHit hit;

		if (Physics.Raycast (transform.position, direction, 1f))
			return false;
		if (!Physics.Raycast (target, Vector3.down, 1f))
			return false;
		if (Physics.Raycast (target, Vector3.down, out hit, 1f)) {
			GameObject go = hit.transform.gameObject;
			if (go.GetComponent<Tile> () != null) {
				Tile tile = go.GetComponent<Tile> ();
				if (!tile.isWalkable)
					return false;
				if (tile.isOccupied)
					return false;
			}
		}
		return true;
	}

	protected Vector3 GetDirectionForMove (GameObject goTarget){
		Vector3 direction = goTarget.transform.position - transform.position;
		direction.y = 0;
		return direction;
	}

	protected Facing GetFacingForMove (GameObject goTarget) {
		Vector3 direction = GetDirectionForMove (goTarget);
		Facing facing = Facing.North;

		if (direction == Vector3.left)
			facing = Facing.North;
		if (direction == Vector3.right)
			facing = Facing.South;
		if (direction == Vector3.back)
			facing = Facing.West;
		if (direction == Vector3.forward)
			facing = Facing.East;

		return facing;
	}

	protected void MoveOrRotate (Facing face, Vector3 direction) {		
		if (facingDirection == face)
			InitiateMove (direction);
		else
			InitiateRotate (face, direction);
	}

	protected void InitiateMove (Vector3 direction){
		Vector3 target = transform.position + direction;
		GameObject targetGO = FindTileLocation (target);
		if (PathClear (direction)) {
			targetGO.GetComponent<Tile> ().isOccupied = true;
			StartCoroutine (ExecuteMove (targetGO));
		}	
	}

	protected void InitiateRotate (Facing face, Vector3 direction){
		facingDirection = face;
		StartCoroutine (ExecuteRotate (direction));
	}

	#endregion

	#region COROUTINES

	protected IEnumerator ExecuteMove (GameObject targetGO) {
		Vector3 target = new Vector3 (targetGO.transform.position.x, tileLocation.transform.position.y - 0.5f + 1.25f, targetGO.transform.position.z);
		isMoving = true;
		while (transform.position != target) {
			transform.position = Vector3.MoveTowards (transform.position, target, cardinalSpeed * Time.deltaTime);
			yield return null;
		}
		tileLocation.GetComponent<Tile> ().isOccupied = false;
		tileLocation = FindTileLocation (transform.position);
		isMoving = false;
	}

	protected IEnumerator ExecuteRotate (Vector3 target) {
		isMoving = true;
		Quaternion targetRotation = Quaternion.LookRotation (target);

		while (transform.rotation != targetRotation) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, rotateSpeed);
			yield return null;
		}

		isMoving = false;
	}

	#endregion
}
