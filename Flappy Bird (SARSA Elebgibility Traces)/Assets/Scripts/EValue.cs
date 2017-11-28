using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class EValue
    {//state the bird is in
        private State state;

        // action to choose
        private Enum.Action action;

        // reward the bird gets when choosing the action
        private double value;

        public EValue(State state, Enum.Action action, float reward)
        {
            this.state = state;
            this.action = action;
            this.value = reward;
        }

        public void setValue(double reward)
        {
            this.value = reward;
        }

        public double getValue()
        {
            return value;
        }

        public Enum.Action getAction()
        {
            return action;
        }

        public State getState()
        {
            return state;
        }
        public override bool Equals(System.Object obj)
        {
            if (obj == null) return false;

            EValue classifier = obj as EValue;

            if (!this.action.Equals(classifier.action)
                || !this.getState().Equals(classifier.getState()))
            {
                return false;
            }
            return true;
        }

    }
}
