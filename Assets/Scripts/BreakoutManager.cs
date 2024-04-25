using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakoutManager : MonoBehaviour
{
    //I use a static variable to make this accessible from anywhere
    public static BreakoutManager Me;
    //As a manager, I keep a link to all the major game elements
    public PaddleController Paddle;
    public BallController Ball;
    
    //The brick prefab
    public BrickController BrickPrefab;
    
    //I keep a list of all bricks that exist
    public List<BrickController> AllBricks;
    
    void Start()
    {
        //I need to register myself as 'the' BreakoutManager
        BreakoutManager.Me = this;

        //This is the code for spawning bricks. It's not very good.
        //How could we make this spawn lots of bricks more efficiently?
        //This will spawn 10 bricks in random spots
       // for (int n = 0; n < 10; n++)
        //{
            //Instantiate(BrickPrefab, new Vector3(Random.Range(-8f,8f), Random.Range(1f,4f), 0), Quaternion.identity);
        //}

        //This will spawn 4 bricks next to each other
        for (float x = -7; x < 8; x += 2)
        {
            for (float y = 0; y < 4; y += 0.5f)
            {
                Instantiate(BrickPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        //Check to see if all the bricks have been broken
        if (AllBricks.Count == 0)
        {
            //If so, win
            SceneManager.LoadScene("You Win");
        }
    }


    
}
