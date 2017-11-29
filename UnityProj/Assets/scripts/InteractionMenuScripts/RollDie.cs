using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RollDie : MonoBehaviour, IPointerUpHandler {
    public Transform dieTransform;
    private Rigidbody thisBody;

    public virtual void OnPointerUp(PointerEventData ped)
    {
        thisBody = dieTransform.GetComponent<Rigidbody>();
        thisBody.maxAngularVelocity = 50;
        StartCoroutine(Roll());
    }

    IEnumerator Roll()
    {
        var force = Random.Range(72, 125);
        thisBody.AddForce(0, force, 0);
        yield return new WaitForSeconds(0.05f);
        var speed = Random.Range(10, 25);
        thisBody.angularVelocity = Random.insideUnitSphere * speed;
    }


}
