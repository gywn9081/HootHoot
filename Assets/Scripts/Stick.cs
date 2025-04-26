using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private Transform basePoint;
    public float yawFloat;
    public float pitchFloat;
    public float rollFloat;
    // this is a number between -1 and 1, at .8, it locks until .9 (aka you will never get a number between .8 and .9),
    // where it then locks to 1 (afterburners)
    
    //315-360, 0-45

    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion tempRotation = transform.localRotation;

        transform.rotation = new Quaternion(0, 0, 0, 0);

        float tempY = tempRotation.eulerAngles.y;

        if(!(tempY >= 315 || tempY <= 45))
        {
            if(tempY < 180)
            {
                tempY = 45;
            }
            else
            {
                tempY = 315;
            }
        }

        if(tempY >= 315)
        {
            tempY -= 360;
        }

        tempY /= 45;

        yawFloat = tempY;

        // pitchFloat = transform.localRotation.
    }
}
