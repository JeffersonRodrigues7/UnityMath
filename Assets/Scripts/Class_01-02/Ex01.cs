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
            Debug.Log($"Dist�ncia bomba-player: {distance}, Player ser� morto");
        }
        else
        {
            Debug.Log($"Dist�ncia bomba-player: {distance}, Player n�o ser� morto");
        }
    }
}
