﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onToggle : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        Canvas canvas = this.GetComponentInParent<Canvas>();
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(canvas.pixelRect.width, canvas.pixelRect.height);
    }
}
