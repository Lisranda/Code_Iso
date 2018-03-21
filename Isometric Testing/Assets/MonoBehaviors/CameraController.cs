using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	GameObject player;
	Vector3 initialPosition;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		transform.position = player.transform.position - 30 * transform.forward;
	}

	void Update () {
		
	}

	void LateUpdate () {
		transform.position = player.transform.position - 30 * transform.forward;
	}
}
