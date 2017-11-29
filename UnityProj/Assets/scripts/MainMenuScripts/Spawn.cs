using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spawn : MonoBehaviour, IPointerUpHandler
{
    private GameObject prefab;
    private string[] args;
    private Vector3 startingPosition = new Vector3(0, 1, 0);

    public virtual void OnPointerUp(PointerEventData ped)
    {
        SpawnObject();
    }

    private void SpawnObject()
    {
        GameObject currObject = Instantiate(prefab) as GameObject;
        switch (currObject.tag)
        {
            case "Figurine":
                currObject.GetComponent<Figurine>().Instantiate(args[0], args[1], args[2]);
                break;
            case "Deck":
                currObject.GetComponent<Deck>().InstantiateDeck(args[0], args[1], args[2]);
                break;
            default:
                currObject.name = args[0];
                break;
        }
        //currObject.transform.position = startingPosition;
    }

    public void instatiate(GameObject pFab, string[] args)
    {
        this.prefab = pFab;
        this.args = args;
    }
}
