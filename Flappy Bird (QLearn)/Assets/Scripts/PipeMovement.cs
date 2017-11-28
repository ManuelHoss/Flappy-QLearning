using UnityEngine;
using System.Collections;

public class PipeMovement : MonoBehaviour {

	public float speed = 1;

	private GameManager gameMgr;

	void Awake() {
		gameMgr = FindObjectOfType(typeof(GameManager)) as GameManager;
	}

	void Start() {
		gameMgr.AddPipe(gameObject);
	}

	void Update() {
		var x = -speed * Time.deltaTime;
		transform.Translate(new Vector3(x, 0));

		if (transform.position.x < gameMgr.ScreenRect.x - 1) {
			Destroy(gameObject);
		}
	}

	void OnDestroy() {
		gameMgr.RemovePipe(gameObject);
	}

}
