using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlay : MonoBehaviour
{   

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.canClimbLedge == true)
        {
            transform.position = new Vector3(0f,transform.position.y, -10f);
        }else{
            transform.position = new Vector3(0f, player.transform.position.y, -10f);
        }
    }
}
