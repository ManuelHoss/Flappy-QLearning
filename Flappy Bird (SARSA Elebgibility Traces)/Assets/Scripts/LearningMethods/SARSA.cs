using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.LearningMethods
{
    static class SARSA
    {
        public const double gamma = 1.0;
        public const double learningRate = 0.7;

        public static QValue updateTable(QValue firstQValue, QValue secondQValue, float reward)
        {
            firstQValue.setValue(firstQValue.getValue() + learningRate * (reward + gamma * secondQValue.getValue() - firstQValue.getValue()));
            return firstQValue;
        }

        public static List<QValue> updateTable(State currentState, Enum.Action actionOne, State nextState, Enum.Action actionTwo, float reward, List<QValue> qValues)
        {
            var firstQValue = getQValueEntry(currentState, actionOne, qValues);
            var secondQValue = getQValueEntry(nextState, actionTwo, qValues);
            firstQValue.setValue(firstQValue.getValue() + learningRate * (reward + gamma * secondQValue.getValue() - firstQValue.getValue()));
            return qValues;
        }

        public static QValue getQValueEntry(State state, Assets.Scripts.Enum.Action action, List<QValue> qValues)
        {
            foreach (QValue value in qValues)
            {
                if (value.getState().Equals(state) && value.getAction().Equals(action))
                {
                    return value;
                }
            }
            return null;
        }
    }
}
