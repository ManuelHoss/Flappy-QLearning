using UnityEngine;

public class BirdRL : BirdAI
{

    protected override State DetermineState()
    {
        // use coords.x and coords.y to deduct state
        var coords = LowerPipeTopLeftCoords();

        var xState = (int)Mathf.Round((coords.x - transform.position.x) * 2);
        var yState = (int)Mathf.Round((coords.y - transform.position.y) * 2);

        return new State(xState, yState);
    }

    protected override Action SelectAction(State state)
    {
        // return Action.Nop or Action.Flap depending on the state
        if (qLearningTable.ContainsKey(state))
        {
            //epsilon-greedy action selection
            var epsilon = Random.value;
            if (epsilon < 0.999f)
            {
                Debug.Log("choosing..." + qLearningTable[state][Action.Flap] + " - " + qLearningTable[state][Action.Nop]);
                // If QValue for Flap is higher than for Nap in current state
                if (qLearningTable[state][Action.Flap] > qLearningTable[state][Action.Nop])
                {
                    // Perform Action
                    return Action.Flap;
                }
                // If QValue for Flap is lower than for Nap in current state
                else 
                {
                    return Action.Nop;
                }
            }
            else
            {
                return SelectRandomAction();
            }
        }
        else
        {
            qLearningTable.Add(state, new System.Collections.Generic.Dictionary<Action, double> { { Action.Flap, 0 }, { Action.Nop, 0 } });
            return SelectRandomAction();
        }
    }
}
