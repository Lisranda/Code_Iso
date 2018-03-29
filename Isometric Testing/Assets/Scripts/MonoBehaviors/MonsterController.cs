using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterController : NPCController {
	[SerializeField] protected float rangeHostile = 4f;
	[SerializeField] protected GameObject targetHostile;
	public LayerMask hostileTo;
	protected List<GameObject> targetsInRange;

	protected override void Start () {
		base.Start ();
		targetsInRange = new List<GameObject> ();
	}

	protected override void Update () {
		if (!isServer) {
			return;
		}

		base.Update ();
		DetectHostiles ();
		GetKey ();
		MoveOnPath ();
	}

	protected void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, rangeHostile);
	}

	protected void DetectHostiles ()
	{
		targetsInRange.Clear ();

		Collider[] hits = Physics.OverlapSphere (transform.position, rangeHostile, hostileTo);
		if (hits.Length > 0) {
			foreach (Collider c in hits) {
				targetsInRange.Add (c.gameObject);
			}
		}

		if (targetHostile != null) {
			if (!targetsInRange.Contains (targetHostile)) {
				targetHostile = null;
			}
		}

		if (targetsInRange.Count > 0) {
			if (targetHostile == null) {
				targetHostile = targetsInRange [0];
			}
		}
	}

	protected void GetKey () {		

		if (Input.GetKeyDown ("m")) {
			int locX = Random.Range (0, 10);
			int locY = 0;
			int locZ = Random.Range (0, 10);

			Vector3 location = new Vector3 ((float)locX, (float)locY, (float)locZ);
//			Vector3 location = new Vector3 (0f, 0f, 0f);
			Debug.Log (location);
			if (FindTileLocation (location) != null) {
				if (FindTileLocation (location).GetComponent<Tile> ().isWalkable && !FindTileLocation (location).GetComponent<Tile> ().isOccupied) {
					tileTarget = FindTileLocation (location);
				}
			}
		}
	}

}
