using UnityEngine;

public class LineRendererPositionUpdater : MonoBehaviour
{
    private LineRenderer lr;

    public Transform[] Position;

    private void Start()
    {
        lr = base.transform.GetComponent<LineRenderer>();
    }
    private void Update()
    {
        for (int i = 0; i < Position.Length; i++)
        {
            lr.SetPosition(i, Position[i].position);
        }
    }
}
