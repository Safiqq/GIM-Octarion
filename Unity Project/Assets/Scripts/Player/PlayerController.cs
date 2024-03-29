using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace InputAssets
{
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class PlayerController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Player's health")]
		public int health = 3;
		[Tooltip("Enemies killed")]
		public int count = 0;
		[Tooltip("Player's speed")]
		public int speed = 1;

		[Tooltip("Tells whether player is targeting an enemy")]
		public bool isTargeting = false;
        [Tooltip("The enemy that player is currently targeting")]
		public GameObject currentTarget = null;
		[Tooltip("Projectile prefab")]
		public GameObject projectilePrefab;
		[Tooltip("Projectile speed")]
		public float projectileSpeed = 10;

		private Rigidbody2D _rb;
		private Queue projectiles = new Queue();

#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private InputManager _input;
		private GameObject _mainCamera;
		private Spawner spawner;
		private int targetIdx = 0;
		[SerializeField] public TMP_Text HPtext;

		private bool IsCurrentDeviceMouse
		{
			get
			{
#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
			}
		}

		private void Awake()
		{
			GameObject newProjectile;

			spawner = GameObject.Find("Enemy Spawner").GetComponent<Spawner>();

			for (int i = 0; i < 50; i++)
			{
				newProjectile = Instantiate(projectilePrefab, transform);
				newProjectile.SetActive(false);

				projectiles.Enqueue(newProjectile);
			}
			HPtext.text = "" + health;
		}

		private void Start()
		{
			_rb = GetComponent<Rigidbody2D>();
			_input = GetComponent<InputManager>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#endif

		}

		private void FixedUpdate()
		{
			// put Move function in fixedUpdate if objects movement is also calculated in fixedUpdate
			Move();
			
		}

		private void Update()
		{
			ChangeTarget();
		}

		private void LateUpdate()
		{

		}


		private void Move()
		{
			Vector2 inputVelocity = _input.move;
			Vector2 currentVelocity = Vector2.zero;
			Vector2 newVelocity = ((Vector2.up * inputVelocity.y) + (Vector2.right * inputVelocity.x)).normalized * speed;

			_rb.velocity = Vector2.SmoothDamp(_rb.velocity, newVelocity, ref currentVelocity, Time.deltaTime);
		}

		private void ChangeTarget()
        {

			if (_input.target)
            {
				// DumpToConsole(_input);
				isTargeting = !isTargeting;

				if (!isTargeting)
                {
					CancelTarget();
                }
            }

			if (isTargeting)
            {
				if (currentTarget == null)
                {
					if (spawner.activeEnemies.Count > 0)
                    {
						PickTarget(0);
					}
                }

				else if (_input.changeTarget || currentTarget.GetComponent<Enemy>().enemyState == Enemy.EnemyState.dead)
                {
					FindTarget();
                }
            }

			// remove currentTarget after exiting target mode
			else if (currentTarget != null)
            {
				currentTarget = null;
            }
        }

		private void FindTarget()
        {
			int i;
			bool found;

			if (spawner.activeEnemies.Count == 0)
            {
				CancelTarget();
            }

			else
            {
				i = (targetIdx + 1) % spawner.activeEnemies.Count;
				found = false;

				while (i < spawner.activeEnemies.Count && !found && i != targetIdx)
				{
					if (currentTarget != spawner.activeEnemies[i])
					{
						CancelTarget();
						PickTarget(i);
						found = true;
					}

					else
					{
						i = (i + 1) % spawner.activeEnemies.Count;
					}
				}
			}
		}

		private void PickTarget(int idx)
        {
			currentTarget = spawner.activeEnemies[idx];
			currentTarget.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
			currentTarget.GetComponent<Enemy>().isOnTarget = true;

			targetIdx = idx;
		}

		private void CancelTarget()
        {
			currentTarget.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
			currentTarget.GetComponent<Enemy>().isOnTarget = false;
			currentTarget = null;

			targetIdx = -1;
		}
		public void Shoot(char character = '\0')
		{
			GameObject currentProjectile = (GameObject) projectiles.Dequeue();
			currentProjectile.transform.position = transform.position;
			projectiles.Enqueue(currentProjectile);

			currentProjectile.GetComponent<Projectile>().character = character;
			currentProjectile.SetActive(true);
		}

		private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
				if(collision.gameObject.GetComponent<Enemy>().health > 0)
				{
					health--;
				}
				else
				{
					count++;
				}
				HPtext.text = "" + health;
				if (health <= 0)
                {
					SceneManager.LoadScene("Game Over");
                }
				if (count >= 10)
				{
					SceneManager.LoadScene("Win");
				}
            }
        }
    }
}