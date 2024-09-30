using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Corpse : MonoBehaviour
{
    [SerializeField]
    private GameObject corpse;
    private static List<Vector3> corpsePositions = new List<Vector3>();

    public void AddCorpse(Vector3 position)
    {
        corpsePositions.Add(position);
    }

    public void SpawnCorpse()
    {
        for (int i = 0; i < corpsePositions.Count; i++)
        {
            Instantiate(corpse, corpsePositions[i], Quaternion.identity);
        }
    }

    public void ClearCorpses()
    {
        corpsePositions = new List<Vector3>();
    }

    public List<Vector3> getPositions()
    {
        return corpsePositions;
    }


}
