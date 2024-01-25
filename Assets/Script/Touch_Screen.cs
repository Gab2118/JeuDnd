using UnityEngine;
using UnityEngine.SceneManagement;


// CE SCRIPT � POUR BUT  DE SIMPLEMENT PERMETTRE LE TOUCH DE L'�CRAN ET LE CHANGEMENT DE SCENE POUR LA PAGE D'ACCUEIL
public class StartGameOnClick : MonoBehaviour
{
    // Le nom de la sc�ne que vous souhaitez charger (dans cet exemple, "Presentation").
    public string sceneToLoad = "Scene_attente";

    private void Update()
    {
        // V�rifiez si l'utilisateur clique avec le bouton gauche de la souris.
        if (Input.GetMouseButtonDown(0))
        {
            // Chargez la sc�ne de pr�sentation.
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
