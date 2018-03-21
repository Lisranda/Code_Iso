using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	float playerCardinalSpeed = 5f;
	float playerRotateSpeed = 3f;
	bool isMoving = false;
	public enum Facing {North, East, South, West};
	public Facing facingDirection = new Facing ();
	public GameObject tileLocation;

	void Start () {
		SetInitialFacing ();
		SetInitialPosition ();
	}
		
	void Update () {
//		DrawForwardRay ();
		MovementInput ();
	}

	void DrawForwardRay () {
		Vector3 fwd = transform.TransformDirection (Vector3.forward) * 10;
		Debug.DrawRay (transform.position, fwd, Color.green);
	}

	GameObject FindTileLocation (Vector3 location) {
		RaycastHit hit;

		if (Physics.Raycast (location, Vector3.down, out hit, 1f)) {
			return hit.transform.gameObject;
		}
		return null;
	}

	IEnumerator MoveToward (GameObject targetGO) {
		Vector3 target = new Vector3 (targetGO.transform.position.x, 1.25f, targetGO.transform.position.z);
		isMoving = true;
		while (transform.position != target) {
			transform.position = Vector3.MoveTowards (transform.position, target, playerCardinalSpeed * Time.deltaTime);
			yield return null;
		}
		tileLocation.GetComponent<TileData> ().isOccupied = false;
		tileLocation = FindTileLocation (transform.position);
		isMoving = false;
	}

	IEnumerator RotatePlayer (Vector3 target) {
		isMoving = true;
		Quaternion targetRotation = Quaternion.LookRotation (target);

		while (transform.rotation != targetRotation) {
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, playerRotateSpeed);
			yield return null;
		}

		isMoving = false;
	}

	void SetInitialFacing () {
		if (transform.forward == Vector3.left)
			facingDirection = Facing.North;
		if (transform.forward == Vector3.right)
			facingDirection = Facing.South;
		if (transform.forward == Vector3.back)
			facingDirection = Facing.West;
		if (transform.forward == Vector3.forward)
			facingDirection = Facing.East;
	}

	void SetInitialPosition () {
		tileLocation = FindTileLocation (transform.position);
		tileLocation.GetComponent<TileData> ().isOccupied = true;
		transform.position = new Vector3 (tileLocation.transform.position.x, 1.25f, tileLocation.transform.position.z);
	}

	bool PathClear (Vector3 direction) {
		Vector3 target = transform.position + direction;
		RaycastHit hit;

		if (Physics.Raycast (transform.position, direction, 1f))
			return false;
		if (!Physics.Raycast (target, Vector3.down, 1f))
			return false;

		if (Physics.Raycast (target, Vector3.down, out hit, 1f)) {
			GameObject go = hit.transform.gameObject;
			TileData tileData = go.GetComponent<TileData> ();

			if (!tileData.isWalkable)
				return false;
			if (tileData.isOccupied)
				return false;
		}
		return true;
	}

	void MovementInput ()
	{
		if (!isMoving && Input.GetKey ("w") && !Input.GetKey ("s") && !Input.GetKey ("a") && !Input.GetKey ("d")) {
			MovePlayer (Facing.North, Vector3.left);
		}
		if (!isMoving && Input.GetKey ("s") && !Input.GetKey ("w") && !Input.GetKey ("a") && !Input.GetKey ("d")) {
			MovePlayer (Facing.South, Vector3.right);
		}
		if (!isMoving && Input.GetKey ("a") && !Input.GetKey ("d") && !Input.GetKey ("w") && !Input.GetKey ("s")) {
			MovePlayer (Facing.West, Vector3.back);
		}
		if (!isMoving && Input.GetKey ("d") && !Input.GetKey ("a") && !Input.GetKey ("w") && !Input.GetKey ("s")) {
			MovePlayer (Facing.East, Vector3.forward);
		}
	}

	void MovePlayer (Facing face, Vector3 direction) {		
		Vector3 target = transform.position + direction;
		GameObject targetGO = FindTileLocation (target);
		if (facingDirection == face) {	
			if (PathClear (direction)) {
				targetGO.GetComponent<TileData> ().isOccupied = true;
				StartCoroutine (MoveToward (targetGO));
			}				
		} else {
			facingDirection = face;
			StartCoroutine (RotatePlayer (direction));
		}
	}
}
