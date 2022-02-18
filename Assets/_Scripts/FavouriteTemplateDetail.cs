using System;
using UnityEngine;

[Serializable]
public struct FavouriteTemplateDetail
{
    public int ShortingNo;

    public int UniqueNo;

    public GameObject ParticleButton;

    public ParticlePrefab _particlePrefab;

    public string _bundlePath;

    public string _decryptedBundlePath;

    public string _thumbnailPath;

    public string _PrefabName;

    public string _TemplateName;

    public bool IsBundle;

    public string DownloadAt;
}
