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
        "Sorci�re",
        "D�moniste",
        "Barde",
        "Clerc",
        "Moine",
        "N�cromancien",
        "R�deur",
        "Assassin",
        "Paladin"
    };
    private string[] descriptions = {
        "Le guerrier peut rejoindre un groupe de mission et ainsi choisir de faire r�ussir ou �chouer (deux minimum).",
        "On ne sait pas encore.",// � revoir
        "La sorci�re peut apercevoir le choix d'une personne pour la mission en cour.",
        "Le d�moniste lance une mal�diction qui emp�che un joueur de partir � la prochaine mission.", // � revoir
        "Le barde peut Annuler le r�sultat d'une mission.",
        "La clerc permet de donner une b�n�diction pour changer le chef de camp pour la prochaine mission.",
        "Le moine d�cide d'am�ner une personne m�diter avec soi, ce dernier ne peut participer � la mission du jour.",  // � revoir
        "Le d�moniste invoque des zombies.Lancer un d� 20 (1 jusqu'� 10 il perd le contr�le des zombies et ceux-ci donnent une carte �chec, 11 jusqu'� 20 garde le contraire).",
        "Le r�deur peut envoyez un loup espionner 3 joueur de votre choix. Le loup hurlera si l'un des joueurs n'est pas de votre camps",
        "L'assassin peut emp�che le joueur d'activer sa comp�tence lors de la prochaine mission.",
        "En �change de r�v�ler votre identit�, annuler le r�sultat d'une manche de votre choix et cette manche ne sera pas rejouer."
    };


    // �coute de clique des fl�ches du carrousel
    void Start()
    {
        UpdateUI();
        leftArrow.onClick.AddListener(PreviousClass);
        rightArrow.onClick.AddListener(NextClass);
    }

    // Mettre � jour les informations afficher
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
    // Aller � la classe pr�c�dente
    public void PreviousClass()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = titles.Length - 1;
        UpdateUI();
    }
}
