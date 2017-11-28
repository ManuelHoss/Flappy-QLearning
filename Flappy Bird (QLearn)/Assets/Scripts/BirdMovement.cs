using UnityEngine;

public class BirdMovement : MonoBehaviour
{

    public float flapSpeed = 4;
    public float fallSpeed = 14;

    private float verticalSpeed;
    public Vector3 initialPosition;

    private bool isDead = false;

    private GameManager gameMgr;

    public void Flap()
    {
        verticalSpeed = flapSpeed;
    }

    void Awake()
    {
        gameMgr = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        var y = transform.position.y + verticalSpeed * Time.deltaTime;
        verticalSpeed -= fallSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Die();
    }

    void Die()
    {
        isDead = true;
        transform.position = initialPosition;
        verticalSpeed = 0;
        (FindObjectOfType(typeof(BirdAI)) as BirdAI).PersistData();
        gameMgr.NextEpisode();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void ResetIsDead()
    {
        isDead = false;
    }
}
