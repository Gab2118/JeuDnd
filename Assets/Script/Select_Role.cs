using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class RandomWordDisplay : MonoBehaviour
{
    public TMP_Text text_role; // Reference to the text_role TMP_Text
    public string mot_selectionner { get; private set; }

    public string joueurUn = "";
    public string joueurDeux = "";
    public string joueurTrois = "";
    public string joueurQuatre = "";
    public string joueurCinq = "";
    public string joueurSix = "";

    // liste des nom de classe
    private string[] wordBank = { "Asssassin", "R�deur", "Barbare", "Moine", "Barde", "Occultiste", "Sorci�re", "Paladin", "Guerrier", "Clerc", "N�cromancien" };


    private void Start()
    {
        // Appelez la fonction SlowDownWords apr�s 5 secondes.
        Invoke("AssignRandomWordsToPlayers", 5.0f);

    }

    private void Update()
    {
        // You can add any periodic updates here, if necessary.
    }

    private void AssignRandomWordsToPlayers()
    {
        List<string> availableWords = new List<string>(wordBank);
        // Assignez un mot al�atoire � chaque VARIABLE joueur
        joueurUn = GetRandomWord(availableWords);
        UpdatePlayerRoleText(); // Update the text_role to match joueurUn

        joueurDeux = GetRandomWord(availableWords);
        joueurTrois = GetRandomWord(availableWords);
        joueurQuatre = GetRandomWord(availableWords);
        joueurCinq = GetRandomWord(availableWords);
        joueurSix = GetRandomWord(availableWords);

        // Affichez les mots s�lectionn�s pour chaque joueur dans la console
        Debug.Log("Joueur Un : " + joueurUn);
        Debug.Log("Joueur Deux : " + joueurDeux);
        Debug.Log("Joueur Trois : " + joueurTrois);
        Debug.Log("Joueur Quatre : " + joueurQuatre);
        Debug.Log("Joueur Cinq : " + joueurCinq);
        Debug.Log("Joueur Six : " + joueurSix);

        ChangerVersSceneChoixChef();
    }

    private void UpdatePlayerRoleText()
    {
        // Ensure text_role is not null before trying to set its text property
        if (text_role != null)
        {
            text_role.text = joueurUn; // Set the text_role text to the value of joueurUn
        }
    }

    private string GetRandomWord(List<string> words)
    {
        // Retourne un mot al�atoire de la liste et le retire
        int randomIndex = Random.Range(0, words.Count);
        string selectedWord = words[randomIndex];
        words.RemoveAt(randomIndex);
        return selectedWord;
    }
    private void ChangerVersSceneChoixChef()
    {
        // Appelle la m�thode "ChargerSceneChoixChef" apr�s un d�lai de 5 secondes.
        Invoke("ChargerSceneChoixChef", 5f);
    }
    private void ChargerSceneChoixChef()
    {

        // Chargez la sc�ne "Scene_choix_chef".
        SceneManager.LoadScene("Scene_choix_chef");
    }
}
