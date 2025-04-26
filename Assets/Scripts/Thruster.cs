using UnityEngine;

public class Thruster : MonoBehaviour
{
    [SerializeField] private Transform baseTransform;
    [SerializeField] private float baseTransformLength = 0.5f; // this is the length of the base transform divided by to (used to clamp the thruster)
    public float toThrust; 
    // this is a number between -1 and 1, at .8, it locks until .9 (aka you will never get a number between .8 and .9),
    // where it then locks to 1 (afterburners)
    
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 tempPosition = new Vector3(baseTransform.position.x, baseTransform.position.y + 1.2f, transform.position.z);
        tempPosition.z = Mathf.Clamp(tempPosition.z, baseTransform.position.z - baseTransformLength, baseTransform.position.z + baseTransformLength);

        if(tempPosition.z > (baseTransform.position.z + baseTransformLength * 0.8f))
        {
            if(tempPosition.z < (baseTransform.position.z + baseTransformLength * 0.9f))
            {
                tempPosition.z = baseTransform.position.z + baseTransformLength * 0.8f;
            }
            else
            {
                tempPosition.z = baseTransform.position.z + baseTransformLength;
            }
        }

        transform.position = tempPosition;
        transform.rotation = new Quaternion(0, 0, 0, 0);

        toThrust = transform.localPosition.z * 2;

        transform.localScale = new Vector3(1, 1, 0.2f);
    }
}
