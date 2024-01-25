using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

// CE SCRIPT � POUR BUT DE DISTRIBUER LES CLASSES AUX SIX JOUEUR ET DE LES GARDER EN M�MOIRE

public class Select_Classe : MonoBehaviour
{
    public TMP_Text text_class; 
    public string mot_selectionner { get; private set; }


    public string joueurUn = "";
    public string joueurDeux = "";
    public string joueurTrois = "";
    public string joueurQuatre = "";
    public string joueurCinq = "";
    public string joueurSix = "";

    public List<string> PlayerNames { get; private set; }
    // liste des nom de classe
    private string[] classBank = { "Asssassin", "R�deur", "Barbare", "Moine", "Barde", "Occultiste", "Sorci�re", "Paladin", "Guerrier", "Clerc", "N�cromancien" };
  




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
        List<string> availableWords = new List<string>(classBank);
        // Assignez un mot al�atoire � chaque VARIABLE joueur
        joueurUn = GetRandomWord(availableWords);
        UpdatePlayerClassText();

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

    private void UpdatePlayerClassText()
    {

        if (text_class != null)
        {
            text_class.text = joueurUn;
        }
    }
    public List<string> GetPlayerNames()
    {
        return PlayerNames;
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