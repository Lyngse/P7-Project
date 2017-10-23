using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.Classes
{
    public class TouchInfo
    {
        public int fingerID;
        public Transform hitTransform;
        public float touchTimer;
        public bool isMoving = false;

        public TouchInfo(int fID, Transform hTransform, float timer)
        {
            this.fingerID = fID;
            this.hitTransform = hTransform;
            this.touchTimer = timer;
        }        
    }
}
