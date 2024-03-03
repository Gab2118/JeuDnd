using UnityEngine;
using TMPro;
using static SocketManager;
using UnityEngine.SceneManagement;
using System.Collections;

public class Select_Role : MonoBehaviour
{
    public TMP_Text text_role;

    private void Awake()
    {
        SocketManager.OnRoleAssigned += UpdateRoleUI;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(BeginSceneAfterDelay(4));
    }

    private IEnumerator BeginSceneAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay); // Attend 10 secondes
        SceneManager.LoadScene("scene_game_chef"); // Charge la scène souhaitée
    }

    private void UpdateRoleUI(PlayerInfo playerInfo)
    {
        if (playerInfo != null && playerInfo.playerId == SocketManager.Instance.Socketid)
        {
            UpdateRoleText(playerInfo.playerRoleName);
        }
        else
        {
            Debug.LogError("Les informations du role ne sont pas disponibles ou ne correspondent pas au joueur actuel.");
        }
    }

    private void UpdateRoleText(string chefName)
    {
        if (text_role != null)
        {
            text_role.text = $"Votre role est : {chefName}";
        }
        else
        {
            Debug.LogError("Référence TMP_Text pour le chef est null.");
        }
    }

    private void OnDestroy()
    {
        SocketManager.OnChefAssigned -= UpdateRoleUI;
    }
}