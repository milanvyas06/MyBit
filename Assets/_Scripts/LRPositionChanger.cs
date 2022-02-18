using UnityEngine;

public class LRPositionChanger : MonoBehaviour
{
    private LineRenderer lr;

    public Transform[] lstOfTransform;

    private void Update()
    {
        for (int i = 0; i < lstOfTransform.Length; i++)
        {
            lr.SetPosition(i, lstOfTransform[i].position);
        }
    }

}
