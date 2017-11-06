using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.scripts.Classes;

public class Deck : MonoBehaviour {

    private List<Transform> _cards;
    private List<Transform> _dealtCards;
    private string deckSourceUrl;
    private Texture2D sourceTex;
    private string cardBackUrl;
    private Texture2D cardBack;

    public bool isFaceDown = true;

    private void Start()
    {
        _cards = new List<Transform>();
        _dealtCards = new List<Transform>();
        StartCoroutine(InsBack());      
    }

    public Deck(string deckSource, string cardBack)
    {
        this.deckSourceUrl = deckSource;
        this.cardBackUrl = cardBack;
    }

    IEnumerator InsBack()
    {
        WWW backWWW = new WWW("http://www.google.fr/url?source=imglanding&ct=img&q=http://mywastedlife.com/CAH/img/back-white.png&sa=X&ved=0CAkQ8wdqFQoTCIOlwO7IhcYCFQFYFAodYnoAUg&usg=AFQjCNGdlrUGLinNrm18KedLAfCNPW3x6w");
        yield return backWWW;
        this.cardBack = backWWW.texture;
        ImageForDeck();        
        StartCoroutine(InsFront());
    }

    IEnumerator InsFront()
    {
        WWW deckWWW = new WWW("http://i.imgur.com/hgumn3h.jpg");
        yield return deckWWW;
        this.sourceTex = deckWWW.texture;
        InstantiateDeck();
    }    

    public void InstantiateDeck()
    {
        var cardHeight = this.sourceTex.height / 7;
        var cardWidth = this.sourceTex.width / 10;
        for (int i = 0; i < 10; i++)
        {
            for (int y = 0; y < 7; y++)
            {
                var cardPrefab = Resources.Load<Transform>("Prefabs/Card");

                Transform newCard = Instantiate(cardPrefab);

                //newCard.gameObject.SetActive(false);
                //newCard.GetComponent<Card>().backImg = cardBack;
                //newCard.GetComponent<Card>().frontImg = CropImageToCard(sourceTex, (i * cardWidth), (y * cardHeight), cardWidth, cardHeight);

                _cards.Add(newCard);
            }
        }

        foreach (Transform card in _cards)
        //{
        //    card.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = card.GetComponent<Card>().frontImg;
        //    card.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = card.GetComponent<Card>().backImg;
        //}

        this.enabled = true;
    }

    private void ImageForDeck()
    {
        Vector3[] vertices =
        {
                new Vector3(-0.5f, 0, -0.5f),
                new Vector3(0.5f, 0, -0.5f),
                new Vector3(-0.5f, 0, 0.5f),
                new Vector3(0.5f, 0, 0.5f),
            };
        Vector2[] uvs =
        {
                new Vector2(0, 0),
                new Vector2(1f, 0),
                new Vector2(0, 1f),
                new Vector2(1f, 1f),
            };
        int[] triangles =
        {
                0, 2, 1,
                1, 2, 3,
            };

        MeshRenderer mr = transform.GetChild(0).GetComponent<MeshRenderer>();
        mr.material.mainTexture = cardBack;
        Mesh mesh = transform.GetChild(0).GetComponent<MeshFilter>().mesh;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ResetDeck()
    {
        InstantiateDeck();
        _dealtCards.Clear();
    }

    public void ShuffleDeck()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < this._cards.Count; i++)
        {
            int j = random.Next(i, this._cards.Count);
            Transform temp = this._cards[i];
            this._cards[i] = this._cards[j];
            this._cards[j] = temp;
        }

        GetComponent<Rigidbody>().AddForce(0, 100, 0);
    }

    public void DealToPlayerFromDeck()
    {
        if (this.isFaceDown)
        {
            //Send the first card of the list to the given player's hand
        }
        else
        {
            //Send the last card of the list to the given player's hand
        }
    }

    public void DealXCardsToEachPlayer(int amount)
    {
        //for (int i = 0; i < amount; i++)
        //{
        //    foreach (var player in Players)
        //    {
        //        DealToPlayerFromDeck();
        //    }
        //}
    }

    public void DrawToTable()
    {
        if (this.isFaceDown && _cards.Count >= 1)
        {
            //Send the first card of the list to the table next to the deck
            _cards[0].transform.Rotate(new Vector3(-180, 0, 180));
            _cards[0].transform.position = new Vector3((transform.position.x + 7), 5, transform.position.z);
            _cards[0].transform.gameObject.SetActive(true);
            _dealtCards.Add(_cards[0]);
            _cards.Remove(_cards[0]);
        }
        else if(!this.isFaceDown && _cards.Count >= 1)
        {
            //Send the last card of the list to the table next tot the deck
            _cards[_cards.Count - 1].transform.position = new Vector3((transform.position.x + 7), 5, transform.position.z);
            _cards[_cards.Count - 1].transform.gameObject.SetActive(true);
            _dealtCards.Add(_cards[_cards.Count - 1]);
            _cards.Remove(_cards[_cards.Count - 1]);
        }
    }

    //Cropping the image file for each card, will be split into 70 different cards.
    public Texture2D CropImageToCard(Texture2D sourceTex, float sourceX, float sourceY, float sourceWidth, float sourceHeight)
    {
        int x = Mathf.FloorToInt(sourceX);
        int y = Mathf.FloorToInt(sourceY);
        int width = Mathf.FloorToInt(sourceWidth);
        int height = Mathf.FloorToInt(sourceHeight);

        Color[] pix = sourceTex.GetPixels(x, y, width, height);
        Texture2D destTex = new Texture2D(width, height);
        destTex.SetPixels(pix);
        destTex.Apply();
        return destTex;
    }
}
