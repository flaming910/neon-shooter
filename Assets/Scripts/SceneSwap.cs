using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour {

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void DifficultyScene()
    {
        SceneManager.LoadScene("DifficultySelect");
    }

    public void LevelUpScene()
    {
        SceneManager.LoadScene("LevelUpScene");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
