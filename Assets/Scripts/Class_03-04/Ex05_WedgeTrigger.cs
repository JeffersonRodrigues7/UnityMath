using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Ex05_WedgeTrigger : MonoBehaviour
{
    public Transform target;

    public float radius = 1;
    public float height = 1;

    [Range(-1,1)]
    public float angThresh = 0.5f;//not an actual angle

    private void OnDrawGizmos()
    {
        localSpace();
        //worldSpace();
    }

    public void worldSpace()
    {
        Vector3 origin = transform.position; //Posição de baixo do cilindro
        Vector3 up = transform.up;
        Vector3 right = transform.right;
        Vector3 forward = transform.forward;

        Vector3 top = origin + up * height;//Posição do topo do cilindro

        Handles.DrawWireDisc(origin, up, radius); //Desenhando Disco da parte de baixo(onde os pés da turreta está)
        Handles.DrawWireDisc(top, up, radius); //Desenhando Disco da patr de cima(onde o pico da turreta está)

        float p = angThresh; //Seria o produto vetorial entre a direção forward do player e a direção até o inimigo
        float x = Mathf.Sqrt(1 - p * p);

        //Abaixo vamois desenhar um raio que vai do centro do objeto até a ponta do arco que leva em conta a abertura, que se dá pelo anglethreshold e pelo x calculado
        Vector3 vLeft = (forward * p + right * (-x)) * radius; //Ponto de colisão com o arco ao lado esquerdo do centro
        Vector3 vRight = (forward * p + right * x) * radius; //Ponto de colisão com o arco ao lado direito do centro

        //Desenhando raios
        Gizmos.DrawRay(origin, vLeft);
        Gizmos.DrawRay(origin, vRight);
        Gizmos.DrawRay(top, vLeft);
        Gizmos.DrawRay(top, vRight);

        Gizmos.DrawLine(origin, top);
        Gizmos.DrawLine(origin + vLeft, top + vLeft);
        Gizmos.DrawLine(origin + vRight, top + vRight);
    }

    public void localSpace()
    {
        //Makes gizmos e Handles relative to this transform ao invés do world, então muita coisa pode ser jogada fora
        Gizmos.matrix = Handles.matrix =  transform.localToWorldMatrix;

        //Se o objeto estiver dentro do range, pintaremos tudo de branco, caso contrário, ficará vermelho
        Gizmos.color = Handles.color = Contains(target.position) ? Color.white : Color.red;

        //Observe que não precisamos mais dos atributos de origin, up, right e forward, pois gizmos e handle estão no local space

        Vector3 top =  new Vector3(0, height, 0);//Posição do topo do cilindro

        Handles.DrawWireDisc(Vector3.zero, Vector3.up, radius); //Desenhando Disco da parte de baixo(onde os pés da turreta está)
        Handles.DrawWireDisc(top, Vector3.up, radius); //Desenhando Disco da patr de cima(onde o pico da turreta está)

        float p = angThresh; //Seria o produto vetorial entre a direção forward do player e a direção até o inimigo
        float x = Mathf.Sqrt(1 - p * p);

        //Abaixo vamois desenhar um raio que vai do centro do objeto até a ponta do arco que leva em conta a abertura,
        //que se dá pelo anglethreshold e pelo x calculado
        Vector3 vLeft = new Vector3(-x, 0, p)*radius;//Definindo ponto que toca arco do lado direito do centro
        Vector3 vRight = new Vector3(x, 0, p)*radius;

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
        if(flatDistance > radius) return false; //outisde of the infinity cylinder


        //we are inside
        return true;
    }

}
