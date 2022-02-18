using UnityEngine;

[CreateAssetMenu(fileName = "New Particle", menuName = "Create Particle")]
public class ParticlePrefab : ScriptableObject
{
    public int UniqueID;

    public GameObject Prefab;

    public Sprite _Thumbnail;

    public string _Name;

    public bool _UnlockDefault;

    public bool _IsUnlocked;
}
