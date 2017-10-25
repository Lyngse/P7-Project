﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.scripts.Classes;

public class DeckController : MonoBehaviour {
    private List<Card> _cards = new List<Card>();
    private List<Card> _dealtCards = new List<Card>();
    private Deck _deck;
    public Texture2D sourceTex;
    public Transform cardPrefab;

    // Use this for initialization
    void Start () {
        InstantiateDeck();
	}

    //Instantiate the deck
    public void InstantiateDeck()
    {
        var cardHeight = 4095 / 7;
        var cardWidth = 4095 / 10;
        for (int i = 0; i < 10; i++)
        {
            for (int y = 0; y < 7; y++)
            {
                var card = new Card();
                var newCard = Instantiate(cardPrefab);
                MeshRenderer mr = newCard.GetComponent<MeshRenderer>();
                newCard.gameObject.SetActive(false);
                card.mr = mr;
                card.transform = newCard;
                card.frontImg = CropImageToCard(sourceTex, (i * cardWidth), (y * cardHeight), cardWidth, cardHeight);
                _cards.Add(card);
            }
        }

        Vector3[] vertices =
        {
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
        };
        Vector2[] uvs =
        {
            new Vector2(0, 0),
            new Vector2(100f, 0),
            new Vector2(0, 100f),
            new Vector2(100f, 100f),
        };
        int[] triangles =
        {
            0, 2, 1,
			1, 2, 3,
        };

        foreach (Card card in _cards)
        {
            card.mr.material.mainTexture = card.frontImg;
            Mesh mesh = card.transform.GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
        }
        _deck = new Deck(_cards);
    }

    //Reset deck by calling InstantiateDeck again and clear the list if dealtCards
    public void ResetDeck()
    {
        InstantiateDeck();
        _dealtCards.Clear();
    }

    public void ShuffleDeck()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < _deck.cards.Count; i++)
        {
            int j = random.Next(i, _deck.cards.Count);
            Card temp = _deck.cards[i];
            _deck.cards[i] = _deck.cards[j];
            _deck.cards[j] = temp;
        }
    }

    public void DealToPlayerFromDeck()
    {
        if(_deck.isFaceDown)
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
        if (_deck.isFaceDown)
        {
            //Send the first card of the list to the table next to the deck
            //_cards[0].transform.rotation();
            _cards[0].transform.gameObject.SetActive(true);
            _dealtCards.Add(_cards[0]);
            _cards.Remove(_cards[0]);
        }
        else
        {
            //Send the last card of the list to the table next tot the deck
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
