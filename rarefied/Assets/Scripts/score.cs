using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class score : MonoBehaviour
{

    public static int point;
    public int atualPoints;

    // Start is called before the first frame update
    void Start()
    {
        point = 0;
    }

    // Update is called once per frame
    void Update()
    {
        atualPoints = point;
    }
}
