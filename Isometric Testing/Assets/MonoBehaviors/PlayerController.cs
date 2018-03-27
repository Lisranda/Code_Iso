using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : PawnController {

	public Camera cam;
	Quaternion rotation;
	public LayerMask fadeLayers;
	List<GameObject> oldFades;
	float time = 0f;
	public enum BlendMode {	Opaque,	Cutout,	Fade, Transparent }

	#region BEHAVIOURS

	protected override void Start () {	
		base.Start ();
		oldFades = new List<GameObject> ();
		if (!isLocalPlayer) {
			cam.enabled = false;
		}			
	}

	public override void OnStartLocalPlayer () {
		SetInitialPosition ();
		InitializeCamera ();
//		SetInitialFacing ();
		SetLocalPlayer ();
	}
		
	protected override void Update () {
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
		MovementTarget ();
	}

	#endregion

	#region OBJECT FADING

	void FindObstruction () {
		float range = Vector3.Distance (cam.transform.position, transform.position);

		Ray ray = cam.ScreenPointToRay (cam.WorldToScreenPoint (transform.position));
		RaycastHit[] hits = Physics.RaycastAll (ray, range, fadeLayers);

		if (oldFades.Count > 0) {
			time += Time.deltaTime;

			if (time > 1f) {
				for (int i = 0; i < oldFades.Count; i++) {				
					GameObject go = oldFades [i];
					Material m = go.GetComponent<Renderer> ().material;
					SetupMaterialWithBlendMode (m, BlendMode.Opaque);
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
				SetupMaterialWithBlendMode (m, BlendMode.Fade);
				m.color = new Color (m.color.r, m.color.g, m.color.b, .50f);
				oldFades.Add (h.transform.gameObject);				
			}
		}
	}

	public void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
	{
		switch (blendMode)
		{
		case BlendMode.Opaque:
			material.SetOverrideTag("RenderType", "");
			material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = -1;
			break;
		case BlendMode.Cutout:
			material.SetOverrideTag("RenderType", "TransparentCutout");
			material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			material.SetInt("_ZWrite", 1);
			material.EnableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 2450;
			break;
		case BlendMode.Fade:
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.EnableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
			break;
		case BlendMode.Transparent:
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
			break;
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
		if (Input.GetKey ("w") && !Input.GetKey ("s") && !Input.GetKey ("a") && !Input.GetKey ("d")) {
			MoveOrRotate (Facing.North, Vector3.left);
			tileTarget = null;
		}
		if (Input.GetKey ("s") && !Input.GetKey ("w") && !Input.GetKey ("a") && !Input.GetKey ("d")) {
			MoveOrRotate (Facing.South, Vector3.right);
			tileTarget = null;
		}
		if (Input.GetKey ("a") && !Input.GetKey ("d") && !Input.GetKey ("w") && !Input.GetKey ("s")) {
			MoveOrRotate (Facing.West, Vector3.back);
			tileTarget = null;
		}
		if (Input.GetKey ("d") && !Input.GetKey ("a") && !Input.GetKey ("w") && !Input.GetKey ("s")) {
			MoveOrRotate (Facing.East, Vector3.forward);
			tileTarget = null;
		}
	}

	void MouseOver () {
		if (RayToTile () != null) {
			RayToTile ().GetComponent<Tile> ().AddHighlight ();
		}
	}

	void MovementTarget() {
		if (tileTarget != null) {
			tileTarget.GetComponent<Tile> ().AddMovementTarget ();
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
