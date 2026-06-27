using UnityEngine;

// ============================================================
// backgroundScroller — Fundo infinito com duas cópias
//
// Cria uma segunda cópia do fundo ao lado da original.
// As duas se movem pra esquerda juntas. Quando uma sai
// da tela, é reposicionada do outro lado.
// Isso dá a ilusão de um fundo contínuo e infinito.
// ============================================================
public class backgroundScroller : MonoBehaviour
{
    [Header("Scroll")]
    public float scrollSpeed = 2f;

    private GameObject copy;
    private float width;
    private float startX;

    void Start()
    {
        startX = transform.position.x;
        width = GetComponent<Renderer>().bounds.size.x;

        // Cria cópia à direita da original
        copy = Instantiate(gameObject, transform.position, transform.rotation);
        copy.transform.position = new Vector3(startX + width, transform.position.y, transform.position.z);

        // Remove este script da cópia (imediato, pra evitar loop)
        DestroyImmediate(copy.GetComponent<backgroundScroller>());
    }

    void Update()
    {
        float move = Vector3.left.x * scrollSpeed * Time.deltaTime;

        transform.position += new Vector3(move, 0, 0);
        copy.transform.position += new Vector3(move, 0, 0);

        float limit = startX - width;

        if (transform.position.x < limit)
        {
            transform.position = new Vector3(
                copy.transform.position.x + width,
                transform.position.y,
                transform.position.z
            );
        }

        if (copy.transform.position.x < limit)
        {
            copy.transform.position = new Vector3(
                transform.position.x + width,
                copy.transform.position.y,
                copy.transform.position.z
            );
        }
    }
}
