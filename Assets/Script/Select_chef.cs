using UnityEngine;
using TMPro;
using static SocketManager;
using UnityEngine.SceneManagement;
using System.Collections;

public class Select_Chef : MonoBehaviour
{
    public TMP_Text text_chef;

    private void OnEnable()
    {
        SocketManager.OnChefAssigned += UpdateChefUI;
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(BeginSceneAfterDelay(4));
    }
    private IEnumerator BeginSceneAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay); // Attend 10 secondes
        SceneManager.LoadScene("Scene_choix_role"); // Charge la sc�ne souhait�e
    }

    private void OnDisable()
    {
        SocketManager.OnChefAssigned -= UpdateChefUI;
    }

 private void UpdateChefUI(PlayerInfo playerInfo)
{
  // Recherche du joueur qui est le chef
PlayerInfo chef = SocketManager.Instance.playerInfos.Find(p => p.isChief); 

if (chef != null)
{
    Debug.Log("Nom du Chef: " + chef.playerClass); // Log pour vérification
    text_chef.text = $"Le chef est : {chef.playerClass}"; // Mise à jour du texte avec le nom du chef
}
else
{
    Debug.LogError("Les informations du chef ne sont pas disponibles.");
}
}

}
