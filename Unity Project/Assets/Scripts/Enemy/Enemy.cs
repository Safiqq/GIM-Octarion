using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { mobs, bomb, boss};

    [Header("Enemy Properties")]
    [Tooltip("Enemy type")]
    public EnemyType enemyType;
    [Tooltip("Text to type in order to destroy enemy")]
    public string text = "johanes";
    [Tooltip("Enemy speed toward player")]
    public float speed = 1;
    [Tooltip("Angle from y axis which represent enemy's angle of view when chasing player (in degrees)")]
    public float viewAngle = 60;
    [Tooltip("Force magnitude when enemy collides with player")]
    public float knockBackMagnitude = 10;

    private Transform playerTransform;
    private Rigidbody2D rb, playerRB;
    private InputAssets.InputManager _input;
    private bool ignoreLimit = false;
    private Spawner spawner;
    private float yLowerBound = -10;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").GetComponentInChildren<InputAssets.PlayerController>().transform;
        _input = playerTransform.GetComponent<InputAssets.InputManager>();
        playerRB = playerTransform.GetComponent<Rigidbody2D>();

        spawner = GameObject.Find("Enemy Spawner").GetComponent<Spawner>();

        rb = GetComponent<Rigidbody2D>();
        ignoreLimit = false;
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Move();

        if (transform.position.y < yLowerBound)
        {
            Terminate();
        }
    }

    void Update()
    {
        DetectType();
    }

    private void LateUpdate()
    {
        DetectLife();
    }

    void Move()
    {
        if (enemyType != EnemyType.boss)
        {
            float deltaY = playerTransform.position.y - transform.position.y;
            float deltaX = (playerTransform.position.x - transform.position.x);
            float currentAngle = Mathf.Atan2(deltaX, deltaY);
            float radViewAngle = viewAngle * Mathf.PI / 180;
            Vector2 currentVelocity = Vector2.zero;

            if (!ignoreLimit && (Mathf.Atan2(deltaX, Mathf.Abs(deltaY)) < -radViewAngle || (Mathf.Atan2(deltaX, Mathf.Abs(deltaY)) > radViewAngle)))
            {
                // clamp velocity direction angle from y axis (max 45 degrees)

                deltaX = Mathf.Tan(radViewAngle) * Mathf.Abs(deltaY) * (deltaX / Mathf.Abs(deltaX));
            }

            Vector2 newVelocity = ((Vector2.up * deltaY) + (Vector2.right) * (deltaX)).normalized * speed;

            if (playerTransform.position.y < transform.position.y || ignoreLimit)
            {
                rb.velocity = Vector2.SmoothDamp(rb.velocity, newVelocity, ref currentVelocity, Time.deltaTime);
            }
        }
    }

    void DetectType()
    {
        char currentChar;
        int idx;

        if (text.Length > 0)
        {
            currentChar = text[0];

            if (currentChar >= 'a' && currentChar <= 'z')
            {
                idx = currentChar - 'a';
              
                if (_input.alphabets[idx])
                {
                    text = text.Remove(0, 1);
                }
            }
        }
    }

    void DetectLife()
    {
        if (text.Length == 0)
        {

            if (enemyType != EnemyType.bomb)
            {
                Terminate();
            }

            else if (enemyType == EnemyType.bomb)
            {
                if (!ignoreLimit)
                {
                    speed *= 10;
                    ignoreLimit = true;
                }
            }

            UpdateSpawnerCount();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 

        if (collision.gameObject.GetComponent<InputAssets.PlayerController>())
        {
            HitPlayer();
        }
    }

    void HitPlayer()
    {
        playerRB.AddForce((playerTransform.position - transform.position).normalized * knockBackMagnitude, ForceMode2D.Impulse);

        if (enemyType != EnemyType.boss)
        {
            Terminate();
        }
        
        else
        {

        }
    }

    void Terminate()
    {
        gameObject.SetActive(false);


    }

    void UpdateSpawnerCount()
    {
        if (enemyType == EnemyType.mobs)
        {
            spawner.activeMobsCount--;
        }

        else if (enemyType == EnemyType.bomb)
        {
            spawner.activeBombsCount--;
        }

        else if (enemyType == EnemyType.boss)
        {
            spawner.activeBossesCount--;
        }
    }
}
