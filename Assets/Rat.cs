using System.Collections;
using UnityEngine;
using TMPro;

public class Rat : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float jetpackUpSpeed;
    public float horizontalSpeed = 5f;
    public LogicManager logicManager;
    public HeartDisplay heartDisplay;
    public bool ratIsAlive = true;

    public int maxHealth = 3;
    public int currentHealth;
    public bool isInvulnerable = false;

    public float invulnerabilityDuration = 1f;

    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaConsumeRate = 20f;
    public float staminaRecoverRate = 10f;
    public float staminaRecoverDelay = 1f;
    public float staminaHorizontalCost = 5f;
    public TMP_Text staminaText;

    private float staminaRecoverTimer;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        logicManager = FindAnyObjectByType<LogicManager>();
        heartDisplay = FindAnyObjectByType<HeartDisplay>();
        currentHealth = maxHealth;
        heartDisplay.UpdateHearts(currentHealth);
        currentStamina = maxStamina;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!ratIsAlive) return;

        bool jetpackKeyHeld = Input.GetKey(KeyCode.Space);
        bool jetpackActive = jetpackKeyHeld && currentStamina > 0;
        bool usingStamina = false;

        if (jetpackActive)
        {
            currentStamina -= staminaConsumeRate * Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jetpackUpSpeed);
            usingStamina = true;
        }

        float horizontalInput = 0;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        if (horizontalInput != 0 && currentStamina > 0)
        {
            currentStamina -= staminaHorizontalCost * Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;
            myRigidbody.velocity = new Vector2(horizontalInput * horizontalSpeed, myRigidbody.velocity.y);
            usingStamina = true;
        }

        if (usingStamina)
        {
            staminaRecoverTimer = 0f;
        }
        else if (currentStamina < maxStamina)
        {
            staminaRecoverTimer += Time.deltaTime;
            if (staminaRecoverTimer >= staminaRecoverDelay)
            {
                currentStamina += staminaRecoverRate * Time.deltaTime;
                if (currentStamina > maxStamina) currentStamina = maxStamina;
            }
        }

        if (staminaText != null)
            staminaText.text = "Stamina: " + Mathf.RoundToInt(currentStamina);

        float rotationZ = Mathf.Clamp(myRigidbody.velocity.y * 2f, -30f, 30f);
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        if (transform.position.y > 14 || transform.position.y < -14)
        {
            ratIsAlive = false;
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
        if (isInvulnerable || !ratIsAlive) return;

        currentHealth--;
        heartDisplay.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            ratIsAlive = false;
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
