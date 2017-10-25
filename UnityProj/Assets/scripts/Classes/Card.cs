using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.Classes
{
    public class Card
    {
        //Needs to store some information about face and back side of the card in two different variables, not sure what to store them as.
        //public string imgUrl;
        public Texture2D frontImg;
        //public Texture2D backImg;
        public bool isFaceDown = true;
        public MeshRenderer mr;
        public Transform transform;

        public Card()
        {
        }
    }
}
