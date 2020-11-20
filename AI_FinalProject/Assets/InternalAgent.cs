using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class InternalAgent : MonoBehaviour
{
    //contains the values estimates
    public float[][] valueTable;   
    //AI Movement Action
    int action = -1;
    //Initial epsilon value for random action selection
    float epsilon = 1; 
    //AI last state
    int lastState;

    //initializes the board in relation to AI
    public void SendParameters()
    {
        valueTable = new float[100][];
        action = 0;
        for (int i = 0; i < 100; i++)
        {
            valueTable[i] = new float[100];
            for (int j = 0; j < 4; j++)
            {
                valueTable[i][j] = 0.0f;
            }
        }
    }

    //Picks an action to make
	public float[] GetAction()
    {
        action = valueTable[lastState].ToList().IndexOf(valueTable[lastState].Max());
        if (Random.Range(0f, 1f) < epsilon)
        {
            action = Random.Range(0, 3);
        }
        if (epsilon > 0.1f)
        {
            epsilon = epsilon - ((1f - 0.1f) / (float)2000);
        }
        float currentQ = valueTable[lastState][action];
        return new float[1] { action };
    }

    //Gets the values in the valueTable
    public float[] GetValue()
    {
        float[] value_table = new float[valueTable.Length];
        for (int i = 0; i < 100; i++)
        {
            value_table[i] = valueTable[i].Average();
        }
        return value_table;
    }

    //Send the next action for AI to make
    public void SendState(List<float> state, float reward, bool done)
    {
        int nextState = Mathf.FloorToInt(state.First());
        if (action != -1)
        {
            if (done == true)
            {
                valueTable[lastState][action] += 0.5f * (reward - valueTable[lastState][action]);
            }
            else
            {
                valueTable[lastState][action] += 0.5f * (reward + 0.99f * valueTable[nextState].Max() - valueTable[lastState][action]);
            }
        }
        lastState = nextState;
    }
}


