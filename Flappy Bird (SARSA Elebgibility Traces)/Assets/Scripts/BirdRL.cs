using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BirdRL : BirdAI {

	protected override State DetermineState() {
        
		// use coords.x and coords.y to deduct state
		var coords = LowerPipeTopLeftCoords();
		var xState = (int) Mathf.Round((coords.x - transform.position.x) *1.7f);
		var yState = (int) Mathf.Round((coords.y - transform.position.y) *1.7f);
        return new State(xState, yState);
    }

    public int getYDistance() {
        var coords = LowerPipeTopLeftCoords();
        return (int)Mathf.Round(Math.Abs(((coords.y - transform.position.y) * 2f)));
    }

    public override float getRewardWithYDistance(bool isDead)
    {
        if (isDead)
        {
            return -1000 - getYDistance();
        }
        return (1 - getYDistance());
    }
}
