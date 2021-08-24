using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    private Maze maze;

    private float screenWidth = 16f;

    private float sreenHeight = 10f;

    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private GameObject wallPerfab;

    void Start()
    {
        maze = new Maze(width, height);
        float wallWidth = screenWidth / width;
        float wallheight = sreenHeight / height;

        int counter = 0;
        while(counter < 2)
        {
            float currentPositionX = -screenWidth / 2;
            float currentPositionY = -sreenHeight / 2;

            for (int i = 0; i <= width + 1; ++i)
            {
                for (int j = 0; j <= height; ++j)
                {
                    GameObject wall = Instantiate(wallPerfab, new Vector2(currentPositionX, currentPositionY), Quaternion.identity);
                    currentPositionY += wallheight;

                    if (counter == 1)
                    {
                        wall.transform.localScale = new Vector2(screenWidth / width, 0.3f);
                        wall.transform.position = new Vector2(wall.transform.position.x - wallWidth / 2, wall.transform.position.y + wallheight / 2);
                        wall.transform.Rotate(new Vector3(0, 0, 90));
                    }
                    else
                    {
                        wall.transform.localScale = new Vector2(sreenHeight / height, 0.3f);
                    }              
                }
                currentPositionX += wallWidth;
                currentPositionY = -sreenHeight / 2;
            }
            counter++;
        }      
    }
}
