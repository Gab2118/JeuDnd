using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ajoutez cette ligne pour utiliser TextMeshPro

public class PlayerChoiceHandler : MonoBehaviour
{
    public Button[] buttons; // Assigne tes boutons ici depuis l'�diteur Unity
    public TextMeshProUGUI counterText; // Assigne ton �l�ment de texte TMP ici depuis l'�diteur Unity

    private int selectedCount = 0; // Nombre de boutons s�lectionn�s
    private Color32 selectedColor = new Color32(255, 0, 0, 255); // D�finissez ici la couleur de fond pour les boutons s�lectionn�s.

    void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => ButtonClicked(btn)); // Ajoute un �couteur d'�v�nement pour chaque bouton
        }
        UpdateCounterText(); // Initialisez le texte du compteur
    }

    void ButtonClicked(Button btn)
    {
        if (selectedCount < 2) // V�rifie si moins de deux boutons ont �t� s�lectionn�s
        {
            selectedCount++; // Incr�mente le nombre de boutons s�lectionn�s
            btn.GetComponent<Image>().color = selectedColor; // Change la couleur de fond du bouton
            UpdateCounterText(); // Met � jour le texte du compteur

            if (selectedCount == 2) // Si deux boutons ont �t� s�lectionn�s, d�sactive tous les boutons
            {
                foreach (Button button in buttons)
                {
                    button.interactable = false; // D�sactive le bouton
                }
            }
        }
    }

    void UpdateCounterText()
    {
        counterText.text = $"{selectedCount}/2"; // Met � jour le texte du compteur global
    }
}
