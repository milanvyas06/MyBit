using System.Collections.Generic;
using UnityEngine;

public class ExportManager : MonoBehaviour
{
    public static ExportManager instance;

    public float beatValue;

    public float simulationValue;

    public AssetBundle selectedThemeBundle;

    public GameObject selectedTheme;

    public string str;

    public AssetBundle particleBundle;

    [HideInInspector]
    public string ignoreME;

    public GameObject selectedParticle;

    public string prefabName;

    public Sprite selectedImageSprite;

    public List<Sprite> id;

    public AudioClip selectionAudioClip;

    public string audioPath;

    public Material lightMaterial;

    public TextData _myTextData;

    public int particleToUnlock;

    public ParticlePrefab particlePrefab;

    public int exportResolution;

    public Sprite selectedLayer;

    public bool shouldLoadPartileFromBundle;

    public string thumbPath;

    public string particlePath;

    public string themeName;

    public GameObject transactionData;

    public bool selectedMultipleImage;

    public int themeToUnlock;

    public bool isInterupt;

    public string songName;

    public bool shouldRemoveWatermark = false;

    private void Awake()
    {
        if (instance != null)
        {
            Object.DestroyImmediate(base.gameObject);
            return;
        }
        instance = this;
        Object.DontDestroyOnLoad(base.gameObject);
    }
}
