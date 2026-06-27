using UnityEngine;

// ============================================================
// PipeSpawnerScript — Cria obstáculos automaticamente na tela
//
// O que faz:  Fica gerando novos obstáculos (canos) a cada
//             X segundos, em alturas aleatórias.
//             Alterna entre o cano normal e o cano com espinhos
//             a cada spawn.
//
// Como usar:  Coloca esse script num GameObject vazio na cena
//             (ex: "Pipe Spawner") e arrasta os dois prefabs
//             nas variáveis "pipe" e "pipeSpike" no Inspector.
// ============================================================
public class PipeSpawnerScript : MonoBehaviour
{
    // --- Configurações (aparecem no Inspector do Unity) ---

    [Header("Referências")]
    public GameObject pipe;           // Cano normal (Pipe.prefab)
    public GameObject pipeSpike;      // Cano com espinhos (PipeSpike.prefab)

    [Header("Timer")]
    public float SpawnRate = 2;       // De quantos em quantos segundos cria um obstáculo

    [Header("Altura aleatória")]
    public float HeightOffset = 10;   // O obstáculo pode nascer até 10 unidades pra cima ou pra baixo

    // --- Interno (não aparece no Inspector) ---

    private float timer = 0;          // Conta o tempo desde o último obstáculo criado
    private bool isPipeTurn = true;   // true = pipe normal, false = pipeSpike

    // -------------------------------------------------------
    // SpawnPipe — Cria um obstáculo na tela
    //
    // 1. Alterna entre pipe normal e pipeSpike
    // 2. Escolhe uma altura aleatória
    // 3. Cria o obstáculo nessa posição
    // -------------------------------------------------------
    void SpawnPipe()
    {
        // Alterna qual prefab vai ser spawnado agora
        GameObject prefab = isPipeTurn ? pipe : pipeSpike;
        isPipeTurn = !isPipeTurn;

        // Calcula os limites: posição atual do spawner ± HeightOffset
        float heighestPoint = transform.position.y + HeightOffset;
        float lowestPoint = transform.position.y - HeightOffset;

        // Cria (instancia) o obstáculo numa posição X fixa, mas Y aleatório
        Instantiate(
            prefab,                                                     // O modelo (pipe ou pipeSpike)
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
