using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadAssetBundle : MonoBehaviour
{
    public string bundleUrl;
    public string _prefabName;

    public GameObject particleParent;

    AssetBundle assetBundle;

    private IEnumerator Start()
    {
        WWW www = new WWW(bundleUrl);
        yield return www;
        if (www.error != null)
        {
            throw new System.Exception("There is an error + " + www.error);
        }
        assetBundle = www.assetBundle;

        if (assetBundle != null)
        {
            Debug.Log("We Found an asset.!");
        }
        StartCoroutine(LoadAsset("new", _prefabName));
    }



    IEnumerator LoadAsset(string assetBundleName, string objectNameToLoad)
    {

        assetBundle.Unload(true);
        var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(bundleUrl);
        yield return assetBundleCreateRequest;

        AssetBundle asseBundle = assetBundleCreateRequest.assetBundle;
        if (assetBundle == null)
        {
            Debug.Log("Found an assebundle null.!");
            yield return null;
        }

        string[] data = asseBundle.GetAllAssetNames();

        AssetBundleRequest asset = asseBundle.LoadAssetAsync<GameObject>(objectNameToLoad);

        yield return asset;


        var materials = asseBundle.LoadAllAssets<Material>();
        foreach (Material m in materials)
        {
            var shaderName = m.shader.name;
            var newShader = Shader.Find(shaderName);
            if (newShader == null)
            {
                switch (shaderName)
                {
                    case "Particles/Alpha Blended":
                        {

                            newShader = Shader.Find("Mobile/Particles/Alpha Blended");
                            break;
                        }
                    default:
                        break;
                }

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

        if (asset.asset == null)
        {
            Debug.Log("Asset is null.!");
            yield return null;
        }

        Instantiate(asset.asset as GameObject, particleParent.transform);
    }
}



