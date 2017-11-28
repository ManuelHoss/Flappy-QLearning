using Assets.Scripts.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.LearningMethods
{
    class QLearning
    {

        public const double gamma = 1.0;
        public const double learningRate = 0.7;

        /**
        * Selects the best classifier containing the best action for Situation
        */
        public static QValue SelectClassifierWithBestActionFromQTable(State state, List<QValue> qTable)
        {
            var states = qTable.FindAll(c => c.getState().Equals(state)).ToArray();
            QValue classifier = states[0].getValue() >= states[1].getValue() ? states[0] : states[1];
            return classifier;
        }

        public static List<QValue> updateTable(QValue lastClassifier, QValue currentClassifier, List<QValue> qTable, float reward)
        {
            foreach (QValue classifierToBeUpdated in qTable)
            {
                if (classifierToBeUpdated.Equals(lastClassifier))
                {
                    double newValue = classifierToBeUpdated.getValue() +
                        learningRate * (reward + gamma * SelectClassifierWithBestActionFromQTable(currentClassifier.getState(),qTable).getValue() - classifierToBeUpdated.getValue());
                    classifierToBeUpdated.setValue(newValue);
                }
            }

            return qTable;
        }

    }
}
