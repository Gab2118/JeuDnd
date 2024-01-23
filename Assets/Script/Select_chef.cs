using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Directive pour utiliser TextMeshPro

public class Select_chef : MonoBehaviour
{
    public TMP_Text textField; // Utilisez TMP_Text ici pour les champs TextMeshPro
    private List<string> banqueChef = new List<string>
    {
        "Voleur", "R�deur", "Cultiste", "Moine", "Barde",
        "Occultiste", "Sorci�re", "Paladin", "Guerrier", "Clerc", "N�cromancien"
    }; // Votre banque de mots

    private string nom_chef; // Variable pour stocker le nom final du chef

    // Start est appel� avant la premi�re mise � jour de frame
    void Start()
    {
        StartCoroutine(ChangeTextPeriodically());
    }

    // Coroutine pour changer le texte � intervalles r�guliers
    IEnumerator ChangeTextPeriodically()
    {
        int iterations = 0;
        while (iterations < 5 / 0.05f) // Continue jusqu'� 5 secondes (5s / 0.05s par it�ration)
        {
            nom_chef = banqueChef[Random.Range(0, banqueChef.Count)]; // Choisit un mot al�atoire de la banque de mots
            textField.text = nom_chef; // Assigne le mot al�atoire au champ texte
            yield return new WaitForSeconds(0.05f); // Attend 0.05 seconde avant de continuer
            iterations++;
        }

        AfficherChef(); // Appelle la fonction pour afficher le chef s�lectionn� dans la console
    }

    // Fonction pour afficher le nom du chef dans la console
    void AfficherChef()
    {
        Debug.Log("Le chef est : " + nom_chef);
    }
}
