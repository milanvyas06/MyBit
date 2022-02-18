using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ParticleDataCollector : MonoBehaviour
{
    public string _MyCategoryName;

    public Button SelectParticleButton;

    public Button UnlockButton;

    public Text ParticleName;

    public Image Border;

    public Image Thumbnail;

    public bool _IsBundle;

    public Image _NewTag;

    public bool _IsThumbnailLoaded;

    public string _ThumbnailPath;

    private GameObject _MySelf;

    private bool IsThumbnailLoaded;

    private void Awake()
    {
        _MySelf = base.transform.gameObject;
        InvokeRepeating("StartToLoadThumb", 0.5f, 0.5f);
    }

    private IEnumerator LoadThumgRoutine(string path, Image img)
    {
        if (path != null && path != string.Empty)
        {
            Texture2D texture2D = null;
            WWW wWW = new WWW("file:///" + path);
            while (!wWW.isDone)
            {
                yield return null;
            }
            try
            {
                texture2D = wWW.texture;
            }
            catch (Exception arg)
            {
                Debug.Log("Texture Loading Exception : " + arg);
            }
            try
            {
                if (texture2D != null)
                {
                    Sprite sprite2 = img.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
                }
            }
            catch (Exception arg2)
            {
                Debug.Log("Failed to convert sprite : " + arg2);
            }
            IsThumbnailLoaded = true;
            CancelInvoke();
        }
    }

    private void StartToLoadThumb()
    {
        if (_IsBundle && SettingManager.instance.catName == _MyCategoryName && !IsThumbnailLoaded)
        {
            float num = Vector3.Distance(SettingManager.instance.pointCheck.transform.position, base.transform.position);
            if (num <= 810f)
            {
                LoadThumbnail();
            }
        }
    }

    public void LoadThumbnail()
    {
        StartCoroutine(LoadThumgRoutine(_ThumbnailPath, Thumbnail));
    }
}
