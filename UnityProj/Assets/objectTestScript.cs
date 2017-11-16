using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class objectTestScript : MonoBehaviour
{

    public GameObject realCoin;

	// Use this for initialization
	void Start ()
	{
	    //var mymeshfilter = GetComponent<MeshFilter>();
	    //var objImporter = new ObjImporter();
	    //mymeshfilter.mesh = objImporter.ImportFile("C:\\Users\\Andreas Smed\\Documents\\GitHub\\P7-Project\\UnityProj\\coin.obj");
	    ////mymeshfilter.mesh.triangles = mymeshfilter.mesh.triangles.Reverse().ToArray();
        //mymeshfilter.mesh.RecalculateNormals();
        //mymeshfilter.mesh.RecalculateTangents();
        //
        //
        //var renderer = GetComponent<MeshRenderer>();
	    //var texture = Resources.Load<Texture2D>("coin");
	    //
        //var bytes = System.IO.File.ReadAllBytes("C:\\Users\\Andreas Smed\\Documents\\GitHub\\P7-Project\\UnityProj\\Assets\\Resources\\coin.png");
        //var newTexture = new Texture2D(280,672, TextureFormat.DXT1,false);
	    //newTexture.LoadImage(bytes);
        //
        //
	    //renderer.material.mainTexture = texture;
	    //var realMesh = realCoin.GetComponent<MeshFilter>().mesh;
	}

    Texture2D FlipTexture(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);

        int xN = original.width;
        int yN = original.height;


        for (int i = 0; i < xN; i++)
        {
            for (int j = 0; j < yN; j++)
            {
                flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));
            }
        }
        flipped.Apply();

        return flipped;
    }

    // Update is called once per frame
    void Update () {
    }
}
