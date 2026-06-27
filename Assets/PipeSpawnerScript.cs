using UnityEngine;

// ============================================================
// PipeSpawnerScript — Cria canos automaticamente na tela
//
// O que faz:  Fica gerando novos obstáculos (canos) a cada
//             X segundos, em alturas aleatórias.
//
// Como usar:  Coloca esse script num GameObject vazio na cena
//             (ex: "Pipe Spawner") e arrasta o modelo do cano
//             na variável "pipe" no Inspector.
// ============================================================
public class PipeSpawnerScript : MonoBehaviour
{
    // --- Configurações (aparecem no Inspector do Unity) ---

    [Header("Referências")]
    public GameObject pipe;        // O cano que vai ser criado (arrasta o prefab aqui)

    [Header("Timer")]
    public float SpawnRate = 2;    // De quantos em quantos segundos cria um cano novo

    [Header("Altura aleatória")]
    public float HeightOffset = 10; // O cano pode nascer até 10 unidades pra cima ou pra baixo

    // --- Interno (não aparece no Inspector) ---

    private float timer = 0;       // Conta o tempo desde o último cano criado

    // -------------------------------------------------------
    // SpawnPipe — Cria um cano na tela
    //
    // 1. Escolhe uma altura aleatória entre "ponto mais alto" e "ponto mais baixo"
    // 2. Cria o cano nessa posição
    // -------------------------------------------------------
    void SpawnPipe()
    {
        // Calcula os limites: posição atual do spawner ± HeightOffset
        float heighestPoint = transform.position.y + HeightOffset;
        float lowestPoint = transform.position.y - HeightOffset;

        // Cria (instancia) o cano numa posição X fixa, mas Y aleatório
        Instantiate(
            pipe,                                                       // O modelo do cano
            new Vector3(
                transform.position.x,                                   // X = mesma posição do spawner
                Random.Range(lowestPoint, heighestPoint),               // Y = aleatório entre os limites
                0                                                       // Z = 0 (jogo 2D)
            ),
            transform.rotation                                          // Rotação = igual ao spawner
        );
    }

    // -------------------------------------------------------
    // Start — Executa UMA vez quando o jogo começa
    //
    // Já cria o primeiro cano na hora, pra não ficar esperando
    // -------------------------------------------------------
    void Start()
    {
        // Cria um cano imediatamente ao iniciar
        SpawnPipe();
    }

    // -------------------------------------------------------
    // Update — Executa a cada frame (várias vezes por segundo)
    //
    // Fica contando o tempo. Quando passa o tempo definido
    // em SpawnRate, cria outro cano e reseta o contador.
    // -------------------------------------------------------
    void Update()
    {
        // Se ainda não passou o tempo de espera...
        if (timer < SpawnRate)
        {
            timer = timer + Time.deltaTime;   // Soma o tempo que passou desde o último frame
        }
        else
        {
            // Já passou o tempo → cria um cano novo
            SpawnPipe();
            timer = 0;                        // Reseta o contador pra começar de novo
        }
    }
}
