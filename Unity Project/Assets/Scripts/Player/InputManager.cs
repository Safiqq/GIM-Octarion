using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace InputAssets
{
	public class InputManager : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;


#if ENABLE_INPUT_SYSTEM
		
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}

}