using UnityEngine;
using UnityEngine.UI;
using TMPro; 


// CE SCRIPT EST UTILISER POUR LE CARROUSEL DU GUIDE DES JOUEUR PAR RAPPROT AU CLASSE

public class CarouselController : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button leftArrow;
    public Button rightArrow;

    private int currentIndex = 0;
    private string[] titles = {
        "Guerrier",
        "Barbare",
        "Sorcière",
        "Démoniste",
        "Barde",
        "Clerc",
        "Moine",
        "Nécromancien",
        "Rôdeur",
        "Assassin",
        "Paladin"
    };
    private string[] descriptions = {
        "Le guerrier peut rejoindre un groupe de mission et ainsi choisir de faire réussir ou échouer (deux minimum).",
        "On ne sait pas encore.",// à revoir
        "La sorcière peut apercevoir le choix d'une personne pour la mission en cour.",
        "Le démoniste lance une malédiction qui empêche un joueur de partir à la prochaine mission.", // à revoir
        "Le barde peut Annuler le résultat d'une mission.",
        "La clerc permet de donner une bénédiction pour changer le chef de camp pour la prochaine mission.",
        "Le moine décide d'amèner une personne méditer avec soi, ce dernier ne peut participer à la mission du jour.",  // à revoir
        "Le démoniste invoque des zombies.Lancer un dé 20 (1 jusqu'à 10 il perd le contrôle des zombies et ceux-ci donnent une carte échec, 11 jusqu'à 20 garde le contraire).",
        "Le rôdeur peut envoyez un loup espionner 3 joueur de votre choix. Le loup hurlera si l'un des joueurs n'est pas de votre camps",
        "L'assassin peut empêche le joueur d'activer sa compétence lors de la prochaine mission.",
        "En échange de révêler votre identité, annuler le résultat d'une manche de votre choix et cette manche ne sera pas rejouer."
    };


    // Écoute de clique des flèches du carrousel
    void Start()
    {
        UpdateUI();
        leftArrow.onClick.AddListener(PreviousClass);
        rightArrow.onClick.AddListener(NextClass);
    }

    // Mettre à jour les informations afficher
    void UpdateUI()
    {
        titleText.text = titles[currentIndex];
        descriptionText.text = descriptions[currentIndex];
    }


    // Aller vers la prochaine classe
    public void NextClass()
    {
        currentIndex = (currentIndex + 1) % titles.Length;
        UpdateUI();
    }
    // Aller à la classe précédente
    public void PreviousClass()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = titles.Length - 1;
        UpdateUI();
    }
}
