using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawn : NetworkBehaviour {
	public GameObject monsterPrefab;

	void Start () {
		if (isServer) {
			GameObject go = (GameObject)Instantiate (monsterPrefab);
			NetworkServer.Spawn (go);
		}
	}
}
