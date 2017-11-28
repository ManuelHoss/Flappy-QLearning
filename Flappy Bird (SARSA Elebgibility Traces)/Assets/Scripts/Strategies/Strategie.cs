using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Strategies
{
    public abstract class Strategie
    {
        public abstract QValue getQValueForStrategie(State state, List<QValue> qValues);

        public abstract EValue getEValueForStrategie(State state, List<EValue> eValues);

        public List<QValue> getSuitableStates(State state, List<QValue> qValues)
        {
            return qValues.Where(i => i.getState().Equals(state)).ToList<QValue>();
        }

        public List<EValue> getSuitableStates(State state, List<EValue> eValues)
        {
            return eValues.Where(i => i.getState().Equals(state)).ToList<EValue>();
        }

        public QValue getQValue(State state, Enum.Action action, List<QValue> qValues)
        {
            return qValues.Where(i => i.getState().Equals(state) && i.getAction().Equals(action)).FirstOrDefault();
        }

        public EValue getEValue(State state, Enum.Action action, List<EValue> eValues)
        {
            return eValues.Where(i => i.getState().Equals(state) && i.getAction().Equals(action)).FirstOrDefault();
        }
    }
}
