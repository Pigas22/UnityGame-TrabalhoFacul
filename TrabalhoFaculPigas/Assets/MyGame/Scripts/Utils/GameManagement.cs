using UnityEngine;

static class GameManagement
{
    private static GameObject playerObject = GameObject.Find("Player");

    private static Camera mainCamera = Camera.main;

    public static GameObject PlayerObject
    {
        get { return playerObject; }
    }

    public static Camera MainCamera
    {
        get { return mainCamera; }
    }

    public static Vector3 positionToViewPortPoint(GameObject obj)
    {
        return mainCamera.WorldToViewportPoint(obj.transform.position);
    }

    public static Vector3 viewPortPointToPosition(Vector3 viewPortPoint)
    {
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

    public static bool OutOfCam(GameObject obj)
    {
        return OutOfXAxe(obj) || OutOfYAxe(obj);
    }
}