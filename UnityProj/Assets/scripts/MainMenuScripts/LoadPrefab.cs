using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SimpleFileBrowser.Scripts.GracesGames;


public class LoadPrefab : MonoBehaviour, IPointerUpHandler
{
    FileBrowser browser = new FileBrowser();

    public virtual void OnPointerUp(PointerEventData ped)
    {
        LoadPrefabToMenu();
    }

    private void LoadPrefabToMenu()
    {
        //browser
    }
}
