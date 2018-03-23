using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PawnController {

	void Start () {
		SetInitialFacing ();
		SetInitialPosition ();
	}
		
	void Update () {
		DrawForwardRay ();
		MovementInput ();
		RightClick ();
		MoveOnPath ();
	}

	void LateUpdate () {		
		MouseOver ();
	}

	GameObject RayToTile () {
		GameObject go;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 50, ~(1 << 11))) {
			go = hit.transform.gameObject;
			if (go.GetComponent<Tile> () != null) {
				return go;				
			}
		}
		return null;
	}

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
}
