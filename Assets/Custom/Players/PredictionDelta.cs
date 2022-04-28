using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionDelta {
	struct Delta {
		public readonly double timeStamp;
		public readonly Vector3 frameDelta;
		public Delta (double timeStamp, Vector3 frameDelta) {
			this.timeStamp = timeStamp;
			this.frameDelta = frameDelta;
		}
	}
	List<Delta> clientSidePredictionDeltas = new List<Delta> ();
	public void AddDelta (Vector3 delta) {
		clientSidePredictionDeltas.Add (new Delta (NetworkTime.time, delta * Time.fixedDeltaTime));
	}
	public Vector3 Reconciliate (double timeStamp, double rtt = 0f) {
		// Remove all deltas from before the latest server update, they will not be needed
		for (int i = 0; i < clientSidePredictionDeltas.Count; i++) {
			if (clientSidePredictionDeltas [0].timeStamp < timeStamp - rtt / 2f) {
				clientSidePredictionDeltas.RemoveAt (0);
			} else {
				break;
			}
		}
		// Sum all remaining deltas to re-predict the player's position
		Vector3 output = new Vector3 ();
		foreach (Delta delta in clientSidePredictionDeltas) {
			output += delta.frameDelta;
		}
		return output;
	}
}