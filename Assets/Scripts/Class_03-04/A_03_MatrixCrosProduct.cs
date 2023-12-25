using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_03_MatrixCrosProduct : MonoBehaviour
{

    public Transform turret;

public void OnDrawGizmos()
{
    if (turret == null) return;

    Ray ray = new Ray(transform.position, transform.forward); // transform.forward = Z axis(azul), para onde est� olhando

    if(Physics.Raycast(ray, out RaycastHit hit))
    {
        turret.position = hit.point;//Movendo turret para posi��o que a camera est� olhando

        //Grahm-Schmidt orthonormalization
        Vector3 yAxis = hit.normal; //Pegando normal da superficie

        /**Calculando o vetor X resultado do CrossProduct, que � um vetor tangente a superficie e perpendicular ao ray.direction e a normal
            * ray.direction: Dire��o para a qual a camera est� olhando
            * yAxis: Normal da superficie
            * 
            * Observe que se invertemos ray.direction e yAxis o resultado � diferente, em alguns casos, talvez devamos tentar qual traz
            * o resultado correto, pois isso est� relacionando com a regra da m�o esquerda.
            * 
            * Precisamos normalizar o valor, pois caso tenhamos angulos muito rasos, ou seja, caso o vetor que estamos olhando
            * e o vetor normal estejam muito similares, o tamanho do vetor x pode acabar sendo afetado
            */
        Vector3 xAxis = Vector3.Cross(yAxis, ray.direction).normalized;

        /**Calculando Z(Forward), que � a dire��o que a turreta deve olhar a partir do CrossProduct entre X e Y. 
            * Como ambos j� est�o normalizados, o resultado(Z), tbm j� vai estar normalizado, ou seja, com comprimento 1.
            */
        Vector3 zAxis = Vector3.Cross(xAxis, yAxis);

        /**A fun��o Quaternion.LookRotation d� uma orienta��o baseado em uma forwardDirection(Z, azul) e UpDirection(Y(Normal), verde)
            */
        turret.rotation = Quaternion.LookRotation(zAxis, yAxis);

        Gizmos.color = Color.white; //Desenhando um raycast entre a camera a o ponto na superficie que ela bate
        Gizmos.DrawLine(ray.origin, hit.point);

        Gizmos.color = Color.green; //Desenhando a normal da superficie
        Gizmos.DrawRay(hit.point, yAxis);

        Gizmos.color = Color.red; //Desenhando o vetor x, tangente a superficie
        Gizmos.DrawRay(hit.point, xAxis);

        Gizmos.color = Color.blue; //Desenhando o vetor z, dire��o que a turreta deve olhar
        Gizmos.DrawRay(hit.point, zAxis);
        /** Faz a turreta olhar em dire��o ao qual a camera est� olhando, mas n�o exatamente o que queremos, pois desta forma,
            * a turreta vai ficar olhando diretamente para o ch�o
            */
        //turret.rotation = Quaternion.LookRotation(ray.direction);
    }



        //Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix; //Obt�m a matriz de transforma��o localToWorld

        //localToWorldMatrix.MultiplyPoint3x4()//ignora a �ltima linha que � in�til na maioria dos casos

        /**Transforming between local and world
         * Fun��es que utilizamos ao fazer transforma��es no espa�o de algum objeto 
         * M = LocalToWorld Matrix
         */
        //transform.TransformPoint(localOffset)          // M* (v.x, v.y, v.z, 1) -> Local to World, levando em conta a posi��o
        //transform.InverseTransformPoint()   // M^-1* (v.x, v.y, v.z, 1) -> World to Local, levando em conta a posi��o
        //transform.TransformVector()         // M* (v.x, v.y, v.z, 0) -> -> Local to World, n�o levando em conta a posi��o
        //transform.InverseTransformVector()  // M^-1* (v.x, v.y, v.z, 0) -> World to Local, n�o levando em conta a posi��o
    }

}
