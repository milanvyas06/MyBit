using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadTemplate : MonoBehaviour
{
    public static LoadTemplate instance;

    public GameObject particleSystem;

    public Text txt;


    private void Awake()
    {
        instance = this;
    }
    public void LoadParticleTemplate(GameObject particle, GameObject particleBtn, int uid, ParticlePrefab particlePrefab)
    {
        ExportManager.instance.particleToUnlock = uid;
        ExportManager.instance.shouldLoadPartileFromBundle = false;
        ExportManager.instance.particlePrefab = particlePrefab;
        if (SettingManager.instance.isPlaying)
        {
            Time.timeScale = 1f;
            AudioHelperMain.instance.audioSourceAndroid.Play();
        }
        try
        {
            UnityEngine.Object.Destroy(SettingManager.instance.selectedParticleTemplate);
        }
        catch (Exception)
        {
        }
        GameObject gameObject = UnityEngine.Object.Instantiate(particle, new Vector3(0f, 0f, 0f), Quaternion.identity);
        gameObject.transform.parent = particleSystem.transform;
        gameObject.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
        gameObject.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        gameObject.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        SettingManager.instance.selectedParticleTemplate = gameObject;
        SettingManager.instance.ApplyBeatSettingForParticles(SettingManager.instance.selectedParticleTemplate.GetComponent<ParticleData>());
        SettingManager.instance.SimulationController();
        ExportManager.instance.selectedParticle = particle;
        ExportManager.instance._myTextData.data = new List<string>(0);
        UserTextList[] texts = gameObject.transform.GetComponent<ParticleData>()._Texts;
        foreach (UserTextList userTextList in texts)
        {
            string placeHolder = userTextList._PlaceHolder;
            ExportManager.instance._myTextData.data.Add(placeHolder);
        }
        Camera[] camera = gameObject.transform.GetComponent<ParticleData>()._Camera;
        foreach (Camera camera2 in camera)
        {
            camera2.targetTexture = ProjectorManager.instance.rednTexture;
        }
        changeBorderSprite(particleBtn);
    }


    public void LoadParticleAssetBundle(string uId, string bundlePath, string thumbnail, string themeName, string _prefab, GameObject particleButton)
    {
        SettingManager.instance._PreviewLoadingPanel.SetActive(value: true);
        try
        {
            ExportManager.instance.particleBundle.Unload(unloadAllLoadedObjects: false);
        }
        catch (Exception)
        {
        }
        StartCoroutine(MakeAssetBundleCreateRequestAndLoad(uId, bundlePath, thumbnail, themeName, _prefab, particleButton));
    }

    public IEnumerator MakeAssetBundleCreateRequestAndLoad(string uid, string bundlePath, string thumbnail, string themeName, string prefabName, GameObject particleButton)
    {
        Debug.Log("Prefab Name : " + prefabName + " Bundle Path : " + bundlePath);
        AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return assetBundleCreateRequest;
        if (assetBundleCreateRequest != null)
        {
            AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;
            if (assetBundle == null)
            {
                Debug.Log("Bundle file loaded from storage but cannot generate.");
            }
            if (SettingManager.instance.inProcessAssetBundle != null)
            {
                SettingManager.instance.inProcessAssetBundle.Unload(unloadAllLoadedObjects: true);
            }
            SettingManager.instance.inProcessAssetBundle = assetBundle;
            ExportManager.instance.particleBundle = assetBundle;
            if (assetBundle == null)
            {
                Debug.Log("Asset Bundle is null.!");
            }
            AssetBundleRequest assetBundleRequest = assetBundle.LoadAssetAsync<GameObject>(prefabName);

            ReShaderMapping(assetBundle);

            yield return assetBundleRequest;
            GameObject original = assetBundleRequest.asset as GameObject;
            GameObject gameObject = UnityEngine.Object.Instantiate(original, new Vector3(0f, 0f, 0f), Quaternion.identity);
            if (gameObject != null)
            {
                try
                {
                    UnityEngine.Object.Destroy(SettingManager.instance.selectedParticleTemplate);
                }
                catch (Exception)
                {
                }
                gameObject.transform.parent = particleSystem.transform;
                gameObject.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
                gameObject.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                gameObject.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                SettingManager.instance.selectedParticleTemplate = gameObject;
                SettingManager.instance.ApplyBeatSettingForParticles(SettingManager.instance.selectedParticleTemplate.GetComponent<ParticleData>());
                SettingManager.instance.SimulationController();
                ExportManager.instance._myTextData.data = new List<string>(0);
                UserTextList[] texts = gameObject.transform.GetComponent<ParticleData>()._Texts;
                foreach (UserTextList userTextList in texts)
                {
                    string placeHolder = userTextList._PlaceHolder;
                    ExportManager.instance._myTextData.data.Add(placeHolder);
                }
                for (int j = 0; j < gameObject.transform.GetComponent<ParticleData>()._Camera.Length; j++)
                {
                    gameObject.transform.GetComponent<ParticleData>()._Camera[j].targetTexture = ProjectorManager.instance.rednTexture;
                    Debug.Log("Texture Attached to lline renderer");
                }
            }
        }
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.root.bridge.AndroidPluginClass");
                androidJavaClass2.CallStatic("removePrtclTag", @static, uid);
            }
            else
            {
                Debug.Log("New particle tag is removed for UId :- " + uid);
            }
        }
        catch (Exception ex2)
        {
            Debug.Log("Error Launch1 " + ex2.Message);
        }
        SettingManager.instance._PreviewLoadingPanel.SetActive(value: false);
    }


    private void ReShaderMapping(AssetBundle assetBundle)
    {
        var materials = assetBundle.LoadAllAssets<Material>();
        foreach (Material m in materials)
        {
            var shaderName = m.shader.name;
            var newShader = Shader.Find(shaderName);
            if (newShader == null)
            {
                bool isShaderApplied = false;
                switch (shaderName)
                {

                    case "Particles/Alpha Blended":
                        {
                            newShader = Shader.Find("Mobile/Particles/Alpha Blended");
                            isShaderApplied = true;
                            break;
                        }
                    case "Particles/Additive (Soft)":
                        {
                            newShader = Shader.Find("Mobile/Particles/Additive");
                            isShaderApplied = true;
                            break;

                        }
                    default:
                        break;
                }
                if (!isShaderApplied) Debug.Log(" Shader Found Null and no applied.! " + shaderName);
            }

            if (newShader != null)
            {
                m.shader = newShader;
                Debug.Log("Done with shader apply process.!");
            }
            else
            {
                Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + m.name);
            }
        }
    }

    public void LoadParticleTemplateForPreviewOnly(GameObject prefab, int uid)
    {
        ExportManager.instance.particleToUnlock = uid;
        if (SettingManager.instance.isPlaying)
        {
            Time.timeScale = 1f;
            AudioHelperMain.instance.audioSourceAndroid.Play();
        }
        GameObject gameObject = UnityEngine.Object.Instantiate(prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        gameObject.transform.parent = SettingManager.instance.previewSystem.transform;
        gameObject.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
        gameObject.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        gameObject.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        SettingManager.instance.selectedParticleData = gameObject;
        Camera[] camera = gameObject.transform.GetComponent<ParticleData>()._Camera;
        foreach (Camera camera2 in camera)
        {
            camera2.targetTexture = ProjectorManager.instance.rednTexture;
        }
    }

    public void changeBorderSprite(GameObject btnContainer)
    {
        if (SettingManager.instance.isPlaying)
        {
            Time.timeScale = 1f;
            AudioHelperMain.instance.audioSourceAndroid.Play();
        }
        if (SettingManager.instance.selectedParticle != null)
        {
            SettingManager.instance.selectedParticle.transform.GetComponent<ParticleDataCollector>().Border.sprite = SettingManager.instance.particleUnSelectBorder;
        }
        SettingManager.instance.selectedParticle = btnContainer;
        SettingManager.instance.selectedParticle.transform.GetComponent<ParticleDataCollector>().Border.sprite = SettingManager.instance.particleSelectedBorder;
    }

}
