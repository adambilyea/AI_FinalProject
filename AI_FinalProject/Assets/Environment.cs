using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Environment : MonoBehaviour
{
    public float reward = 0;
    public bool done;
    public int maxSteps;
    public int currentStep;
    public bool begun;
    public bool acceptingSteps;

    public InternalAgent agent;
    public int frameToSkip;
    public int comPort;
    public int framesSinceAction;
    public string currentPythonCommand;
    public bool skippingFrames;
    public float[] actions;
    public float waitTime;
    public int episodeCount;
    public bool humanControl;

    public int bumper;

    public virtual void SetUp()
    {
        begun = false;
        acceptingSteps = true;
    }

    public virtual List<float> collectState()
    {
        List<float> state = new List<float>();
        return state;
    }

    public virtual void Step()
    {
        acceptingSteps = false;
        currentStep += 1;
        if (currentStep >= maxSteps)
        {
            done = true;
        }

        reward = 0;
        actions = agent.GetAction();
        framesSinceAction = 0;

        int sendAction = Mathf.FloorToInt(actions[0]);
        //MiddleStep(sendAction);

        StartCoroutine(WaitStep());
    }

    public virtual void EndStep()
    {
        agent.SendState(collectState(), reward, done);
        skippingFrames = false;
        acceptingSteps = true;
    }

    public virtual void Reset()
    {
        reward = 0;
        currentStep = 0;
        episodeCount++;
        done = false;
        acceptingSteps = false;
    }

    public virtual void EndReset()
    {
        agent.SendState(collectState(), reward, done);
        skippingFrames = false;
        acceptingSteps = true;
        begun = true;
        framesSinceAction = 0;
    }

    public virtual void RunMdp()
    {
        if (acceptingSteps == true)
        {
            if (done == false)
            {
                Step();
            }
            else
            {
                Reset();
            }
        }
    }

    public IEnumerator WaitStep()
    {
        yield return new WaitForSeconds(waitTime);
        EndStep();
    }

}
