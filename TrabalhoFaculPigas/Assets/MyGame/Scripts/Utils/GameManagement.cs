using System;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;

static class GameManagement
{
    private static bool isDebugging = false;

    private static float musicVolume = 0.1f;
    private static GameObject currentPlayer;
    private static int currentSkinIndex = 0;
    private static readonly GameObject[] playerPrefabs =  new GameObject[] {
        Resources.Load<GameObject>("Prefabs/Players/VirtualGuyPlayer"),
        Resources.Load<GameObject>("Prefabs/Players/MaskDudePlayer"),
        Resources.Load<GameObject>("Prefabs/Players/NinjaFrogPlayer"),
        Resources.Load<GameObject>("Prefabs/Players/PinkManPlayer")
    };
    public static void ConfigPlayerSkin()
    {
        Vector3 playerPosition;
        // Destroi o player atual se existir
        if (currentPlayer != null) {
            playerPosition = currentPlayer.transform.position;
            UnityEngine.Object.Destroy(currentPlayer);
        }
        else { playerPosition = Vector3.zero; }

        // Instancia o novo player
        GameObject prefab = playerPrefabs[currentSkinIndex];
        currentPlayer = UnityEngine.Object.Instantiate(prefab, playerPosition, Quaternion.identity);
        currentPlayer.tag = "Player";
    }
    // Apenas para o Menu Principal
    public static int CurrentSkinIndex
    {
        get => currentSkinIndex;
        set => currentSkinIndex = value;
    }

    public static float MusicVolume
    {
        get => musicVolume;
        set => musicVolume = value;
    }

    public static GameObject CurrentPlayer
    {
        get { return currentPlayer; }
        set { currentPlayer = value; }
    }

    private static Camera mainCamera = Camera.main;
    public static Camera MainCamera
    {
        get
        {
            UpdateMainCamera();
            return mainCamera;
        }
    }

    public static void UpdateMainCamera()
    {
        mainCamera = Camera.main;
    }

    public static Vector3 positionToViewPortPoint(GameObject obj)
    {
        UpdateMainCamera();
        return mainCamera.WorldToViewportPoint(obj.transform.position);
    }
    public static Vector3 positionToViewPortPoint(Vector3 position)
    {
        UpdateMainCamera();
        return mainCamera.WorldToViewportPoint(position);
    }

    public static Vector3 viewPortPointToPosition(Vector3 viewPortPoint)
    {
        UpdateMainCamera();
        return mainCamera.ViewportToWorldPoint(viewPortPoint);
    }


    public static bool OutOfXAxe(GameObject obj)
    {
        Vector3 viewportPoint = positionToViewPortPoint(obj);
        return viewportPoint.x < 0 || viewportPoint.x > 1;
    }

    public static bool OutOfYAxe(GameObject obj)
    {
        Vector3 viewportPoint = positionToViewPortPoint(obj);
        return viewportPoint.y < 0 || viewportPoint.y > 1;
    }

    public static bool OutOfYDownAxe(GameObject obj)
    {
        Vector3 viewportPoint = positionToViewPortPoint(obj);
        return viewportPoint.y < 0;
    }

    public static bool OutOfCam(GameObject obj)
    {
        if (obj.CompareTag("Player")) return OutOfXAxe(obj) || OutOfYDownAxe(obj);
   //     else if (obj.CompareTag("Enemy")) return OutOfCamLimits(obj);
        else return OutOfXAxe(obj) || OutOfYAxe(obj);
    }

    
    // Mostra no console qual objeto está abrindo ou fechando
    public static void DebugIsOpenMenu(string name, bool isOpenning)
    {
        if (isOpenning) DebugLog("Abrindo: " + name);
        else DebugLog("Fechando: " + name);
    }
    public static void DebugIsOpenMenu(GameObject obj, bool isOpenning)
    {
        if (isOpenning) DebugLog("Abrindo: " + obj.name);
        else DebugLog("Fechando: " + obj.name);
    }

    public static void DebugLog(string msg)
    {
        if (isDebugging) Debug.Log(msg);
    }

    public static void AtualizaScoreInfoPlayer(PlayerManager playerManager, TextMeshProUGUI kiwiCollectableInfoText, TextMeshProUGUI orangeCollectableInfoText, TextMeshProUGUI pontosTotaisText)
    {
        if (playerManager.GetTotalScore() >= 0)
        {
            var lista = playerManager.GetCollectedItensInfos();

            if (lista.Count > 0)
            {
                DebugLog("Primeiro item : " + playerManager.GetCollectedItensInfos()[0].ToString());
                foreach (CollectedItensInfo item in lista)
                {
                    if (item.NameItem == "Kiwi")
                    {
                        kiwiCollectableInfoText.text = item.Qtd + " X " + item.Value + " = " + (item.Qtd * item.Value);
                    }
                    else if (item.NameItem == "Orange")
                    {
                        orangeCollectableInfoText.text = item.Qtd + " X " + item.Value + " = " + (item.Qtd * item.Value);
                    }
                }
            }
            else
            {
                DebugLog("Lista : " + playerManager.GetCollectedItensInfos().ToString());
            }

            pontosTotaisText.text = "Total de Pontos: " + playerManager.GetTotalScore().ToString();
        }
        else
        {
            pontosTotaisText.text = "Total de Pontos: --";
        }
    }
}