using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o2Tank : MonoBehaviour
{
    public bool isOnPlayer;

    public LayerMask whatIsPlayer;

    public Transform playerCheck;

    public float playerCheckRadius;

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
            
            if(Health.health < 5)
            {
                Destroy(gameObject);
                Health.health += 1;
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerCheckRadius);
    }
}
