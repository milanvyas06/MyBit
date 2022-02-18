using System;

[Serializable]
public class ImagesManager
{
    public int _TotalImages;

    public float clipLength;

    public float calculateDistance;

    public float distance;

    public float CurrentImage;

    public float Counter;

    public void ResetImageManager()
    {
        _TotalImages = 0;
        clipLength = 0f;
        calculateDistance = 0f;
        distance = 0f;
        CurrentImage = 0f;
        Counter = 0f;
    }
}
