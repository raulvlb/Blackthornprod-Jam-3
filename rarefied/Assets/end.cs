using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class end : MonoBehaviour
{

    public static bool terminou;

    public Text display;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(terminou)
        {
            display.text = "SCORE: " + score.point.ToString();
        }else{
            display.text = "IT WASN'T THIS TIME :(";
        }
    }
}
