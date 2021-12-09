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
    private bool coinCollected = false;
    private Animator animator;

    [SerializeField]
    private MazeManager mazeManager;
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        SetDirection(vertical, horizontal);
        if(vertical != 0 || horizontal != 0)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }
    }
    private void SetDirection(float vertical, float horizontal)
    {
        if (horizontal >= 0)
        {
            rotation = 0;
        }
        else if (horizontal < 0)
        {
            rotation = 180;
            horizontal = -horizontal;
        }
        transform.Translate(new Vector3(horizontal, vertical, 0) * Time.deltaTime * 3f);
        transform.localRotation = Quaternion.Euler(0, rotation, 0);
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

        if (collision.tag == "Coin")
        {
            if (!coinCollected)
            {
                levelManager.CollectCoin();
                coinCollected = true;
                StartCoroutine(CoinCollectedCooldown());
            }      
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator CoinCollectedCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        coinCollected = false;
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(startPositionX, startPositionY);
        levelManager.playerMovementTrack = new List<Coordinates>();
    }
}