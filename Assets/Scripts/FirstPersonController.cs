using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FirstPersonController : MonoBehaviour
{
    //The camera is inside the player
    public Camera Eyes;
    
    public Rigidbody RB;
    public Projectile3DController ProjectilePrefab;
    //My Sound Effects
    public AudioClip JumpSFX;
    public AudioClip OuchSFX;
    public AudioClip MusicSFX;
    //TextMeshPro is a component that draws text on the screen.
    //TextMeshProUGUI draws text that only shows on the game screen instead of in the game itself.
    //We use this one to show our score.
    public TextMeshProUGUI ScoreText;
    public AudioSource AS;
    
    //Character stats
    public float MouseSensitivity = 3;
    public float WalkSpeed = 10;
    public float JumpPower = 7;
    //This is how many points we currently have
    public static int Score = 0;
    
    //A list of all the solid objects I'm currently touching
    public List<GameObject> Floors;

    
    
    void Start()
    {
        //Play music
        AS.PlayOneShot(MusicSFX);
        Score = 0;
        //During setup we call UpdateScore to make sure our score text looks correct
        UpdateScore();
        //Turn off my mouse and lock it to center screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    
    void Update()
    {
        //If my mouse goes left/right my body moves left/right
        float xRot = Input.GetAxis("Mouse X") * MouseSensitivity;
        transform.Rotate(0,xRot,0);
        //If my mouse goes up/down my aim (but not body) go up/down
        //float yRot = -Input.GetAxis("Mouse Y") * MouseSensitivity;
        //Eyes.transform.Rotate(yRot,0,0);
        //This code should make it so when you look up or down theres a cap to how far you can look in either direction
        float yRot = -Input.GetAxis("Mouse Y") * MouseSensitivity;
        Vector3 eRot = Eyes.transform.localRotation.eulerAngles;
        eRot.x += yRot;
        if (eRot.x < -180) eRot.x += 360;
        if (eRot.x > 180) eRot.x -= 360;
        eRot = new Vector3(Mathf.Clamp(eRot.x, -90, 90),0,0);
        Eyes.transform.localRotation = Quaternion.Euler(eRot);
        Debug.Log("NoSpeen");

        //Movement code
        if (WalkSpeed > 0)
        {
            //My temp velocity variable
            Vector3 move = Vector3.zero;
            
            //transform.forward/right are relative to the direction my body is facing
            if (Input.GetKey(KeyCode.W))
                move += transform.forward;
            if (Input.GetKey(KeyCode.S))
                move -= transform.forward;
            if (Input.GetKey(KeyCode.A))
                move -= transform.right;
            if (Input.GetKey(KeyCode.D))
                move += transform.right;
            //I reduce my total movement to 1 and then multiply it by my speed
            move = move.normalized * WalkSpeed;
            
            //If I hit jump and am on the ground, I jump
            if (JumpPower > 0 && Input.GetKeyDown(KeyCode.Space) && OnGround())
                move.y = JumpPower;
            else  //Otherwise, my Y velocity is whatever it was last frame
                move.y = RB.velocity.y;
            
            //Plug my calculated velocity into the rigidbody
            RB.velocity = move;
        }

        //If I click. . .
        if (Input.GetMouseButtonDown(0))
        {
            //Spawn a projectile right in front of my eyes
            Instantiate(ProjectilePrefab, Eyes.transform.position + Eyes.transform.forward,
                Eyes.transform.rotation);
        }
    }

    //I count as being on the ground if I'm touching at least one solid object
    //This isn't a perfect way of doing this. Can you think of at least one way it might go wrong?
    public bool OnGround()
    {
        return Floors.Count > 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        //If I touch something and it's not already in my list of things I'm touching. . .
            //Add it to the list
        if (!Floors.Contains(other.gameObject))
            Floors.Add(other.gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        //When I stop touching something, remove it from the list of things I'm touching
        Floors.Remove(other.gameObject);
    }
    //Functions are shortcut phrases for when you don't wanna copy paste of bunch of code over and over again
    //This function updates the game's score text to show how many points you have
    //Even if your 'score' variable goes up, if you don't update the text the player doesn't know
    public void UpdateScore()
    {
        ScoreText.text = "Score: " + Score;
    }
}