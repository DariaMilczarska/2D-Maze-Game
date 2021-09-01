using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int level; //for the future
    
    [SerializeField]
    private Player player;

    [SerializeField]
    private Treasure treasure;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.ResetPosition();
        }
    }

    public void SetUpGame(float size, Transform playerPosition, Transform treasurePosition)
    {
        SetUpComponentsSize(size);
        SetUpPlayerPosition(playerPosition);
        SetUpTreasurePosition(treasurePosition);
    }

    private void SetUpComponentsSize(float size)
    {
        player.transform.localScale = new Vector2(2 * size, 2 * size);
        treasure.transform.localScale = new Vector2(4 * size, 4 * size);
    }

    private void SetUpTreasurePosition(Transform transform)
    {
        if(transform != null)
        {
            treasure.transform.position = transform.position;
        }      
    }
    private void SetUpPlayerPosition(Transform transform)
    {
        if(transform != null)
        {
            player.SetUpPosition(transform.position.x, transform.position.y);
            player.transform.position = transform.position;
        }       
    }
}
