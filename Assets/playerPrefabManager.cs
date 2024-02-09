using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerPrefabManager : NetworkBehaviour
{
    public Button yesButton; // Assure-toi que ce bouton est assigné dans l'éditeur Unity

    // Cette méthode est appelée lorsqu'un joueur est activé.
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        AddButtonListener();
    }

    private void AddButtonListener()
    {
        if (yesButton != null)
        {
            yesButton.onClick.AddListener(() =>
            {
                if (isLocalPlayer) // Vérifie si le script est exécuté par le joueur local
                {
                    CmdUseSkill();
                }
            });
        }
    }

    [Command]
    public void CmdUseSkill()
    {
        // Place ici la logique pour utiliser la compétence
        // Cette partie s'exécute sur le serveur

        RpcUseSkill();
    }

    [ClientRpc]
    void RpcUseSkill()
    {
        // Cette partie s'exécute sur tous les clients
        // Utilise cette fonction pour mettre à jour l'interface utilisateur ou les états du jeu sur tous les clients après l'utilisation de la compétence
    }
}
