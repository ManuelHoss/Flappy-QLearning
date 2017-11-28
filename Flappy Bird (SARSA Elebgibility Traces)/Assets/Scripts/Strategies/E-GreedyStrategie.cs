using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Strategies
{
    class E_GreedyStrategie : Strategie
    {
        double delta = 0.1;

        public override QValue getQValueForStrategie(State state, List<QValue> qValues)
        {

            Double epsilon = new Random().NextDouble();
            var states = getSuitableStates(state, qValues).FindAll(c => c.getState().Equals(state)).ToArray();

            if (epsilon <= delta) {
                return states[0].getValue() >= states[1].getValue() ? states[1] : states[0];
            }
            
            return states[0].getValue() >= states[1].getValue() ? states[0] : states[1];
        }

        public override EValue getEValueForStrategie(State state, List<EValue> eValues)
        {

            Double epsilon = new Random().NextDouble();
            var states = getSuitableStates(state, eValues).FindAll(c => c.getState().Equals(state)).ToArray();

            if (epsilon <= delta)
            {
                return states[0].getValue() >= states[1].getValue() ? states[1] : states[0];
            }

            return states[0].getValue() >= states[1].getValue() ? states[0] : states[1];
        }
    }
}
