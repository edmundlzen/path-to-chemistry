using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = 5f;
    public VariableJoystick variableJoystick;
    public CharacterController characterController;
    public Transform cameraTransform;
    public float cameraSensitivity;
    Vector3 moveDirection = Vector3.zero;
    int leftFingerId, rightFingerId;
    float halfScreenWidth;
    Vector2 lookInput;
    float cameraPitch;
    // Start is called before the first frame update
    void Start()
    {

        leftFingerId = -1;
        rightFingerId = -1;

        halfScreenWidth = Screen.width / 2;

    }

    // Update is called once per frame
    void Update()
    {

        GetTouchInput();
        JoystickMove();

        if (rightFingerId != -1)
        {

            LookAround();
        }

    }

    void JoystickMove()
    {
        moveDirection = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {

            Touch t = Input.GetTouch(i);

            switch (t.phase)
            {
                case TouchPhase.Began:

                    if (t.position.x < halfScreenWidth && leftFingerId == -1)
                    {
                        leftFingerId = t.fingerId;
                        print("Tracking left finger");
                    }

                    else if (t.position.x > halfScreenWidth && rightFingerId == -1)
                    {

                        rightFingerId = t.fingerId;
                        Debug.Log("Tracking right finger");

                    }

                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:

                    if (t.fingerId == leftFingerId)
                    {

                        leftFingerId = -1;
                        Debug.Log("Stopped tracking left finger");
                    }

                    else if (t.fingerId == rightFingerId)
                    {

                        rightFingerId = -1;
                        Debug.Log("Stopped tracking right finger");
                    }

                    break;
                case TouchPhase.Moved:

                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime;
                    }

                    break;
                case TouchPhase.Stationary:

                    if (t.fingerId == rightFingerId)
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
