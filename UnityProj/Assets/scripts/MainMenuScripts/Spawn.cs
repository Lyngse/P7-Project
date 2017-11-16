using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spawn : MonoBehaviour, IPointerUpHandler
{
    private GameObject prefab;
    private Vector3 startingPosition = new Vector3(0, 1, 0);

    public virtual void OnPointerUp(PointerEventData ped)
    {
        SpawnObject();
    }

    private void SpawnObject()
    {
        GameObject currObject = Instantiate(prefab) as GameObject;
        currObject.transform.position = startingPosition;
    }

    public void setPrefab(GameObject pFab)
    {
        this.prefab = pFab;
    }
}
