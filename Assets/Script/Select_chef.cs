using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Directive pour utiliser TextMeshPro

public class Select_chef : MonoBehaviour
{
    public TMP_Text textField; // Utilisez TMP_Text ici pour les champs TextMeshPro
    private List<string> banqueChef; // Variable pour stocker les noms des chefs

    private string nom_chef; // Variable pour stocker le nom final du chef

    // Start est appelé avant la première mise à jour de frame
    void Start()
    {
        // Trouve l'instance de RandomWordDisplay et récupère la liste des noms des joueurs
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

    // Coroutine pour changer le texte à intervalles réguliers
    IEnumerator ChangeTextPeriodically()
    {
        int iterations = 0;
        while (iterations < 5 / 0.05f) // Continue jusqu'à 5 secondes (5s / 0.05s par itération)
        {
            if (banqueChef != null && banqueChef.Count > 0)
            {
                nom_chef = banqueChef[Random.Range(0, banqueChef.Count)]; // Choisit un mot aléatoire de la banque de mots
                textField.text = nom_chef; // Assigne le mot aléatoire au champ texte
            }
            else
            {
                Debug.LogError("banqueChef is null or empty!");
            }

            yield return new WaitForSeconds(0.05f); // Attend 0.05 seconde avant de continuer
            iterations++;
        }

        AfficherChef(); // Appelle la fonction pour afficher le chef sélectionné dans la console
    }

    // Fonction pour afficher le nom du chef dans la console
    void AfficherChef()
    {
        Debug.Log("Le chef est : " + nom_chef);
    }
}
