using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public GameObject bird;
    public int pipeSpawnIntervalSecs = 2;
    public GameObject pipesPrefab;

    public int passedPipes = 0;

    public Rect ScreenRect { get; private set; }

    private readonly List<GameObject> pipes = new List<GameObject>();

    public void AddPipe(GameObject o)
    {
        pipes.Add(o);
    }

    public void RemovePipe(GameObject o)
    {
        pipes.Remove(o);
    }

    public GameObject NextPipes()
    {
        passedPipes = 0;
        foreach (var p in pipes)
        {
            if (p.transform.position.x > bird.transform.position.x)
            {
                return p;
            }
            passedPipes++;
        }
        return null;
    }

    public void NextEpisode()
    {
        FileWriter.append(passedPipes.ToString(), "passedPipes");
        passedPipes = 0;
        foreach (var p in pipes)
        {
            GameObject.Destroy(p);
        }
        pipes.Clear();
        CancelInvoke();
        Start();
    }

    void Awake()
    {
        FileWriter.clearFile("passedPipes");

        var topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        var topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));
        var bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1));
        // var bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));

        ScreenRect = new Rect(topLeft.x, topLeft.y, topRight.x - topLeft.x, bottomLeft.y - topLeft.y);
    }

    void Start()
    {
        SpawnPipes();
        InvokeRepeating("SpawnPipes", pipeSpawnIntervalSecs, pipeSpawnIntervalSecs);
    }

    void SpawnPipes()
    {
        var y = Random.Range(ScreenRect.yMin + 1, ScreenRect.yMax - 1);
        Instantiate(pipesPrefab, new Vector3(ScreenRect.xMax + 1, y), pipesPrefab.transform.rotation);
    }

}
