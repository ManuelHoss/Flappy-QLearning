using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public struct State {

    
	public readonly int vDist, hDist;

	public State(int hDist, int vDist) {
		this.hDist = hDist;
		this.vDist = vDist;
	}

	public override bool Equals(object obj)
	{
		if (obj == null) { return false; }

		if (obj.GetType() == typeof(State)) {
			var otherState = (State) obj;
			return otherState.hDist == this.hDist
				&& otherState.vDist == this.vDist;
		}

		return false;
	}

	public override int GetHashCode()
	{
		var hash = 13;
		hash = hash * 7 + hDist.GetHashCode();
		hash = hash * 7 + vDist.GetHashCode();
		return hash;
	}

	public override string ToString ()
	{
		return string.Format("{0},{1}", hDist, vDist);
	}

}
