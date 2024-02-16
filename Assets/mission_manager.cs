using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Importer le namespace pour utiliser UI Button

public class mission_manager : MonoBehaviour
{
    [SerializeField] private Button failButton; // Bouton pour faire �chouer la mission
    [SerializeField] private Button successButton; // Bouton pour faire r�ussir la mission

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        // Assurez-vous que les boutons sont assign�s pour �viter les erreurs
        if (failButton != null)
        {
            failButton.onClick.AddListener(OnMissionFailButtonClicked);
        }
        if (successButton != null)
        {
            successButton.onClick.AddListener(OnMissionSuccessButtonClicked);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Fonction appel�e lorsque le bouton d'�chec de mission est cliqu�
    private void OnMissionFailButtonClicked()
    {
        Debug.Log("Le joueur a fait �chouer la mission");
    }

    // Fonction appel�e lorsque le bouton de r�ussite de mission est cliqu�
    private void OnMissionSuccessButtonClicked()
    {
        Debug.Log("Le joueur a r�ussi la mission");
    }
}
