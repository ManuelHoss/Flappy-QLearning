using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.LearningMethods
{
    static class SARSA_LAMBDA
    {
        public const double gamma = 1.0;
        public const double learningRate = 0.7;
        public const double lambda = 0.9;

        public static QValue updateTable(QValue firstQValue, QValue secondQValue, float reward)
        {
            firstQValue.setValue(firstQValue.getValue() + learningRate * (reward + gamma * secondQValue.getValue() - firstQValue.getValue()));
            return firstQValue;
        }

        public static List<QValue> updateQTable(State currentState, Enum.Action actionOne, State nextState, Enum.Action actionTwo, float reward, List<QValue> qValues, List<EValue> eValues)
        {
            var firstQValue = getQValueEntry(currentState, actionOne, qValues);
            var eValue = getEValueEntry(currentState, actionOne, eValues);
            var secondQValue = getQValueEntry(nextState, actionTwo, qValues);
            firstQValue.setValue(firstQValue.getValue() + learningRate * (reward + gamma * secondQValue.getValue() - firstQValue.getValue()) * eValue.getValue());
            return qValues;
        }

	    public static List<EValue> updateETable(State currentState, Enum.Action actionOne, State nextState, Enum.Action actionTwo, List<EValue> eValues)
        {
            var eValue = getEValueEntry(currentState, actionOne, eValues);
            
            if (currentState.Equals(nextState) && actionOne == actionTwo)
            {
                eValue.setValue((gamma * lambda * eValue.getValue()) + 1);
            }
            else
            {
                eValue.setValue(gamma * lambda * eValue.getValue());
            }
            return eValues;
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

        public static EValue getEValueEntry(State state, Assets.Scripts.Enum.Action action, List<EValue> eValues)
        {
            foreach (EValue value in eValues)
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
