using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coletaveis : MonoBehaviour
{
    public AudioSource myFX;
    public AudioClip clickFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(coinPoints.isOnPlayer)
        {
            myFX.PlayOneShot (clickFX);
        }
    }
}
