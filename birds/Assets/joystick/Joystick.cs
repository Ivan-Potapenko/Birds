using UnityEngine;

namespace UI
{
    public class Joystick : MonoBehaviour
    {
        [SerializeField]
        private GameObject _stick;

        [SerializeField]
        private float _joystickRadius;

        [SerializeField]
        private int _touchJoystickID;


        [SerializeField]
        private Vector2 _joystickVector;



        public Vector2 JoystickVector { get => _joystickVector; }


        private void Start()
        {
            _touchJoystickID = -1;
        }


        void Update()
        {
            UpdateJoystickDirection();
        }

        private void UpdateJoystickDirection()
        {
            _joystickVector = Vector2.zero;
            _stick.transform.position = gameObject.transform.position;
            if (Input.touchCount > 0)
            {
                if (_touchJoystickID != -1)
                {
                    SetStickPosition();
                }
                else
                {
                    _touchJoystickID = -1;
                    FindNewJoystickTouch();
                }
            }
            else
            {
                _touchJoystickID = -1;
            }
        }

        private Vector2 ConvertVector3ToVector2(Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }


        private void SetStickPosition()
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == _touchJoystickID && Input.touches[i].phase != TouchPhase.Ended)
                {
                    _joystickVector = Input.touches[i].position - ConvertVector3ToVector2(gameObject.transform.position);
                    if ((Input.touches[i].position - ConvertVector3ToVector2(gameObject.transform.position)).magnitude >
                        _joystickRadius)
                    {
                        var stickVector = ConvertVector3ToVector2(gameObject.transform.position) + _joystickVector.normalized *
                            _joystickRadius;
                        _stick.transform.position = new Vector3(stickVector.x, stickVector.y, 0);
                    }
                    else
                    {
                        _stick.transform.position = Input.touches[i].position;
                    }
                    return;

                }
            }
            FindNewJoystickTouch();
        }

        private void FindNewJoystickTouch()
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if ((Input.touches[i].phase != TouchPhase.Ended) &&
                   (Input.touches[i].position - ConvertVector3ToVector2(gameObject.transform.position)).magnitude <=
                   (_joystickRadius + _joystickRadius / 3))
                {
                    Debug.Log("FindNewJoystickTouch()");
                    _touchJoystickID = Input.touches[i].fingerId;
                    _joystickVector = Input.touches[i].position - ConvertVector3ToVector2(gameObject.transform.position);

                    _stick.transform.position = Input.touches[i].position;
                    return;
                }

            }
        }


    }

}
