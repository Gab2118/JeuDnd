using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// CE SCRIPT À POUR BUT DE GÈRER LE UI ET PERMETTRE AU CHEF DE CHOISIR DEUX JOUEUR À ENVOYEZ EN MISSION
public class PlayerChoiceHandler : MonoBehaviour
{
    public Button[] buttons;
    public TextMeshProUGUI decompte;
    public Button Bouton_choix_finit; 

    private int selectionnerBtn = 0;
    private Color32 couleurChangement = new Color32(255, 0, 0, 255);
    private Color32 couleurOriginale;
    private Dictionary<Button, bool> boutonEtat;

    void Start()
    {
        boutonEtat = new Dictionary<Button, bool>();
        couleurOriginale = buttons[0].GetComponent<Image>().color;

        foreach (Button btn in buttons)
        {
            boutonEtat.Add(btn, false);
            btn.onClick.AddListener(() => BoutonCliquer(btn));
        }
        UpdateDecompteText();

        Bouton_choix_finit.interactable = false; 
    }

    void BoutonCliquer(Button btn)
    {
        bool estSelectionne = boutonEtat[btn];

        if (!estSelectionne && selectionnerBtn < 2)
        {
            selectionnerBtn++;
            boutonEtat[btn] = true;
            btn.GetComponent<Image>().color = couleurChangement;
        }
        else if (estSelectionne)
        {
            selectionnerBtn--;
            boutonEtat[btn] = false;
            btn.GetComponent<Image>().color = couleurOriginale;
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
}
