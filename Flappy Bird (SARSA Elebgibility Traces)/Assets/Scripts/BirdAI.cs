using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.UI;
using Assets.Scripts.LearningMethods;
using Assets.Scripts.Strategies;
using System;

[RequireComponent(typeof(BirdMovement))]
public abstract class BirdAI : MonoBehaviour {

	private BirdMovement birdMvmt;
	private GameManager gameMgr;

	private State lastState;
    private QValue lastQValue = new QValue(new State(), Assets.Scripts.Enum.Action.Begin, 0);
    private EValue lastEValue = new EValue(new State(), Assets.Scripts.Enum.Action.Begin, 0);
    private float reward;

    private List<QValue> qValues = new List<QValue>();
    private List<EValue> eValues = new List<EValue>(); 
    public List<QValue> getQTable() { return qValues; }
    public List<EValue> getETable() { return eValues; }

    protected int pipes;
    protected int highestValue = 0;
    private Assets.Scripts.Enum.Action action; 

    void Start() {
        pipes = 0;
        birdMvmt.setAttempt(0);
    }
    

    public int getHighestValue()
    {
       // highestValue = highestValue >= gameMgr.getNumberOfPipes() ? highestValue : gameMgr.getNumberOfPipes();
        return highestValue;
    }

    protected Vector3 LowerPipeTopLeftCoords() {
		// find the lower segment of next pipes
		var lowerPipe = gameMgr.NextPipes().transform.FindChild("Upper Pipe");
		var renderer = lowerPipe.GetComponent<SpriteRenderer>();
        // get top left coordinate
        // -
        var x = renderer.bounds.center.x - renderer.bounds.extents.x;
        var y = renderer.bounds.center.y + renderer.bounds.extents.y;
        return new Vector3(x, y);
	}

	protected bool MorePipes() {
		var nextPipes = gameMgr.NextPipes();
		return nextPipes != null;
	}
    
	protected abstract State DetermineState();

    void OnApplicationQuit()
    {
        XMLParser.Searialize(qValues);
    }
	
	protected Assets.Scripts.Enum.Action SelectRandomAction() {
		var val = UnityEngine.Random.value;
		if (val < .5f) {
			return Assets.Scripts.Enum.Action.Nop;
		} else {
			return Assets.Scripts.Enum.Action.Flap;
		}
	}
	
	void Awake() {
		birdMvmt = GetComponent<BirdMovement>();
        qValues = XMLParser.DeSerialize();
		gameMgr = FindObjectOfType(typeof(GameManager)) as GameManager;
	}

    void Update()
    {
        var currentState = DetermineState();
        if (currentState.Equals(lastState))
        {
            return;
        }
        //Add Value if not allready existant
        addQValue(currentState);
        addEValue(currentState);

        //qLearning(currentState, new GreedyStrategie());
        sARSALAMBDA(currentState, new GreedyStrategie());
    }

    private void qLearning(State currentState, Strategie strategie)
    {
        var qValue = strategie.getQValueForStrategie(currentState, qValues);
        ExecuteAction(qValue.getAction());
        if (!lastQValue.getAction().Equals(Assets.Scripts.Enum.Action.Begin))
        {
            qValues = QLearning.updateTable(lastQValue,qValue,qValues,getReward());
        }
        lastState = currentState;
        lastQValue = qValue;
    }

    private void sARSA(State currentState, Strategie strategie)
    {
        firstRound(currentState, strategie);

        //Q(s,a)
        QValue qValue = strategie.getQValue(lastState,action, qValues);
        // a ausführen
        ExecuteAction(qValue.getAction());
        // r beobachten
        reward = getReward();
        //s' beobachten
        currentState = DetermineState();
        
        //Q(s',a') gemäß Strategie
        QValue currentQValue = strategie.getQValueForStrategie(currentState, qValues);
        //Tabelle aktualisieren
        qValues = SARSA.updateTable(lastState, lastQValue.getAction(), currentState, currentQValue.getAction(), reward, qValues);
        
        // s<-s', a<-a'
        lastState = currentState;
        lastQValue = currentQValue;
        action = currentQValue.getAction();

    }

    private void sARSALAMBDA(State currentState, Strategie strategie)
    {
        firstRound(currentState, strategie);

        //Q(s,a)
        QValue qValue = strategie.getQValue(lastState,action, qValues);
        //e(s,a) <- e(s,a)+1
        EValue eValue = strategie.getEValue(lastState,action, eValues);
        //Stacking 
        //--> Replacing: eValue.setValue(1);
        eValue.setValue(eValue.getValue() + 1);
        // a ausführen
        ExecuteAction(qValue.getAction());
        // r beobachten
        reward = getReward();
        //s' beobachten
        currentState = DetermineState();
        
        //Q(s',a') gemäß Strategie
        QValue currentQValue = strategie.getQValueForStrategie(currentState, qValues);
        //Tabelle aktualisieren
        qValues = SARSA_LAMBDA.updateQTable(lastState, lastQValue.getAction(), currentState, currentQValue.getAction(), reward, qValues, eValues);
        //e(s,a) <- 𝛾𝜆e(s,a)
        //updateETable --> eValue.setValue(gamma * lambda * eValue.getValue());
        eValues = SARSA_LAMBDA.updateETable(lastState, lastQValue.getAction(), currentState, currentQValue.getAction(), eValues);

        // s<-s', a<-a'
        lastState = currentState;
        lastQValue = currentQValue;
        action = currentQValue.getAction();

    }

    private void firstRound(State currentState, Strategie strategie) {
        if (lastQValue.getAction().Equals(Assets.Scripts.Enum.Action.Begin))
        {
            lastState = currentState;
            lastQValue = strategie.getQValueForStrategie(lastState, qValues);
            action = lastQValue.getAction();
        }
    }




    private int returnIndexOfQValue(QValue qVal)
    {
        return qValues.IndexOf(qVal);
    }

    Assets.Scripts.Enum.Action ExecuteAction(Assets.Scripts.Enum.Action action)
	{
		if (action == Assets.Scripts.Enum.Action.Flap) {
			birdMvmt.Flap();
		} else {
			// nop, do nothing
		}
        return action;
	}

    /* If a new state appears it musst be added to the Q-table
    **/
    public void addQValue(State state)
    {
        foreach(QValue c in getQTable())
        {
            if (c.getState().Equals(state)) return;
        }
        getQTable().Add(new QValue(state, Assets.Scripts.Enum.Action.Nop, 5));
        getQTable().Add(new QValue(state, Assets.Scripts.Enum.Action.Flap, 5));
    }

    public void addEValue(State state)
    {
        foreach (EValue c in getETable())
        {
            if (c.getState().Equals(state)) return;
        }

        getETable().Add(new EValue(state, Assets.Scripts.Enum.Action.Nop, 5));
        getETable().Add(new EValue(state, Assets.Scripts.Enum.Action.Flap, 5));
    }


    
    public float getReward() 
    {
        if (birdMvmt.getBirdIsDead()) { 
            return -500;
        }
        return (1);
    }

    public abstract float getRewardWithYDistance(bool isDead);

}
