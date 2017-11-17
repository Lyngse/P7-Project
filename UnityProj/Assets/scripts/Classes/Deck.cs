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
    private int deckSize = 52;
    public bool isFaceDown = true;
    WWWController wwwcontroller;
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

    private void Start()
    {
        InstantiateDeck(deckSourceUrl, cardBackUrl);
    }

    public void InstantiateDeck(string frontUrl, string backUrl)
    {
        deckSourceUrl = frontUrl;
        cardBackUrl = backUrl;
        wwwcontroller = GameObject.Find("SceneScripts").GetComponent<WWWController>();
        ImageForDeck(Resources.Load<Texture2D>("loading"), wwwcontroller, false);
        _cards = new List<int>(Enumerable.Range(0, deckSize));
        _dealtCards = new List<int>();        
        StartCoroutine(wwwcontroller.getDeck(frontUrl, backUrl, x => ImageForDeck(x.Second, wwwcontroller, true)));
    }

    private void ImageForDeck(Texture2D deckBack, WWWController wwwcontroller, bool areCardsInitialized)
    {       
        MeshRenderer backMr = transform.GetChild(0).GetComponent<MeshRenderer>();
        Mesh backMesh = transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        backMesh.Clear();
        backMesh.vertices = vertices;
        backMesh.triangles = triangles;
        backMesh.uv = uvs;
        backMesh.RecalculateNormals();
        transform.GetChild(0).gameObject.SetActive(true);        
        backMr.material.mainTexture = deckBack;

        if (areCardsInitialized)
        {
            
            wwwcontroller.GetCard(_cards[_cards.Count - 1], deckSourceUrl, cardBackUrl, (x => ChangeBottomCardTexture(x.First)));            
        }        
    }

    private void ChangeBottomCardTexture(Texture2D texture)
    {
        MeshRenderer frontMr = transform.GetChild(1).GetComponent<MeshRenderer>();
        frontMr.material.mainTexture = texture;
        Mesh frontMesh = transform.GetChild(1).GetComponent<MeshFilter>().mesh;
        frontMesh.Clear();
        frontMesh.vertices = vertices;
        frontMesh.triangles = triangles;
        frontMesh.uv = uvs;
        frontMesh.RecalculateNormals();
        
    }

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

        wwwcontroller.GetCard(_cards[_cards.Count - 1], deckSourceUrl, cardBackUrl, (x => ChangeBottomCardTexture(x.First)));

        GetComponent<Rigidbody>().AddForce(0, 100, 0);
    }

    public void DealToPlayer(Utility.ClientColor color)
    {
        if(_cards.Count > 0)
        {
            int cardID;
            bool changeBottom = false;
            if (this.isFaceDown)
            {
                cardID = _cards[0];
            }
            else
            {
                cardID = _cards[_cards.Count - 1];
                changeBottom = true;
            }
            var cardPrefab = Resources.Load<Transform>("Prefabs/Card");
            Transform cardTransform = Instantiate(cardPrefab);
            cardTransform.GetComponent<Card>().Instantiate(cardID, deckSourceUrl, cardBackUrl, false);
            cardTransform.GetComponent<Card>().DealToPlayer(color);
            Destroy(cardTransform.gameObject);
            ChangeHeight();
            _cards.Remove(cardID);
            if(changeBottom)
                wwwcontroller.GetCard(_cards[_cards.Count - 1], deckSourceUrl, cardBackUrl, (x => ChangeBottomCardTexture(x.First)));
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
            bool changeBottom = false;

            if (this.isFaceDown)
            {
                cardID = _cards[0];
                _dealtCards.Add(_cards[0]);
            }
            else
            {
                cardID = _cards[_cards.Count - 1];
                _dealtCards.Add(_cards[_cards.Count - 1]);
                changeBottom = true;
            }
            
            newCard.GetComponent<Card>().Instantiate(cardID, deckSourceUrl, cardBackUrl, isFaceDown);
            newCard.position = new Vector3((transform.position.x + 7), 5, transform.position.z);
            newCard.gameObject.SetActive(true);
            ChangeHeight();
            _cards.Remove(cardID);
            if(changeBottom)
                wwwcontroller.GetCard(_cards[_cards.Count - 1], deckSourceUrl, cardBackUrl, (x => ChangeBottomCardTexture(x.First)));
        } 
    }
    
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
