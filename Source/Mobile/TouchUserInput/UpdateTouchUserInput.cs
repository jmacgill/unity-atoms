using UnityEngine;

namespace UnityAtoms.Mobile
{
    /* Updates the TouchUserInputVariable on every Update tick. Meant to be placed on a OnUpdateMonoHook.
    */
    [CreateAssetMenu(menuName = "Unity Atoms/Mobile/Touch User Input/Update (OnUpdateMonoHook)", fileName = "UpdateTouchUserInputVariable")]
    public sealed class UpdateTouchUserInput : VoidAction
    {
        public TouchUserInputVariable TouchUserInputVariable;

        private TouchUserInput.State _inputState = TouchUserInput.State.None;

        private Vector2 _inputPos = Vector2.zero;

        private Vector2 _inputPosLastFrame = Vector2.zero;

        private Vector2 _inputPosLastDown = Vector2.zero;

        public override void Do()
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                _inputPos = Input.GetTouch(0).position;
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    _inputPosLastDown = _inputPos;
                    _inputState = TouchUserInput.State.Down;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    _inputState = TouchUserInput.State.Up;
                }
                else
                {
                    _inputState = TouchUserInput.State.Drag;
                }
            }
            else
            {
                _inputPos = Vector2.zero;
                _inputState = TouchUserInput.State.None;
            }
#elif UNITY_EDITOR || UNITY_STANDALONE
            _inputPos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                _inputPosLastDown = _inputPos;
                _inputState = TouchUserInput.State.Down;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _inputState = TouchUserInput.State.Up;
            }
            else if (Input.GetMouseButton(0))
            {
                _inputState = TouchUserInput.State.Drag;
            }
            else
            {
                _inputState = TouchUserInput.State.None;
            }
#endif

            TouchUserInputVariable.SetValue(new TouchUserInput(_inputState, _inputPos, _inputPosLastFrame, _inputPosLastDown));
            _inputPosLastFrame = _inputPos;
        }
    }
}
