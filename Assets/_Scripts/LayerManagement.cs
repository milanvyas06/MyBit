using UnityEngine;

public class LayerManagement : MonoBehaviour
{
    public static LayerManagement instance;

    public Sprite[] sprites;

    public int currentLayerIndex;

    public Sprite[] layerSelectUnSelectImg;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Object.DontDestroyOnLoad(base.gameObject);
        }
        else
        {
            Object.DestroyImmediate(base.gameObject);
        }
    }
    public void Start()
    {
        currentLayerIndex = (sprites.Length - 1);
    }
    public void OnLayerButtonClicked()
    {
        currentLayerIndex++;
        if (currentLayerIndex >= sprites.Length)
        {
            currentLayerIndex = 0;
        }
        if (currentLayerIndex == sprites.Length - 1)
        {
            SettingManager.instance.changeLayer.sprite = layerSelectUnSelectImg[1];
        }
        else
        {
            SettingManager.instance.changeLayer.sprite = layerSelectUnSelectImg[0];
        }
        SettingManager.instance.layerIndex.sprite = sprites[currentLayerIndex];
        ExportManager.instance.selectedLayer = sprites[currentLayerIndex];
    }
    public void SetOnExport()
    {
        ApplyExportSetting.instance.layerImage.sprite = sprites[currentLayerIndex];
    }
}
