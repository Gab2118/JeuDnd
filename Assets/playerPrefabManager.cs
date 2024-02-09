using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerPrefabManager : NetworkBehaviour
{
    public Button yesButton; // Assure-toi que ce bouton est assign� dans l'�diteur Unity

    // Cette m�thode est appel�e lorsqu'un joueur est activ�.
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
                if (isLocalPlayer) // V�rifie si le script est ex�cut� par le joueur local
                {
                    CmdUseSkill();
                }
            });
        }
    }

    [Command]
    public void CmdUseSkill()
    {
        // Place ici la logique pour utiliser la comp�tence
        // Cette partie s'ex�cute sur le serveur

        RpcUseSkill();
    }

    [ClientRpc]
    void RpcUseSkill()
    {
        // Cette partie s'ex�cute sur tous les clients
        // Utilise cette fonction pour mettre � jour l'interface utilisateur ou les �tats du jeu sur tous les clients apr�s l'utilisation de la comp�tence
    }
}
