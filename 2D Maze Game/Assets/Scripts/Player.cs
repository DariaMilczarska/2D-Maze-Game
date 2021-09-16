using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private bool playerPositionSet;
    private float startPositionX;
    private float startPositionY;

    [SerializeField]
    private MazeManager mazeManager;
    void Start()
    {    
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        float speed = 250f;

        if (rigidBody != null)
        {
            rigidBody.velocity = (new Vector2(horizontal, vertical) * speed * Time.deltaTime);
        }
    }
    public void SetUpPosition(float posX, float posY)
    {
        startPositionX = posX;
        startPositionY = posY;
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(startPositionX, startPositionY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Treasure")
        {
            ResetPosition();
        }
    }

    private void SetDirection(float vertical, float horizontal)
    {
        float rotation = 0f;
        if(vertical > 0)
        {
            rotation = 90;
        }
        else if(vertical < 0)
        {
            rotation = 270;
        }
        if (horizontal > 0)
        {
            rotation = 0;
        }
        else
        {
            rotation = 180;
        }

        transform.localRotation = new Quaternion() new Vector3(0, 0, rotation);
    }

}
