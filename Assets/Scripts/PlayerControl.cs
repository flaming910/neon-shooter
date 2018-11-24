using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public static Vector3 playerPos;
    public static float health; 

	float xVelAdj;
	float yVelAdj;
	float xFire;
	float yFire;
    public static float enemyDied;
    public static float damage;
    float speed = 6;

    float leftBorder;
    float rightBorder;
    float bottomBorder;
    float topBorder;
    Vector3 playerSize;


    Vector3 lastKnownRotation;
    float powerUp;
    float speedUp;
    float lifeUp;
    public AudioClip bulletSound;
    public AudioClip gotHit;
    public AudioClip blewUp;
    public Transform bulletShot;
    public Transform bulletShotRed;
    float shotDelay;
    public static float baseDelay;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        damage = PlayerPrefs.GetFloat("Damage") + 1;
        health = PlayerPrefs.GetFloat("Health") + 5;
        speed = PlayerPrefs.GetFloat("Speed") + 6;
        baseDelay = 0.05f;

        playerSize = GetComponent<Renderer>().bounds.size;
        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -1)).x + (playerSize.x / 2);
        rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, -1)).x - (playerSize.x / 2);
        bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -1)).y + (playerSize.y / 2);
        topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -1)).y - (playerSize.y / 2);
        
    }
	
	// Update is called once per frame
	void Update () {

        

        playerPos = transform.position;

        transform.position = (new Vector3(
             Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
             Mathf.Clamp(transform.position.y, bottomBorder, topBorder),
             transform.position.z)
         );

        xVelAdj = Input.GetAxis ("xMove");
		yVelAdj = Input.GetAxis ("yMove");

        xFire = Input.GetAxis("xShoot");
        yFire = Input.GetAxis("yShoot");

        if (enemyDied == 1)
        {
            audioSource.clip = blewUp;
            audioSource.Play();
            enemyDied = 0;
        }

        if (powerUp == 2)
        {
            damage = 2;
            StartCoroutine(powerUPTimer());
        }
        if (powerUp == 1)
        {
            damage = 1;
            powerUp = 0;
        }

        if (speedUp == 2)
        {
            speed = 9;
            StartCoroutine(speedUPTimer());
        }
        if (speedUp == 1)
        {
            speed = 6;
            speedUp = 0;
        }

        if (lifeUp == 1)
        {
            health += 1;
            lifeUp = 0;           
        }

        float movingDirection = Mathf.Atan2(-Input.GetAxis("xMove"), -Input.GetAxis("yMove")) * Mathf.Rad2Deg;
        float shootingDirection = Mathf.Atan2(-Input.GetAxis("xShoot"), Input.GetAxis("yShoot")) * Mathf.Rad2Deg;
        if (xFire != 0 || yFire != 0)
        {
            transform.rotation = Quaternion.AngleAxis(shootingDirection, Vector3.forward);
            lastKnownRotation = GetComponent<Transform>().eulerAngles;
        }
        else if (xVelAdj != 0 || yVelAdj != 0)
        {
            transform.rotation = Quaternion.AngleAxis(movingDirection, Vector3.forward);
            lastKnownRotation = GetComponent<Transform>().eulerAngles;
        } else
        {
            GetComponent<Transform>().eulerAngles = lastKnownRotation;
        }

        GetComponent<Rigidbody> ().velocity = new Vector3 (xVelAdj * speed, yVelAdj * -speed,0);
    
        if ((xFire> 0.2 || xFire <-0.2) && (shotDelay == 0))
        {
            if (powerUp == 2)
            {
                Instantiate(bulletShotRed, transform.position, bulletShotRed.rotation);
            } else { 
                Instantiate(bulletShot, transform.position, bulletShot.rotation);
            }
            shotDelay = baseDelay;
            audioSource.clip = bulletSound;
            audioSource.Play();
            StartCoroutine(delayRest() );
        }

        if ((yFire > 0.2 || yFire < -0.2) && (shotDelay == 0))
        {
            shotDelay = baseDelay;
            if (powerUp == 2)
            {
                Instantiate(bulletShotRed, transform.position, bulletShotRed.rotation);
            }
            else
            {
                Instantiate(bulletShot, transform.position, bulletShot.rotation);
            }
            audioSource.clip = bulletSound;
            audioSource.Play();
            StartCoroutine(delayRest() );
        }

        


    }

    IEnumerator powerUPTimer()
    {
        yield return new WaitForSeconds(15);
        powerUp = 1;
    }
    IEnumerator speedUPTimer()
    {
        yield return new WaitForSeconds(15);
        speedUp = 1;
    }

    IEnumerator delayRest()
    {
        yield return new WaitForSeconds(baseDelay);
        shotDelay = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "EnemyPentagon(Clone)" || other.name == "EnemyRhombus(Clone)")
        {
            audioSource.clip = gotHit;
            audioSource.Play();
            if (health == 1)
            {
                GameMaster.isDead = 1;
                Destroy(gameObject);
                health -= 1;

            }
            else
            {
                health -= 1;
            }
        }

        if (other.name == "DmgUp(Clone)")
        {
            powerUp = 2;
        }
        if (other.name == "SpeedUp(Clone)")
        {
            speedUp = 2;
        }
        if (other.name == "LifeUp(Clone)")
        {
            lifeUp = 1;
        }

    }
}
