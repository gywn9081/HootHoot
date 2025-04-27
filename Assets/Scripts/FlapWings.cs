using UnityEngine;
using UnityEngine.InputSystem;

public class FlapWings : MonoBehaviour
{
    [SerializeField] InputActionReference leftHand;
    [SerializeField] InputActionReference rightHand;

    [SerializeField] float thrustConstant = 10f;
    [SerializeField] float gravityConstant = 0.2f;

    [SerializeField] Transform bodyTransform;
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

        Vector2 distance = new Vector2(currentVectorRight.x - currentVectorLeft.x, currentVectorRight.z - currentVectorLeft.z);

        bodyTransform.position += (toApplyRight + toApplyLeft) * Time.deltaTime;

        if(distance.magnitude == 0f)
        {
            bodyTransform.position += Vector3.up * 9.81f * gravityConstant * 20 * Time.deltaTime;
        }
        else
        {
            bodyTransform.position += Vector3.down * 9.81f * gravityConstant * 1/distance.magnitude * Time.deltaTime;
        }
        
        rightHandVector = rightHand.action.ReadValue<Vector3>();
        leftHandVector = leftHand.action.ReadValue<Vector3>();

        if(bodyTransform.position.y < 50f)
        {
            bodyTransform.position = new Vector3(bodyTransform.position.x, -5f, bodyTransform.position.z);
        }
    }
}
