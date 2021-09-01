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

}
