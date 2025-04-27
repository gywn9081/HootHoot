using UnityEngine;
using UnityEngine.InputSystem;

public class testScript : MonoBehaviour
{
    [SerializeField] InputActionReference leftHand;
    [SerializeField] InputActionReference rightHand;
    [SerializeField] InputActionReference headset;

    [SerializeField] InputActionReference leftHandQuaternion;
    [SerializeField] InputActionReference rightHandQuaternion;

    [SerializeField] InputActionReference grabActionLeft;
    [SerializeField] InputActionReference grabActionRight;

    PlanePhysics planeScript;



    Vector3 rightHandVector;
    Vector3 leftHandVector;

    Quaternion rightHandRotation;
    Quaternion leftHandRotation;

    public Stick stickScript;
    public Thruster thrusterScript;



    float tolerance = 1f;

    [SerializeField] Transform stickTransform;
    [SerializeField] Transform thrusterTransform;

    void Update()
    {
        // Plane.AddForce(physics.getForceVector(0, 1000, 100000, 1, 2000));
        Debug.Log($"{leftHand.action.ReadValue<Vector3>()}, {rightHand.action.ReadValue<Vector3>()}, {headset.action.ReadValue<Vector3>()}");

        rightHandVector = rightHand.action.ReadValue<Vector3>();
        leftHandVector = leftHand.action.ReadValue<Vector3>();

        // rightHandRotation = rightHandQuaternion.action.ReadValue<Quaternion>();
        // leftHandRotation = leftHandQuaternion.action.ReadValue<Quaternion>();

        if((leftHandVector.x - stickTransform.position.x) < tolerance && (leftHandVector.y - stickTransform.position.y) < tolerance && (leftHandVector.z - stickTransform.position.z) < tolerance && grabActionLeft.action.ReadValue<float>() > 0.1f)
        {
            stickTransform.position = new Vector3(leftHandVector.x, leftHandVector.y, leftHandVector.z);
            stickTransform.rotation = leftHandQuaternion.action.ReadValue<Quaternion>();
        }
        else if((rightHandVector.x - stickTransform.position.x) < tolerance && (rightHandVector.y - stickTransform.position.y) < tolerance && (rightHandVector.z - stickTransform.position.z) < tolerance && grabActionRight.action.ReadValue<float>() > 0.1f)
        {
            stickTransform.position = new Vector3(rightHandVector.x, rightHandVector.y, rightHandVector.z);
            stickTransform.rotation = rightHandQuaternion.action.ReadValue<Quaternion>();
        }

        stickScript.UpdateStick();

        thrusterScript.UpdateThruster();
    }
}
