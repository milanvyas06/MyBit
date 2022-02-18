using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BundleBuilder : Editor
{
    [MenuItem("Assets/ Build AssetBundle")]
    static void BuildAllAssetBundle()
    {
        BuildPipeline.BuildAssetBundles(@"F:\AssetBundle\Android\Particle", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    }
}
