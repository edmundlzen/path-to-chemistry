using UnityEngine;
using UnityEngine.Serialization;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = 5f;
    public VariableJoystick variableJoystick;
    public GameObject joystick;
    public GameObject useButton;
    public CharacterController characterController;
    public Transform cameraTransform;
    public float cameraSensitivity = 1f;
    public bool freeze = false;

    [SerializeField] private float jumpSpeed = 5f;

    [SerializeField] private bool desktopInput;

    private int _leftFingerId, _rightFingerId;
    private Vector3 _moveDirection = Vector3.zero;
    private float cameraPitch;
    private float halfScreenWidth;
    private Vector2 lookInput;
    private bool touchWeaponOn;

    void Start()
    {
        if (!desktopInput)
        {
            _leftFingerId = -1;
            _rightFingerId = -1;

            halfScreenWidth = Screen.width / 2;
            return;
        }

        joystick.SetActive(false);
        // useButton.SetActive(false);
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze) return;

        if (!desktopInput)
        {
            GetTouchInput();
            JoystickMove();

            return;
        }

        // We are grounded, so recalculate move direction based on axes
        var forward = transform.TransformDirection(Vector3.forward);
        var right = transform.TransformDirection(Vector3.right);
        var curSpeedX = speed * Input.GetAxis("Vertical");
        var curSpeedY = speed * Input.GetAxis("Horizontal");
        var movementDirectionY = _moveDirection.y;
        _moveDirection = forward * curSpeedX + right * curSpeedY;

        if (Input.GetButton("Jump") && !freeze && characterController.isGrounded)
            _moveDirection.y = jumpSpeed;
        else
            _moveDirection.y = movementDirectionY;

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded) _moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(_moveDirection * Time.deltaTime);

        cameraPitch += -Input.GetAxis("Mouse Y") * cameraSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * cameraSensitivity, 0);
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
            for (var i = 0; i < Input.touchCount; i++)
            {
                var t = Input.GetTouch(i);

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
                            lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime;

                        break;
                    case TouchPhase.Stationary:

                        if (t.fingerId == _rightFingerId) lookInput = Vector2.zero;

                        break;
                }
            }
        }
    }