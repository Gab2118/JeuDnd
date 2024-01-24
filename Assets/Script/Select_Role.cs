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

    public List<string> PlayerNames { get; private set; }
    // liste des nom de classe
    private string[] wordBank = { "Asssassin", "R�deur", "Barbare", "Moine", "Barde", "Occultiste", "Sorci�re", "Paladin", "Guerrier", "Clerc", "N�cromancien" };
    // Liste des roles 




    private void Start()
    {
        PlayerNames = new List<string>();
        DontDestroyOnLoad(this);
        Invoke("AssignRandomWordsToPlayers", 5.0f);

    }

    private void Update()
    {
     // enlever a la fin si inutilis�
    }

    private void AssignRandomWordsToPlayers()
    {
        List<string> availableWords = new List<string>(wordBank);
        // Assignez un mot al�atoire � chaque VARIABLE joueur
        joueurUn = GetRandomWord(availableWords);
        UpdatePlayerRoleText();

        joueurDeux = GetRandomWord(availableWords);
        joueurTrois = GetRandomWord(availableWords);
        joueurQuatre = GetRandomWord(availableWords);
        joueurCinq = GetRandomWord(availableWords);
        joueurSix = GetRandomWord(availableWords);

        // Ajoute les noms des joueurs � la liste
        PlayerNames.Add(joueurUn);
        PlayerNames.Add(joueurDeux);
        PlayerNames.Add(joueurTrois);
        PlayerNames.Add(joueurQuatre);
        PlayerNames.Add(joueurCinq);
        PlayerNames.Add(joueurSix);
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
       
        if (text_role != null)
        {
            text_role.text = joueurUn; 
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
        Invoke("ChargerSceneChoixChef", 5f);
    }
    private void ChargerSceneChoixChef()
    {


        SceneManager.LoadScene("Scene_choix_chef");
    }

 
}
