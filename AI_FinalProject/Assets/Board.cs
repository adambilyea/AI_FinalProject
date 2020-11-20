//Eashvar Balachandran
//Adam Bilyea
//Ricky Caceres
//Shairanthan Maheswaran
//November 11, 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Board : MonoBehaviour
{
    /*******Variables*******/
    public float reward = 0;
    public bool done;
    public int maxSteps;
    public int currentStep;
    public bool begun;
    public bool canStep;
    public AI agent;
    public float[] actions;
    public float speed;
    public int generationCount;
    float generationReward;
    public string[] players;
    public List<GameObject> actorObjs;
    public GameObject AI;
    public GameObject hole;
    public GameObject prize;
    public GameObject wallObjs;
    private GameObject rewardObjs;
    private Vector3[] positions;
    private Vector3 rewardPos;
    private int boardSize = 10;
    private int[] board;
    public Text generations;
    public Text stepText;

    public AudioSource music;
    public AudioSource resetSound;


    ///////////////////////////////////////////////////

    void Start()
    {
        positions = new Vector3[100];
        board = new int[100];

        music.Play();
        resetSound.Play();

    //Set positions
    int number = 0;
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                positions[number] = new Vector3(x * 10, 1, z * 10);
                number++;

            }
        }
        //board set to 0
        for (int x = 0; x < 100; x++)
        {
            board[x] = 0;
        }
        newGame();
    }

    void Update()
    {
        //update UI
        stepText.text = currentStep.ToString();
        generations.text = generationCount.ToString();
        
        //switches between AI can step or Reseting to beginning
        if (canStep == true)
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

    //Spawn our Holes, Walls and our Reward in random spots
    public void newGame()
    {
        //spawn holes
        for (int x = 0; x < 15; x++)
        {
            while (true)
            {

                int rand = Random.Range(0, 100);
                if (board[rand] == 0 && rand != 0)
                {
                    GameObject clone;
                    clone = Instantiate(hole, positions[rand], Quaternion.Euler(0, 0, 0));
                    board[rand] = 1;
                    break;
                }
            }
        }

        //spawn wallobj
        for (int x = 0; x < 10; x++)
        {
            while (true)
            {
                int rand = Random.Range(0, 100);
                if (board[rand] == 0 && rand != 0)
                {
                    GameObject clone;
                    clone = Instantiate(wallObjs, positions[rand], Quaternion.Euler(0, 0, 0));
                    board[rand] = 1;
                    break;
                }
            }
        }

        //spawn reward
            while (true)
            {
                int rand = Random.Range(0, 100);
                if (board[rand] == 0 && rand != 0)
                {
                    GameObject clone;
                    clone = Instantiate(prize, positions[rand], Quaternion.Euler(0, 0, 0));
                    rewardPos = positions[rand];
                    rewardObjs = clone;
                    board[rand] = 1;
                    break;
                }
            }

        //instatiate our AI
        agent = new AI();
        agent.SendParameters();
        Reset();
    }

    //check what the AI current state is
    public List<float> collectState()
    {
        List<float> state = new List<float>();
        float point = (boardSize * AI.transform.position.x) + AI.transform.position.z;
        state.Add(point / 10);
        return state;
    }

    //Reset our board and AI to beginning after each generation
    public void Reset()
    {
        ResetVar();
        rewardObjs.transform.position = rewardPos;
        AI.transform.position = new Vector3(0, 2, 0);
        generationReward = 0;

        agent.SendState(collectState(), reward, done);
        canStep = true;
        begun = true;
    }

    //Move our AI
    public void PositionStep(int action)
    {
        reward = -0.10f;
        
        //Move AI Right
        if (action == 0)
        {
            //checks to make sure we are not colliding with a wall so we can continue that way
            Collider[] posCheck = Physics.OverlapBox(new Vector3(AI.transform.position.x, 0, AI.transform.position.z + 10), new Vector3(0.3f, 0.3f, 0.3f));
            if (posCheck.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
            {
                AI.transform.position = new Vector3(AI.transform.position.x, 2, AI.transform.position.z + 10);
            }
        }
        
        //Move AI Left
        else if (action == 1)
        {
            Collider[] posCheck = Physics.OverlapBox(new Vector3(AI.transform.position.x, 0, AI.transform.position.z - 10), new Vector3(0.3f, 0.3f, 0.3f));
            if (posCheck.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
            {
                AI.transform.position = new Vector3(AI.transform.position.x, 2, AI.transform.position.z - 10);
            }
        }
        
        //Move AI Forward
        else if (action == 2)
        {
            Collider[] posCheck = Physics.OverlapBox(new Vector3(AI.transform.position.x - 10, 0, AI.transform.position.z), new Vector3(0.3f, 0.3f, 0.3f));
            if (posCheck.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
            {
                AI.transform.position = new Vector3(AI.transform.position.x - 10, 2, AI.transform.position.z);
            }
        }

        //Move AI Backwards
        else if (action == 3)
        {
            Collider[] posCheck = Physics.OverlapBox(new Vector3(AI.transform.position.x + 10, 0, AI.transform.position.z), new Vector3(0.3f, 0.3f, 0.3f));
            if (posCheck.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
            {
                AI.transform.position = new Vector3(AI.transform.position.x + 10, 2, AI.transform.position.z);
            }
        }

        //check if we collided with a hole or a reward
        Collider[] hitObjs = Physics.OverlapBox(AI.transform.position, new Vector3(0.3f, 0.3f, 0.3f));

        //colided with a chest and give a positive reward
        if (hitObjs.Where(col => col.gameObject.tag == "prize").ToArray().Length == 1)
        {
            Debug.Log("Reward got");
            reward = 1;
            done = true;
            
            //reward is transported so it can not be retrieved in the same generation
            for (int i = 0; i < hitObjs.Length; i++)
            {
                if (hitObjs[i].gameObject.tag == "prize")
                {
                    hitObjs[i].transform.position = new Vector3(-100, -100, -100);
                }
            }
        }
        //if AI collides with a hole then reset and give negative reward
        if (hitObjs.Where(col => col.gameObject.tag == "hole").ToArray().Length == 1)
        {
            reward = -1;
            done = true;
        }
    }

    //Reset Scene if board is impossible for AI to complete
    public void reload()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //change the value of the AI Speed
    public void OnValueChanged()
    {
        speed = GameObject.Find("Slider").GetComponent<Slider>().value;
    }

    //Reset the board variables
    public virtual void ResetVar()
    {
        reward = 0;
        currentStep = 0;
        generationCount++;
        done = false;
        canStep = false;
    }
    
    //facilitates the movement of the AI 
    public virtual void Step()
    {
        canStep = false;
        currentStep += 1;
        if (currentStep >= maxSteps)
        {
            done = true;
        }

        reward = 0;
        actions = agent.GetAction();

        int sendAction = Mathf.FloorToInt(actions[0]);
        PositionStep(sendAction);

        StartCoroutine(Wait());
    }

    //Wait before AI can step again
     public IEnumerator Wait()
    {
        yield return new WaitForSeconds(speed);
        
        agent.SendState(collectState(), reward, done);
        canStep = true;
    }

}

