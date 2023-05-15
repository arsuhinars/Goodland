using System.Collections.Generic;
using UnityEngine;

public class SpawnableGroup : MonoBehaviour, ISpawnable
{
    [SerializeField] private int m_minSpawnCount;
    [SerializeField] private int m_maxSpawnCount;
    [SerializeField] private SerializedInterfacesArray<ISpawnable> m_spawnables;

    public void Spawn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < m_spawnables.Array.Length; ++i)
        {
            m_spawnables.Array[i].Kill();
        }

        m_minSpawnCount = Mathf.Max(m_minSpawnCount, 0);
        m_maxSpawnCount = Mathf.Clamp(
            m_maxSpawnCount,
            m_minSpawnCount,
            m_spawnables.Array.Length
        );

        int count = Random.Range(m_minSpawnCount, m_maxSpawnCount + 1);
        
        var spawnables = new List<ISpawnable>(m_spawnables.Array);
        for (int i = 0; i < count; ++i)
        {
            int idx = Random.Range(0, spawnables.Count);
            spawnables[idx].Spawn();
            spawnables.RemoveAt(idx);
        }
    }

    public void Kill()
    {
        for (int i = 0; i < m_spawnables.Array.Length; ++i)
        {
            m_spawnables.Array[i].Kill();
        }

        gameObject.SetActive(false);
    }

    private void Awake()
    {
        m_spawnables.Initialize();
    }
}
