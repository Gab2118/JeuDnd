using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameOnClick : MonoBehaviour
{
    // Le nom de la scène que vous souhaitez charger (dans cet exemple, "Presentation").
    public string sceneToLoad = "Scene_attente";

    private void Update()
    {
        // Vérifiez si l'utilisateur clique avec le bouton gauche de la souris.
        if (Input.GetMouseButtonDown(0))
        {
            // Chargez la scène de présentation.
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
