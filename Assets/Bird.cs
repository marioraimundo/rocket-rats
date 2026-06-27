using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength;
    public LogicManager logicManager;
    public HeartDisplay heartDisplay;
    public bool birdIsAlive = true;

    public int maxHealth = 3;
    public int currentHealth;
    public bool isInvulnerable = false;

    public float invulnerabilityDuration = 1f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        logicManager = FindAnyObjectByType<LogicManager>();
        heartDisplay = FindAnyObjectByType<HeartDisplay>();
        currentHealth = maxHealth;
        heartDisplay.UpdateHearts(currentHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!birdIsAlive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.velocity = Vector2.up * flapStrength;
        }

        float horizontalInput = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
            horizontalInput = -5;
        else if (Input.GetKey(KeyCode.RightArrow))
            horizontalInput = 5;

        if (horizontalInput != 0)
            myRigidbody.velocity = new Vector2(horizontalInput, myRigidbody.velocity.y);

        float rotationZ = Mathf.Clamp(myRigidbody.velocity.y * 4f, -90f, 90f);
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        if (transform.position.y > 14 || transform.position.y < -14)
        {
            birdIsAlive = false;
            logicManager.GameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (isInvulnerable || !birdIsAlive) return;

        currentHealth--;
        heartDisplay.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            birdIsAlive = false;
            logicManager.GameOver();
            return;
        }

        StartCoroutine(InvulnerabilityRoutine());
    }

    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float timer = 0f;
        while (timer < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }
}
