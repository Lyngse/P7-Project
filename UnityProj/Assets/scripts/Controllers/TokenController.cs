using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenController : MonoBehaviour {
    public Texture2D img1;
    public Texture2D img2;
	// Use this for initialization
	void Start () {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = img1;
        transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = img2;
    }

}
