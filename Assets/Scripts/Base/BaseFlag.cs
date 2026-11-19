using UnityEngine;

public class BaseFlag : MonoBehaviour
{
    public bool IsPlaced => gameObject.activeSelf;

    public Vector3 Position => transform.position;

    public void Place(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}