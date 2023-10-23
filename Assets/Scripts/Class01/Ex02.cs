using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex02 : MonoBehaviour
{
    [SerializeField] private int maxHits = 5;

    private int hits = 0;
    private Vector3 OriginalPosition;

    private void Start()
    {
        OriginalPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        //if (hits > maxHits) return;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(OriginalPosition, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            hits++;
            Debug.DrawRay(OriginalPosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            OriginalPosition = hit.point;
        }
        else
        {
            Debug.DrawRay(OriginalPosition, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
