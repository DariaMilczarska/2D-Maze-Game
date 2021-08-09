using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float startPositionX = -8.11f;
    private float startPositionY = -4.29f;
    void Start()
    {
        ResetPosition();
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
        float speed = 5f;

        transform.Translate(new Vector3(horizontal, vertical, 0.0f) * speed * Time.deltaTime);
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
