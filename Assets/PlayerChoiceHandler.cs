using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ajoutez cette ligne pour utiliser TextMeshPro

public class PlayerChoiceHandler : MonoBehaviour
{
    public Button[] buttons; // Assigne tes boutons ici depuis l'éditeur Unity
    public TextMeshProUGUI counterText; // Assigne ton élément de texte TMP ici depuis l'éditeur Unity

    private int selectedCount = 0; // Nombre de boutons sélectionnés
    private Color32 selectedColor = new Color32(255, 0, 0, 255); // Définissez ici la couleur de fond pour les boutons sélectionnés.

    void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => ButtonClicked(btn)); // Ajoute un écouteur d'événement pour chaque bouton
        }
        UpdateCounterText(); // Initialisez le texte du compteur
    }

    void ButtonClicked(Button btn)
    {
        if (selectedCount < 2) // Vérifie si moins de deux boutons ont été sélectionnés
        {
            selectedCount++; // Incrémente le nombre de boutons sélectionnés
            btn.GetComponent<Image>().color = selectedColor; // Change la couleur de fond du bouton
            UpdateCounterText(); // Met à jour le texte du compteur

            if (selectedCount == 2) // Si deux boutons ont été sélectionnés, désactive tous les boutons
            {
                foreach (Button button in buttons)
                {
                    button.interactable = false; // Désactive le bouton
                }
            }
        }
    }

    void UpdateCounterText()
    {
        counterText.text = $"{selectedCount}/2"; // Met à jour le texte du compteur global
    }
}
