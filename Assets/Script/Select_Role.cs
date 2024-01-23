using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RandomWordDisplay : MonoBehaviour
{
    public TMP_Text wordText;
    public string mot_selectionner { get; private set; }

    private string[] wordBank = { "Voleur", "Rôdeur", "Cultiste", "Moine", "Barde", "Occultiste", "Sorcière", "Paladin", "Guerrier", "Clerc", "Nécromancien" };
    private int currentIndex = 0;
    private float changeInterval = 0.1f; // Intervalle de changement initial en secondes
    private float timer = 0.0f;
    private bool slowingDown = false;

    private void Start()
    {
        // Affichez un mot aléatoire au début.
        ShowRandomWord();
        // Appelez la fonction SlowDownWords après 5 secondes.
        Invoke("SlowDownWords", 5.0f);
    }

    private void Update()
    {
        // Incrémentez le compteur de temps.
        timer += Time.deltaTime;

        // Si le temps écoulé atteint l'intervalle de changement, changez le mot.
        if (timer >= changeInterval)
        {
            ShowRandomWord();
            timer = 0.0f; // Réinitialisez le compteur de temps.
        }
    }

    private void ShowRandomWord()
    {
        // Choisissez un mot aléatoire dans la banque.
        int randomIndex = Random.Range(0, wordBank.Length);
        string randomWord = wordBank[randomIndex];

        // Affichez le mot aléatoire.
        wordText.text = randomWord;
    }

    private void SlowDownWords()
    {
        // Arrêtez de changer les mots après 5 secondes.
        changeInterval = float.MaxValue;

        // Obtenez le mot actuellement affiché.
        string displayedWord = wordText.text;

        // Ajoutez le statut "mot_selectionne" au mot.
        mot_selectionner = "Le role sélectionné est " + displayedWord;

        // Affichez le contenu de "mot_selectionner" dans la console.
        Debug.Log("Contenu de mot_selectionner : " + mot_selectionner);

        // Attendez 5 secondes avant de changer de scène vers "Scene_choix_chef".
        Invoke("ChangerVersSceneChoixChef", 5.0f);
    }

    private void ChangerVersSceneChoixChef()
    {
        // Chargez la scène "Scene_choix_chef".
        SceneManager.LoadScene("Scene_choix_chef");
    }
}