using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { mobs, bomb, boss};
    public enum EnemyState { live, dead}

    private string[] kata = new[]
    {
        "sendiri", "bukan", "sejak", "dia", "bila", "terhadap", "akan", "terjadi", "jumlah", "jauh",
        "tidak", "bagian", "juga", "sudah", "antara", "uang", "ada", "hidup", "baik", "kepada",
        "kami", "merupakan", "masuk", "tapi", "punya", "sebab", "bahwa", "ini", "ketika", "bukan"
    };

    [Header("Enemy Properties")]
    [Tooltip("Enemy type")]
    public EnemyType enemyType;
    [Tooltip("Text to type in order to destroy enemy")]
    private string text;
    [Tooltip("Enemy speed toward player")]
    public int health;

    public float speed = 1;
    [Tooltip("Angle from y axis which represent enemy's angle of view when chasing player (in degrees)")]
    public float viewAngle = 60;
    [Tooltip("Force magnitude when enemy collides with player")]
    public float knockBackMagnitude = 0;
    [Tooltip("Enemy canvas to show its text")]
    public Canvas enemyCanvas;
    [Tooltip("Enemy's current state")]
    public EnemyState enemyState;
    [Tooltip("Tells whether player is targeting on the enemy or not")]
    public bool isOnTarget = false;

    private Transform playerTransform;
    private Rigidbody2D rb, playerRB;
    private InputAssets.InputManager _input;
    private bool ignoreLimit = false;
    private Spawner spawner;
    private float yLowerBound = -10;
    private TextMeshProUGUI textUI;
    private InputAssets.PlayerController playerController;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").GetComponentInChildren<InputAssets.PlayerController>().transform;
        _input = playerTransform.GetComponent<InputAssets.InputManager>();
        playerRB = playerTransform.GetComponent<Rigidbody2D>();
        playerController = playerTransform.GetComponent<InputAssets.PlayerController>();

        spawner = GameObject.Find("Enemy Spawner").GetComponent<Spawner>();

        enemyCanvas = GetComponentInChildren<Canvas>();
        textUI = enemyCanvas.GetComponentInChildren<TextMeshProUGUI>();

        enemyState = EnemyState.live;

        rb = GetComponent<Rigidbody2D>();
        ignoreLimit = false;
    }

    void Start()
    {
        int rand = Random.Range(0, kata.Length - 1);
        text = kata[rand];
        health = text.Length;
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
        textUI.text = text;
        DetectType();
    }

    void LateUpdate()
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

        if (text.Length > 0 && isOnTarget)
        {
            currentChar = text[0];

            if (currentChar >= 'a' && currentChar <= 'z')
            {
                idx = currentChar - 'a';
              
                if (_input.alphabets[idx])
                {
                    text = text.Remove(0, 1);
                    playerController.Shoot(currentChar);
                }
            }
        }
    }

    void DetectLife()
    {
        if (health == 0 && enemyState == EnemyState.live)
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        // hitting player
        if (collision.gameObject.GetComponent<InputAssets.PlayerController>())
        {
            HitPlayer();
        }

        // hitting projectile
        else if (collision.gameObject.GetComponent<Projectile>() && gameObject == collision.gameObject.GetComponent<Projectile>().target)
        {
            health--;
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
        enemyState = EnemyState.dead;
        gameObject.SetActive(false);

        UpdateSpawnerCount();
        spawner.activeEnemies.Remove(gameObject);
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
