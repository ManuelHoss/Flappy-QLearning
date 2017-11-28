using System;


namespace Assets.Scripts
{
    /*
    *   QValue is a triple containing 
        status the bird is in
        action the bird chooses
        the reward bird gets for choosing the described action 
    * */
    [System.Serializable]
    public class QValue : System.Object
    {

        //state the bird is in
        private State state;

        // action to choose
        public Enum.Action action;

        // reward the bird gets when choosing the action
        private double value;

        public QValue(State state, Enum.Action action, float reward)
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

            QValue classifier = obj as QValue;

            if (!this.action.Equals(classifier.action)
                || !this.getState().Equals(classifier.getState()))
            {
                return false;
            }
            return true;
        }

    }
}
