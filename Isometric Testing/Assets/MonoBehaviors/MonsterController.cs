using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterController : NPCController {

	protected override void Update () {
		base.Update ();
		GetKey ();
		MoveOnPath ();
	}

	protected void GetKey () {		

		if (Input.GetKeyDown ("m")) {
			int locX = Random.Range (0, 10);
			int locY = 0;
			int locZ = Random.Range (0, 10);

			Vector3 location = new Vector3 ((float)locX, (float)locY, (float)locZ);
			Debug.Log (location);
			if (FindTileLocation (location) != null) {
				if (FindTileLocation (location).GetComponent<Tile> ().isWalkable && !FindTileLocation (location).GetComponent<Tile> ().isOccupied) {
					tileTarget = FindTileLocation (location);
				}
			}
		}
	}

}
