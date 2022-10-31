using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using TMPro;

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

		[Tooltip("Tells whether player is targeting an enemy")]
		public bool isTargeting = false;
        [Tooltip("The enemy that player is currently targeting")]
		public GameObject currentTarget = null;

#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private InputManager _input;
		private GameObject _mainCamera;
		private Spawner spawner;
		private int targetIdx = 0;

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
			spawner = GameObject.Find("Enemy Spawner").GetComponent<Spawner>();
		}

		private void Start()
		{
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
			
		}

		private void ChangeTarget()
        {

			if (_input.target)
            {
				isTargeting = !isTargeting;
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

			targetIdx = idx;
		}

		private void CancelTarget()
        {
			currentTarget.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
			currentTarget = null;

			targetIdx = -1;
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
				health--;

				if (health <= 0)
                {

                }
            }
        }
    }
}