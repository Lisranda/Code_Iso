using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {
	GameObject player;
	Vector3 initialPosition;
	Quaternion rotation;
	Vector3 position;

	void Start () {		
		rotation = transform.rotation;
		player = transform.parent.gameObject;
		transform.position = player.transform.position - 30 * transform.forward;
	}

	void Update () {		
	}

	void LateUpdate () {
		transform.rotation = rotation;
		transform.position = player.transform.position - 30 * transform.forward;		
	}
}
