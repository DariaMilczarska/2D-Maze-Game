using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private LevelManager levelManager;
    private Rigidbody2D rigidBody;

    private float startPositionX;
    private float startPositionY;
    private float rotation = 0;

    [SerializeField]
    private MazeManager mazeManager;
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (rigidBody != null)
        {
            rigidBody.velocity = (new Vector2(horizontal, vertical) * LevelParameters.speed * Time.deltaTime);
            SetDirection(vertical, horizontal);
        }  
        
    }

    private void SetDirection(float vertical, float horizontal)
    {
        if (vertical > 0)
        {
            rotation = 90;
        }
        else if (vertical < 0)
        {
            rotation = 270;
        }
        else if (horizontal > 0)
        {
            rotation = 0;
        }
        else if (horizontal < 0)
        {
            rotation = 180;
        }

        transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    public void SetUpPosition(float posX, float posY)
    {
        startPositionX = posX;
        startPositionY = posY;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Treasure")
        {           
            levelManager.TreasureHitted();
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(startPositionX, startPositionY);
        levelManager.playerMovementTrack = new List<Coordinates>();
    }
}
