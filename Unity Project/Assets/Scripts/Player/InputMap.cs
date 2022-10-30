using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace InputAssets
{
    public class InputMap : MonoBehaviour
    {
        private PlayerInput playerInput;
        private string defaultMap;
        private Coroutine currentCoroutine = null;

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            defaultMap = playerInput.defaultActionMap;
        }

        // change player input map


        // change player input map with delay between transition
        public void ChangeMap(string newMap, float transitionTime = 0)
        {
            playerInput.currentActionMap.Disable();

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(DelayedChangeMap(newMap, transitionTime));
        }

        IEnumerator DelayedChangeMap(string newMap, float transitionTime)
        {
            yield return new WaitForSeconds(transitionTime);

            playerInput.SwitchCurrentActionMap(newMap);
        }
    }

}
