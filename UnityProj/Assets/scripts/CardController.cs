using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.scripts.Classes;

public class CardController : MonoBehaviour {
    private List<Card> _cards;
    private Deck _deck;


	// Use this for initialization
	void Start () {
        _deck = new Deck(_cards);
	}

    public void ShuffleDeck(Deck deck)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < deck.cards.Count; i++)
        {
            int j = random.Next(i, deck.cards.Count);
            Card temp = deck.cards[i];
            deck.cards[i] = deck.cards[j];
            deck.cards[j] = temp;
        }
    }


    //Need to make a prefab of the card, which then can be spawned.
    public void DealToPlayerFromDeck(Deck deck)
    {
        if(deck.isFaceDown)
        {
            //Send the first card of the list to the given player's hand
        }
        else
        {
            //Send the last card of the list to the given player's hand
        }
    }

    public void DealToPlayer(Card card)
    {
        //Send card to given player's hand
    }

    public void DrawToTable(Deck deck)
    {
        if (deck.isFaceDown)
        {
            //Send the first card of the list to the table next to the deck
        }
        else
        {
            //Send the last card of the list to the table next tot the deck
        }
    }

    public void PlaceCardOnTop(Card card)
    {

    }
}
