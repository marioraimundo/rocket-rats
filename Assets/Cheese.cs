using UnityEngine;

public class Cheese : MonoBehaviour
{
    private CheeseCounter cheeseCounter;
    public float rotationSpeed = 90f;
    private bool collected;

    void Start()
    {
        cheeseCounter = FindAnyObjectByType<CheeseCounter>();
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
            if (cheeseCounter != null)
                cheeseCounter.AddCheese();
            Destroy(gameObject);
        }
    }
}
