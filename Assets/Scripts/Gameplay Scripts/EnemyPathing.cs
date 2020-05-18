using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{



    Vector3 enemyPos;
    float xDiff;
    float yDiff;
    float AbsXDiff;
    float AbsYDiff;
    float xEnemyVel;
    float yEnemyVel;
    float localHealth;
    float time;
    int growthDirection = 1;
    float baseVel = 1.25f;

    // Use this for initialization
    void Start()
    {
        localHealth = GameMaster.healthPoints;
    }

    // Update is called once per frame
    void Update()
    {

        print("changed some code");

        if (gameObject.name == "EnemyRhombus(Clone)")
        {
            baseVel = 2f;
            localHealth = localHealth / 3;
        }
        if (gameObject.name == "EnemyPentagon(Clone)")
        {

            if (transform.localScale == new Vector3(1, 1, 1))
            {
                growthDirection = 0;

            }
            else if (transform.localScale == new Vector3(0.6f, 0.6f, 0.6f))
            {
                growthDirection = 1;
            }

            if (growthDirection == 1)
            {
                transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            }
            else if (growthDirection == 0)
            {
                transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            }

        }

        enemyPos = transform.position;
        xDiff = PlayerControl.playerPos.x - enemyPos.x;
        yDiff = PlayerControl.playerPos.y - enemyPos.y;
        AbsXDiff = Mathf.Abs(xDiff);
        AbsYDiff = Mathf.Abs(yDiff);

        if (AbsXDiff > AbsYDiff)
        {
            if (xDiff > 0)
            {
                xEnemyVel = baseVel;
            }
            else
            {
                xEnemyVel = -baseVel;
            }
            time = AbsXDiff / baseVel; //Time to reach the a value of player
            yEnemyVel = yDiff / time; //change y speed to make enemy reach player at same time
        }
        else if (AbsYDiff > AbsXDiff)
        {
            if (yDiff > 0)
            {
                yEnemyVel = baseVel;
            }
            else
            {
                yEnemyVel = -baseVel;
            }
            time = AbsYDiff / baseVel;
            xEnemyVel = xDiff / time;
        }

        GetComponent<Rigidbody>().velocity = new Vector3(xEnemyVel, yEnemyVel, 0);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Projectile(Clone)" || other.name == "ProjectileRed(Clone)")
        {
            if (localHealth <= 1)
            {
                Destroy(gameObject);
                PlayerControl.enemyDied = 1;
                GameMaster.playerScore += 10 * GameMaster.healthPoints;
            }
            else
            {
                localHealth -= PlayerControl.damage;
            }
        }

        if (other.name == "PlayerModel")
        {
            Destroy(gameObject);
        }
    }
}
