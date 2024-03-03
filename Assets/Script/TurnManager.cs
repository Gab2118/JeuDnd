using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class TurnManager : MonoBehaviour
{
    // page de jeu principal ( panel_jeu_principal
    public Button btnChoixChef;
    public Button btnUtiliseCompetence; // bouton pour utilis� sa comp�tence
    private Dictionary<string, bool> competenceUtiliseePourClasse = new Dictionary<string, bool>();
    public TMP_Text roleDescriptionText; // description du role d'un joueur
    public TMP_Text turnText; // texte pou afficher la classe du joueur
    public TMP_Text skillDescriptionText; // description de la comp�tence
    public Image classImageDisplay; // image utilis� pour afficher l'image de chaque classe (les sprites des classes)
    public Button nextTurnButton; // bouton prochain tour

    // Pour la gestion des couleurs des boutons de choix pour cible et ce qui � rapport avec le Panel_competence_choix_joueur
    public GameObject panelCompetenceChoixJoueur;
    public Button[] choiceButtons; // les six boutons pour choisir une cible apr�s avoir accepter d'utiliser sa comp�tence
    private Color32 defaultButtonColor = new Color32(190, 190, 190, 255); // Couleur initiale des boutons (parmit les 6)
    private Color32 selectedButtonColor = new Color32(255, 0, 0, 255); // Couleur lorsqu'un bouton est s�lectionn�
    private int selectedButtonCount = 0; // nombre de bouton s�lectionner
    private List<Button> selectedButtons = new List<Button>(); // list des bouton s�lectionner
    public Button btn_confirm_choix; // bouton de confirmation pour la cible du panel : Panel_competence_choix_joueur
    public TMP_Text text_confirm; // texte du bouton afin de afficher le nombre de s�lectionner (0/1 ou 1/1)

    // pour contenir les informations de chaque comp�tence pour chaque classe
    [System.Serializable]
    public class Skill
    {
        public string skillName;
        public string description;
    }

    // contenir les sprites par rapport � chaque classes
    [System.Serializable]
    public class ClassImage
    {
        public string className;
        public Sprite classSprite;
    }

    // panel comp�tence afin de permettre au joueur de choisir d'utiliser ou non sa comp�tence
    public GameObject panelCompetence; // Le panel panel_competence
    public Button yesButton; // accepter d'utiliser sa comp�tence
    public Button noButton; // refuser d'utiliser sa comp�tence
    public TMP_Text skillNameText;

    // r�cup�ration des donn�es des autres script 
    private Select_Role selectRoleScript; // script pour la distribution des r�le
    private Select_Classe selectClasseScript; // script pour la s�lection des classes
    private Select_Chef selectChefScript;


    // autre divers
    public List<ClassImage> classImages; // Liste des paires classe-image (voir l'inspecteur)
    private Dictionary<string, bool> skillUsed = new Dictionary<string, bool>(); // Pour stocker si la comp�tence a �t� utilis�e pour chaque classe
    private int currentPlayerIndex = 1;
    private int turnCounter = 0;
    private Dictionary<string, Sprite> classImageDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, Color> classColors = new Dictionary<string, Color>(); // Pour stocker les couleurs des classes
    private Dictionary<string, Skill> skillBank = new Dictionary<string, Skill>(); // Dictionnaire pour stocker les comp�tences des classes

    private int completeTurnCount = 0; // Pour compter le nombre de tours complets

    // d�claration pour les comp�tence cot� assassin
    private string classeCibleAssassin; // stock le nom de la classe cibler
    private bool isAssassinSkillUsedThisTurn = false; // v�rifie si la comp�tence de l'assassin a �t� utilis�
    private string targetClassForTextChange = null;// stocke le nom de la classe pour le texte du bouton d'utilisation de la comp�tence doit �tre modifi� 
    private int turnsSinceAssassinSkillUsed = 0; // compte le nombre de tours �coul�s depuis que la comp�tence de l'assassin a �t� utilis�e.
    private int TourDepuisAssassinat = 0; // enregistre le tour pendant lequel la comp�tence de l'assassin a �t� utilis�e pour la derni�re fois.
    // clerc 
    private string classeCibleClerc;
    // D�moniste
    private string classeCibleDemoniste;
    private bool doitReactiverBoutonPourCibleDemoniste = false;
    private int tourCompetenceDemonisteUtilisee = 0;
    // d�claration pour le moine
    public string classeCibleMoine;
    private int tourCompetenceMoineUtilisee = 0;








    void Start()
    {

        selectChefScript = FindObjectOfType<Select_Chef>(); // recherche dans la scene le script select_chef
        selectRoleScript = FindObjectOfType<Select_Role>(); // recherche dans la scene le script select_role
        selectClasseScript = FindObjectOfType<Select_Classe>(); // recherche dans la scene le script select_classe

        // void start du panel_competence
        yesButton.onClick.AddListener(OnYesButtonClicked); // �couteur de clique du bouton oui
        noButton.onClick.AddListener(OnNoButtonClicked); // �couteur de clique du bouton non

        // void start du Panel_competence_choix_joueur
        btn_confirm_choix.onClick.AddListener(OnConfirmButtonClicked);

        // Assurez-vous que la liste playerInfos dans SocketManager est correctement initialis�e et remplie
        if (SocketManager.Instance.playerInfos.Count >= choiceButtons.Length)
        {
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                TextMeshProUGUI buttonText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                // Utilisez l'attribut appropri� de playerInfo pour le texte du bouton
                buttonText.text = SocketManager.Instance.playerInfos[i].playerClass; // ou playerId selon ce que vous voulez afficher
            }
        }
        else
        {
            Debug.LogError("Pas assez d'informations sur les joueurs pour initialiser les boutons.");
        }

        btn_confirm_choix.interactable = false; // logique de false ou true pour activer ou d�sactiver pour le bouton de confirmation du choix de la cible
        // pour chaque des six bouton lance une fonctione
        foreach (var button in choiceButtons)
        {
            // lancer la fonction
            button.onClick.AddListener(() => OnChoiceButtonClicked(button));
        }
        // void start page jeu principal
        InitializeClassColors(); // d�finie les couleurs de chaque clase de personnage ( couleur de nom)
        InitializeClassImagesDictionary(); //fonction pour associe une image pour chaque classe
        InitializeSkillBank(); // fonction pour Initialiser la banque de comp�tences
        //SetStartingPlayerAsChef(); // fonction faire d�buter le chef en premier
        nextTurnButton.onClick.AddListener(OnNextTurnButtonClicked); // �couteur de clique pour le bouton tour suivant
        UpdateTurnTextAndImage(); // mettre � jour les text et images
        DontDestroyOnLoad(this);
    }


    // initialiser les couleur pour chaque classe
    private void InitializeClassColors()
    {
        classColors.Add("Guerrier", Color.red);
        classColors.Add("Barbare", Color.blue);
        classColors.Add("Sorci�re", Color.magenta);
        classColors.Add("D�moniste", Color.black);
        classColors.Add("Barde", Color.green);
        classColors.Add("Clerc", Color.yellow);
        classColors.Add("Moine", Color.gray);
        classColors.Add("N�cromancien", Color.cyan);
        classColors.Add("R�deur", Color.white);
        classColors.Add("Assassin", new Color(1f, 0.5f, 0)); // Orange
        classColors.Add("Paladin", Color.white);
    }
    //fonction pour associe une image pour chaque classe
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

    // banque de donn�e des classes reli� � leurs comp�tences et leurs descriptions
    private void InitializeSkillBank()
    {
        skillBank.Add("Guerrier", new Skill { skillName = "Leadership", description = "Votre comp�tences vous permet de rejoindre un groupe de mission et ainsi d'y aller � trois. La majorit� l'emporte." });
        skillBank.Add("Barbare", new Skill { skillName = "Rage int�grante", description = "On ne sait pas encore ce qu'il peut faire." });
        skillBank.Add("Sorci�re", new Skill { skillName = "Vision secr�te", description = "Votre comp�tence vous permet d'apercevoir le choix de quelqu'un de la mission actuel." });
        skillBank.Add("D�moniste", new Skill { skillName = "Mal�diction sur la vie", description = "Votre comp�tence vous permet de lancer une mal�diction sur un joueur, il ne pourra pas participer" });
        skillBank.Add("Barde", new Skill { skillName = "Musique de la m�moire", description = "Annule le r�sultat d'une mission." });
        skillBank.Add("Clerc", new Skill { skillName = "D�mocratie pur", description = "Donner une b�n�diction pour changer le chef de camp du jour m�me." });
        skillBank.Add("Moine", new Skill { skillName = "Relaxation", description = "Am�ne une personne m�diter avec toi, ce dernier ne peut participer � la mission du jour." });
        skillBank.Add("N�cromancien", new Skill { skillName = "Zombieland", description = "Lance un d� 20 (1 jusqu'� 10 il perd le contr�le des zombies et ceux-ci donnent une carte �chec) (11 jusqu'� 20 garde le contr�le et le contraire se produit)." });
        skillBank.Add("R�deur", new Skill { skillName = "Chasse de loup", description = "On ne sait pas encore." });
        skillBank.Add("Assassin", new Skill { skillName = "La discretion", description = "Emp�che le joueur d'activer sa comp�tence." });
        skillBank.Add("Paladin", new Skill { skillName = "Jet de lumi�re", description = "En �change de d�voiler son r�le, annule le r�sultat d'une manche. ON NE REFAIT PAS CETTE MANCHE." });
    }


    // fonction pour que le chef commence en premier
    private void SetStartingPlayerAsChef()
    {
        string chefId = SocketManager.Instance.GetCurrentChefId();
        var chefInfo = SocketManager.Instance.playerInfos.Find(player => player.playerId == chefId);
        string chefName = chefInfo != null ? chefInfo.playerName : "Chef inconnu";

        for (int i = 0; i < SocketManager.Instance.playerInfos.Count; i++)
        {
            if (SocketManager.Instance.playerInfos[i].playerId == chefId)
            {
                currentPlayerIndex = i;
                break;
            }
        }
    }

    //fonction � chaque fois que on clique sur tour suivant
    private void OnNextTurnButtonClicked()
    {
        // Incrementer l'index du joueur actuel et le compteur de tour
        currentPlayerIndex = (currentPlayerIndex + 1) % SocketManager.Instance.playerInfos.Count;
        turnCounter++;

        // Verifier si tous les joueurs ont jou� une fois
        if (turnCounter >= SocketManager.Instance.playerInfos.Count)
        {
            turnCounter = 0; // R�initialiser le compteur de tours pour le prochain tour
            completeTurnCount++; // Incr�menter le compteur de tours complets
            Debug.Log("Classe Cible D�moniste: " + classeCibleDemoniste);


            // V�rifier si l'assassin a utilis� sa comp�tence ce tour
            if (isAssassinSkillUsedThisTurn)
            {
                DisableTargetClassCompetenceButton(classeCibleAssassin);
                isAssassinSkillUsedThisTurn = false;
            }

            // Si le Clerc a d�sign� un nouveau chef, faire de cette cible le chef
            if (!string.IsNullOrEmpty(classeCibleClerc))
            {
                // Envoyer la requ�te de changement de chef au serveur
                var changeChefData = new { newChefId = classeCibleClerc };
                SocketManager.Instance.SendData("changeChef", JsonUtility.ToJson(changeChefData));
            }

            // R�initialiser le compteur de tours depuis l'utilisation de la comp�tence de l'assassin
            turnsSinceAssassinSkillUsed++;

            // V�rifier si deux tours se sont �coul�s depuis l'utilisation de la comp�tence de l'assassin
            if (turnsSinceAssassinSkillUsed >= 2)
            {
                EnableTargetClassCompetenceButton(classeCibleAssassin);
                turnsSinceAssassinSkillUsed = 0; // R�initialiser le compteur
            }
            if (doitReactiverBoutonPourCibleDemoniste)
            {
                PlayerChoiceHandler playerChoiceHandler = FindObjectOfType<PlayerChoiceHandler>();
                playerChoiceHandler.SetPlayerButtonActiveState(classeCibleDemoniste, true);
                classeCibleDemoniste = null;
                doitReactiverBoutonPourCibleDemoniste = false;
            }
            Debug.Log($"{completeTurnCount},{tourCompetenceDemonisteUtilisee}");
            if (completeTurnCount == tourCompetenceDemonisteUtilisee + 2)
            {
                Debug.Log("on est ici");
                classeCibleDemoniste = ""; // R�initialiser la cible
                PlayerChoiceHandler playerChoiceHandler = FindObjectOfType<PlayerChoiceHandler>();
                if (playerChoiceHandler != null)
                {
                    playerChoiceHandler.ResetBoutonsNePasReactiver();
                }
            }

            if (completeTurnCount == tourCompetenceDemonisteUtilisee + 1)
            {
                Debug.Log("debut de la competence du d�moniste");
                PlayerChoiceHandler playerChoiceHandler = FindObjectOfType<PlayerChoiceHandler>();
                if (playerChoiceHandler != null)
                {
                    playerChoiceHandler.SetPlayerButtonActiveState(classeCibleDemoniste, false);
                }
            }

            if (completeTurnCount == tourCompetenceMoineUtilisee + 2)
            {
                classeCibleMoine = "";
                PlayerChoiceHandler playerChoiceHandler = FindObjectOfType<PlayerChoiceHandler>();
                if (playerChoiceHandler != null)
                {
                    playerChoiceHandler.ReinitialiserSelectionForces();
                }
            }

            if (completeTurnCount == tourCompetenceMoineUtilisee + 1)
            {
                Debug.Log("debut de la competence du moine");
                PlayerChoiceHandler playerChoiceHandler = FindObjectOfType<PlayerChoiceHandler>();
                if (playerChoiceHandler != null && !string.IsNullOrEmpty(classeCibleMoine))
                {
                    playerChoiceHandler.ForcerSelectionBouton(classeCibleMoine);
                }
            }


            LoadEmptyScene();

        }


        UpdateTurnTextAndImage(); // Mettre � jour l'affichage pour le prochain tour
    }






    // changer les informations � chaque tour
    private void UpdateTurnTextAndImage()
    {
        if (SocketManager.Instance.playerInfos.Count > currentPlayerIndex)
        {

            string currentPlayerClass = SocketManager.Instance.playerInfos[currentPlayerIndex].playerClass; // R�cup�re la classe du joueur actuel
            // convertie les couleur en format hexad�cimal RGB
            string colorHex = ColorUtility.ToHtmlStringRGB(classColors.ContainsKey(currentPlayerClass) ? classColors[currentPlayerClass] : Color.white);
            turnText.text = $"<color=#{colorHex}>{currentPlayerClass}</color>"; // changer la couleur

            // changer l'image
            if (classImageDictionary.ContainsKey(currentPlayerClass))
            {
                classImageDisplay.sprite = classImageDictionary[currentPlayerClass];
            }
            // changer le text de la description et nom de la comp�tence
            if (skillBank.ContainsKey(currentPlayerClass))
            {
                skillNameText.text = skillBank[currentPlayerClass].skillName;
                skillDescriptionText.text = skillBank[currentPlayerClass].description;
            }
            // aller chercher la description du role du joueur et l'afficher
            var currentPlayerRoleDescription = SocketManager.Instance.playerInfos[currentPlayerIndex].PlayerRoleDescription;
            if (!string.IsNullOrEmpty(currentPlayerRoleDescription))
            {
                roleDescriptionText.text = currentPlayerRoleDescription;
            }

            // quand cest le tour du chef changer le text du btn : Btn_choix_chef
            string currentPlayerId = SocketManager.Instance.playerInfos[currentPlayerIndex].playerId; // Acc�dez � l'ID du joueur actuel
            if (currentPlayerId == SocketManager.Instance.currentChefId) // Comparez avec l'ID du chef
            {
                btnChoixChef.GetComponentInChildren<TMP_Text>().text = "Faire votre choix";
                btnChoixChef.interactable = true;
            }
            else
            {
                btnChoixChef.GetComponentInChildren<TMP_Text>().text = "En attente du chef";
                btnChoixChef.interactable = false;
            }

            // G�rer l'activation du bouton Btn_utilise_competence
            if (competenceUtiliseePourClasse.ContainsKey(currentPlayerClass) && competenceUtiliseePourClasse[currentPlayerClass])
            {
                // Si la comp�tence a �t� utilis�e pour la classe en cours, d�sactiver le bouton et mettre � jour le texte
                btnUtiliseCompetence.interactable = false;
                btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Comp�tence utilis�e";
            }
            else
            {
                // Sinon, activer le bouton et r�initialiser le texte par d�faut
                btnUtiliseCompetence.interactable = true;
                btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Utiliser comp�tence"; // Mettez ici le texte que vous voulez par d�faut
            }
            // changer le text du bouton pour utiliser la comp�tence et le d�sactiver
            if (targetClassForTextChange == currentPlayerClass)
            {
                btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Comp�tence bloquer par l'assassin";
                targetClassForTextChange = null;
            }

        }
    }





    // charger la sc�ne suivante  � ABSOLUMENT GARDER DONC PAS TOUCHE PLEASE :)
    private void LoadEmptyScene()
    {
        SceneManager.LoadScene("scene_choix_mission");
    }

    // si le joueur accepte d'utilis� sa comp�tence
    private void OnYesButtonClicked()
    {
        var currentPlayerInfo = SocketManager.Instance.playerInfos[currentPlayerIndex];
        string currentPlayerClass = currentPlayerInfo.playerClass;
        Debug.Log(currentPlayerClass + " a utilis� sa comp�tence.");
        skillUsed[currentPlayerClass] = true; // Mettre � jour le statut de la comp�tence comme utilis�e
        panelCompetence.SetActive(false); // Fermer le panneau de comp�tences

        btnUtiliseCompetence.interactable = false;
        btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Comp�tence utilis�e";
        competenceUtiliseePourClasse[currentPlayerClass] = true;

        // Si l'assassin accepte t'utiliser sa comp�tence
        if (currentPlayerClass == "Assassin")
        {
            // Activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

            // D�sactivez le bouton avec le texte "Assassin"
            Button[] buttons = panelCompetenceChoixJoueur.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null && buttonText.text == "Assassin")
                {
                    button.interactable = false; // D�sactivez le bouton
                    break; // Quittez la boucle car vous avez trouv� le bouton
                }
            }
        }
        // si la sorci�re accepte d'utiliser sa comp�tence
        if (currentPlayerClass == "Sorci�re")
        {

            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);
        }
        // si le d�moniste refuse d'utilis� sa comp�tence
        if (currentPlayerClass == "D�moniste")
        {
            panelCompetenceChoixJoueur.SetActive(true);
            tourCompetenceDemonisteUtilisee = completeTurnCount;
        }
        // si le clerc accepte d'utiliser sa comp�tence
        if (currentPlayerClass == "Clerc")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }
        // si le moine accepte d'utiliser sa comp�tence
        if (currentPlayerClass == "Moine")
        {
            tourCompetenceMoineUtilisee = completeTurnCount;
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }
        if (currentPlayerClass == "Guerrier")
        {
            PlayerChoiceHandler playerChoiceHandler = FindObjectOfType<PlayerChoiceHandler>();
            if (playerChoiceHandler != null)
            {
                playerChoiceHandler.AddPlayerToSelected("Guerrier");
            }
        }

        // NOTE : AJOUTER LES AUTRES CLASSES PLUS TARD
    }


    // si le joueur refuse d'utiliser sa comp�tence
    private void OnNoButtonClicked()
    {
        Debug.Log("Comp�tence non utilis�e.");

        // Acc�dez � l'ID du joueur actuel en utilisant l'index 'currentPlayerIndex'
        var currentPlayerId = SocketManager.Instance.playerInfos[currentPlayerIndex].playerId;

        // Utilisez l'ID du joueur pour mettre � jour le statut de la comp�tence comme non utilis�e
        // 'skillUsed' utilise d�sormais l'ID du joueur comme cl� au lieu du nom ou d'une autre propri�t�
        if (skillUsed.ContainsKey(currentPlayerId))
        {
            skillUsed[currentPlayerId] = false;
        }
        else
        {
            // G�rer le cas o� le joueur n'est pas encore dans 'skillUsed', si n�cessaire
            skillUsed.Add(currentPlayerId, false);
        }

        panelCompetence.SetActive(false); // Fermer le panneau de comp�tences
    }
    private void OnChoiceButtonClicked(Button clickedButton)
    {

        if (selectedButtons.Contains(clickedButton))
        {
            // D�s�lectionner le bouton
            clickedButton.image.color = defaultButtonColor;
            selectedButtons.Remove(clickedButton);
            selectedButtonCount--; // diminuer de -1, le nombre de bouton s�lectionner
        }
        else if (selectedButtonCount < 2) // si le nombre de bouton s�lectionner est plus petit que 2 faire ceci
        {
            // S�lectionner le bouton
            clickedButton.image.color = selectedButtonColor;
            selectedButtons.Add(clickedButton);
            selectedButtonCount++; // ajouter + 1 au nombre de bouton
        }

        // Mettre � jour le texte confirm
        text_confirm.text = $"{selectedButtonCount}/1";

        // Activer ou d�sactiver le bouton de confirmation
        btn_confirm_choix.interactable = (selectedButtonCount == 1);

        // R�cup�rer le nom de la classe du joueur actuel en utilisant currentPlayerIndex
        string currentPlayerClass = SocketManager.Instance.playerInfos[currentPlayerIndex].playerClass;

        // R�activer ou d�sactiver les autres boutons si n�cessaire
        foreach (var button in choiceButtons)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            // V�rifier si le bouton est celui de l'Assassin et si c'est le tour de l'Assassin
            if (buttonText != null && buttonText.text == "Assassin" && currentPlayerClass == "Assassin")
            {
                button.interactable = false; // Gardez le bouton de l'Assassin d�sactiv�
                continue; // Continuez avec le prochain bouton dans la boucle
            }

            if (!selectedButtons.Contains(button) && selectedButtonCount < 1)
            {
                button.interactable = true; // R�activer les bouton
            }
            else if (!selectedButtons.Contains(button) && selectedButtonCount == 1)
            {
                button.interactable = false; // D�sactiver les bouton
            }
        }
    }

    private void UpdateConfirmButtonState()
    {
        // mettre � jour le statut du bouton
        text_confirm.text = $"{selectedButtonCount}/1";
        btn_confirm_choix.interactable = (text_confirm.text == "1/1");
    }

    private void OnConfirmButtonClicked()
    {
        // Supposons que vous avez un moyen d'identifier la classe du joueur actuel via SocketManager
        string currentPlayerClass = SocketManager.Instance.GetCurrentPlayerClass(currentPlayerIndex);

        // Votre logique reste similaire, mais vous devez adapter la r�cup�ration et l'action sur les classes
        switch (currentPlayerClass)
        {
            case "Sorci�re":
                Debug.Log("La sorci�re aper�oit le choix de quelqu'un");
                // Envoyer une commande au serveur pour la sorci�re
                SocketManager.Instance.SendData("SorciereAction", "{}");
                break;
            case "D�moniste":
                if (selectedButtons.Count > 0)
                {
                    string targetClass = selectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().text;
                    // Envoyer une commande au serveur pour le d�moniste avec la classe cible
                    SocketManager.Instance.SendData("DemonisteAction", JsonUtility.ToJson(new { targetClass }));
                    Debug.Log($"Le D�moniste � maudit la cible suivant: {targetClass}");
                }
                break;
            case "Clerc":
                if (selectedButtons.Count > 0)
                {
                    string targetClass = selectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().text;
                    // Envoyer une commande au serveur pour le clerc avec la classe cible
                    SocketManager.Instance.SendData("ClercAction", JsonUtility.ToJson(new { targetClass }));
                    Debug.Log($"Le Clerc a choisi sa cible: {targetClass}");
                }
                break;
            case "Moine":
                if (selectedButtons.Count > 0)
                {
                    string targetClass = selectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().text;
                    // Envoyer une commande au serveur pour le moine avec la classe cible
                    SocketManager.Instance.SendData("MoineAction", JsonUtility.ToJson(new { targetClass }));
                    Debug.Log($"Le Moine a forc� la s�lection sur : {targetClass}");
                }
                break;
            case "Assassin":
                if (selectedButtons.Count > 0)
                {
                    string targetClass = selectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().text;
                    // Envoyer une commande au serveur pour l'assassin avec la classe cible
                    SocketManager.Instance.SendData("AssassinAction", JsonUtility.ToJson(new { targetClass }));
                    Debug.Log($"L'assassin a cibl� la classe: {targetClass}");
                }
                break;
        }

        // R�initialiser les boutons apr�s confirmation
        ResetButtonSelection();
    }

    private void ResetButtonSelection()
    {
        foreach (var button in selectedButtons)
        {
            button.image.color = defaultButtonColor;
        }
        selectedButtons.Clear();
        selectedButtonCount = 0;
        UpdateConfirmButtonState();
    }


    // fonction comp�tence assassin pour d�sactiver le bouton comp�tence de sa cible
    private void DisableTargetClassCompetenceButton(string targetClassName)
    {
        // trouver l'index de la cible
        int targetPlayerIndex = SocketManager.Instance.playerInfos
        .FindIndex(playerInfo => playerInfo.playerClass == targetClassName);
        // si trouver 
        if (targetPlayerIndex != -1)
        {
            // si il exist, considerer que l'assassin a utiliser sa comp�tence
            competenceUtiliseePourClasse[targetClassName] = true;
            if (targetPlayerIndex == currentPlayerIndex) // si la cible est en train de jouer, d�sactiver son bouton comp�tence
            {
                btnUtiliseCompetence.interactable = false;
            }
            // Stockez la classe cibl�e pour le changement de texte au prochain tour
            targetClassForTextChange = targetClassName;
        }


    }
    // r�activer le bouton de la cible
    private void EnableTargetClassCompetenceButton(string targetClassName)
    {
        // Trouver le joueur cibl� par le nom de classe dans les informations des joueurs du SocketManager
        var targetPlayerInfo = SocketManager.Instance.playerInfos
            .FindIndex(playerInfo => playerInfo.playerClass == targetClassName);

        if (targetPlayerInfo != -1) // V�rifier si le joueur a �t� trouv�
        {
            competenceUtiliseePourClasse[targetClassName] = false;
            if (targetPlayerInfo == currentPlayerIndex) // Si le joueur cibl� est le joueur actuel
            {
                btnUtiliseCompetence.interactable = true;
                btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Utiliser comp�tence";
            }
            turnsSinceAssassinSkillUsed = 0; // R�initialiser le compteur de tours depuis l'utilisation de la comp�tence
        }
    }




}
