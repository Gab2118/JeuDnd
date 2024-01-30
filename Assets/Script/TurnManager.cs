using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    [System.Serializable]
    public class Skill
    {
        public string skillName;
        public string description;
    }

    [System.Serializable]
    public class ClassImage
    {
        public string className;
        public Sprite classSprite;
    }

    public TMP_Text roleDescriptionText;
    private Select_role selectRoleScript;

    public Button yesButton;
    public Button noButton;
    public TMP_Text turnText;
    public TMP_Text skillNameText; // Texte TMP pour afficher le nom de la compétence
    public TMP_Text skillDescriptionText;
 
    public Button nextTurnButton;
    public Image classImageDisplay;
    public GameObject panelCompetence; // Référence au panneau de compétences
    public List<ClassImage> classImages; // Liste des paires classe-image définies dans l'inspecteur
    private Dictionary<string, bool> skillUsed = new Dictionary<string, bool>(); // Pour stocker si la compétence a été utilisée pour chaque classe

    private Select_Classe selectClasseScript;
    private Select_chef selectChefScript;
    private int currentPlayerIndex = 0;
    private int turnCounter = 0;
    private Dictionary<string, Sprite> classImageDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, Color> classColors = new Dictionary<string, Color>(); // Pour stocker les couleurs des classes
    private Dictionary<string, Skill> skillBank = new Dictionary<string, Skill>(); // Dictionnaire pour stocker les compétences des classes

    void Start()
    {
        selectRoleScript = FindObjectOfType<Select_role>();
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
        selectClasseScript = FindObjectOfType<Select_Classe>();
        selectChefScript = FindObjectOfType<Select_chef>();

        if (selectClasseScript == null || selectChefScript == null || classImageDisplay == null || panelCompetence == null || skillNameText == null)
        {
            Debug.LogError("Required components or scripts not found in the scene!");
            return;
        }

        InitializeClassColors();
        InitializeClassImagesDictionary();
        InitializeSkillBank(); // Initialiser la banque de compétences
        SetStartingPlayerAsChef();
        nextTurnButton.onClick.AddListener(OnNextTurnButtonClicked);
        UpdateTurnTextAndImage();
    }


    private void InitializeClassColors()
    {
        classColors.Add("Guerrier", Color.red);
        classColors.Add("Barbare", Color.blue);
        classColors.Add("Sorcière", Color.magenta);
        classColors.Add("Démoniste", Color.black);
        classColors.Add("Barde", Color.green);
        classColors.Add("Clerc", Color.yellow);
        classColors.Add("Moine", Color.gray);
        classColors.Add("Nécromancien", Color.cyan);
        classColors.Add("Rôdeur", Color.white);
        classColors.Add("Assassin", new Color(1f, 0.5f, 0)); // Orange
        classColors.Add("Paladin", Color.white);
    }

    private void InitializeClassImagesDictionary()
    {
        foreach (ClassImage classImage in classImages)
        {
            if (!classImageDictionary.ContainsKey(classImage.className))
            {
                classImageDictionary.Add(classImage.className, classImage.classSprite);
            }
        }
    }

    private void InitializeSkillBank()
    {
        skillBank.Add("Guerrier", new Skill { skillName = "Leadership", description = "Votre compétences vous permet de rejoindre un groupe de mission et ainsi d'y aller à trois. La majorité l'emporte." });
        skillBank.Add("Barbare", new Skill { skillName = "Rage intégrante", description = "On ne sait pas encore ce qu'il peut faire." });
        skillBank.Add("Sorcière", new Skill { skillName = "Vision secrète", description = "Votre compétence vous permet d'apercevoir le choix de quelqu'un de la mission actuel." });
        skillBank.Add("Démoniste", new Skill { skillName = "Malédiction sur la vie", description = "Votre compétence vous permet de lancer une malédiction sur un joueur, il ne pourra pas participer" });
        skillBank.Add("Barde", new Skill { skillName = "Musique de la mémoire", description = "Annule le résultat d'une mission." });
        skillBank.Add("Clerc", new Skill { skillName = "Démocratie pur", description = "Donner une bénédiction pour changer le chef de camp du jour même." });
        skillBank.Add("Moine", new Skill { skillName = "Relaxation", description = "Amène une personne méditer avec toi, ce dernier ne peut participer à la mission du jour." });
        skillBank.Add("Nécromancien", new Skill { skillName = "Zombieland", description = "Lance un dé 20 (1 jusqu'à 10 il perd le contrôle des zombies et ceux-ci donnent une carte échec) (11 jusqu'à 20 garde le contrôle et le contraire se produit)." });
        skillBank.Add("Rôdeur", new Skill { skillName = "Chasse de loup", description = "On ne sait pas encore." });
        skillBank.Add("Assassin", new Skill { skillName = "La discretion", description = "Empêche le joueur d'activer sa compétence." });
        skillBank.Add("Paladin", new Skill { skillName = "Jet de lumière", description = "En échange de dévoiler son rôle, annule le résultat d'une manche. ON NE REFAIT PAS CETTE MANCHE." });
    }

    private void SetStartingPlayerAsChef()
    {
        string chefName = selectChefScript.GetNomChef();
        currentPlayerIndex = selectClasseScript.PlayerNames.IndexOf(chefName);
        if (currentPlayerIndex == -1)
        {
            Debug.LogError("Le chef n'a pas été trouvé dans la liste des joueurs !");
            currentPlayerIndex = 0;
        }
    }

    private void OnNextTurnButtonClicked()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % selectClasseScript.PlayerNames.Count;
        UpdateTurnTextAndImage();
        turnCounter++;

        if (turnCounter >= selectClasseScript.PlayerNames.Count)
        {
            LoadEmptyScene();
        }
    }

    private void UpdateTurnTextAndImage()
    {
        if (selectClasseScript.PlayerNames.Count > currentPlayerIndex)
        {
            string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];
            string colorHex = ColorUtility.ToHtmlStringRGB(classColors.ContainsKey(currentPlayerClass) ? classColors[currentPlayerClass] : Color.white);
            turnText.text = $"<color=#{colorHex}>{currentPlayerClass}</color>";

            if (classImageDictionary.ContainsKey(currentPlayerClass))
            {
                classImageDisplay.sprite = classImageDictionary[currentPlayerClass];
            }
            else
            {
                Debug.LogWarning($"Aucune image trouvée pour la classe {currentPlayerClass}");
            }

            // Mettre à jour le texte de la compétence et sa description
            if (skillBank.ContainsKey(currentPlayerClass))
            {
                skillNameText.text = skillBank[currentPlayerClass].skillName;
                skillDescriptionText.text = skillBank[currentPlayerClass].description; // Mettez à jour le texte de la description de la compétence
                panelCompetence.SetActive(true);
            }
            else
            {
                skillNameText.text = "Compétence inconnue";
                skillDescriptionText.text = ""; // Réinitialiser le texte de la description de la compétence
                panelCompetence.SetActive(false);
            }

            // Récupérer la description du rôle du joueur actuel et la mettre à jour
            Select_role.Role currentPlayerRole = selectRoleScript.GetPlayerRole(currentPlayerIndex);
            if (currentPlayerRole != null)
            {
                roleDescriptionText.text = currentPlayerRole.description;
            }
            else
            {
                roleDescriptionText.text = "Description du rôle inconnue";
            }
        }
        else
        {
            Debug.LogError("Player index is out of range!");
        }
    }


    private void LoadEmptyScene()
    {
        SceneManager.LoadScene("scene_vide");
    }
    private void OnYesButtonClicked()
    {
        string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];
        Debug.Log(currentPlayerClass + " a utilisé sa compétence.");
        skillUsed[currentPlayerClass] = true; // Mettre à jour le statut de la compétence comme utilisée
        panelCompetence.SetActive(false); // Fermer le panneau de compétences
    }

    private void OnNoButtonClicked()
    {
        Debug.Log("Compétence non utilisée.");
        skillUsed[selectClasseScript.PlayerNames[currentPlayerIndex]] = false; // Mettre à jour le statut de la compétence comme non utilisée
        panelCompetence.SetActive(false); // Fermer le panneau de compétences
    }
}
