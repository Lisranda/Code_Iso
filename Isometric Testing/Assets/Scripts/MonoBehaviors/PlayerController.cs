using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PawnController {

	public Camera cam;
	Quaternion rotation;
	public LayerMask fadeLayers;
	List<GameObject> oldFades;
	float time = 0f;

	#region BEHAVIOURS

	protected override void Start () {	
		base.Start ();
		oldFades = new List<GameObject> ();
		InitializeCamera ();
		SetLocalPlayer ();
	}

	protected override void Update ()	{
		base.Update ();
		SprintEnabled ();
		OccupyTile (tileLocation);
		MovementInput ();
		MoveOnPath ();
		Attack ();

		RightClick ();
//		FindObstruction ();
	}

	protected override void LateUpdate ()	{
		base.LateUpdate ();
		FixCamera ();
		FindObstruction ();
		MouseOver ();
		MovementTarget ();
	}

	protected override void FixedUpdate () {
		
	}

	#endregion

	#region OBJECT FADING

	void FindObstruction () {
		float range = Vector3.Distance (cam.transform.position, tileLocation.transform.position);

		Ray ray = cam.ScreenPointToRay (cam.WorldToScreenPoint (tileLocation.transform.position));
		RaycastHit[] hits = Physics.RaycastAll (ray, range, fadeLayers);

		if (oldFades.Count > 0) {
			time += Time.deltaTime;

			if (time > 0f) {
				for (int i = 0; i < oldFades.Count; i++) {				
					GameObject go = oldFades [i];
					Material m = go.GetComponent<Renderer> ().material;
					ShaderRefresh.SetupMaterialWithBlendMode (m, ShaderRefresh.BlendMode.Opaque);
					m.color = new Color (m.color.r, m.color.g, m.color.b, 1f);
					oldFades.Remove (go);
					i--;
					time = 0f;
				}
			}
		}

		if (hits.Length > 0) {
			foreach (RaycastHit h in hits) {
				Material m = h.transform.gameObject.GetComponent<Renderer> ().material;
				ShaderRefresh.SetupMaterialWithBlendMode (m, ShaderRefresh.BlendMode.Transparent);
				m.color = new Color (m.color.r, m.color.g, m.color.b, .50f);
				oldFades.Add (h.transform.gameObject);				
			}
		}
	}

	#endregion

	#region UTILITY FUNCTIONS

	void SetLocalPlayer () {
		transform.tag = "Local Player";
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
		if (Input.GetKey ("w") && !Input.GetKey ("s") && !Input.GetKey ("a") && !Input.GetKey ("d") && !Input.GetKey ("q")) {
			MoveOrRotate (Facing.North, Vector3.left);
			tileMoveTarget = null;
		}
		if (Input.GetKey ("s") && !Input.GetKey ("w") && !Input.GetKey ("a") && !Input.GetKey ("d") && !Input.GetKey ("q")) {
			MoveOrRotate (Facing.South, Vector3.right);
			tileMoveTarget = null;
		}
		if (Input.GetKey ("a") && !Input.GetKey ("d") && !Input.GetKey ("w") && !Input.GetKey ("s") && !Input.GetKey ("q")) {
			MoveOrRotate (Facing.West, Vector3.back);
			tileMoveTarget = null;
		}
		if (Input.GetKey ("d") && !Input.GetKey ("a") && !Input.GetKey ("w") && !Input.GetKey ("s") && !Input.GetKey ("q")) {
			MoveOrRotate (Facing.East, Vector3.forward);
			tileMoveTarget = null;
		}
	}

	void SprintEnabled () {
		if (Input.GetKey ("left shift"))
			isSprinting = true;
		else
			isSprinting = false;
	}

	void Attack () {
		if (Input.GetKey ("q") && !IsAttacking ()) {
			gameObject.GetComponent<Animator> ().SetBool ("AttackBool", true);
			AnimateAttack ();
			tileMoveTarget = null;
			Debug.Log ("Attacking");
//			RayForward ();
		}
	}

	void MouseOver () {
		if (RayToTile () != null) {
			RayToTile ().GetComponent<Tile> ().AddHighlight ();
		}
	}

	void MovementTarget() {
		if (tileMoveTarget != null) {
			tileMoveTarget.GetComponent<Tile> ().AddMovementTarget ();
		}
	}

	void RightClick (){
		if (Input.GetMouseButton (1)) {
			if (RayToTile () != null) {
				if (RayToTile ().GetComponent<Tile> ().isWalkable && !RayToTile ().GetComponent<Tile> ().isOccupied) {
					tileMoveTarget = RayToTile ();
				}
			}
		}
	}

	#endregion

}
