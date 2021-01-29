using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinPoints : MonoBehaviour
{
    public static bool isOnPlayer;

    public LayerMask whatIsPlayer;

    public Transform playerCheck;

    public float playerCheckRadius;

    public AudioSource myFX;
    public AudioClip clickFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isOnPlayer = Physics2D.OverlapCircle(transform.position, playerCheckRadius, whatIsPlayer);

        if(isOnPlayer)
        {
            
            myFX.PlayOneShot (clickFX);
            Destroy(gameObject);
            score.point += 1;
                
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerCheckRadius);
    }
}
