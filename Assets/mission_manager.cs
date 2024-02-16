using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Importer le namespace pour utiliser UI Button

public class mission_manager : MonoBehaviour
{
    [SerializeField] private Button failButton; // Bouton pour faire échouer la mission
    [SerializeField] private Button successButton; // Bouton pour faire réussir la mission

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        // Assurez-vous que les boutons sont assignés pour éviter les erreurs
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

    // Fonction appelée lorsque le bouton d'échec de mission est cliqué
    private void OnMissionFailButtonClicked()
    {
        Debug.Log("Le joueur a fait échouer la mission");
    }

    // Fonction appelée lorsque le bouton de réussite de mission est cliqué
    private void OnMissionSuccessButtonClicked()
    {
        Debug.Log("Le joueur a réussi la mission");
    }
}
