using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace InputAssets
{
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class PlayerController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;

#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private InputManager _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

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

		}

		private void Start()
		{
			_input = GetComponent<InputManager>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			
#endif

		}

		private void FixedUpdate()
		{
			// put Move function in fixedUpdate if objects movement is also calculated in fixedUpdate
			Move();
			
		}

		private void Update()
		{
		
		}

		private void LateUpdate()
		{

		}


		private void Move()
		{
			
		}
	}
}