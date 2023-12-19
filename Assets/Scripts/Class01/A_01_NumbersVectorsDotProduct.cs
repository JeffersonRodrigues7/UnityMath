using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_01_NumbersVectorsDotProduct : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Transform Enemy;

    [SerializeField] private bool Distance_Length_Direction = false;
    [SerializeField] private bool Dot_Product = true;

    private void OnDrawGizmos()
    {
        if (Distance_Length_Direction) DistanceLengthDirection();
        else if (Dot_Product) DotProduct();
    }

    public void DistanceLengthDirection()
    {
        /**
        * Distância entre 2 pontos 
        */
        Vector2 distancePlayer_Enemy = Enemy.position - Player.position; //Distância Vetorial que somada a posição do Player chegamos ao Inimigo
        Vector2 distanceEnemy_Player = Player.position - Enemy.position; //Distância Vetorial que somada a posição do Inimigo chegamos ao Player

        /**
         * Magnitude/Comprimento do vetor (No caso seria a distância entre 2 pontos) 
         */
        float lengthA_B = distancePlayer_Enemy.magnitude;
        //float lengthA_B = Mathf.Sqrt(distancePlayer_Enemy.x * distancePlayer_Enemy.x + distancePlayer_Enemy.y * distancePlayer_Enemy.y);

        /**
         * Acima calculamos basicamente a distância entre 2 pontos, porém podemos fazer isso diretamente utilizando a função Distance
         * Ela calcula a distância entre os vetores, gerando um vetor, e então a partir desse vetor, pega sua magnitude, que corresponde a distância entre os 2 pontos
         */
        float lengthA_B2 = Vector2.Distance(Player.position, Enemy.position); //Distância Absoluta do ponto A ao Ponto B (e Vice-Versa)

        /**
         * Direção para a qual um objeto está apontando.
         */
        Vector2 directionPlayer = distancePlayer_Enemy.normalized; //Direção para a qual o Player está olhando, basicamente é o vetor ponto normalizado
        //directionPlayer = distancePlayer_Enemy / lengthA_B;

        Vector2 directionEnemy = distanceEnemy_Player.normalized; //Direção para a qual o Inimigo está olhando, basicamente é o vetor ponto normalizado
        //directionEnemy = distanceEnemy_Player / lengthA_B;

        /**
        * DEBUGS
        */
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(default, Player.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(default, Enemy.position);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(Player.position, Enemy.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(directionPlayer, 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(directionEnemy, 0.25f);

        Debug.Log($"=======================================");
        Debug.Log($"Distância Vetorial Do Player ao Enemy: {distancePlayer_Enemy}");
        Debug.Log($"Distância Vetorial Do Enemy ao Player: {distanceEnemy_Player}");

        Debug.Log($" Magnitude/Comprimento do vetor: {lengthA_B}");

        Debug.Log($"Direção/Normalização Player: {directionPlayer}");
        Debug.Log($"Direção/Normalização Enemy: {directionEnemy}");
    }

    public void DotProduct()
    {
        Vector2 DistanceEnemy_Player = Player.position - Enemy.position; //Distância Vetorial que somada a posição do Inimigo chegamos ao Player

        /**
        * Direção para a qual um objeto está apontando.
        */
        Vector2 EnemyNormalized = Enemy.position.normalized; //Vetor Posição do Inimigo normalizada, poderia ser uma direção
        //directionPlayer = distancePlayer_Enemy / lengthA_B;


        /**
         * Projeção Escalar (Caso específico do Produto Vetorial) do Player no Enemy, útil para verificar quem está na frente de quem
         * Ela retorna uma Distância com sinal, que indica quem está na frente ou atrás
         * 
         * Se > 0 Então Player está na frente do Inimigo
         * Se < 0 Então Player está atrás do Inimigo
         * Se = 0 Player e Inimigo estão empatados
         * 
         * Observe que o vetor no qual vai haver a projeção escalar está normalizado, com isso eu consigo capturar a distância e saber quem está na frente ou atrás
         * Caso nenhum deles estivesse normalizado, eu saberia quem está na frente ou atrás, porém, não teria a distância exata entre eles
         */
        float scProj = Vector2.Dot(EnemyNormalized, DistanceEnemy_Player);
        //float scProj = EnemyNormalized.x * DistanceEnemy_Player.x + EnemyNormalized.y * DistanceEnemy_Player.y;

        /**
         * Projeção Vetorial
         * Este vetor indica o ponto em que o vetor está sendo projetado. 
         * No caso exemplo, ele indica em qual ponto da reta que atravessa o inimigo, o vetor do Player vai ser projetado.
         * Basicamente temos a coordenadas onde ocorre a projeção
         * Útil para por exemplo indicar o ponto em que o carro está na frente de outro
         */
        Vector2 vectProject = EnemyNormalized * scProj;

        /**
        * DEBUGS
        */
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(default, Player.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(default, Enemy.position);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(Player.position, Enemy.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(vectProject, 0.25f);

        Debug.Log(scProj);
    }
}
