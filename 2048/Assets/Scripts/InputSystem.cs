using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InputOptions
{
    [Space] public string name = null;
    [Space] public KeyCode[] keyCode = new KeyCode[0];
    [Space] public UnityEvent events;
}

namespace Unity.InputSystem
{
    public class InputSystem : MonoBehaviour
    {
        public InputOptions[] inputOptions;

        private void Awake()
        {
            #if UNITY_EDITOR

            if (IncompletedFields(this.inputOptions))
            {
                UnityEditor.EditorApplication.isPlaying = false;
                throw new System.Exception("Some fields are incompleted");
            }

            if (CheckNonRepeatedKeys(AllKeysInOne()))
            {
                UnityEditor.EditorApplication.isPlaying = false;
                throw new System.Exception("Keys are repeated");
            }

            #endif
        }

        private void Update()
        {
            for (int a = 0; a < this.inputOptions.Length; a++)
                if (DetectKeyDown(this.inputOptions[a].keyCode)) this.inputOptions[a].events.Invoke();
        }

        private bool DetectKeyDown(KeyCode[] keys)
        {
            bool pressed = false;

            for (int a = 0; a < keys.Length; a++)
                if (Input.GetKeyDown(keys[a])) return true;

            return pressed;
        }

        private KeyCode[] AllKeysInOne()
        {
            int keyNumber = 0;

            for (int a = 0; a < this.inputOptions.Length; a++)
                for (int b = 0; b < this.inputOptions[a].keyCode.Length; b++)
                    keyNumber++;

            KeyCode[] allKeys = new KeyCode[keyNumber];
            keyNumber = 0;

            for (int a = 0; a < this.inputOptions.Length; a++)
                for (int b = 0; b < this.inputOptions[a].keyCode.Length; b++)
                {
                    allKeys[keyNumber] = this.inputOptions[a].keyCode[b];
                    keyNumber++;
                }

            return allKeys;

        }

        private bool CheckNonRepeatedKeys(KeyCode[] keys)
        {
            bool isRepeatedKeys = false;

            for (int a = 0; a < keys.Length; a++)
                for (int b = 0; b < keys.Length; b++)
                {
                    if (a == b) continue;

                    if (keys[a].Equals(keys[b]))
                    {
                        isRepeatedKeys = true;
                        break;
                    }
                }

            return isRepeatedKeys;
        }

        private bool IncompletedFields(InputOptions[] inputOptions)
        {
            bool areIncompletedFields = false;

            for (int a = 0; a < inputOptions.Length; a++)
                if (inputOptions[a].name == null || inputOptions[a].keyCode.Length == 0 || inputOptions[a].events.GetPersistentEventCount() == 0)
                {
                    areIncompletedFields = true;
                    break;
                }

            return areIncompletedFields;
        }
    }
}