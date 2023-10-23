using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex01 : MonoBehaviour
{
    [SerializeField] private float radius = 1.0f;
    [SerializeField] private Transform bombPosition;
    [SerializeField] private Transform playerPosition;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bombPosition.position, radius);

        Vector2 distance = bombPosition.position - playerPosition.position;
        float length = Mathf.Sqrt(distance.x*distance.x + distance.y*distance.y);
        Gizmos.DrawLine(bombPosition.position, playerPosition.position);

        if(length <= radius)
        {
            Debug.Log($"Distância bomba-player: {distance}, Player será morto");
        }
        else
        {
            Debug.Log($"Distância bomba-player: {distance}, Player não será morto");
        }
    }
}
