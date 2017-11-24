using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButtonGenerator : MonoBehaviour {

    private GameObject[] prefabs;
    private RectTransform spawnWindow;
    private Vector3[] buttonPositions;
    public GameObject spawnButtonPrefab;
    List<Tuple<GameObject, string[]>> spawnableObjects = new List<Tuple<GameObject, string[]>>();

    void Start () {

        this.spawnWindow = this.GetComponent<RectTransform>();
        buttonPositions = GenerateButtonPositions();

        spawnWindow.sizeDelta = new Vector2 (((Screen.width / 5) * 4), ((Screen.height / 5) * 4));

        prefabs = Resources.LoadAll<GameObject>("Prefabs");
        GameObject figurinePrefab = Resources.Load<GameObject>("Prefabs/Figurine");
        spawnableObjects.Add(Tuple.New(figurinePrefab, new string[] { "red pawn", "red", "http://pastebin.ca/3399547" }));
        spawnableObjects.Add(Tuple.New(figurinePrefab, new string[] { "green pawn", "green", "http://pastebin.ca/3399547" }));
        spawnableObjects.Add(Tuple.New(figurinePrefab, new string[] { "blue pawn", "blue", "http://pastebin.ca/3399547" }));
        spawnableObjects.Add(Tuple.New(figurinePrefab, new string[] { "yellow pawn", "yellow", "http://pastebin.ca/3399547" }));
        for (int i = 0; i < spawnableObjects.Count; i++)
        {
            GameObject spawnButton = (GameObject)Instantiate(spawnButtonPrefab);
            spawnButton.transform.SetParent(spawnWindow);
            spawnButton.transform.localScale = new Vector3(1, 1, 1);
            Spawn spawn = spawnButton.GetComponent<Spawn>();
            spawn.instatiate(spawnableObjects[i].First, spawnableObjects[i].Second);
            spawnButton.transform.GetComponent<RectTransform>().position = spawnWindow.position + buttonPositions[i];
            spawnButton.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Spawn " + spawnableObjects[i].Second[0];
        }
    }

    public Vector3[] GenerateButtonPositions()
    {
        var windowHeight = (Screen.height / 5) * 4;
        var windowWidth = (Screen.width / 5) * 4;

        var buttonsPrColumn = Mathf.FloorToInt(windowHeight / 60);

        Vector3[] positionsList = new Vector3[50];
        int x = -((Screen.width/5) * 2) + 10;
        int y = (Screen.height/5) * 2;
        int startY = (Screen.height / 5) * 2;

        for (int i = 0; i < 50; i++)
        {
            if (i % buttonsPrColumn == 0 && i != 0)
            {
                x += 170;
                y = startY - 60;
            } else
            {
                y -= 60;
            }

            Vector3 position = new Vector3(x, y, 0);

            positionsList[i] = position;
        }

        return positionsList;
    }
}
