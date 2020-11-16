using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class InternalAgent : AI
{
    public float[][] q_table;   // The matrix containing the values estimates.
    float learning_rate = 0.5f; // The rate at which to update the value estimates given a reward.
    int action = -1;
    float gamma = 0.99f; // Discount factor for calculating Q-target.
    float e = 1; // Initial epsilon value for random action selection.
    float eMin = 0.1f; // Lower bound of epsilon.
    int annealingSteps = 2000; // Number of steps to lower e to eMin.
    int lastState;

    public override void SendParameters(EnvironmentParameters env)
    {
        q_table = new float[100][];
        action = 0;
        for (int i = 0; i < 100; i++)
        {
            q_table[i] = new float[100];
            for (int j = 0; j < 4; j++)
            {
                q_table[i][j] = 0.0f;
            }
        }
    }

    /// <summary>
    /// Picks an action to take from its current state.
	/// </summary>
	/// <returns>The action choosen by the agent's policy</returns>
	public override float[] GetAction()
    {
        
        action = q_table[lastState].ToList().IndexOf(q_table[lastState].Max());
        if (Random.Range(0f, 1f) < e)
        {
            action = Random.Range(0, 3);
        }
        if (e > eMin)
        {
            e = e - ((1f - eMin) / (float)annealingSteps);
        }
        float currentQ = q_table[lastState][action];
        return new float[1] { action };
    }

    /// <summary>
    /// Gets the values stored within the Q table.
    /// </summary>
    /// <returns>The average Q-values per state.</returns>
    public override float[] GetValue()
    {
        float[] value_table = new float[q_table.Length];
        for (int i = 0; i < 100; i++)
        {
            value_table[i] = q_table[i].Average();
         
        }
        return value_table;
    }

    
    public override void SendState(List<float> state, float reward, bool done)
    {
        int nextState = Mathf.FloorToInt(state.First());
        if (action != -1)
        {
            if (done == true)
            {
                q_table[lastState][action] += learning_rate * (reward - q_table[lastState][action]);
            }
            else
            {
                q_table[lastState][action] += learning_rate * (reward + gamma * q_table[nextState].Max() - q_table[lastState][action]);
            }
        }
        lastState = nextState;
    }

   
}


