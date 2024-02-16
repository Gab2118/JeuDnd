using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerChoiceHandler : MonoBehaviour
{
    public Button[] buttons; // Groupe des six boutons, un pour chaque joueur
    private int selectionnerBtn = 0; // Nombre de boutons sélectionnés
    private Color32 couleurOriginale; // Couleur originale des boutons
    public Color32 couleurChangement = new Color32(255, 0, 0, 255); // Couleur pour indiquer la sélection
    public TextMeshProUGUI decompte; // Texte affichant le nombre de joueurs sélectionnés
    public Button Bouton_choix_finit; // Bouton pour confirmer le choix
    private Select_Classe selectClasseScript; // Accès aux scripts
    private Dictionary<Button, bool> boutonEtat; // Stocke si un bouton est sélectionné ou non
    private List<string> joueursSelectionnes = new List<string>(); // Liste pour stocker les joueurs sélectionnés
    public Dictionary<Button, bool> boutonsNePasReactiver = new Dictionary<Button, bool>(); // Pour marquer les boutons qui ne doivent pas être réactivés
    private Dictionary<Button, bool> boutonsSelectionForces = new Dictionary<Button, bool>();
    private int toursDepuisModification = 0;

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
    public void ReinitialiserCouleurBoutons()
    {
        foreach (Button btn in buttons)
        {
            selectionnerBtn = 0;
            UpdateDecompteText();
            // Réinitialiser la couleur du bouton à la couleur originale
            // Vérifiez d'abord si le bouton ne doit pas rester désactivé à cause d'une autre logique
            if (!boutonsNePasReactiver.ContainsKey(btn) || !boutonsNePasReactiver[btn])
            {
                btn.GetComponent<Image>().color = couleurOriginale;
                btn.interactable = true;


            }
        }
    }

    public void IncrementerToursDepuisModification()
    {
        toursDepuisModification++;

        // Vérifier si deux tours se sont écoulés
        if (toursDepuisModification >= 2)
        {
            // Réinitialiser boutonsNePasReactiver
            foreach (var btn in boutonsNePasReactiver.Keys.ToList())
            {
                boutonsNePasReactiver[btn] = false;
            }

            // Possiblement réactiver les boutons ici si nécessaire
            // Ou laisser la logique de réactivation à une autre partie du code qui vérifie boutonsNePasReactiver

            toursDepuisModification = 0; // Réinitialiser le compteur
        }
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
                continue; // Ne pas réactiver ce bouton spécifiquement
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
            Debug.Log("Joueur sélectionné : " + joueur);
        }
        ReinitialiserCouleurBoutons();

        // Ajout pour réinitialiser l'état de sélection de tous les boutons
        foreach (Button btn in buttons)
        {
            boutonEtat[btn] = false; // Mettre à jour l'état de sélection à false
            btn.GetComponent<Image>().color = couleurOriginale; // Réinitialiser la couleur du bouton
            selectionnerBtn--;
        }

        // Réinitialiser le compteur de boutons sélectionnés et mettre à jour le texte
        selectionnerBtn = 0;
        UpdateDecompteText();

        // Assurez-vous que les boutons sont interactifs à nouveau si nécessaire
        foreach (Button btn in buttons)
        {
            if (!boutonsNePasReactiver.ContainsKey(btn) || !boutonsNePasReactiver[btn])
            {
                btn.interactable = true;
            }
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
                    // Marquer le bouton pour ne pas être réactivé automatiquement
                    boutonsNePasReactiver[button] = true;
                }
                else
                {
                    // Enlever le marqueur si le bouton est réactivé manuellement
                    boutonsNePasReactiver[button] = false;
                }
                break;
                // probleme ici
            }
        }
    }

    public void ResetBoutonsNePasReactiver()
    {
        foreach (var button in boutonsNePasReactiver.Keys.ToList())
        {
            boutonsNePasReactiver[button] = false;
            button.interactable = true;
        }
    }


    public void ForcerSelectionBouton(string playerName)
    {
        foreach (Button button in buttons)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>().text == playerName)
            {

                string nomJoueur = button.GetComponentInChildren<TextMeshProUGUI>().text;
                // Forcer la sélection et le changement de couleur
                boutonEtat[button] = true; // Simuler une sélection
                button.GetComponent<Image>().color = couleurChangement; // Changer la couleur
                boutonsSelectionForces[button] = true; // Marquer comme sélectionné par le Moine
                joueursSelectionnes.Add(nomJoueur);





                // Assurez-vous que le bouton n'est pas cliquable
                button.interactable = false;
                selectionnerBtn++;
                UpdateDecompteText();
                break;
            }
        }
    }
    public void ReinitialiserSelectionForces()
    {

        foreach (var entry in boutonsSelectionForces)
        {
            if (entry.Value) // Si la sélection a été forcée par le Moine
            {
                // Réinitialiser l'état à non sélectionné sans changer l'interactivité
                boutonEtat[entry.Key] = false;
                entry.Key.GetComponent<Image>().color = couleurOriginale;
                UpdateDecompteText();
            }
        }
        // Nettoyer après réinitialisation
        boutonsSelectionForces.Clear();
    }

    public void AddPlayerToSelected(string playerName)
    {
        if (!joueursSelectionnes.Contains(playerName))
        {
            joueursSelectionnes.Add(playerName);
            Debug.Log("Selected Players: " + String.Join(", ", joueursSelectionnes));
        }
    }

    public void ViderSelectionJoueurs()
    {

        joueursSelectionnes.Clear();
    }

}
