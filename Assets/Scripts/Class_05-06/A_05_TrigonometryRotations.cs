using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class A_05_TrigonometryRotations : MonoBehaviour
{
    //anglDegreesRadiansPart
    [Range(0, 360)]
    public float angDeg = 0;


    public Transform target;

    public float radius = 1;
    public float height = 1;

    //WedgeTrigger com ângulos
    [Range(0, 360)]
    public float fovDeg = 45;//fov = field of view

    float fovRad => fovDeg * Mathf.Deg2Rad;
    float angThresh => Mathf.Cos(fovDeg * Mathf.Deg2Rad/2);//Agora não estou mais definindo angThresh = p hardcoded

    private void OnDrawGizmos()
    {
        //Quaternion up90 = Quaternion.AngleAxis(90, Vector3.up);//Permite rotacionar usando o formato de Angle Axis

        //Quaternion rotA = Quaternion.Euler(90, 90, 45);
        //Quaternion rotB = Quaternion.AngleAxis(60, Vector3.right);
        //Quaternion rotCombined = rotA * rotB;//ordem importa
        //Quaternion rotInverse = Quaternion.Inverse(rotCombined);
        //Vector3 rotateVector = rotInverse * vLeft;//Ordem importa, rotação vem primeiro

        //anglDegreesRadiansPart();

        //Makes gizmos e Handles relative to this transform ao invés do world, então muita coisa pode ser jogada fora
        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;

        //Se o objeto estiver dentro do range, pintaremos tudo de branco, caso contrário, ficará vermelho
        Gizmos.color = Handles.color = Contains(target.position) ? Color.white : Color.red;

        //Observe que não precisamos mais dos atributos de origin, up, right e forward, pois gizmos e handle estão no local space

        Vector3 top = new Vector3(0, height, 0);//Posição do topo do cilindro

        float p = angThresh; //Seria o produto vetorial entre a direção forward do player e a direção até o inimigo
        float x = Mathf.Sqrt(1 - p * p);

        //Abaixo vamois desenhar um raio que vai do centro do objeto até a ponta do arco que leva em conta a abertura,
        //que se dá pelo anglethreshold e pelo x calculado
        Vector3 vLeft = new Vector3(-x, 0, p) * radius;//Definindo ponto que toca arco do lado direito do centro
        Vector3 vRight = new Vector3(x, 0, p) * radius;

        //Desenhando arco que começa da esquerda(Vleft) e uma qtd de graus (fovDeg) para direita, considerando o raio(radiusOuter)
        Handles.DrawWireArc(default, Vector3.up, vLeft, fovDeg, radius);
        Handles.DrawWireArc(top, Vector3.up, vLeft, fovDeg, radius);

        //Desenhando raios
        Gizmos.DrawRay(default, vLeft);//default neste caso é Vector3.zero
        Gizmos.DrawRay(default, vRight);
        Gizmos.DrawRay(top, vLeft);
        Gizmos.DrawRay(top, vRight);

        Gizmos.DrawLine(default, top);
        Gizmos.DrawLine(vLeft, top + vLeft);
        Gizmos.DrawLine(vRight, top + vRight);
    }

    //verifica se uma posição está contida na figura
    public bool Contains(Vector3 position)
    {
        Vector3 vecToTargetWorld = (position - transform.position); //Direção da Turreta ao target no world space

        //inserve transform is world to local
        //Transformando tudo para local, deixa tudo mais fácil
        Vector3 vecToTarget = transform.InverseTransformVector(vecToTargetWorld);

        /** height check */
        //height position check, usando local space fica fácil, só comparar com a parte de baixo(0) e a de cima (height)
        if (vecToTarget.y < 0 || vecToTarget.y > height) return false;//outisde the height range

        //Se tivessemos usando world space, seria muito mais complicado: Vector3.Dot(transform.up, vecToTargetWorld);
        //Teriamos a distância projetada usando nosso vetor relativo, e então fariamos o range check usando este valor

        /** angular check */
        Vector3 flatDirToTarget = vecToTarget;

        //Observe que a altura do objeto aqui é irrelevante, ele só precisa estar no range pré determinado
        //Mas se não fizermos y = 0, a altura do objeto irá influenciar no cálculo
        flatDirToTarget.y = 0;
        float flatDistance = flatDirToTarget.magnitude;
        flatDirToTarget /= flatDistance;//flatDirToTarget.normalized;

        //Se estivermos nas coordenadas locais, o vetor I projetado no forward nada mais é do que P, que por sua vez é a coordenada z.
        //flatDirToTarget.z é basicamente a projeção da seta vermelha no forward, por isso podemos comparar ele com o angThresh
        if (flatDirToTarget.z < angThresh) return false;//outside of the angular wedge

        /** cylindrical radial */
        if (flatDistance > radius) return false; //outisde of the infinity cylinder


        //we are inside
        return true;
    }

    public void anglDegreesRadiansPart()
    {
        Handles.DrawWireDisc(Vector3.zero, Vector3.forward, 1);

        float angRad = angDeg * Mathf.Deg2Rad;

        //float angTurns = (float)EditorApplication.timeSinceStartup;//tempo em turns

        Vector2 v = new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));//Achando respectivamente os pontos em X e Y do vetor
        //Vector2 v = new Vector2(Mathf.Cos(angTurns*Mathf.PI*2), Mathf.Sin(angTurns * Mathf.PI * 2));//Achando respectivamente os pontos em X e Y do vetor

        Gizmos.DrawRay(default, v);
    }


    private void Update()
    {
        float myAngleDeg = 45f;
        float myAngleRad = myAngleDeg * Mathf.Deg2Rad;//convert degrees to radians
        myAngleDeg = myAngleRad * Mathf.Deg2Rad;//convert radians to degrees
    }

}
