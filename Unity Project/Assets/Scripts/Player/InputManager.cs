using UnityEngine;
using System.Collections;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace InputAssets
{
	public class InputManager : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		[Header("Alphabet Input Values")]
		public bool[] alphabets = new bool[26];
		[Header("Capslock Input Value")]
		public bool capsLock;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;


#if ENABLE_INPUT_SYSTEM
		
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnA(InputValue value)
		{
			AlphabetInput(value.isPressed, 0);
		}
		public void OnB(InputValue value)
		{
			AlphabetInput(value.isPressed, 1);
		}
		public void OnC(InputValue value)
		{
			AlphabetInput(value.isPressed, 2);
		}
		public void OnD(InputValue value)
		{
			AlphabetInput(value.isPressed, 3);
		}
		public void OnE(InputValue value)
		{
			AlphabetInput(value.isPressed, 4);
		}
		public void OnF(InputValue value)
		{
			AlphabetInput(value.isPressed, 5);
		}
		public void OnG(InputValue value)
		{
			AlphabetInput(value.isPressed, 6);
		}
		public void OnH(InputValue value)
		{
			AlphabetInput(value.isPressed, 7);
		}
		public void OnI(InputValue value)
		{
			AlphabetInput(value.isPressed, 8);
		}
		public void OnJ(InputValue value)
		{
			AlphabetInput(value.isPressed, 9);
		}
		public void OnK(InputValue value)
		{
			AlphabetInput(value.isPressed, 10);
		}
		public void OnL(InputValue value)
		{
			AlphabetInput(value.isPressed, 11);
		}
		public void OnM(InputValue value)
		{
			AlphabetInput(value.isPressed, 12);
		}
		public void OnN(InputValue value)
		{
			AlphabetInput(value.isPressed, 13);
		}
		public void OnO(InputValue value)
		{
			AlphabetInput(value.isPressed, 14);
		}
		public void OnP(InputValue value)
		{
			AlphabetInput(value.isPressed, 15);
		}
		public void OnQ(InputValue value)
		{
			AlphabetInput(value.isPressed, 16);
		}
		public void OnR(InputValue value)
		{
			AlphabetInput(value.isPressed, 17);
		}
		public void OnS(InputValue value)
		{
			AlphabetInput(value.isPressed, 18);
		}
		public void OnT(InputValue value)
		{
			AlphabetInput(value.isPressed, 19);
		}
		public void OnU(InputValue value)
		{
			AlphabetInput(value.isPressed, 20);
		}
		public void OnV(InputValue value)
		{
			AlphabetInput(value.isPressed, 21);
		}
		public void OnW(InputValue value)
		{
			AlphabetInput(value.isPressed, 22);
		}
		public void OnX(InputValue value)
		{
			AlphabetInput(value.isPressed, 23);
		}
		public void OnY(InputValue value)
		{
			AlphabetInput(value.isPressed, 24);
		}
		public void OnZ(InputValue value)
		{
			AlphabetInput(value.isPressed, 25);
		}

#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void AlphabetInput(bool newValue, int idx)
		{
            alphabets[idx] = newValue;
			StartCoroutine(CancelAlphabetInput(idx));
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		IEnumerator CancelAlphabetInput(int idx)
        {
            yield return new WaitForEndOfFrame();
            alphabets[idx] = false;
        }

        private void Awake()
        {

        }
    }

}