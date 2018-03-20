using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	float playerCardinalSpeed = 5f;
	float playerDiagonalSpeed;
	bool isMoving = false;

	void Start () {
		playerDiagonalSpeed = playerCardinalSpeed / 2;
	}
		
	void Update () {
		CardinalMovement ();
//		DiagonalMovement ();
	}

	IEnumerator MoveToward (Vector3 target) {
		isMoving = true;
		while (transform.position != target) {
			transform.position = Vector3.MoveTowards (transform.position, target, playerCardinalSpeed * Time.deltaTime);
			yield return null;
		}
		isMoving = false;
	}

	bool PathClear (float dX, float dY, float dZ) {
		//This method will determine if the space in front is clear or obstructed (Raycast?)
		//It will then determine of the tile is walkable (also Raycast?)
		Vector3 direction = new Vector3 (dX, dY, dZ);
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

	void CardinalMovement ()
	{
		if (!isMoving && PathClear (-1f, 0f, 0f) && Input.GetKey ("w") && !Input.GetKey ("s") && !Input.GetKey ("a") && !Input.GetKey ("d")) {
			Vector3 target = new Vector3 (transform.position.x - 1f, transform.position.y, transform.position.z);
			StartCoroutine (MoveToward (target));
		}
		if (!isMoving && PathClear (1f, 0f, 0f) && Input.GetKey ("s") && !Input.GetKey ("w") && !Input.GetKey ("a") && !Input.GetKey ("d")) {
			Vector3 target = new Vector3 (transform.position.x + 1f, transform.position.y, transform.position.z);
			StartCoroutine (MoveToward (target));
		}
		if (!isMoving && PathClear (0f, 0f, -1f) && Input.GetKey ("a") && !Input.GetKey ("d") && !Input.GetKey ("w") && !Input.GetKey ("s")) {
			Vector3 target = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 1f);
			StartCoroutine (MoveToward (target));
		}
		if (!isMoving && PathClear (0f, 0f, 1f) && Input.GetKey ("d") && !Input.GetKey ("a") && !Input.GetKey ("w") && !Input.GetKey ("s")) {
			Vector3 target = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 1f);
			StartCoroutine (MoveToward (target));
		}
	}

	void DiagonalMovement () {
		if (Input.GetKey ("w") && Input.GetKey ("a") && !Input.GetKey ("s") && !Input.GetKey ("d")) {
			transform.Translate (-playerDiagonalSpeed * Time.deltaTime, 0, -playerDiagonalSpeed * Time.deltaTime);
		}else if (Input.GetKey ("w") && Input.GetKey ("d") && !Input.GetKey ("s") && !Input.GetKey ("a")) {
			transform.Translate (-playerDiagonalSpeed * Time.deltaTime, 0, playerDiagonalSpeed * Time.deltaTime);
		}

		if (Input.GetKey ("s") && Input.GetKey ("a") && !Input.GetKey ("w") && !Input.GetKey ("d")) {
			transform.Translate (playerDiagonalSpeed * Time.deltaTime, 0, -playerDiagonalSpeed * Time.deltaTime);
		}else if (Input.GetKey ("s") && Input.GetKey ("d") && !Input.GetKey ("w") && !Input.GetKey ("a")) {
			transform.Translate (playerDiagonalSpeed * Time.deltaTime, 0, playerDiagonalSpeed * Time.deltaTime);
		}
	}
}
