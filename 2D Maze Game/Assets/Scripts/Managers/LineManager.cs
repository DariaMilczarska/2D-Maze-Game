using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    private LineRenderer shortestPathRenderer;
    private LineRenderer playerPathRenderer;

    void Start()
    {
        shortestPathRenderer = this.transform.Find("ShortestPathRenderer").GetComponent<LineRenderer>();
        playerPathRenderer = this.transform.Find("PlayerPathRenderer").GetComponent<LineRenderer>();
    }


    public void CreateShortestPath(List<Transform> points, float size)
    {
        shortestPathRenderer.positionCount = points.Count;
        shortestPathRenderer.startColor = new Color(0.1142755f, 0.5660378f, 0.1413812f);
        shortestPathRenderer.endColor = new Color(0.1142755f, 0.5660378f, 0.1413812f);
        shortestPathRenderer.startWidth =  size;
        shortestPathRenderer.endWidth = size;
        for (int i = 0; i < points.Count; ++i)
        {
            shortestPathRenderer.SetPosition(i, points[i].position);
        }
    }

    public void CreatePlayertPath(List<Transform> points, float size)
    {
        playerPathRenderer.positionCount = points.Count;
        playerPathRenderer.startColor = new Color(0.4150943f, 0.04464219f, 0.08974946f);
        playerPathRenderer.endColor = new Color(0.4150943f, 0.04464219f, 0.08974946f);
        playerPathRenderer.startWidth = size/2;
        playerPathRenderer.endWidth = size/2;
        for (int i = 0; i < points.Count; ++i)
        {
            playerPathRenderer.SetPosition(i, points[i].position);
        }
    }

}
