using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButtonGenerator : MonoBehaviour {

    private GameObject[] prefabs;
    private RectTransform spawnWindow;
    private Vector3[] buttonPositions;
    public GameObject spawnButtonPrefab;
    List<Tuple<GameObject, string[]>> spawnableObjects = new List<Tuple<GameObject, string[]>>();
    int spawnWindowWidth;
    int spawnWindowHeight;

    void Start () {
        spawnWindowWidth = (Screen.width / 5) * 2;
        spawnWindowHeight = (Screen.height / 16) * 13;
        this.spawnWindow = this.GetComponent<RectTransform>();
        buttonPositions = GenerateButtonPositions();

        spawnWindow.sizeDelta = new Vector2 (spawnWindowWidth, spawnWindowHeight);

        prefabs = Resources.LoadAll<GameObject>("Prefabs");
        GameObject figurinePrefab = Resources.Load<GameObject>("Prefabs/Figurine");
        spawnableObjects.Add(Tuple.New(figurinePrefab, new string[] { "Red pawn", "red", "https://pastebin.ca/3399547" }));
        spawnableObjects.Add(Tuple.New(figurinePrefab, new string[] { "Green pawn", "green", "https://pastebin.ca/3399547" }));
        spawnableObjects.Add(Tuple.New(figurinePrefab, new string[] { "Blue pawn", "blue", "https://pastebin.ca/3399547" }));
        spawnableObjects.Add(Tuple.New(figurinePrefab, new string[] { "Yellow pawn", "yellow", "https://pastebin.ca/3399547" }));

        GameObject deckPrefab = Resources.Load<GameObject>("Prefabs/Deck");
        spawnableObjects.Add(Tuple.New(deckPrefab, new string[] { "Deck", "https://i.imgur.com/iSAo3YC.jpg", "https://i.imgur.com/PwhF8u0.jpg" }));

        GameObject diePrefab = Resources.Load<GameObject>("Prefabs/LudoD6");
        spawnableObjects.Add(Tuple.New(diePrefab, new string[] { "Ludo Die", "", "" }));

        GameObject boardPrefab = Resources.Load<GameObject>("Prefabs/Ludoboard");
        spawnableObjects.Add(Tuple.New(boardPrefab, new string[] { "Ludoboard", "", "" }));

        for (int i = 0; i < spawnableObjects.Count; i++)
        {
            GameObject spawnButton = (GameObject)Instantiate(spawnButtonPrefab);
            spawnButton.transform.SetParent(spawnWindow);
            spawnButton.transform.localScale = new Vector3(1, 1, 1);
            Spawn spawn = spawnButton.GetComponent<Spawn>();
            spawn.instatiate(spawnableObjects[i].First, spawnableObjects[i].Second);
            spawnButton.transform.GetComponent<RectTransform>().position = spawnWindow.position + buttonPositions[i];
            spawnButton.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = spawnableObjects[i].Second[0];
        }
    }

    public Vector3[] GenerateButtonPositions()
    {

        var buttonsPrColumn = Mathf.FloorToInt(spawnWindowHeight / 40);

        Vector3[] positionsList = new Vector3[50];
        int x = 10;
        int y = -35;
        int startY = y;

        for (int i = 0; i < 50; i++)
        {
            if (i % buttonsPrColumn == 0 && i != 0)
            {
                x += 135;
                y = startY - 40;
            } else
            {
                y -= 40;
            }

            Vector3 position = new Vector3(x, y, 0);

            positionsList[i] = position;
        }

        return positionsList;
    }
}
