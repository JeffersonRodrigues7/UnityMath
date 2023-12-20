using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ex03 : MonoBehaviour
{
    public Vector2 localCoord; //Coordenada local que será convertida em coordenada global
    public Vector2 worldCoord; //Coordenada global que será convertida em coordenada local

    public bool EX3a = false;
    public bool EX3b = false;

private void OnDrawGizmos()
{
    if (EX3a)
    {
        // Converte as coordenadas locais para coordenadas globais.
        worldCoord = localToWorld(localCoord);

        // Desenha uma esfera na posição global calculada.
        Gizmos.DrawSphere(worldCoord, 0.2f);
    }

    else if (EX3b)
    {
        // Se a condição EX3b for verdadeira, isso significa que as coordenadas locais serão atualizadas no Inspector.
        // As coordenadas mundiais (worldCoord) serão convertidas em coordenadas locais usando a função worldToLocal.
        localCoord = worldToLocal(worldCoord);
    }
}

// Converte coordenadas mundiais para coordenadas locais em relação ao objeto atual.
private Vector2 worldToLocal(Vector2 world)
{
    // Calcula o vetor relativo subtraindo a posição do objeto atual das coordenadas mundiais.
    Vector2 relative = world - (Vector2)transform.position;

    // Calcula as coordenadas locais usando o produto escalar (dot product) com os vetores da direita e para cima do objeto atual.
    float x = Vector2.Dot(relative, transform.right);
    float y = Vector2.Dot(relative, transform.up);

    // Retorna as coordenadas locais resultantes como um novo vetor.
    return new Vector2(x, y);
}

    // Converte coordenadas locais para coordenadas globais.
    private Vector2 localToWorld(Vector2 local)
    {
        // Obtém a posição atual do objeto.
        Vector2 pos = transform.position;

        // Adiciona a contribuição da direção da direita multiplicada pela coordenada local x.
        pos += local.x * (Vector2)transform.right;

        // Adiciona a contribuição da direção para cima multiplicada pela coordenada local y.
        pos += local.y * (Vector2)transform.up;

        // Retorna a posição global resultante.
        return pos;
    }
}
