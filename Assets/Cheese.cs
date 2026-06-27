using UnityEngine;

public class Cheese : MonoBehaviour
{
    public LogicManager logicManager;
    public float rotationSpeed = 90f;
    private bool collected;

    void Start()
    {
        logicManager = FindAnyObjectByType<LogicManager>();
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected && collision.gameObject.layer == 3)
        {
            collected = true;
            logicManager.AddScore();
            Destroy(gameObject);
        }
    }
}
