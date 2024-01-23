using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RandomWordDisplay : MonoBehaviour
{
    public TMP_Text wordText;
    public string mot_selectionner { get; private set; }

    private string[] wordBank = { "Voleur", "R�deur", "Cultiste", "Moine", "Barde", "Occultiste", "Sorci�re", "Paladin", "Guerrier", "Clerc", "N�cromancien" };
    private int currentIndex = 0;
    private float changeInterval = 0.1f; // Intervalle de changement initial en secondes
    private float timer = 0.0f;
    private bool slowingDown = false;

    private void Start()
    {
        // Affichez un mot al�atoire au d�but.
        ShowRandomWord();
        // Appelez la fonction SlowDownWords apr�s 5 secondes.
        Invoke("SlowDownWords", 5.0f);
    }

    private void Update()
    {
        // Incr�mentez le compteur de temps.
        timer += Time.deltaTime;

        // Si le temps �coul� atteint l'intervalle de changement, changez le mot.
        if (timer >= changeInterval)
        {
            ShowRandomWord();
            timer = 0.0f; // R�initialisez le compteur de temps.
        }
    }

    private void ShowRandomWord()
    {
        // Choisissez un mot al�atoire dans la banque.
        int randomIndex = Random.Range(0, wordBank.Length);
        string randomWord = wordBank[randomIndex];

        // Affichez le mot al�atoire.
        wordText.text = randomWord;
    }

    private void SlowDownWords()
    {
        // Arr�tez de changer les mots apr�s 5 secondes.
        changeInterval = float.MaxValue;

        // Obtenez le mot actuellement affich�.
        string displayedWord = wordText.text;

        // Ajoutez le statut "mot_selectionne" au mot.
        mot_selectionner = "Le role s�lectionn� est " + displayedWord;

        // Affichez le contenu de "mot_selectionner" dans la console.
        Debug.Log("Contenu de mot_selectionner : " + mot_selectionner);

        // Attendez 5 secondes avant de changer de sc�ne vers "Scene_choix_chef".
        Invoke("ChangerVersSceneChoixChef", 5.0f);
    }

    private void ChangerVersSceneChoixChef()
    {
        // Chargez la sc�ne "Scene_choix_chef".
        SceneManager.LoadScene("Scene_choix_chef");
    }
}