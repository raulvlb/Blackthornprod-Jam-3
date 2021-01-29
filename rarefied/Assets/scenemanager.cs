using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Score()
    {
        SceneManager.LoadScene("Scores");
    }

    public void Creditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void play()
    {
        SceneManager.LoadScene("Jogo");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
