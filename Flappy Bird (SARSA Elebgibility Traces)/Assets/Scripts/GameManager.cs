using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class GameManager : MonoBehaviour {

	public GameObject bird;
	public int pipeSpawnIntervalSecs = 2;
	public GameObject pipesPrefab;
    public int countEpisodes = 0;
	public Rect ScreenRect { get; private set; }

	private readonly List<GameObject> pipes = new List<GameObject>();

	public void AddPipe(GameObject o) {
		pipes.Add(o);
	}
	
	public void RemovePipe(GameObject o) {
		pipes.Remove(o);
	}
	
	public GameObject NextPipes() {
		foreach (var p in pipes) {
			if (p.transform.position.x > bird.transform.position.x) {
				return p;
			}
		}
		return null;
	}

	public void NextEpisode() {
		foreach (var p in pipes) {
			GameObject.Destroy(p);
		}
		pipes.Clear();
		CancelInvoke();
		Start();
        
	}

	void Awake() {
		var topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
		var topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));
		var bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1));
		// var bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
		
		ScreenRect = new Rect(topLeft.x, topLeft.y, topRight.x - topLeft.x, bottomLeft.y - topLeft.y);
	}

	void Start() {
        countEpisodes++;
        SpawnPipes();
		InvokeRepeating("SpawnPipes", pipeSpawnIntervalSecs, pipeSpawnIntervalSecs);
	}

	void SpawnPipes() {
		var y = Random.Range(ScreenRect.yMin + 1, ScreenRect.yMax - 1);
		Instantiate(pipesPrefab, new Vector3(ScreenRect.xMax + 1, y), pipesPrefab.transform.rotation);
	}

    public int getNumberOfPipes() {
        if ((pipes.Count-4) >= 0) return (pipes.Count - 4);
        else return 0;
    }

    void OnDestroy()
    {
        using (XmlWriter writer = XmlWriter.Create("birdRL.xml"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Flappy Bird");
            writer.WriteStartElement("Anzahl der angezeigten pipes: "+ pipes.Count);
            writer.WriteStartElement("Anzahl der episoden: " + countEpisodes);
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

    }

}
