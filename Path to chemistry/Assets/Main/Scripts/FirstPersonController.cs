using UnityEngine;

    public class FirstPersonController : MonoBehaviour
    {
        public float speed = 10f;
        public float gravity = 5f;
        public VariableJoystick variableJoystick;
        public CharacterController characterController;
        public Transform cameraTransform;
        public float cameraSensitivity;
        private Vector3 _moveDirection = Vector3.zero;
        int _leftFingerId, _rightFingerId;
        float halfScreenWidth;
        Vector2 lookInput;
        float cameraPitch;
        // Start is called before the first frame update
        void Start()
        {

            _leftFingerId = -1;
            _rightFingerId = -1;

            halfScreenWidth = Screen.width / 2;

        }

        // Update is called once per frame
        void Update()
        {

            GetTouchInput();
            JoystickMove();

            if (_rightFingerId != -1)
            {

                LookAround();
            }

        }

        void JoystickMove()
        {
            _moveDirection = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical);
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= speed;

            _moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(_moveDirection * Time.deltaTime);
        }

        void GetTouchInput()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {

                Touch t = Input.GetTouch(i);

                switch (t.phase)
                {
                    case TouchPhase.Began:

                        if (t.position.x < halfScreenWidth && _leftFingerId == -1)
                        {
                            _leftFingerId = t.fingerId;
                            print("Tracking left finger");
                        }

                        else if (t.position.x > halfScreenWidth && _rightFingerId == -1)
                        {

                            _rightFingerId = t.fingerId;
                            Debug.Log("Tracking right finger");

                        }

                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:

                        if (t.fingerId == _leftFingerId)
                        {

                            _leftFingerId = -1;
                            Debug.Log("Stopped tracking left finger");
                        }

                        else if (t.fingerId == _rightFingerId)
                        {

                            _rightFingerId = -1;
                            Debug.Log("Stopped tracking right finger");
                        }

                        break;
                    case TouchPhase.Moved:

                        if (t.fingerId == _rightFingerId)
                        {
                            lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime;
                        }

                        break;
                    case TouchPhase.Stationary:

                        if (t.fingerId == _rightFingerId)
                        {

                            lookInput = Vector2.zero;
                        }
                        break;
                }
            }
        }

        void LookAround()
        {
            cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

            transform.Rotate(transform.up, lookInput.x);
        }
    }
