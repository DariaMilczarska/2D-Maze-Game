using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{ 
    public void SetUpCoordinates(Transform treasure)
    {
        this.transform.position = treasure.position;
    }

}
