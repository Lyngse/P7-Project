using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour, IPointerUpHandler
{

    public virtual void OnPointerUp(PointerEventData ped)
    {
        OnExitGame();
    }

    private void OnExitGame()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}
