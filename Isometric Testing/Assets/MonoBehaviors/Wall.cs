using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : TerrainObject {
//	GameObject playerGO;

	void Start () {
//		playerGO = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update () {
//		FadeWall ();
	}

//	void FadeWall () {		
//		if (playerGO.transform.position.x < transform.position.x || playerGO.transform.position.z < transform.position.z) {
//			Vector3 d = playerGO.transform.position - transform.position;
//			float distance = Mathf.Sqrt (Mathf.Pow (d.x, 2f) + Mathf.Pow (d.z, 2f));
//
//			if (distance < 5f) {
//				Renderer r = GetComponent<Renderer> ();
//				Color fade = new Color (r.material.color.r, r.material.color.g, r.material.color.b, 100f);
//				r.material.renderQueue = 3000;
//				r.material.color = fade;
//			} else {
//				Renderer r = GetComponent<Renderer> ();
//				Color fade = new Color (r.material.color.r, r.material.color.g, r.material.color.b, 255f);
//				r.material.renderQueue = 2000;
//				r.material.color = fade;
//			}
//
//		}
//	}

}
