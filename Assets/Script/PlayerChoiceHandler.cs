using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// CE SCRIPT PERMETS AUX CHEF DE CHOISIR 2 JOUEURS � ENVOYEZ EN MISSION

public class PlayerChoiceHandler : MonoBehaviour
{
    // groupe des six bouton, un pour chaque joueur ( avec leur nom �crit dedans)
    public Button[] buttons;
    
    // nombre de bouton s�lectionner
    private int selectionnerBtn = 0;

    // couleur de visualiser la s�lection
    private Color32 couleurOriginale; // couleur original
    private Color32 couleurChangement = new Color32(255, 0, 0, 255);

    // text affichant en direct le nombre de joueur s�lectionner
    public TextMeshProUGUI decompte;

    // btn pour confirmer le choix
    public Button Bouton_choix_finit;

    // acc�s au scripts
    private Select_Classe selectClasseScript;

 
  
 // stocker si il est s�lectionner ou non
    private Dictionary<Button, bool> boutonEtat;
    private List<string> joueursSelectionnes; // Liste pour stocker les joueurs s�lectionn�s

    void Start()
    {
        selectClasseScript = FindObjectOfType<Select_Classe>();
        List<string> playerClasses = selectClasseScript.GetPlayerNames();
        joueursSelectionnes = new List<string>();

        boutonEtat = new Dictionary<Button, bool>();
        couleurOriginale = buttons[0].GetComponent<Image>().color;

        for (int i = 0; i < buttons.Length; i++)
        {
            boutonEtat.Add(buttons[i], false);
            int buttonIndex = i; // Capture l'index actuel de la boucle
            buttons[i].onClick.AddListener(() => BoutonCliquer(buttonIndex)); // Utilisez buttonIndex ici
            if (i < playerClasses.Count)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerClasses[i];
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
        UpdateDecompteText();

        Bouton_choix_finit.onClick.AddListener(AfficherJoueursSelectionnes);
        Bouton_choix_finit.interactable = false;

        // Afficher les joueurs s�lectionn�s ou un message si aucun joueur n'est s�lectionn� au d�but de la sc�ne
        if (joueursSelectionnes.Count > 0)
        {
            foreach (string joueur in joueursSelectionnes)
            {
                Debug.Log("Joueur s�lectionn� : " + joueur);
            }
        }
        else
        {
            Debug.Log("Aucun joueur s�lectionn�.");
        }
    }

    void BoutonCliquer(int buttonIndex)
    {
        Button btn = buttons[buttonIndex]; // Obtenez le bouton � partir de l'index
        bool estSelectionne = boutonEtat[btn];
        string nomJoueur = btn.GetComponentInChildren<TextMeshProUGUI>().text;

        if (!estSelectionne && selectionnerBtn < 2)
        {
            selectionnerBtn++;
            boutonEtat[btn] = true;
            btn.GetComponent<Image>().color = couleurChangement;
            joueursSelectionnes.Add(nomJoueur); // Ajoutez le joueur � la liste des joueurs s�lectionn�s
        }
        else if (estSelectionne)
        {
            selectionnerBtn--;
            boutonEtat[btn] = false;
            btn.GetComponent<Image>().color = couleurOriginale;
            joueursSelectionnes.Remove(nomJoueur); // Retirez le joueur de la liste des joueurs s�lectionn�s
        }

        UpdateDecompteText();
        VerifierEtatBoutons();
        VerifierBoutonChoixFinit();
    }

    void VerifierEtatBoutons()
    {
        foreach (Button btn in buttons)
        {
            if (!boutonEtat[btn] && selectionnerBtn < 2)
            {
                btn.interactable = true;
            }
            else if (!boutonEtat[btn] && selectionnerBtn >= 2)
            {
                btn.interactable = false;
            }
        }
    }

    void UpdateDecompteText()
    {
        decompte.text = $"{selectionnerBtn}/2";
    }

    void VerifierBoutonChoixFinit()
    {
        Bouton_choix_finit.interactable = (selectionnerBtn == 2);
    }

    void AfficherJoueursSelectionnes()
    {
        if (joueursSelectionnes.Count > 0)
        {
            foreach (string joueur in joueursSelectionnes)
            {
                Debug.Log("Joueur s�lectionn� : " + joueur);
            }
        }
        else
        {
            Debug.Log("Aucun joueur s�lectionn�.");
        }
    }


    // Pour la gestion des couleurs des boutons
    public Button[] choiceButtons; // Assurez-vous d'assigner vos boutons dans l'inspecteur Unity
    private Color32 defaultButtonColor = new Color32(255, 255, 255, 255); // Couleur initiale des boutons
    private Color32 selectedButtonColor = new Color32(255, 0, 0, 255); // Couleur lorsqu'un bouton est s�lectionn�
    private Button selectedButton;
}
