using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public List<GameObject> playerImages;
    public TMP_Text timerText;
    private Coroutine countdownCoroutine;

    private void OnEnable()
    {
        SocketManager.OnLobbyUpdated += UpdateLobby;
    }

    private void OnDisable()
    {
        SocketManager.OnLobbyUpdated -= UpdateLobby;
    }

    public void UpdateLobby(int playerCount)
    {
        Debug.Log("Mise à jour du lobby avec " + playerCount + " joueurs");
        for (int i = 0; i < playerImages.Count; i++)
        {
            playerImages[i].SetActive(i < playerCount);
        }

        if (playerCount == 2 && countdownCoroutine == null)  //CHANGER LES VALEURS SELON LE NOMBRE DE JOUEUR VISÉ
        {
            countdownCoroutine = StartCoroutine(StartCountdown(5));
        }
        else if (playerCount < 2 && countdownCoroutine != null) //CHANGER LES VALEURS SELON LE NOMBRE DE JOUEUR VISÉ
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
            timerText.text = ""; // Reset le texte du timer
        }
    }

    private IEnumerator StartCountdown(int duration)
    {
        int timeLeft = duration;
        timerText.gameObject.SetActive(true);

        while (timeLeft > 0)
        {
            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        timerText.gameObject.SetActive(false);
        SceneManager.LoadScene("scene_Select_classe");
    }
}