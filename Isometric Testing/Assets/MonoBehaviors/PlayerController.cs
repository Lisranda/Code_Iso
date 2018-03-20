using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	float playerCardinalSpeed = 5f;
	float playerRotateSpeed = 3f;
	bool isMoving = false;
	public enum Facing {North, East, South, West};
	public Facing facingDirection = new Facing ();

	void Start () {
		SetInitialFacing ();
	}
		
	void Update () {
//		DrawForwardRay ();
		MovementInput ();
	}

	void DrawForwardRay () {
		Vector3 fwd = transform.TransformDirection (Vector3.forward) * 10;
		Debug.DrawRay (transform.position, fwd, Color.green);
	}

	IEnumerator MoveToward (Vector3 target) {
		isMoving = true;
		while (transform.position != target) {
			transform.position = Vector3.MoveTowards (transform.position, target, playerCardinalSpeed * Time.deltaTime);
			yield return null;
		}
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

	bool PathClear (Vector3 direction) {
		Vector3 target = transform.position + direction;
		RaycastHit hit;

		if (Physics.Raycast (transform.position, direction, 1f)) {
			return false;
		} else if (!Physics.Raycast (target, Vector3.down, 1f)) {
			return false;
		} else if (Physics.Raycast (target, Vector3.down, out hit, 1f)) {
			GameObject go = hit.transform.gameObject;
			TileData tileData = go.GetComponent<TileData> ();

			if (!tileData.isWalkable) {
				return false;
			}
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
		if (facingDirection == face) {	
			if (PathClear (direction))
				StartCoroutine (MoveToward (target));
		} else {
			facingDirection = face;
			StartCoroutine (RotatePlayer (direction));
		}
	}
}
