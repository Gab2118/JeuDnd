using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerChoiceHandler : MonoBehaviour
{
    public Button[] buttons;
    public TextMeshProUGUI decompte;
    public Button Bouton_choix_finit;

    private Select_Classe selectClasseScript;
    private int selectionnerBtn = 0;
    private Color32 couleurChangement = new Color32(255, 0, 0, 255);
    private Color32 couleurOriginale;
    private Dictionary<Button, bool> boutonEtat;

    void Start()
    {
        selectClasseScript = FindObjectOfType<Select_Classe>();
        List<string> playerClasses = selectClasseScript.GetPlayerNames();

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

        Bouton_choix_finit.interactable = false;
    }

    void BoutonCliquer(int buttonIndex)
    {
        Button btn = buttons[buttonIndex]; // Obtenez le bouton à partir de l'index
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
