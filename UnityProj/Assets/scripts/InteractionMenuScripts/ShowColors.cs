using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.scripts.Classes;

public class ShowColors : MonoBehaviour, IPointerUpHandler
{
    public MenuInfo menu;
    public Transform cardTransform;
    private HostScript currentHost;
    private List<Vector3> positions;
    public Button colorPicker;
    void start()
    {

    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        positions = new List<Vector3>();
        positions.Add(new Vector3(53, -22, 0));
        positions.Add(new Vector3(75, -75, 0));
        positions.Add(new Vector3(53, -128, 0));
        positions.Add(new Vector3(0, -150, 0));
        positions.Add(new Vector3(-53, -128, 0));
        positions.Add(new Vector3(-75, -75, 0));
        positions.Add(new Vector3(-53, -22, 0));
        positions.Add(new Vector3(0, -75, 0));

        colorPicker = menu.canvas.transform.GetChild(1).GetComponent<Button>();
        currentHost = GameObject.Find("NetworkHost").GetComponent<HostScript>();

        menu.canvas.transform.GetChild(0).GetComponent<Button>().gameObject.SetActive(false);
        menu.canvas.transform.GetChild(1).GetComponent<Button>().gameObject.SetActive(true);
        currentHost.clientColors.Add(Utility.ClientColor.red);
        currentHost.clientColors.Add(Utility.ClientColor.black);
        currentHost.clientColors.Add(Utility.ClientColor.blue);
        currentHost.clientColors.Add(Utility.ClientColor.green);
        currentHost.clientColors.Add(Utility.ClientColor.orange);
        currentHost.clientColors.Add(Utility.ClientColor.purple);
        currentHost.clientColors.Add(Utility.ClientColor.white);
        currentHost.clientColors.Add(Utility.ClientColor.yellow);


        for (int i = 0; i < currentHost.clientColors.Count; i++)
        {
            Button newBtn = Instantiate(Resources.Load<Button>("ColorPickButtonPrefab")) as Button;
            newBtn.transform.parent = colorPicker.transform;
            newBtn.transform.position = colorPicker.transform.position + positions[i];
            newBtn.image.color = Utility.colors[(int)currentHost.clientColors[i]];
        }
        for (int i = 0; i < menu.canvas.transform.GetChild(1).childCount; i++)
        {
            var child = menu.canvas.transform.GetChild(1).GetChild(i).GetComponent<DealToPlayer>();
            child.cardTransform = cardTransform;
        }
    }


}

