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

    public GameObject alarm;
    // Start is called before the first frame update
    void Start()
    {
        health = 5;
    }

    // Update is called once per frame
    void Update()
    {
        visibleHealth = health;

        if(health > numOfHearts)
        {
            health = numOfHearts;
        }

        if(health == 1){
            alarm.GetComponent<AudioSource>().volume = 1f;
        }else if(health == 2){
            alarm.GetComponent<AudioSource>().volume = 0.8f;
        }else if(health == 3){
            alarm.GetComponent<AudioSource>().volume = 0.6f;
        }else if(health == 4){
            alarm.GetComponent<AudioSource>().volume = 0.4f;
        }else if(health == 5){
            alarm.GetComponent<AudioSource>().volume = 0.2f;
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
