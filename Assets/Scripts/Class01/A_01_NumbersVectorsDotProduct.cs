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
        * Dist�ncia entre 2 pontos 
        */
        Vector2 distancePlayer_Enemy = Enemy.position - Player.position; //Dist�ncia Vetorial que somada a posi��o do Player chegamos ao Inimigo
        Vector2 distanceEnemy_Player = Player.position - Enemy.position; //Dist�ncia Vetorial que somada a posi��o do Inimigo chegamos ao Player

        /**
         * Magnitude/Comprimento do vetor (No caso seria a dist�ncia entre 2 pontos) 
         */
        float lengthA_B = distancePlayer_Enemy.magnitude;
        //float lengthA_B = Mathf.Sqrt(distancePlayer_Enemy.x * distancePlayer_Enemy.x + distancePlayer_Enemy.y * distancePlayer_Enemy.y);

        /**
         * Acima calculamos basicamente a dist�ncia entre 2 pontos, por�m podemos fazer isso diretamente utilizando a fun��o Distance
         * Ela calcula a dist�ncia entre os vetores, gerando um vetor, e ent�o a partir desse vetor, pega sua magnitude, que corresponde a dist�ncia entre os 2 pontos
         */
        float lengthA_B2 = Vector2.Distance(Player.position, Enemy.position); //Dist�ncia Absoluta do ponto A ao Ponto B (e Vice-Versa)

        /**
         * Dire��o para a qual um objeto est� apontando.
         */
        Vector2 directionPlayer = distancePlayer_Enemy.normalized; //Dire��o para a qual o Player est� olhando, basicamente � o vetor ponto normalizado
        //directionPlayer = distancePlayer_Enemy / lengthA_B;

        Vector2 directionEnemy = distanceEnemy_Player.normalized; //Dire��o para a qual o Inimigo est� olhando, basicamente � o vetor ponto normalizado
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
        Debug.Log($"Dist�ncia Vetorial Do Player ao Enemy: {distancePlayer_Enemy}");
        Debug.Log($"Dist�ncia Vetorial Do Enemy ao Player: {distanceEnemy_Player}");

        Debug.Log($" Magnitude/Comprimento do vetor: {lengthA_B}");

        Debug.Log($"Dire��o/Normaliza��o Player: {directionPlayer}");
        Debug.Log($"Dire��o/Normaliza��o Enemy: {directionEnemy}");
    }

    public void DotProduct()
    {
        Vector2 DistanceEnemy_Player = Player.position - Enemy.position; //Dist�ncia Vetorial que somada a posi��o do Inimigo chegamos ao Player

        /**
        * Dire��o para a qual um objeto est� apontando.
        */
        Vector2 EnemyNormalized = Enemy.position.normalized; //Vetor Posi��o do Inimigo normalizada, poderia ser uma dire��o
        //directionPlayer = distancePlayer_Enemy / lengthA_B;


        /**
         * Proje��o Escalar (Caso espec�fico do Produto Vetorial) do Player no Enemy, �til para verificar quem est� na frente de quem
         * Ela retorna uma Dist�ncia com sinal, que indica quem est� na frente ou atr�s
         * 
         * Se > 0 Ent�o Player est� na frente do Inimigo
         * Se < 0 Ent�o Player est� atr�s do Inimigo
         * Se = 0 Player e Inimigo est�o empatados
         * 
         * Observe que o vetor no qual vai haver a proje��o escalar est� normalizado, com isso eu consigo capturar a dist�ncia e saber quem est� na frente ou atr�s
         * Caso nenhum deles estivesse normalizado, eu saberia quem est� na frente ou atr�s, por�m, n�o teria a dist�ncia exata entre eles
         */
        float scProj = Vector2.Dot(EnemyNormalized, DistanceEnemy_Player);
        //float scProj = EnemyNormalized.x * DistanceEnemy_Player.x + EnemyNormalized.y * DistanceEnemy_Player.y;

        /**
         * Proje��o Vetorial
         * Este vetor indica o ponto em que o vetor est� sendo projetado. 
         * No caso exemplo, ele indica em qual ponto da reta que atravessa o inimigo, o vetor do Player vai ser projetado.
         * Basicamente temos a coordenadas onde ocorre a proje��o
         * �til para por exemplo indicar o ponto em que o carro est� na frente de outro
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
