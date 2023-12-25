using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex04 : MonoBehaviour
{
    public Transform turret;

    public void OnDrawGizmos()
    {
        if (turret == null) return;

        Ray ray = new Ray(transform.position, transform.forward); // transform.forward = Z axis(azul), para onde está olhando

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            turret.position = hit.point;//Movendo turret para posição que a camera está olhando

            //Grahm-Schmidt orthonormalization
            Vector3 yAxis = hit.normal; //Pegando normal da superficie

            /**Calculando Z(Forward), que é a direção que a turreta deve olhar a partir do CrossProduct o eixo x da camera(transform.right) 
             * e a normal da superfície
             * Precisamos normalizar o valor, pois caso tenhamos angulos muito rasos, ou seja, caso o vetor que estamos olhando
             * e o vetor normal estejam muito similares, o tamanho do vetor z pode acabar sendo afetado
             */
            Vector3 zAxis = Vector3.Cross(transform.right, yAxis).normalized;

            /**A função Quaternion.LookRotation dá uma orientação baseado em uma forwardDirection(Z, azul) e UpDirection(Y(Normal), verde)
                */
            turret.rotation = Quaternion.LookRotation(zAxis, yAxis);

            Gizmos.color = Color.white; //Desenhando um raycast entre a camera a o ponto na superficie que ela bate
            Gizmos.DrawLine(ray.origin, hit.point);

            Gizmos.color = Color.green; //Desenhando a normal da superficie
            Gizmos.DrawRay(hit.point, yAxis);

            Gizmos.color = Color.blue; //Desenhando o vetor z, direção que a turreta deve olhar
            Gizmos.DrawRay(hit.point, zAxis);
        }
    }

}
