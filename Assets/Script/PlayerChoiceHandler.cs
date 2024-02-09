using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerChoiceHandler : MonoBehaviour
{
    public Button[] buttons; // Groupe des six boutons, un pour chaque joueur
    private int selectionnerBtn = 0; // Nombre de boutons s�lectionn�s
    private Color32 couleurOriginale; // Couleur originale des boutons
    private Color32 couleurChangement = new Color32(255, 0, 0, 255); // Couleur pour indiquer la s�lection
    public TextMeshProUGUI decompte; // Texte affichant le nombre de joueurs s�lectionn�s
    public Button Bouton_choix_finit; // Bouton pour confirmer le choix
    private Select_Classe selectClasseScript; // Acc�s aux scripts
    private Dictionary<Button, bool> boutonEtat; // Stocke si un bouton est s�lectionn� ou non
    private List<string> joueursSelectionnes = new List<string>(); // Liste pour stocker les joueurs s�lectionn�s
    private Dictionary<Button, bool> boutonsNePasReactiver = new Dictionary<Button, bool>(); // Pour marquer les boutons qui ne doivent pas �tre r�activ�s

    void Start()
    {
        selectClasseScript = FindObjectOfType<Select_Classe>();
        List<string> playerClasses = selectClasseScript.GetPlayerNames();

        boutonEtat = new Dictionary<Button, bool>();
        couleurOriginale = buttons[0].GetComponent<Image>().color;

        for (int i = 0; i < buttons.Length; i++)
        {
            boutonEtat.Add(buttons[i], false);
            int buttonIndex = i;
            buttons[i].onClick.AddListener(() => BoutonCliquer(buttonIndex));
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
    }

    void BoutonCliquer(int buttonIndex)
    {
        Button btn = buttons[buttonIndex];
        bool estSelectionne = boutonEtat[btn];
        string nomJoueur = btn.GetComponentInChildren<TextMeshProUGUI>().text;

        if (!estSelectionne && selectionnerBtn < 2)
        {
            selectionnerBtn++;
            boutonEtat[btn] = true;
            btn.GetComponent<Image>().color = couleurChangement;
            joueursSelectionnes.Add(nomJoueur);
        }
        else if (estSelectionne)
        {
            selectionnerBtn--;
            boutonEtat[btn] = false;
            btn.GetComponent<Image>().color = couleurOriginale;
            joueursSelectionnes.Remove(nomJoueur);
        }

        UpdateDecompteText();
        VerifierEtatBoutons();
        VerifierBoutonChoixFinit();
    }

    void VerifierEtatBoutons()
    {
        foreach (Button btn in buttons)
        {
            if (boutonsNePasReactiver.ContainsKey(btn) && boutonsNePasReactiver[btn])
            {
                continue; // Ne pas r�activer ce bouton sp�cifiquement
            }

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
        foreach (string joueur in joueursSelectionnes)
        {
            Debug.Log("Joueur s�lectionn� : " + joueur);
        }
    }

    public void SetPlayerButtonActiveState(string playerName, bool isActive)
    {
        foreach (Button button in buttons)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>().text == playerName)
            {
                button.interactable = isActive;
                if (!isActive)
                {
                    // Marquer le bouton pour ne pas �tre r�activ� automatiquement
                    boutonsNePasReactiver[button] = true;
                }
                else
                {
                    // Enlever le marqueur si le bouton est r�activ� manuellement
                    boutonsNePasReactiver[button] = false;
                }
                break;
            }
        }
    }
}
