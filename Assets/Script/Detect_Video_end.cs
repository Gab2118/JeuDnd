using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// CE SCRIPT EST UTILISER POUR D�TECTER LA FIN D'UNE VID�O MP4
public class ChangeSceneOnVideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string sceneToLoad = "Scene_Select_role";
    public string sceneOnVideoEnd = "Scene_attente";

    private void Start()
    {
       
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

       
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
   
        vp.loopPointReached -= OnVideoEnd;


        if (SceneManager.GetActiveScene().name == sceneOnVideoEnd)
        {
            SceneManager.LoadScene("presentation");
        }
        else
        {
          
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
