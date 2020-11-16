using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject agent;
    public GameObject hole;
    public GameObject reward;

    private float Time;
    private int gridSize = 10;
    private int[] board;
    private Vector3[] positions;
    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[100];
        board = new int [100];



        //Set positions
        int number = 0;
        for(int x = 0; x < 10; x++)
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

    public void newGame()
    {
        //spawn holes
        for (int x = 0; x < 10; x++)
        {
            while (true)
            {

                int rand = Random.Range(0, 100);
                //Debug.Log(rand);
                if(board[rand] == 0)
                {
                    GameObject clone;
                    clone = Instantiate(hole, positions[rand], Quaternion.Euler(0,0,0));
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
                    clone = Instantiate(reward, positions[rand], Quaternion.Euler(0, 0, 0));
                    //Debug.Log(positions[rand]);
                    board[rand] = 1;
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
