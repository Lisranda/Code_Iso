using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObject : MonoBehaviour {
	public bool obstructsMovement = true;
	public enum Facing { North, East, South, West }
	public Facing facingDirection;
	GameObject player;

	void Start () {
		SetFacing ();
	}

	void Update () {
//		SetLocalPlayer ();
//		FadeWall ();
	}

	protected void SetLocalPlayer () {
		if (player == null)
			player = GameObject.FindGameObjectWithTag ("Local Player");
	}

	protected void SetFacing () {
		if (transform.forward == Vector3.left)
			facingDirection = Facing.North;
		if (transform.forward == Vector3.right)
			facingDirection = Facing.South;
		if (transform.forward == Vector3.back)
			facingDirection = Facing.West;
		if (transform.forward == Vector3.forward)
			facingDirection = Facing.East;
	}

	protected void FadeWall () {
		if (player == null)
			return;
		
		if (facingDirection == Facing.East || facingDirection == Facing.West) {
			if (player.transform.position.z < transform.position.z) {				
				
			}
		}

		if (facingDirection == Facing.South || facingDirection == Facing.North) {
			if (player.transform.position.x < transform.position.x) {
				
			}
		}
	}
}
