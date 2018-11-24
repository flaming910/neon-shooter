﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameMaster : MonoBehaviour {

    public static float playerScore;
    public static int levelUpRequirement = 150;
    public static int level = 1;
    public static int healthPoints = 1;
    public static int isDead;
    

    public Transform enemyPentagon;
    public Transform enemyRhombus;
    public Transform dmgUp;
    public Transform speedUp;
    public Transform lifeUp;
    public Text score;
    public Text health;
    public Text deathScore;
    public GameObject DeathScreen;
    public AudioSource GameMusic1;
    public AudioSource GameMusic2;
    public VideoPlayer Music1Video;
    public VideoPlayer Music2Video;
    float Music1Time;
    float Music2Time;
    float whichSong;

    float spawnLoc;
    float whichVideo;
    float spawnPowerupLocX;
    float spawnPowerupLocY;
    float spawnPowerup;
    float mobSpawned;
    float spawnDelay;
    float baseDelay = 1.5f;
    float whatPowerup;
    int spawnCoord = 20;
    float x;
    float y;
    float highScore;
	// Use this for initialization
	void Start () {
        highScore = PlayerPrefs.GetFloat("High Score");
        StartCoroutine(powerUpDelay());
        whichSong = Random.Range(0, 2);
        Music1Time = GameMusic1.clip.length;
        Music2Time = GameMusic2.clip.length;
        Music1Video.Prepare();
        Music2Video.Prepare();
    }
	
	// Update is called once per frame
	void Update () {
        score.text = "Score: " + playerScore + "\n" + "High Score: " + highScore;
        health.text = "Life: " + PlayerControl.health + "/" + (PlayerPrefs.GetFloat("Health") + 5);
        spawnLoc = Random.Range(0, 361);
        spawnPowerupLocX = Random.Range(-11, 12);
        spawnPowerupLocY = Random.Range(-6, 7);
        whatPowerup = Random.Range(0, 3);

        LevelSystem.levelUp();

        if (whichSong == 0)
        {
            GameMusic1.Play();
            Music2Video.targetCameraAlpha = 0;
            Music1Video.targetCameraAlpha = 1;
            Music1Video.Play();
            print(whichSong);
            whichSong = 2;
            StartCoroutine(songTimer(Music1Time));
        } else if (whichSong == 1)
        {
            GameMusic2.Play();
            Music2Video.targetCameraAlpha = 1;
            Music1Video.targetCameraAlpha = 0;
            Music2Video.Play();
            print(whichSong);
            whichSong = 2;
            StartCoroutine(songTimer(Music2Time));
            
        }


        if (spawnPowerup == 1)
        {
            spawnPowerUp();
            spawnPowerup = 0;
        }

        if (playerScore > highScore)
        {
            highScore = playerScore;
            PlayerPrefs.SetFloat("High Score", highScore);
        }

        if (spawnDelay == 0)
        {
            x = spawnCoord * (Mathf.Sin(Mathf.Deg2Rad * (90 - spawnLoc)));
            y = spawnCoord * (Mathf.Sin(Mathf.Deg2Rad * spawnLoc));
            spawnMob(x, y, -1);
            spawnDelay = baseDelay;
            StartCoroutine(delayRest());              
        }
        
        if (isDead == 1)
        {
            DeathScreen.gameObject.SetActive(true);
            deathScore.text = "Score: " + playerScore + "\n" + "High Score: " + highScore;
            
            
            if(Input.GetKeyDown("space"))
            {
                SceneManager.LoadScene("GameScene");
                isDead = 0;
                playerScore = 0;
                spawnDelay = 0;
                levelUpRequirement = 150;
                level = 1;
                healthPoints = 1;
            }
            if(Input.GetKeyDown(KeyCode.Joystick1Button0) )
            {
                SceneManager.LoadScene("GameScene");
                isDead = 0;
                playerScore = 0;
                spawnDelay = 0;
                levelUpRequirement = 150;
                level = 1;
                healthPoints = 1;
            }
        }


        if (playerScore >= levelUpRequirement)
        {
            healthPoints += level/2;
            level += 1;
            levelUpRequirement = levelUpRequirement * level;
            PlayerControl.baseDelay = PlayerControl.baseDelay / (level * 0.5f);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }


    }

    void spawnMob(float x,float y,float z)
    {
        mobSpawned = Random.Range(0, 2);
        if (mobSpawned == 1f)
        {
            Instantiate(enemyPentagon, new Vector3(x, y, z), enemyPentagon.rotation);
        }
        else if (mobSpawned == 0f)
        {   
            Instantiate(enemyRhombus, new Vector3(x, y, z), enemyRhombus.rotation);
        }
    }

    void spawnPowerUp()
    { 
        if (whatPowerup == 0)
        {
            Instantiate(dmgUp, new Vector3(spawnPowerupLocX, spawnPowerupLocY, -1), dmgUp.rotation);
            StartCoroutine(powerUpDelay());
        } else if (whatPowerup == 1)
        {
            Instantiate(speedUp, new Vector3(spawnPowerupLocX, spawnPowerupLocY, -1), speedUp.rotation);
            StartCoroutine(powerUpDelay());
        } else if (whatPowerup == 2)
        {
            Instantiate(lifeUp, new Vector3(spawnPowerupLocX, spawnPowerupLocY, -1), lifeUp.rotation);
            StartCoroutine(powerUpDelay());
        }

        
    }

    IEnumerator songTimer(float t)
    {
        yield return new WaitForSeconds(t);
        if (t == Music1Time)
        {
            whichSong = 1;
            
        } else if (t == Music2Time)
        {
            whichSong = 0;
        }

    }

    IEnumerator powerUpDelay()
    {
        yield return new WaitForSeconds(Random.Range(20, 40));
        spawnPowerup = 1;
    }

    IEnumerator delayRest()
    {
        yield return new WaitForSeconds(baseDelay/level*0.75f);
        spawnDelay = 0;
    }

}