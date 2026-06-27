using UnityEngine;

public class scoreTrigger : MonoBehaviour
{
    public LogicManager logic;
    private bool alreadyScored;

    void Start()
    {
        logic = FindAnyObjectByType<LogicManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alreadyScored && collision.gameObject.layer == 3)
        {
            alreadyScored = true;
            logic.AddScore();
        }
    }
}
