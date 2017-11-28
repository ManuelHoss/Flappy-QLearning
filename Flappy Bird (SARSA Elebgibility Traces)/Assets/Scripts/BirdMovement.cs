using UnityEngine;
using System.Collections;

public class BirdMovement : MonoBehaviour {

	public float flapSpeed = 4;
	public float fallSpeed = 14;

	private float verticalSpeed;
	private Vector3 initialPosition;

    private GameManager gameMgr;

    private bool isDead = false;

    private int attempt = 0;

    public bool getBirdIsDead() {
        var help = isDead;
        this.isDead = false;
        return help;
    }

    public int getAttempt() { return attempt; }
    public void setAttempt(int attempt) { this.attempt = attempt; }

    public void setBirdIsDead(bool isDead)
    {
        this.isDead = isDead;
    }

	public void Flap() {
		verticalSpeed = flapSpeed;
	}

	void Awake() {
		gameMgr = FindObjectOfType(typeof(GameManager)) as GameManager;
	}

	void Start() {
		initialPosition = transform.position;
        attempt = 0;
    }

	void Update() {
		var y = transform.position.y + verticalSpeed * Time.deltaTime;
		verticalSpeed -= fallSpeed * Time.deltaTime;
		transform.position = new Vector3(transform.position.x, y, transform.position.z);

//		if (Input.GetMouseButtonDown(0)) {
//			Flap();
//		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Die();
	}

	void Die() {
        setBirdIsDead(true);
        attempt = attempt + 1;
        transform.position = initialPosition;
		verticalSpeed = 0;
		gameMgr.NextEpisode();
	}

}
