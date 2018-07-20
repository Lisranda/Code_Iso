using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour {
	[SerializeField] protected float cardinalSpeed;
	[SerializeField] protected float rotateSpeed = 720f;
	[SerializeField] protected bool isMoving = false;
	[SerializeField] protected bool isSprinting = false;
	[SerializeField] protected bool isAttacking = false;
	protected enum Facing {North, East, South, West};
	[SerializeField] protected Facing facingDirection = new Facing ();
	[SerializeField] protected GameObject tileLocation;
	[SerializeField] protected GameObject tileMoveTarget;

	protected virtual void Start () {
		InitializePawn ();
	}

	protected virtual void Update () {
		SetSpeed ();
		OccupyTile (tileLocation);
	}

	protected virtual void LateUpdate () {
		AnimateMoving ();
	}

	protected virtual void FixedUpdate () {
	}

	#region BFS PATHFINDING

	protected void MoveOnPath () {
		//Remove our move target if we are already on it.
		if (tileLocation == tileMoveTarget)
			tileMoveTarget = null;

		if (tileLocation != null && tileMoveTarget != null && !isMoving) {
			List<GameObject> path = Pathfinding.CalculatePathBF (tileLocation, tileMoveTarget);

			if (path.Count == 0) {
				tileMoveTarget = null;
				return;
			}				
				
//			if (path [path.Count - 1] == tileLocation) {
//				tileTarget = null;
//				return;
//				path.Clear ();
//			}

			Vector3 direction = PathfindingGetDirectionForMove (path [path.Count - 1]);
			Facing face = PathfindingGetFacingForMove (path [path.Count - 1]);
			MoveOrRotate (face, direction);			
		} 
	}

	#endregion

	#region COMMANDS

	protected void MakeReserved (GameObject tile) {
		tile.GetComponent<Tile> ().isReserved = true;
	}

	protected void ReleaseReserved (GameObject tile) {
		tile.GetComponent<Tile> ().isReserved = false;
	}

	protected void OccupyTile (GameObject tile) {
		tile.GetComponent<Tile> ().isOccupied = true;
	}

	#endregion

	#region INITIALIZE PAWNS

	protected void InitializePawn () {
		InitializeSpeed ();
		InitializeTileLocation ();
		InitializeFacing ();
	}

	protected void InitializeSpeed () {
		cardinalSpeed = gameObject.GetComponent<CreatureStats> ().GetSpeedNormal ();
	}

	protected void InitializeTileLocation () {
		tileLocation = FindTileLocation (transform.position);
	}

	protected void InitializeFacing () {
		if (transform.forward == Vector3.left)
			facingDirection = Facing.North;
		if (transform.forward == Vector3.right)
			facingDirection = Facing.South;
		if (transform.forward == Vector3.back)
			facingDirection = Facing.West;
		if (transform.forward == Vector3.forward)
			facingDirection = Facing.East;
	}

	#endregion

	#region UTILITY FUNCTIONS

	protected GameObject FindTileLocation (Vector3 location) {
		RaycastHit hit;
		Vector3 mod = new Vector3 (0f, 0.5f, 0f);

		if (Physics.Raycast (location + mod, Vector3.down, out hit, 1f)) {
			return hit.transform.gameObject;
		}
		return null;
	}

	public Vector3 GetForwardVector3 () {
		if (facingDirection == Facing.North)
			return Vector3.left;
		if (facingDirection == Facing.South)
			return Vector3.right;
		if (facingDirection == Facing.West)
			return Vector3.back;
		if (facingDirection == Facing.East)
			return Vector3.forward;
		return Vector3.zero;
	}

	#endregion

	#region MOVEMENT HELPERS

	protected Vector3 PathfindingGetDirectionForMove (GameObject goTarget){
		Vector3 direction = goTarget.transform.position - transform.position;
		direction.y = 0;
		return direction;
	}

	protected Facing PathfindingGetFacingForMove (GameObject goTarget) {
		Vector3 direction = PathfindingGetDirectionForMove (goTarget);
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

	protected int GetNeighborArrayRef (Vector3 direction) {
		int arrayRef = 0;
		if (direction == Vector3.left)
			arrayRef = 0;
		if (direction == Vector3.forward)
			arrayRef = 1;
		if (direction == Vector3.right)
			arrayRef = 2;
		if (direction == Vector3.back)
			arrayRef = 3;

		return arrayRef;
	}

	#endregion

	#region MOVEMENT & ROTATION

	protected void SetSpeed () {
		if (isSprinting)
			cardinalSpeed = gameObject.GetComponent<CreatureStats> ().GetSpeedSprint ();
		else
			cardinalSpeed = gameObject.GetComponent<CreatureStats> ().GetSpeedNormal ();
	}

	protected bool PathClear (GameObject targetGO) {
		if (targetGO == null)
			return false;

		Tile targetTile;
		targetTile = targetGO.GetComponent<Tile> ();

		if (targetTile == null)
			return false;
		if (!targetTile.isWalkable)
			return false;
		if (targetTile.isOccupied)
			return false;

		return true;
	}

	protected bool IsAttacking () {		
		return gameObject.GetComponent<Animator> ().GetBool ("AttackBool");
	}

	protected void MoveOrRotate (Facing face, Vector3 direction) {
		if (isMoving || IsAttacking ())
			return;

		int arrayRef = GetNeighborArrayRef (direction);
		GameObject targetGO = tileLocation.GetComponent<Tile> ().neighbors [arrayRef];

		if (facingDirection == face)
			InitiateMove (targetGO);
		else
			InitiateRotate (face, direction);
	}

	protected void InitiateMove (GameObject targetGO){
		if (PathClear (targetGO)) {
			MakeReserved (targetGO);
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
		isMoving = true;
		Vector3 target = new Vector3 (targetGO.transform.position.x, tileLocation.transform.position.y, targetGO.transform.position.z);
		while (transform.position != target) {
			transform.position = Vector3.MoveTowards (transform.position, target, cardinalSpeed * Time.deltaTime);
			yield return null;
		}
		tileLocation = FindTileLocation (transform.position);
		ReleaseReserved (tileLocation);
		yield return new WaitForEndOfFrame ();
		isMoving = false;
	}

	protected void AnimateMoving () {
		if (isMoving && isSprinting)
			gameObject.GetComponent<Animator> ().SetFloat ("Speed", 1f);
		else if (isMoving)
			gameObject.GetComponent<Animator> ().SetFloat ("Speed", 0.25f);
		else
			gameObject.GetComponent<Animator> ().SetFloat ("Speed", 0f);
	}

	protected void AnimateAttack () {
		gameObject.GetComponent<Animator> ().SetTrigger ("Attack");
	}

	protected IEnumerator ExecuteRotate (Vector3 targetDirection) {
		isMoving = true;
		Quaternion targetRotation = Quaternion.LookRotation (targetDirection);

		while (transform.rotation != targetRotation) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
			yield return null;
		}

		yield return new WaitForEndOfFrame ();
		isMoving = false;
	}

	#endregion
}
