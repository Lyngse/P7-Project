using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ZoomIn()
    {
        transform.Translate(transform.forward);
    }

    public void ZoomOut()
    {
        transform.Translate(-(transform.forward));
    }
}
