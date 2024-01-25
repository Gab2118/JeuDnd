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
        "Il peut rejoindre un groupe de mission et ainsi choisir de faire réussir ou échouer (deux minimum).",
        "On ne sait pas encore.",
        "Peut apercevoir le choix de quelqu'un.",
        "Lance une malédiction qui empêche un joueur de partir à la prochaine mission.",
        "Annule le résultat d'une mission.",
        "Donner une bénédiction pour changer le chef de camp du jour même.",
        "Amène une personne méditer avec toi, ce dernier ne peut participer à la mission du jour.",
        "Lance un dé 20 (1 jusqu'à 10 il perd le contrôle des zombies et ceux-ci donnent une carte échec, 11 jusqu'à 20 garde le contraire).",
        "On ne sait pas encore.",
        "Empêche le joueur d'activer sa compétence.",
        "En échange de révêler votre identité, annuler le résultat d'une manche de votre choix et cette manche ne sera pas rejouer."
    };

    void Start()
    {
        UpdateUI();
        leftArrow.onClick.AddListener(PreviousClass);
        rightArrow.onClick.AddListener(NextClass);
    }

    void UpdateUI()
    {
        titleText.text = titles[currentIndex];
        descriptionText.text = descriptions[currentIndex];
    }

    public void NextClass()
    {
        currentIndex = (currentIndex + 1) % titles.Length;
        UpdateUI();
    }

    public void PreviousClass()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = titles.Length - 1;
        UpdateUI();
    }
}
