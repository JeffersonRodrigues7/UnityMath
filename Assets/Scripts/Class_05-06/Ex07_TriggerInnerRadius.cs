using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ex07_TriggerInnerRadius : MonoBehaviour
{
    public Transform target;

    public float radiusOuter = 1;
    public float radiusInner = 1;
    public float height = 1;

    //WedgeTrigger com �ngulos
    [Range(0, 360)]
    public float fovDeg = 45;//fov = field of view

    float fovRad => fovDeg * Mathf.Deg2Rad;//field of view em radianos
    float angThresh => Mathf.Cos(fovRad / 2);//Agora n�o estou mais definindo angThresh = p hardcoded

    private void OnDrawGizmos()
    {
        localSpace();
    }

    public void localSpace()
    {
        //Makes gizmos e Handles relative to this transform ao inv�s do world, ent�o muita coisa pode ser jogada fora
        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;

        //Se o objeto estiver dentro do range, pintaremos tudo de branco, caso contr�rio, ficar� vermelho
        Gizmos.color = Handles.color = Contains(target.position) ? Color.white : Color.red;

        Vector3 top = new Vector3(0, height, 0);//Posi��o do topo do cilindro

        float p = angThresh; //Seria o produto vetorial entre a dire��o forward do player e a dire��o at� o inimigo
        Debug.Log(p);
        float x = Mathf.Sqrt(1 - p * p);

        //Abaixo vamois desenhar um raio que vai do centro do objeto at� a ponta do arco que leva em conta a abertura,
        //que se d� pelo anglethreshold e pelo x calculado
        Vector3 vLeftDir = new Vector3(-x, 0, p);//Definindo ponto que toca arco do lado direito do centro
        Vector3 vRightDir = new Vector3(x, 0, p);

        Vector3 vLeftOuter = vLeftDir * radiusOuter;
        Vector3 vRightOuter = vRightDir * radiusOuter;
        
        Vector3 vLeftInner = vLeftDir * radiusInner;
        Vector3 vRightInner = vRightDir * radiusInner;

        Handles.DrawWireArc(default, Vector3.up, vLeftOuter, fovDeg, radiusOuter);
        Handles.DrawWireArc(top, Vector3.up, vLeftOuter, fovDeg, radiusOuter);

        Handles.DrawWireArc(default, Vector3.up, vLeftInner, fovDeg, radiusInner);
        Handles.DrawWireArc(top, Vector3.up, vLeftInner, fovDeg, radiusInner);

        //Desenhando raios
        Gizmos.DrawLine(vLeftInner, vLeftOuter);//default neste caso � Vector3.zero
        Gizmos.DrawLine(vRightInner, vRightOuter);
        Gizmos.DrawLine(top + vLeftInner, top + vLeftOuter);
        Gizmos.DrawLine(top + vRightInner, top + vRightOuter);

        Gizmos.DrawLine(vLeftInner, top+ vLeftInner);
        Gizmos.DrawLine(vRightInner, top+ vRightInner);
        Gizmos.DrawLine(vLeftOuter, top + vLeftOuter);
        Gizmos.DrawLine(vRightOuter, top + vRightOuter);
    }


    //verifica se uma posi��o est� contida na figura
    public bool Contains(Vector3 position)
    {
        Vector3 vecToTargetWorld = (position - transform.position); //Dire��o da Turreta ao target no world space

        //inserve transform is world to local
        //Transformando tudo para local, deixa tudo mais f�cil
        Vector3 vecToTarget = transform.InverseTransformVector(vecToTargetWorld);

        /** height check */
        //height position check, usando local space fica f�cil, s� comparar com a parte de baixo(0) e a de cima (height)
        if (vecToTarget.y < 0 || vecToTarget.y > height) return false;//outisde the height range

        //Se tivessemos usando world space, seria muito mais complicado: Vector3.Dot(transform.up, vecToTargetWorld);
        //Teriamos a dist�ncia projetada usando nosso vetor relativo, e ent�o fariamos o range check usando este valor

        /** angular check */
        Vector3 flatDirToTarget = vecToTarget;

        //Observe que a altura do objeto aqui � irrelevante, ele s� precisa estar no range pr� determinado
        //Mas se n�o fizermos y = 0, a altura do objeto ir� influenciar no c�lculo
        flatDirToTarget.y = 0;
        float flatDistance = flatDirToTarget.magnitude;
        flatDirToTarget /= flatDistance;//flatDirToTarget.normalized;

        //Se estivermos nas coordenadas locais, o vetor I projetado no forward nada mais � do que P, que por sua vez � a coordenada z.
        //flatDirToTarget.z � basicamente a proje��o da seta vermelha no forward, por isso podemos comparar ele com o angThresh
        if (flatDirToTarget.z < angThresh) return false;//outside of the angular wedge

        /** cylindrical radial */
        //Verificanso se objeto n�o est� nem muito perto, nem muito longe do inimigo
        if (flatDistance > radiusOuter || flatDistance < radiusInner) return false; //outisde of the infinity cylinder


        //we are inside
        return true;
    }
}

