using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public static int health = 5;

    public int visibleHealth;

    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHearts;
    public Sprite emptyHearts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        visibleHealth = health;

        if(health > numOfHearts)
        {
            health = numOfHearts;
        }


        for (int i = 0; i < hearts.Length; i++)
        {

            if(i < health)
            {
                hearts[i].sprite = fullHearts;
            }else{
                hearts[i].sprite = emptyHearts;
            }


            if(i<numOfHearts)
            {
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }
        }
    }
}
