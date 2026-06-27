using UnityEngine;

public class Bird : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength;
    public LogicManager logicManager;
    public bool birdIsAlive = true;

    void Start()
    {
        logicManager = FindAnyObjectByType<LogicManager>();
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

    // (colisão com canos NÃO mata mais — o pássaro só quica e continua)
}
