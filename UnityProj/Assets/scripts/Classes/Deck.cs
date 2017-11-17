using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.scripts.Classes;
using System.Linq;

public class Deck : MonoBehaviour {

    private List<int> _cards;
    private List<int> _dealtCards;
    private string deckSourceUrl = "http://i.imgur.com/iSAo3YC.jpg";    
    private string cardBackUrl = "http://i.imgur.com/PwhF8u0.jpg";
    public bool isFaceDown = true;
    WWWController wwwcontroller;

    private void Start()
    {
        InstantiateDeck(deckSourceUrl, cardBackUrl);
    }

    public void InstantiateDeck(string frontUrl, string backUrl)
    {
        deckSourceUrl = frontUrl;
        cardBackUrl = backUrl;
        ImageForDeck(Resources.Load<Texture2D>("loading"));
        _cards = new List<int>(Enumerable.Range(0, 52));
        _dealtCards = new List<int>();
        wwwcontroller = GameObject.Find("SceneScripts").GetComponent<WWWController>();
        StartCoroutine(wwwcontroller.getDeck(frontUrl, backUrl, x => ImageForDeck(x.Second)));
    }

    //public void InstantiateDecks(string frontUrl, string backUrl)
    //{
    //    var cardHeight = this.sourceTex.height / 7;
    //    var cardWidth = this.sourceTex.width / 10;
    //    for (int i = 0; i < 10; i++)
    //    {
    //        for (int y = 0; y < 7; y++)
    //        {
    //            var cardPrefab = Resources.Load<Transform>("Prefabs/Card");

    //            Transform newCard = Instantiate(cardPrefab);

    //            newCard.gameObject.SetActive(false);
    //            //newCard.GetComponent<Card>().backImg = cardBack;
    //            newCard.GetComponent<Card>().backImgUrl = cardBackUrl;
    //            //newCard.GetComponent<Card>().frontImg = CropImageToCard(sourceTex, (i * cardWidth), (y * cardHeight), cardWidth, cardHeight);
    //            newCard.GetComponent<Card>().frontImgUrl = deckSourceUrl;

    //            newCard.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = CropImageToCard(sourceTex, (i * cardWidth), (y * cardHeight), cardWidth, cardHeight);
    //            newCard.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = cardBack;

    //            newCard.GetComponent<Card>().id = i;

    //            //newCard.GetComponent<Card>().Instantiate();

    //            _cards.Add(newCard);
    //        }
    //    }

    //    this.enabled = true;
    //}

    private void ImageForDeck(Texture2D deckBack)
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
        mr.material.mainTexture = deckBack;
        Mesh mesh = transform.GetChild(0).GetComponent<MeshFilter>().mesh;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    //public void ResetDeck()
    //{
    //    InstantiateDeck();
    //    _dealtCards.Clear();
    //}

    public void ShuffleDeck()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < this._cards.Count; i++)
        {
            int j = random.Next(i, this._cards.Count);
            int temp = this._cards[i];
            this._cards[i] = this._cards[j];
            this._cards[j] = temp;
        }

        GetComponent<Rigidbody>().AddForce(0, 100, 0);
    }

    public void DealToPlayer(Utility.ClientColor color)
    {
        if(_cards.Count > 0)
        {
            int cardID;
            if (this.isFaceDown)
            {
                //Send the first card of the list to the given player's hand
                //Card card = _cards[0].GetComponent<Card>();
                //HostScript currentHost = GameObject.Find("NetworkHost").GetComponent<HostScript>();
                //currentHost.sendToClient(color, card, "card");
                //_dealtCards.Add(_cards[0]);
                //_cards.Remove(_cards[0]);
                cardID = _cards[0];
            }
            else
            {
                //Send the last card of the list to the given player's hand
                cardID = _cards[_cards.Count - 1];
            }
            var cardPrefab = Resources.Load<Transform>("Prefabs/Card");
            Transform cardTransform = Instantiate(cardPrefab);
            Card card = cardTransform.GetComponent<Card>();
            card.Instantiate(cardID, deckSourceUrl, cardBackUrl, true);
            HostScript currentHost = GameObject.Find("NetworkHost").GetComponent<HostScript>();
            currentHost.sendToClient(color, card, "card");
            ChangeHeight();
            _cards.Remove(cardID);
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
        if(_cards.Count >= 1)
        {
            var cardPrefab = Resources.Load<Transform>("Prefabs/Card");
            Transform newCard = Instantiate(cardPrefab);
            int cardID;

            if (!this.isFaceDown)
            {
                cardID = _cards[0];
                //newCard.Rotate(new Vector3(-180, 0, 180));
                _dealtCards.Add(_cards[0]);
            }
            else
            {
                cardID = _cards[_cards.Count - 1];
                _dealtCards.Add(_cards[_cards.Count - 1]);
            }
            
            newCard.GetComponent<Card>().Instantiate(cardID, deckSourceUrl, cardBackUrl, false);
            newCard.position = new Vector3((transform.position.x + 7), 5, transform.position.z);
            newCard.gameObject.SetActive(true);
            ChangeHeight();
            _cards.Remove(cardID);
            Debug.Log(_cards.Count);
        } 
    }

    //Not sure if this will work by scaling the deck.
    private void ChangeHeight()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(rectTransform.localScale.x, rectTransform.localScale.y - (rectTransform.localScale.y / _cards.Count), rectTransform.localScale.z);
    }

    public bool IsEmpty()
    {
        if(_cards.Count < 1)
        {
            return true;
        }
        return false;
    }
}
