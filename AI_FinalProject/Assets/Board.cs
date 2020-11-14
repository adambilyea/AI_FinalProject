using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject agent;
    private float Time;
    private int gridSize = 10;
    private int[] board;
    private Vector3[] positions;
    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[100];
        board = new int [100];
        newGame(); 

        //Set positions
        for(int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                positions[z * x] = new Vector3(x * 10, 1, z * 10);
                Debug.Log(positions[z * x]);
            }

        }
    }

    public void newGame()
    {
        for (int x = 0; x < 100; x++)
        {
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
