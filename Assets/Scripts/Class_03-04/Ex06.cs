using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex06 : MonoBehaviour
{
    public Transform target;
    public Ex05_WedgeTrigger trigger;
    public Transform gunTf;//gunTransform
    public float smoothingFactor = 1;

    private void Update()
    {
        if (trigger.Contains(target.position))
        {
            //note: world space
            Vector3 vecToTarget = target.position - gunTf.position;//não preciso normalizar

            Quaternion targetRotation = Quaternion.LookRotation(vecToTarget, transform.up);

            gunTf.rotation = Quaternion.Slerp(gunTf.rotation, targetRotation, smoothingFactor * Time.deltaTime);

            // gunTf.rotation = Quaternion.LookRotation(vecToTarget, transform.up );//Fazendo turreta olhar para target sem smooth
        }
        else
        {

        }
    }

}
