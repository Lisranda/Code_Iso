using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : PawnController {

	public Camera cam;
	Quaternion rotation;
	public LayerMask fadeLayers;
	List<GameObject> oldFades;

	#region BEHAVIOURS

	void Start () {	
		oldFades = new List<GameObject> ();
		if (!isLocalPlayer) {
			cam.enabled = false;
		}			
	}

	public override void OnStartLocalPlayer () {
//		RandomPlayerColor ();
		SetInitialPosition ();
		InitializeCamera ();
		SetInitialFacing ();
		SetLocalPlayer ();
	}
		
	public override void Update () {
		if (!isLocalPlayer) {
			return;
		}		
		base.Update ();
		DrawForwardRay ();
		MoveOnPath ();
		MovementInput ();
		RightClick ();
		FindObstruction ();
	}

	void LateUpdate () {
		if (!isLocalPlayer)
			return;

		FixCamera ();
		MouseOver ();
	}

	#endregion

	void FindObstruction () {
		Ray ray = cam.ScreenPointToRay (cam.WorldToScreenPoint(transform.position));
		RaycastHit[] hits = Physics.RaycastAll (ray, 50f, fadeLayers);
		Color op;
		Color fade;

		if (oldFades.Count > 0) {
			foreach (GameObject go in oldFades) {
				// Return previously faded things to normal.
				oldFades.Clear ();
			}
		}

		if (hits.Length > 0) {
			foreach (RaycastHit h in hits) {
				Renderer r = h.transform.gameObject.GetComponent<Renderer> ();
				r.material.SetFloat ("_Mode", 2f);
				r.material.color = new Color (r.material.color.r, r.material.color.g, r.material.color.b, .50f);
				r.material.EnableKeyword ("_Color");
			}
		}



	}

	#region UTILITY FUNCTIONS

	void SetLocalPlayer () {
		transform.tag = "Local Player";
	}

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
				if (RayToTile ().GetComponent<Tile> ().isWalkable && !RayToTile ().GetComponent<Tile> ().isOccupied) {
					tileTarget = RayToTile ();
				}
			}
		}
	}

	#endregion

}
