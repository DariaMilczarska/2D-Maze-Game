using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int level; //for the future
    [SerializeField]
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
       //player = GameObject.Find("Player");

        if(player == null)
        {
            Debug.Log("missing players");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.ResetPosition();
        }
    }

    void GenerateMaze()
    {

    }
}
