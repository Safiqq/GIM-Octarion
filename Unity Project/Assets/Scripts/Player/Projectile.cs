using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public char character = '\0';
    float speed;
    Rigidbody2D rb;
    InputAssets.PlayerController playerController;
    GameObject target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GameObject.Find("Player").GetComponentInChildren<InputAssets.PlayerController>();

        speed = playerController.projectileSpeed;
    }

    private void Start()
    {
        rb.velocity = Vector2.up * speed;
    }
    private void FixedUpdate()
    {
        Vector2 currentSpeed = Vector2.zero;

        target = playerController.currentTarget;

        if (target)
        {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, (target.transform.position - transform.position).normalized * speed, ref currentSpeed, Time.deltaTime);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() && playerController.currentTarget == collision.gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}
