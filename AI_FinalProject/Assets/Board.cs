using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : Environment
{
    public List<GameObject> actorObjs;
    public string[] players;
    public GameObject visualAgent;
    int[] objectPositions;
    float episodeReward;

    // our stuff
    //public GameObject agent;
    public GameObject hole;
    public GameObject prize;


    private float Time;
    private int gridSize = 10;
    private int[] board;
    private Vector3[] positions;
    // Start is called before the first frame update

    void Start()
    {
        positions = new Vector3[100];
        board = new int[100];



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

        for (int x = 0; x < 100; x++)
        {
            board[x] = 0;
            //Debug.Log(positions[x]);
        }
        newGame();
    }

    void Update()
    {
        RunMdp();
    }

    public void newGame()
    {
        //spawn holes
        for (int x = 0; x < 10; x++)
        {
            while (true)
            {

                int rand = Random.Range(0, 100);
                //Debug.Log(rand);
                if (board[rand] == 0)
                {
                    GameObject clone;
                    clone = Instantiate(hole, positions[rand], Quaternion.Euler(0, 0, 0));
                    //Debug.Log(positions[rand]);
                    board[rand] = 1;
                    break;
                }
            }
        }

        //spawn rewards
        for (int x = 0; x < 5; x++)
        {
            while (true)
            {

                int rand = Random.Range(0, 100);
                //Debug.Log(rand);
                if (board[rand] == 0)
                {
                    GameObject clone;
                    clone = Instantiate(prize, positions[rand], Quaternion.Euler(0, 0, 0));
                    //Debug.Log(positions[rand]);
                    board[rand] = 1;
                    break;
                }
            }
        }

        SetUp();
        agent = new InternalAgent();
        agent.SendParameters(envParameters);
        Reset();
    }
    public override void SetUp()
    {
        envParameters = new EnvironmentParameters()
        {
            observation_size = 0,
            state_size = gridSize * gridSize,
            action_descriptions = new List<string>() { "Up", "Down", "Left", "Right" },
            action_size = 4,
            env_name = "GridWorld",
            action_space_type = "discrete",
            state_space_type = "discrete",
            num_agents = 1
        };

        List<string> playersList = new List<string>();
        actorObjs = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            playersList.Add("pit");
        }
        playersList.Add("agent");

        for (int i = 0; i < 5; i++)
        {
            playersList.Add("goal");
        }
        players = playersList.ToArray();
    }

    public override List<float> collectState()
    {
        List<float> state = new List<float>();
        float point = (gridSize * visualAgent.transform.position.x) + visualAgent.transform.position.z;
        state.Add(point / 10);
        return state;
    }

    public override void Reset()
    {
        base.Reset();

        //foreach (GameObject actor in actorObjs)
        //{
        //    DestroyImmediate(actor);
        //}
        //actorObjs = new List<GameObject>();

        visualAgent.transform.position = new Vector3(0, 2, 0);
        episodeReward = 0;
        EndReset();
    }

    public override void MiddleStep(int action)
    {
        reward = -0.05f;
        // 0 - Forward, 1 - Backward, 2 - Left, 3 - Right
        if (action == 3)
        {
            Collider[] blockTest = Physics.OverlapBox(new Vector3(visualAgent.transform.position.x + 10, 0, visualAgent.transform.position.z), new Vector3(0.3f, 0.3f, 0.3f));
            if (blockTest.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
            {
                visualAgent.transform.position = new Vector3(visualAgent.transform.position.x + 10, 0, visualAgent.transform.position.z);
            }
        }

        if (action == 2)
        {
            Collider[] blockTest = Physics.OverlapBox(new Vector3(visualAgent.transform.position.x - 10, 0, visualAgent.transform.position.z), new Vector3(0.3f, 0.3f, 0.3f));
            if (blockTest.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
            {
                visualAgent.transform.position = new Vector3(visualAgent.transform.position.x - 10, 0, visualAgent.transform.position.z);
            }
        }

        if (action == 0)
        {
            Collider[] blockTest = Physics.OverlapBox(new Vector3(visualAgent.transform.position.x, 0, visualAgent.transform.position.z + 10), new Vector3(0.3f, 0.3f, 0.3f));
            if (blockTest.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
            {
                visualAgent.transform.position = new Vector3(visualAgent.transform.position.x, 0, visualAgent.transform.position.z + 10);
            }
        }

        if (action == 1)
        {
            Collider[] blockTest = Physics.OverlapBox(new Vector3(visualAgent.transform.position.x, 0, visualAgent.transform.position.z - 10), new Vector3(0.3f, 0.3f, 0.3f));
            if (blockTest.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
            {
                visualAgent.transform.position = new Vector3(visualAgent.transform.position.x, 0, visualAgent.transform.position.z - 10);
            }
        }

        Collider[] hitObjects = Physics.OverlapBox(visualAgent.transform.position, new Vector3(0.3f, 0.3f, 0.3f));
        if (hitObjects.Where(col => col.gameObject.tag == "reward").ToArray().Length == 1)
        {
            reward = 1;
            done = false;
        }
        if (hitObjects.Where(col => col.gameObject.tag == "hole").ToArray().Length == 1)
        {
            reward = -1;
            done = true;
        }
        // Update is called once per frame


    }
}

