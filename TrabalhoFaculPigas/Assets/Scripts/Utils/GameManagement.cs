using UnityEngine;

static class GameManagement
{
    private static Camera mainCamera = Camera.main;

    public static Vector3 positionToViewPortPoint(GameObject obj)
    {
        return mainCamera.WorldToViewportPoint(obj.transform.position);
    }

    public static Vector3 viewPortPointToPosition(Vector3 viewPortPoint)
    {
        return mainCamera.ViewportToWorldPoint(viewPortPoint);
    }
}