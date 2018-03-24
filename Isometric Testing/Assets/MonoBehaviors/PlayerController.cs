using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PawnController {

	public Camera cam;
	Quaternion rotation;

	#region BEHAVIOURS

	void Start () {	
		if (!isLocalPlayer) {
			cam.enabled = false;
		}	
		
	}

	public override void OnStartLocalPlayer () {
//		RandomPlayerColor ();
		SetInitialPosition ();
		InitializeCamera ();
		SetInitialFacing ();
	}
		
	void Update () {
		if (!isLocalPlayer) {
			return;
		}		

		DrawForwardRay ();
		MoveOnPath ();
		MovementInput ();
		RightClick ();
	}

	void LateUpdate () {
		if (!isLocalPlayer)
			return;

		FixCamera ();
		MouseOver ();
	}

	#endregion

	#region UTILITY FUNCTIONS

	void RandomPlayerColor () {
		int random = Random.Range (0, 10);
		Color c;
		if (random == 0)
			c = Color.red;
		else if (random == 1)
			c = Color.blue;
		else if (random == 2)
			c = Color.cyan;
		else if (random == 3)
			c = Color.green;
		else if (random == 4)
			c = Color.magenta;
		else if (random == 5)
			c = Color.yellow;
		else if (random == 6)
			c = Color.white;
		else if (random == 7)
			c = Color.grey;
		else
			c = Color.black;

		transform.GetComponent<Renderer> ().material.color = c;
	}

	void InitializeCamera () {
		rotation = cam.transform.rotation;
		cam.transform.position = transform.position - 30 * cam.transform.forward;
	}

	void FixCamera () {
		cam.transform.rotation = rotation;
		cam.transform.position = transform.position - 30 * cam.transform.forward;
	}

	GameObject RayToTile () {
		GameObject go;
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 50, ~(1 << 11))) {
			go = hit.transform.gameObject;
			if (go.GetComponent<Tile> () != null) {
				return go;				
			}
		}
		return null;
	}

	#endregion

	#region INPUTS

	void MovementInput ()
	{
		if (!isMoving && Input.GetKey ("w") && !Input.GetKey ("s") && !Input.GetKey ("a") && !Input.GetKey ("d"))
			MoveOrRotate (Facing.North, Vector3.left);
		if (!isMoving && Input.GetKey ("s") && !Input.GetKey ("w") && !Input.GetKey ("a") && !Input.GetKey ("d"))
			MoveOrRotate (Facing.South, Vector3.right);
		if (!isMoving && Input.GetKey ("a") && !Input.GetKey ("d") && !Input.GetKey ("w") && !Input.GetKey ("s"))
			MoveOrRotate (Facing.West, Vector3.back);
		if (!isMoving && Input.GetKey ("d") && !Input.GetKey ("a") && !Input.GetKey ("w") && !Input.GetKey ("s"))
			MoveOrRotate (Facing.East, Vector3.forward);
	}

	void MouseOver () {
		if (RayToTile () != null) {
			RayToTile ().GetComponent<Tile> ().AddHighlight ();
		}
	}

	void RightClick (){
		if (Input.GetMouseButton (1)) {
			if (RayToTile () != null) {
				if (RayToTile ().GetComponent<Tile> ().isWalkable) {
					tileTarget = RayToTile ();
				}
			}
		}
	}

	#endregion

}
