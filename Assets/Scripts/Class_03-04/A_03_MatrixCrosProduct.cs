using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_03_MatrixCrosProduct : MonoBehaviour
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

        /**Calculando o vetor X resultado do CrossProduct, que é um vetor tangente a superficie e perpendicular ao ray.direction e a normal
            * ray.direction: Direção para a qual a camera está olhando
            * yAxis: Normal da superficie
            * 
            * Observe que se invertemos ray.direction e yAxis o resultado é diferente, em alguns casos, talvez devamos tentar qual traz
            * o resultado correto, pois isso está relacionando com a regra da mão esquerda.
            * 
            * Precisamos normalizar o valor, pois caso tenhamos angulos muito rasos, ou seja, caso o vetor que estamos olhando
            * e o vetor normal estejam muito similares, o tamanho do vetor x pode acabar sendo afetado
            */
        Vector3 xAxis = Vector3.Cross(yAxis, ray.direction).normalized;

        /**Calculando Z(Forward), que é a direção que a turreta deve olhar a partir do CrossProduct entre X e Y. 
            * Como ambos já estão normalizados, o resultado(Z), tbm já vai estar normalizado, ou seja, com comprimento 1.
            */
        Vector3 zAxis = Vector3.Cross(xAxis, yAxis);

        /**A função Quaternion.LookRotation dá uma orientação baseado em uma forwardDirection(Z, azul) e UpDirection(Y(Normal), verde)
            */
        turret.rotation = Quaternion.LookRotation(zAxis, yAxis);

        Gizmos.color = Color.white; //Desenhando um raycast entre a camera a o ponto na superficie que ela bate
        Gizmos.DrawLine(ray.origin, hit.point);

        Gizmos.color = Color.green; //Desenhando a normal da superficie
        Gizmos.DrawRay(hit.point, yAxis);

        Gizmos.color = Color.red; //Desenhando o vetor x, tangente a superficie
        Gizmos.DrawRay(hit.point, xAxis);

        Gizmos.color = Color.blue; //Desenhando o vetor z, direção que a turreta deve olhar
        Gizmos.DrawRay(hit.point, zAxis);
        /** Faz a turreta olhar em direção ao qual a camera está olhando, mas não exatamente o que queremos, pois desta forma,
            * a turreta vai ficar olhando diretamente para o chão
            */
        //turret.rotation = Quaternion.LookRotation(ray.direction);
    }



        //Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix; //Obtém a matriz de transformação localToWorld

        //localToWorldMatrix.MultiplyPoint3x4()//ignora a última linha que é inútil na maioria dos casos

        /**Transforming between local and world
         * Funções que utilizamos ao fazer transformações no espaço de algum objeto 
         * M = LocalToWorld Matrix
         */
        //transform.TransformPoint(localOffset)          // M* (v.x, v.y, v.z, 1) -> Local to World, levando em conta a posição
        //transform.InverseTransformPoint()   // M^-1* (v.x, v.y, v.z, 1) -> World to Local, levando em conta a posição
        //transform.TransformVector()         // M* (v.x, v.y, v.z, 0) -> -> Local to World, não levando em conta a posição
        //transform.InverseTransformVector()  // M^-1* (v.x, v.y, v.z, 0) -> World to Local, não levando em conta a posição
    }

}
