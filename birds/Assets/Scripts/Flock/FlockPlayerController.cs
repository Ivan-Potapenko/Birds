using UnityEngine;
using Sirenix.OdinInspector;
using UI;


namespace GamePlay
{
    public class FlockPlayerController : MonoBehaviour
    {

        [SerializeField]
        [ValidateInput(nameof(ValidateFlock), defaultMessage: "There is no Flock component on the object")]
        private Flock _flock;

        private bool ValidateFlock()
        {
            return GetComponent<Flock>();
        }

        [SerializeField]
        private Joystick _joystick;

        [SerializeField]
        private float _minDistanceToMoveBetwinMouseAndFlock;

        
        [SerializeField]
        private Vector2 _moveVector;

        void Start()
        {
            _flock = GetComponent<Flock>();
        }


        private void FixedUpdate()
        {
            MoveFlock();
        }


        private void MoveFlock()
        {
            if (_joystick.JoystickVector != Vector2.zero)
            {

                _flock.MoveFLockAndBreakStopCoroutine((_joystick.JoystickVector).normalized);
            }
            else
            if (_moveVector != Vector2.zero)
            {
                _flock.StopFlock(_moveVector.normalized);
            }
            _moveVector = _joystick.JoystickVector;
        }
    }
}