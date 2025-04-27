using UnityEngine;
using UnityEngine.InputSystem;

public class FlapWings : MonoBehaviour
{
    // TODO - tuck in arms = fall, out = not fall
    [SerializeField] InputActionReference leftHand;
    [SerializeField] InputActionReference rightHand;

    [SerializeField] float thrustConstant = 10f;

    [SerializeField] Rigidbody rb;
    Vector3 rightHandVector;
    Vector3 leftHandVector;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rightHandVector = rightHand.action.ReadValue<Vector3>();
        leftHandVector = leftHand.action.ReadValue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toApplyRight = Vector3.zero;
        Vector3 toApplyLeft = Vector3.zero;

        Vector3 currentVectorRight = rightHand.action.ReadValue<Vector3>();
        Vector3 currentVectorLeft = leftHand.action.ReadValue<Vector3>();


        if(rightHandVector.z - currentVectorRight.z > 0)
        {
            toApplyRight.z = (rightHandVector.z - currentVectorRight.z) * thrustConstant;
        }
        if(rightHandVector.x - currentVectorRight.x > 0)
        {
            toApplyRight.x = (rightHandVector.x - currentVectorRight.x) * thrustConstant;
        }
        if(rightHandVector.y - currentVectorRight.y > 0)
        {
            toApplyRight.y = (rightHandVector.y - currentVectorRight.y) * thrustConstant;
        }
        if(leftHandVector.x - currentVectorLeft.x > 0)
        {
            toApplyLeft.x = (leftHandVector.x - currentVectorLeft.x) * thrustConstant;
        }
        if(leftHandVector.y - currentVectorLeft.y > 0)
        {
            toApplyLeft.y = (leftHandVector.y - currentVectorLeft.y) * thrustConstant;
        }
        if(leftHandVector.z - currentVectorLeft.z > 0)
        {
            toApplyLeft.z = (leftHandVector.z - currentVectorLeft.z) * thrustConstant;
        }

        rb.AddForceAtPosition(transform.TransformDirection(toApplyRight), transform.TransformDirection(new Vector3(0.5f, 0, 0)), ForceMode.Force);
        rb.AddForceAtPosition(transform.TransformDirection(toApplyLeft), transform.TransformDirection(new Vector3(-0.5f, 0, 0)), ForceMode.Force);
        
        rightHandVector = rightHand.action.ReadValue<Vector3>();
        leftHandVector = leftHand.action.ReadValue<Vector3>();
    }
}
