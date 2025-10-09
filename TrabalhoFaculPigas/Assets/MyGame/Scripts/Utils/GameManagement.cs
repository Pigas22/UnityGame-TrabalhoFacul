using UnityEngine;

static class GameManagement
{
    private static GameObject playerObject = GameObject.Find("Player");

    private static Camera mainCamera = Camera.main;

    public static GameObject PlayerObject
    {
        get { return playerObject; }
        set { playerObject = value; }
    }

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
        if (obj == playerObject) return OutOfXAxe(obj) || OutOfYDownAxe(obj);
        else return OutOfXAxe(obj) || OutOfYAxe(obj);
    }

    
    // Mostra no console qual objeto está abrindo ou fechando
    public static void DebugIsOpenMenu(string name, bool isOpenning)
    {
        if (isOpenning) Debug.Log("Abrindo: " + name);
        else Debug.Log("Fechando: " + name);
    }
    public static void DebugIsOpenMenu(GameObject obj, bool isOpenning)
    {
        if (isOpenning) Debug.Log("Abrindo: " + obj.name);
        else Debug.Log("Fechando: " + obj.name);
    }
}