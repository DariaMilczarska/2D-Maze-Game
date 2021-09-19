using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private int time = 0;

    [SerializeField]
    private Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetUpTime());
    }


    public IEnumerator SetUpTime()
    {
        while (true)
        {         
            yield return new WaitForSeconds(1);
            timeText.text = "Time: " + ++time;
        }     
    }
}
