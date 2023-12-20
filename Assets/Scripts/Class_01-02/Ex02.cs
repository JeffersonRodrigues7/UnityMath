using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex02 : MonoBehaviour
{
    [SerializeField] private int maxHits = 5;

    private void OnDrawGizmos()
    {
        // Obter a posi��o do transform do objeto como origem
        Vector2 origin = transform.position;

        // Obter a dire��o para a direita no sistema de coordenadas local do objeto
        Vector2 dir = transform.right;

        // Configurar a cor do Gizmos para vermelho
        Gizmos.color = Color.red;

        // Criar um raio com a origem e a dire��o definidas
        Ray ray = new Ray(origin, dir);

        // Desenhar uma linha ou raio no Gizmos
        // As duas linhas abaixo s�o equivalentes
        // Gizmos.DrawLine(origin, origin + dir);
        // Gizmos.DrawRay(origin, dir);

        for (int i = 0; i < maxHits; i++)
        {
            // Realizar um Raycast e verificar se atingiu alguma coisa
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Desenhar uma linha do ponto de origem do raio at� o ponto de impacto
                Gizmos.DrawLine(ray.origin, hit.point);

                // Desenhar uma esfera no ponto de impacto
                Gizmos.DrawSphere(hit.point, 0.2f);

                // Calcular a reflex�o da dire��o do raio usando a fun��o Reflect
                Vector2 reflected = Reflect(ray.direction, hit.normal);
                //Vector2 reflected = Vector2.Reflect(ray.direction, hit.normal); //Essa � a vers�o j� embutida pela Unity

                // Configurar a cor do Gizmos para branco
                Gizmos.color = Color.white;

                // Desenhar uma linha representando a dire��o refletida
                Gizmos.DrawLine(hit.point, (Vector2)hit.point + reflected);

                // Atualizar a dire��o e a origem do raio para a pr�xima itera��o
                ray.direction = reflected;
                ray.origin = hit.point;
            }
            else
            {
                // Se o raio n�o atingiu nada, sair do loop
                break;
            }
        }
    }

    /// <summary>
    /// Fun��o que calcula a reflex�o de uma dire��o em rela��o a uma normal.
    /// </summary>
    /// <param name="inDir">Dire��o de entrada</param>
    /// <param name="n">Normal</param>
    /// <returns>Dire��o refletida</returns>
    private Vector2 Reflect(Vector2 inDir, Vector2 n)
    {
        // Calcular o produto escalar entre a dire��o de entrada e a normal
        float projEscalar = Vector2.Dot(inDir, n); // p = d * n

        // Calcular a dire��o refletida usando a f�rmula: d - 2 * (p) * n
        return inDir - 2 * projEscalar * n;
    }
}
