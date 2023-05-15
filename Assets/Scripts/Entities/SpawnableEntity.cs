using UnityEngine;

public class SpawnableEntity : MonoBehaviour, ISpawnable
{
    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
