using UnityEngine;

static class GameManagement
{
    private static GameObject playerObject = GameObject.Find("Player");

    private static Camera mainCamera = Camera.main;

    public static GameObject PlayerObject
    {
        get { return playerObject; }
    }

    public static Vector3 positionToViewPortPoint(GameObject obj)
    {
        return mainCamera.WorldToViewportPoint(obj.transform.position);
    }

    public static Vector3 viewPortPointToPosition(Vector3 viewPortPoint)
    {
        return mainCamera.ViewportToWorldPoint(viewPortPoint);
    }
}