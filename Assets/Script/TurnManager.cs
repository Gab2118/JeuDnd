using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{

    // Pour la gestion des couleurs des boutons
    public Button[] choiceButtons; // Assurez-vous d'assigner vos boutons dans l'inspecteur Unity
    private Color32 defaultButtonColor = new Color32(190, 190, 190, 255); // Couleur initiale des boutons
    private Color32 selectedButtonColor = new Color32(255, 0, 0, 255); // Couleur lorsqu'un bouton est sélectionné
    private Button selectedButton;
    private int selectedButtonCount = 0;
    private List<Button> selectedButtons = new List<Button>();
    public TMP_Text text_confirm;
    public Button btn_confirm_choix;


    public GameObject panelCompetenceChoixJoueur;

    public Button btnChoixChef;



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

    // panel compétence dans scene_game_chef
    public GameObject panelCompetence;
    public Button yesButton;
    public Button noButton;
    public TMP_Text skillNameText;

    // panel_jeu_principal
    public TMP_Text roleDescriptionText;
    public TMP_Text turnText;
    public TMP_Text skillDescriptionText;
    public Image classImageDisplay;
    public Button nextTurnButton;

    // récupération des données des autres script 
    private Select_role selectRoleScript;
    private Select_Classe selectClasseScript;
    private Select_chef selectChefScript;

    // autre divers
    public List<ClassImage> classImages; // Liste des paires classe-image (voir l'inspecteur)
    private Dictionary<string, bool> skillUsed = new Dictionary<string, bool>(); // Pour stocker si la compétence a été utilisée pour chaque classe
    private int currentPlayerIndex = 0;
    private int turnCounter = 0;
    private Dictionary<string, Sprite> classImageDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, Color> classColors = new Dictionary<string, Color>(); // Pour stocker les couleurs des classes
    private Dictionary<string, Skill> skillBank = new Dictionary<string, Skill>(); // Dictionnaire pour stocker les compétences des classes

    private int completeTurnCount = 0; // Pour compter le nombre de tours complets

  







    void Start()
    {
        selectChefScript = FindObjectOfType<Select_chef>();
        if (selectChefScript == null)
        {
            Debug.LogError("Select_chef script not found!");
        }
        selectRoleScript = FindObjectOfType<Select_role>();
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
        selectClasseScript = FindObjectOfType<Select_Classe>();
        selectChefScript = FindObjectOfType<Select_chef>();
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            // Obtenez le composant TextMeshProUGUI du bouton et définissez-le au nom de classe du joueur
            TextMeshProUGUI buttonText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = selectClasseScript.PlayerNames[i];
        }

        btn_confirm_choix.interactable = false;
        foreach (var button in choiceButtons)
        {
            button.onClick.AddListener(() => OnChoiceButtonClicked(button));
        }


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
        btn_confirm_choix.onClick.AddListener(OnConfirmButtonClicked);
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

    // banque de donnée des classes relié à leurs compétences et leurs descriptions
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


    // Le chef commence en premier
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

    // vérifier si tout les tour sont fait
    private void OnNextTurnButtonClicked()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % selectClasseScript.PlayerNames.Count;
        turnCounter++;

        // Si tous les joueurs ont joué une fois, c'est la fin d'un tour complet
        if (turnCounter >= selectClasseScript.PlayerNames.Count)
        {
            turnCounter = 0; // Réinitialiser le compteur de tours pour le prochain tour
            completeTurnCount++; // Incrémenter le compteur de tours complets
            Debug.Log($"Tour {completeTurnCount} fini"); // Afficher le message dans la console

            // Déterminer et mettre à jour le prochain chef
            int currentChefIndex = selectClasseScript.PlayerNames.IndexOf(selectChefScript.GetNomChef());
            int nextChefIndex = (currentChefIndex + 1) % selectClasseScript.PlayerNames.Count;
            selectChefScript.SetNomChef(selectClasseScript.PlayerNames[nextChefIndex]);

            // Réinitialiser currentPlayerIndex pour que le chef soit le premier joueur du prochain tour
            currentPlayerIndex = nextChefIndex;

            UpdateTurnTextAndImage(); // Mettre à jour l'affichage pour le prochain tour
        }
        else
        {
            UpdateTurnTextAndImage();
        }
    }



    // changer les informations à chaque tour
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
                skillDescriptionText.text = skillBank[currentPlayerClass].description;

                // Afficher le panel de compétence seulement si la compétence n'a pas encore été utilisée
                if (!skillUsed.ContainsKey(currentPlayerClass) || !skillUsed[currentPlayerClass])
                {
                    panelCompetence.SetActive(true);
                }
                else
                {
                    panelCompetence.SetActive(false);
                }
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

            // Récupérer le nom du joueur actuel et activer le bouton Btn_choix_chef si le joueur actuel est le chef
            string currentPlayerName = selectClasseScript.PlayerNames[currentPlayerIndex];
            btnChoixChef.interactable = (currentPlayerName == selectChefScript.GetNomChef());
        }
        else
        {
            Debug.LogError("Player index is out of range!");
        }
    }



    // charger la scène suivante
    private void LoadEmptyScene()
    {
        SceneManager.LoadScene("scene_vide");
    }

    // si le joueur accepte d'utilisé sa compétence
    private void OnYesButtonClicked()
    {
        string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];
        Debug.Log(currentPlayerClass + " a utilisé sa compétence.");
        skillUsed[currentPlayerClass] = true; // Mettre à jour le statut de la compétence comme utilisée
        panelCompetence.SetActive(false); // Fermer le panneau de compétences

        // Si l'assassin accepte t'utiliser sa compétence
        if (currentPlayerClass == "Assassin")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);
        }
        if (currentPlayerClass == "Sorcière")
        {

            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);
        }
        if (currentPlayerClass == " Démoniste")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }
        if (currentPlayerClass == "Clerc")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }
        if (currentPlayerClass == "Moine")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }
    }



    private void OnNoButtonClicked()
    {
        Debug.Log("Compétence non utilisée.");
        skillUsed[selectClasseScript.PlayerNames[currentPlayerIndex]] = false; // Mettre à jour le statut de la compétence comme non utilisée
        panelCompetence.SetActive(false); // Fermer le panneau de compétences
    }
    private void OnChoiceButtonClicked(Button clickedButton)
    {
        if (selectedButtons.Contains(clickedButton))
        {
            // Désélectionner le bouton
            clickedButton.image.color = defaultButtonColor;
            selectedButtons.Remove(clickedButton);
            selectedButtonCount--;
        }
        else if (selectedButtonCount < 2) // Assurez-vous de ne pas sélectionner plus de 2 boutons
        {
            // Sélectionner le bouton
            clickedButton.image.color = selectedButtonColor;
            selectedButtons.Add(clickedButton);
            selectedButtonCount++;
        }

        // Mettre à jour le texte confirm
        text_confirm.text = $"{selectedButtonCount}/1";

        // Activer ou désactiver le bouton de confirmation
        btn_confirm_choix.interactable = (selectedButtonCount == 1);

        // Réactiver ou désactiver les autres boutons si nécessaire
        foreach (var button in choiceButtons)
        {
            if (!selectedButtons.Contains(button) && selectedButtonCount < 1)
            {
                button.interactable = true; // Réactiver le bouton
            }
            else if (!selectedButtons.Contains(button) && selectedButtonCount == 1)
            {
                button.interactable = false; // Désactiver le bouton
            }
        }
    }
    private void UpdateConfirmButtonState()
    {
        text_confirm.text = $"{selectedButtonCount}/1";
        btn_confirm_choix.interactable = (text_confirm.text == "1/1");
    }

    private void OnConfirmButtonClicked()
    {
        // récupérer le tour du joueur en cour
        string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];

        // si tel classes clique sur btn_confirm_choice
        switch (currentPlayerClass)
        {
            case "Sorcière":
                // Implement the logic for Sorcière here
                Debug.Log("La sorcière aperçoit le choix de quelqu'un");
                break;

            case "Démoniste":
                // Implement the logic for Démoniste here
                Debug.Log("Le démoniste à maudit un joueur");
                break;
            case "Clerc":
                // Implement the logic for Clerc here
                Debug.Log("La clerc à nommé un nouveau chef");
                break;

            case "Moine":
                // Implement the logic for Moine here
                Debug.Log("Le moine à amener un joueur à méditer");
                break;

            case "Assassin":
                // Implement the logic for Assassin here
                Debug.Log("L'assassin vole la compétence d'un joueur");
                break;

 
                // si aucune classe en cour ne fait partie de cette liste 
            default:
                Debug.LogWarning($"Unknown class: {currentPlayerClass}");
                break;
        }
    }


}


