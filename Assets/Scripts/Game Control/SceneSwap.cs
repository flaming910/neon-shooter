using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour {

    public bool isMenu;
    // Use this for initialization
	void Start () {        
    }
	
	// Update is called once per frame
	void Update () {
        if(isMenu)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                PlayGame();
            }
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
