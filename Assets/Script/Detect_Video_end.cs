using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ChangeSceneOnVideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string sceneToLoad = "Scene_Select_role";
    public string sceneOnVideoEnd = "Scene_attente";

    private void Start()
    {
        // Assurez-vous que le VideoPlayer est r�f�renc�.
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Abonnez-vous � l'�v�nement de fin de lecture de la vid�o.
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // D�sabonnez-vous de l'�v�nement pour �viter les appels multiples.
        vp.loopPointReached -= OnVideoEnd;

        // V�rifiez si la sc�ne actuelle est "Scene_attente" et changez-la en "presentation" si c'est le cas.
        if (SceneManager.GetActiveScene().name == sceneOnVideoEnd)
        {
            SceneManager.LoadScene("presentation");
        }
        else
        {
            // Sinon, chargez la sc�ne d�finie dans sceneToLoad lorsque la vid�o se termine.
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
