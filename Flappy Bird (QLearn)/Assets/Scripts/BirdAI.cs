using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BirdMovement))]
public abstract class BirdAI : MonoBehaviour
{

    protected BirdMovement birdMvmt;
    private GameManager gameMgr;

    //Variables for QLearning formula
    private double alpha = 0.7, gamma = 0.9;

    private State lastState;
    private Action lastAction;

    public int flaps = 0;
    public int nops = 0;

    // Table with State and Action (String --> Flap or Nop)
    // Double is the QValue
    public Dictionary<State, Dictionary<Action, double>> qLearningTable = new Dictionary<State, Dictionary<Action, double>>()
    {
        { new State(0, 0), new Dictionary<Action, double> { { Action.Flap, 0 }, { Action.Nop, 1 } } }
    };

    public enum Action
    {
        Nop,
        Flap
    }

    protected Vector3 LowerPipeTopLeftCoords()
    {
        // find the lower segment of next pipes
        var lowerPipe = gameMgr.NextPipes().transform.FindChild("Lower Pipe");
        var renderer = lowerPipe.GetComponent<SpriteRenderer>();
        // get top left coordinate
        var x = renderer.bounds.center.x - renderer.bounds.extents.x;
        var y = renderer.bounds.center.y + renderer.bounds.extents.y;
        return new Vector3(x, y);
    }

    protected bool MorePipes()
    {
        var nextPipes = gameMgr.NextPipes();
        return nextPipes != null;
    }

    protected abstract State DetermineState();

    protected abstract Action SelectAction(State state);

    protected Action SelectRandomAction()
    {
        var val = Random.value;
        if (val < .5f)
        {
            return Action.Nop;
        }
        else
        {
            return Action.Flap;
        }
    }

    void Awake()
    {
        FileWriter.clearFile("flaps");
        FileWriter.clearFile("nops");
        FileWriter.clearFile("episodes");
        birdMvmt = GetComponent<BirdMovement>();
        gameMgr = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    void Update()
    {
        // get current state and compare with last
        var currentState = DetermineState();
        if (currentState.Equals(lastState))
        {
            // do nothing if state did not change
            return;
        }

        // state changed => select an action and execute it
        var action = SelectAction(currentState);
        ExecuteAction(action);
         
        // Update QTable
        var reward = birdMvmt.IsDead() ? -1000 : 10 / (int)(Mathf.Abs(currentState.vDist) + 1.001);
        if (birdMvmt.IsDead())
        {
            FileWriter.WriteTable(qLearningTable, "qLearningTable");
            birdMvmt.ResetIsDead();
        }
        Debug.Log("State: " + currentState + " SelectedAction: " + action + " Reward: " + reward);
        qLearningTable[lastState][lastAction] += alpha * (reward + gamma * Mathf.Max((float)qLearningTable[currentState][Action.Flap], (float)qLearningTable[currentState][Action.Nop]) - qLearningTable[lastState][lastAction]);

        // update lastState
        lastState = currentState;
        lastAction = action;
    }

    void ExecuteAction(Action action)
    {
        if (action.Equals(Action.Flap))
        {
            flaps++;
            birdMvmt.Flap();
        }
        else
        {
            nops++;
        }
    }

    public void PersistData()
    {
        FileWriter.append(flaps.ToString(), "flaps");
        FileWriter.append(nops.ToString(), "nops");
        flaps = 0;
        nops = 0;
    }
}
