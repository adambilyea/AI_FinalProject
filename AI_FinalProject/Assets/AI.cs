using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public int Team;
    private int State = 1;
    public GameObject ball;
    private int speed = 5;
    public int Reward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
            {
            //Chase after ball
            case 1:
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, ball.transform.position, step);
                //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.0f);
                //Vector3.Lerp(transform.position, ball.transform.position, Time.time);
                break;
            //Defending
            case 2:
                break;
            default:
                break;
                
            }

    }
}
