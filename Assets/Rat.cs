using System.Collections;
using UnityEngine;
using TMPro;

public class Rat : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float jetpackUpSpeed = 4f;
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
    public float staminaRecoverRate = 40f;
    public float staminaRecoverDelay = 0f;
    public float staminaHorizontalCost = 5f;
    public TMP_Text staminaText;

    private float staminaRecoverTimer;

    // Animation parts (auto-found in Start)
    private Animator fireLeftAnim;
    private Animator fireRightAnim;
    private Animator gunAnim;
    private Transform jetpackLeft;
    private Transform jetpackRight;
    private Transform ear;
    private Transform nose;
    private Transform headTransform;

    private SpriteRenderer[] allRenderers;

    void Start()
    {
        logicManager = FindAnyObjectByType<LogicManager>();
        heartDisplay = FindAnyObjectByType<HeartDisplay>();
        currentHealth = maxHealth;
        heartDisplay.UpdateHearts(currentHealth);
        currentStamina = maxStamina;

        if (myRigidbody == null)
            myRigidbody = GetComponent<Rigidbody2D>();

        // Auto-find animation references from hierarchy
        var body = transform.Find("Body");
        if (body != null)
        {
            headTransform = body.Find("null_head") ?? body.Find("Head");
            if (headTransform != null)
            {
                var headContainer = headTransform.Find("Head") ?? headTransform;
                ear = headContainer.Find("null_Ear") ?? headContainer.Find("Ear");
                nose = headContainer.Find("null_nose") ?? headContainer.Find("Nose");
            }

            var jm = body.Find("JetpackMiddle");
            if (jm != null)
            {
                jetpackLeft = jm.Find("Jetpack_Left");
                if (jetpackLeft != null)
                {
                    var fl = jetpackLeft.Find("Fire_Left");
                    if (fl != null) fireLeftAnim = fl.GetComponent<Animator>();
                }

                jetpackRight = jm.Find("Jetpack_Right");
                if (jetpackRight != null)
                {
                    var fr = jetpackRight.Find("Fire_Right");
                    if (fr != null) fireRightAnim = fr.GetComponent<Animator>();
                }
            }

            var gunObj = body.Find("Gun");
            if (gunObj != null) gunAnim = gunObj.GetComponent<Animator>();
        }

        // Auto-find stamina text in UI
        if (staminaText == null)
        {
            var staminaGO = GameObject.Find("GameUI/Stamina/StaminaText");
            if (staminaGO != null)
                staminaText = staminaGO.GetComponent<TMP_Text>();
        }

        allRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void Update()
    {
        if (!ratIsAlive) return;

        bool jetpackKeyHeld = Input.GetKey(KeyCode.Space);
        float jetpackCostThisFrame = staminaConsumeRate * Time.deltaTime;
        bool jetpackActive = jetpackKeyHeld && currentStamina >= jetpackCostThisFrame;
        bool usingStamina = false;

        if (jetpackActive)
        {
            currentStamina -= jetpackCostThisFrame;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jetpackUpSpeed);
            usingStamina = true;
        }

        float horizontalInput = 0;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        float horizontalCostThisFrame = staminaHorizontalCost * Time.deltaTime;
        if (horizontalInput != 0 && currentStamina >= horizontalCostThisFrame)
        {
            currentStamina -= horizontalCostThisFrame;
            myRigidbody.velocity = new Vector2(horizontalInput * horizontalSpeed, myRigidbody.velocity.y);
            usingStamina = true;
        }

        // Jetpack fire animation — active with jetpack OR horizontal movement
        bool fireActive = jetpackActive || horizontalInput != 0;
        if (fireLeftAnim != null) fireLeftAnim.SetBool("IsThrusting", fireActive);
        if (fireRightAnim != null) fireRightAnim.SetBool("IsThrusting", fireActive);

        // Jetpack nozzle rotation (opposite to movement)
        float nozzleTilt = -horizontalInput * 20f;
        if (jetpackLeft != null)
            jetpackLeft.localRotation = Quaternion.Euler(0, 0, nozzleTilt);
        if (jetpackRight != null)
            jetpackRight.localRotation = Quaternion.Euler(0, 0, nozzleTilt);

        // Ear and nose rotation tied to movement
        float tiltAmount = -horizontalInput * 15f;

        // Jetpack vertical tilt — head leans up when thrusting
        float jetpackTiltTarget = 0f;
        if (jetpackActive)
            jetpackTiltTarget = -8f;

        float lerpSpeed = 5f;

        if (headTransform != null)
            headTransform.localRotation = Quaternion.Lerp(
                headTransform.localRotation,
                Quaternion.Euler(0, 0, jetpackTiltTarget),
                Time.deltaTime * lerpSpeed
            );

        float earJetpackOffset = jetpackActive ? -15f : 0f;
        float noseJetpackOffset = jetpackActive ? -10f : 0f;

        if (ear != null)
            ear.localRotation = Quaternion.Lerp(
                ear.localRotation,
                Quaternion.Euler(0, 0, jetpackTiltTarget + earJetpackOffset + tiltAmount),
                Time.deltaTime * lerpSpeed
            );

        if (nose != null)
            nose.localRotation = Quaternion.Lerp(
                nose.localRotation,
                Quaternion.Euler(0, 0, jetpackTiltTarget + noseJetpackOffset + tiltAmount),
                Time.deltaTime * lerpSpeed
            );

        // Gun shooting on left mouse click
        if (Input.GetKeyDown(KeyCode.Mouse0) && gunAnim != null)
        {
            gunAnim.SetBool("IsShooting", true);
            StartCoroutine(ResetGun());
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

    IEnumerator ResetGun()
    {
        yield return new WaitForSeconds(0.15f);
        if (gunAnim != null)
            gunAnim.SetBool("IsShooting", false);
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
            foreach (var sr in allRenderers)
                sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        foreach (var sr in allRenderers)
            sr.enabled = true;
        isInvulnerable = false;
    }
}
