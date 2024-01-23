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
        // Assurez-vous que le VideoPlayer est référencé.
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Abonnez-vous à l'événement de fin de lecture de la vidéo.
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Désabonnez-vous de l'événement pour éviter les appels multiples.
        vp.loopPointReached -= OnVideoEnd;

        // Vérifiez si la scène actuelle est "Scene_attente" et changez-la en "presentation" si c'est le cas.
        if (SceneManager.GetActiveScene().name == sceneOnVideoEnd)
        {
            SceneManager.LoadScene("presentation");
        }
        else
        {
            // Sinon, chargez la scène définie dans sceneToLoad lorsque la vidéo se termine.
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
