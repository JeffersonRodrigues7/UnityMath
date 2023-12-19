using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_02_SolutionsAssignments_1_3 : MonoBehaviour
{
    [SerializeField] private bool Ex01 = false;
    [SerializeField] private bool Ex02 = false;
    [SerializeField] private bool Ex03 = false;

    [Header("01 - Radial Trigger")]
    [SerializeField] private float radius = 1.0f;
    [SerializeField] private Transform bombPosition;
    [SerializeField] private Transform playerPosition;

    [Header("02 - Bouncing Laser")]
    [SerializeField] private int maxHits = 5;
    private int hits = 0;
    private Vector3 OriginalPosition;

    private void OnDrawGizmos()
    {
        if (Ex01) radialTrigger();
        else if (Ex02) bouncingLaser();
        //else if (Ex03) 
    }

    /**Se objeto entrar no raio da bomba ele será morto */
    private void radialTrigger()
    {
        Gizmos.DrawWireSphere(bombPosition.position, radius);

        Vector2 distance = bombPosition.position - playerPosition.position;
        float length = Mathf.Sqrt(distance.x * distance.x + distance.y * distance.y);
        Gizmos.DrawLine(bombPosition.position, playerPosition.position);

        if (length <= radius)
        {
            Debug.Log($"Distância bomba-player: {distance}, Player será morto");
        }
        else
        {
            Debug.Log($"Distância bomba-player: {distance}, Player não será morto");
        }
    }

    private void bouncingLaser()
    {
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
