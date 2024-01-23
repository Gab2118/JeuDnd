using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Directive pour utiliser TextMeshPro

public class Select_chef : MonoBehaviour
{
    public TMP_Text textField; // Utilisez TMP_Text ici pour les champs TextMeshPro
    private List<string> banqueChef; // Variable pour stocker les noms des chefs

    private string nom_chef; // Variable pour stocker le nom final du chef

    // Start est appel� avant la premi�re mise � jour de frame
    void Start()
    {
        // Trouve l'instance de RandomWordDisplay et r�cup�re la liste des noms des joueurs
        RandomWordDisplay randomWordDisplay = FindObjectOfType<RandomWordDisplay>();
        if (randomWordDisplay != null)
        {
            banqueChef = new List<string>(randomWordDisplay.PlayerNames);
            Debug.Log(banqueChef);
        }
        else
        {
            Debug.LogError("RandomWordDisplay not found!");
        }

        StartCoroutine(ChangeTextPeriodically());
    }

    // Coroutine pour changer le texte � intervalles r�guliers
    IEnumerator ChangeTextPeriodically()
    {
        int iterations = 0;
        while (iterations < 5 / 0.05f) // Continue jusqu'� 5 secondes (5s / 0.05s par it�ration)
        {
            if (banqueChef != null && banqueChef.Count > 0)
            {
                nom_chef = banqueChef[Random.Range(0, banqueChef.Count)]; // Choisit un mot al�atoire de la banque de mots
                textField.text = nom_chef; // Assigne le mot al�atoire au champ texte
            }
            else
            {
                Debug.LogError("banqueChef is null or empty!");
            }

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
