using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.Classes
{
    public class MenuInfo
    {
        public Canvas canvas;
        public Transform hitTransform;
        public bool isOpen;

        public MenuInfo(Canvas inputCanvas, Transform hTransform, bool isEnabled)
        {
            this.canvas = inputCanvas;
            this.hitTransform = hTransform;
            this.isOpen = isEnabled;
        }
    }
}
