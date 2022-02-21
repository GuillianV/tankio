using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerManager))]
class PlayerButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        GameManager m_game = GameManager.Instance;
        PlayerManager playerManager = (PlayerManager) target;

        if (GUILayout.Button("Reset All"))
        {
            playerManager.ResetPlayerManager();
        }

        if (GUILayout.Button("Reset Stats"))
        {
            playerManager.ResetStats();
        }
    }
}
#endif

public class PlayerManager : MonoBehaviour
{
    //Prefab du Player
    public GameObject playerPrefab;

    //Conteneur du parent
    public Transform parentContainer;

    //Scriptable objects pour les tanks
    [Header("Player Stats")]
    public List<ScriptableObject> dataList = new List<ScriptableObject>();

    public List<TextAsset> controllerList = new List<TextAsset>();

    [Header("Player Inputs")]
    public InputActionAsset playerInput;
    public PlayerNotifications notifications = PlayerNotifications.SendMessages;

    //Player instancié
    [HideInInspector] public GameObject player;

    private GameManager m_Game;
    private TankController tankController;

    private BodyManager m_bodyManager;
    private BodyController m_bodyController;
    
    public event EventHandler<TankEvent> OnPlayerDestroyed;
    public event EventHandler<TankEvent> OnPlayerCreated;

    private void Awake()
    {
        m_Game = GameManager.Instance;
    }



    #region CreatePlayer

    //Active le joueur
    public void EnablePlayer()
    {
        player.SetActive(true);
    }

    //Desactive le joueur
    public void DisablePlayer()
    {
        player.SetActive(false);
    }

    
    public void PlayerDestroy(object sender, TagEvent args)
    {

        TankDestroyed tankDestroyed = sender as TankDestroyed;
        if (tankDestroyed.gameObject)
        {
            TankController currentTankController = tankDestroyed.GetComponent<TankController>();
            OnTankDestroyed(currentTankController);
            m_Game.Ui.ShowDeadMenu();
        }
      
    }
    
    public void PlayerCreated(object sender, EventArgs args)
    {
        TankCreate tankCreate = sender as TankCreate;
        TankController currentTankController = tankCreate.GetComponent<TankController>();
        OnTankCreated(currentTankController);
    }
    
    public void OnTankDestroyed(TankController currentTankController)
    {
        OnPlayerDestroyed?.Invoke(this,new TankEvent(currentTankController));
    }
    
    public void OnTankCreated(TankController currentTankController)
    {
        OnPlayerCreated?.Invoke(this,new TankEvent(currentTankController));
    }

    
    
    //Instancie le joueur
    public void InsatanciatePlayer()
    {
        player = Instantiate(playerPrefab,
            new Vector3(parentContainer.transform.position.x, parentContainer.transform.position.y, 0),
            new Quaternion(0, 0, 0, 0), parentContainer) as GameObject;
        //Création du player
        TankDestroyed tankDestroyed = player.GetComponentInChildren<TankDestroyed>();
        TankCreate tankCreate = player.GetComponentInChildren<TankCreate>();

        tankDestroyed.Destroyed += PlayerDestroy;
        tankCreate.Created += PlayerCreated;

     
        controllerList.ForEach(asset =>
        {
            Type abilityType = Type.GetType(asset.name);
            player.AddComponent(abilityType);

        } );
        
       
        
        m_Game.Projectile.LoadPlayerShooter();
        ResetStats();
    }

 
    public void EnableInputs()
    {
        PlayerInput pi = player.AddComponent<PlayerInput>();

        pi.actions = playerInput;
        pi.notificationBehavior = notifications;
        pi.defaultActionMap = "Tank";
        pi.enabled = true;
        pi.ActivateInput();


    }


    //Reset all bonus stats and upgrade of tanks
    public void ResetStats()
    {
        if (player != null)
        {
            tankController = player.GetComponent<TankController>();
            tankController.BindTank(dataList);
            m_bodyManager = tankController.GetTankManager<BodyManager>();
            m_bodyController = m_bodyManager.bodyController;

        }
        else
        {
            Debug.LogWarning("Cant Find Any Player");
        }
    }

    //Destroy player
    public void DestroyPlayer()
    {
        foreach (Transform child in parentContainer)
            Destroy(child.gameObject);
    }

    //Create new Player
    public void CreatePlayer()
    {
        InsatanciatePlayer();
    }

    //Set camera follow new player
    public void SetCameraFollowPlayer()
    {
        m_Game.Camera.SetGameObjectToFollow(player);
    }

    //Set life of player
    public void ResetSetUiLifeOfPlayer()
    {
        if (tankController)
        {
            if (m_bodyController != null)
            {
                m_Game.Ui.SetLifeUI(m_bodyController.GetMaxHealt(), m_bodyController.GetMaxHealt());
            }else
            {
            
                Debug.LogError("Player Manager missing BodyController");
            }
        }
          
    }

    public void SetUiLifeOfPlayer()
    {
        
        if (tankController)
        {
            if (m_bodyController != null)
            {
                m_Game.Ui.SetLifeUI(m_bodyController.GetMaxHealt(), m_bodyController.GetHealt());
            }
            else
            {
            
                Debug.LogError("Player Manager missing BodyController");
            }
        }
        
          
    }


    #endregion




    public void ResetPlayerManager()
    {
        DestroyPlayer();
        CreatePlayer();
        m_Game.Shop.ResetShopManager();
        m_Game.Ui.ResetShopUIManager();
        SetCameraFollowPlayer();
        SetUiLifeOfPlayer();
        m_Game.Ui.HideDeadMenu();
    }
}