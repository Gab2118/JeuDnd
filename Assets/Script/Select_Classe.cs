using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Select_Classe : MonoBehaviour
{
    public TMP_Text text_class;
   

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        UpdatePlayerUI();
        StartCoroutine(BeginSceneAfterDelay(4));
    }
    private IEnumerator BeginSceneAfterDelay(float delay)
    {
    
        yield return new WaitForSeconds(delay); // Attend 10 secondes
        SceneManager.LoadScene("Scene_choix_chef"); // Charge la scène souhaitée
    }
    private void UpdatePlayerUI()
    {
        Debug.Log("Tentative de récupération des infos pour l'ID de socket: " + SocketManager.Instance.Socketid);
        SocketManager.Instance.playerInfos.ForEach(player => Debug.Log("Player ID disponible: " + player.playerId));


        if (!string.IsNullOrEmpty(SocketManager.Instance.Socketid))
        {
            SocketManager.PlayerInfo currentPlayerInfo = SocketManager.Instance.GetCurrentPlayerInfo(SocketManager.Instance.Socketid);

            if (currentPlayerInfo != null)
            {
                UpdatePlayerClassText(currentPlayerInfo.playerClass);
            }
            else
            {
                Debug.LogError("Impossible de récupérer les informations du joueur actuel.");
            }
        }
        else
        {
            Debug.LogError("PlayerID est vide.");
        }
    }
    private void UpdatePlayerClassText(string playerClass)
    {
        if (text_class != null)
        {
            // string chiefText = isChief ? " et vous êtes le chef" : "";/
            text_class.text = $"Votre classe : {playerClass}";
        }
        else
        {
            Debug.LogError("Référence TMP_Text est null.");
        }
    }
}