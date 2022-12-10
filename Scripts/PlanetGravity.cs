using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float masse;
    System.Random random = new();

    // Start is called before the first frame update
    void Start()
    {
        masse = random.Next(50, 100);
    }
}
