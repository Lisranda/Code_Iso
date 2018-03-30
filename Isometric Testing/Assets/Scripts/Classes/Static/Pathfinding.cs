using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding {
	static Dictionary<GameObject,GameObject> goParents;

	public static List<GameObject> CalculatePathBF (GameObject goStart, GameObject goTarget) {
		goParents = new Dictionary<GameObject, GameObject> ();
		List<GameObject> path = new List<GameObject> ();

		GameObject pathTarget = BFS (goStart, goTarget);

		if (pathTarget == null) {
			return path;
		}

		if (pathTarget == goStart) {
//			path.Add (pathTarget);
			return path;
		}

		GameObject pathCurrent = pathTarget;
		while (pathCurrent != goStart) {
			path.Add (pathCurrent);
			pathCurrent = goParents [pathCurrent];
		}

		return path;
	}

	public static GameObject BFS (GameObject goStart, GameObject goTarget){
		Queue<GameObject> goQueue = new Queue<GameObject> ();
		List<GameObject> explored = new List<GameObject> ();
		goQueue.Enqueue (goStart);

		while (goQueue.Count != 0) {
			GameObject goCurrent = goQueue.Dequeue ();
			if (goCurrent == goTarget) {
				return goCurrent;
			}

			List<GameObject> goNeighbors = GetWalkableNeighbors (goCurrent);

			foreach (GameObject go in goNeighbors) {
				if (!explored.Contains (go)) {
					explored.Add (go);
					goParents.Add (go, goCurrent);
					goQueue.Enqueue (go);
				}
			}
		}
		return null;
	}

	public static List<GameObject> GetWalkableNeighbors (GameObject tileGO){
		List<GameObject> goList = new List<GameObject> ();
		foreach (GameObject go in tileGO.GetComponent<Tile> ().neighbors) {
			if (go != null && go.GetComponent<Tile> ().isWalkable && !go.GetComponent<Tile> ().isOccupied) {
				goList.Add (go);
			}
		}
		return goList;
	}
}
