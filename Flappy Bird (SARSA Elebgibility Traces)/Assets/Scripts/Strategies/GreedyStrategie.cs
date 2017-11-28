using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Strategies
{
    class GreedyStrategie : Strategie
    {
        public GreedyStrategie() { }

        public override QValue getQValueForStrategie(State state, List<QValue> qValues)
        {
            var states = getSuitableStates(state, qValues).FindAll(c => c.getState().Equals(state)).ToArray();
            return states[0].getValue() >= states[1].getValue() ? states[0] : states[1];
        }

        public override EValue getEValueForStrategie(State state, List<EValue> eValues)
        {
            var states = getSuitableStates(state, eValues).FindAll(c => c.getState().Equals(state)).ToArray();
            return states[0].getValue() >= states[1].getValue() ? states[0] : states[1];
        }
    }
}
