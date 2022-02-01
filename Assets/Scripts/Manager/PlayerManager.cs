using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

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

 


    //Player instancié
    [HideInInspector] public GameObject player;

    private GameManager m_Game;
    private TankController tankController;

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
        TankDestroyed tankDestroyed = player.GetComponent<TankDestroyed>();
        TankCreate tankCreate = player.GetComponent<TankCreate>();

        tankDestroyed.Destroyed += PlayerDestroy;
        tankCreate.Created += PlayerCreated;   
        m_Game.Projectile.LoadPlayerShooter();
        ResetStats();
    }

    
    
    
    //Reset all bonus stats and upgrade of tanks
    public void ResetStats()
    {
        if (player != null)
        {
            tankController = player.GetComponent<TankController>();
            List<ITankComponent> tankComponentsList = tankController.GetComponents<ITankComponent>().ToList();

            tankComponentsList.ForEach(component =>
            {
                string componentName = component.GetType().FullName.Replace("Controller", String.Empty);
                ScriptableObject dataFound = dataList.FirstOrDefault(data => data.name.Contains(componentName));
                if (dataFound)
                {
                    component.BindData(dataFound);
                    component.BindComponent();
                    component.BindStats();
                    
                }
                    
            });

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
    public void SetUiLifeOfPlayer()
    {
        if(tankController)
             m_Game.Ui.SetLifeUI(tankController.BodyController.GetMaxHealt(), tankController.BodyController.GetMaxHealt());
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