using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public GameObject BeatsSlide;

    public GameObject ParticleSimulationSlider;

    public GameObject RefreshBeatSlider;

    public GameObject RefreshParticleSimulation;

    public GameObject FavParticle;

    public GameObject ChangeLayer;
    public void SetPosition(float beatX, float simulationX)
    {
        Transform transform = BeatsSlide.transform;
        Vector3 localPosition = BeatsSlide.transform.localPosition;
        float y = localPosition.y;
        Vector3 localPosition2 = BeatsSlide.transform.localPosition;
        transform.localPosition = new Vector3(beatX, y, localPosition2.z);
        Transform transform2 = RefreshBeatSlider.transform;
        Vector3 localPosition3 = RefreshBeatSlider.transform.localPosition;
        float y2 = localPosition3.y;
        Vector3 localPosition4 = RefreshBeatSlider.transform.localPosition;
        transform2.localPosition = new Vector3(beatX, y2, localPosition4.z);
        Transform transform3 = ParticleSimulationSlider.transform;
        Vector3 localPosition5 = ParticleSimulationSlider.transform.localPosition;
        float y3 = localPosition5.y;
        Vector3 localPosition6 = ParticleSimulationSlider.transform.localPosition;
        transform3.localPosition = new Vector3(simulationX, y3, localPosition6.z);
        Transform transform4 = RefreshParticleSimulation.transform;
        Vector3 localPosition7 = RefreshParticleSimulation.transform.localPosition;
        float y4 = localPosition7.y;
        Vector3 localPosition8 = RefreshParticleSimulation.transform.localPosition;
        transform4.localPosition = new Vector3(simulationX, y4, localPosition8.z);
        Transform transform5 = FavParticle.transform;
        Vector3 localPosition9 = FavParticle.transform.localPosition;
        float y5 = localPosition9.y;
        Vector3 localPosition10 = FavParticle.transform.localPosition;
        transform5.localPosition = new Vector3(beatX, y5, localPosition10.z);
        Transform transform6 = ChangeLayer.transform;
        Vector3 localPosition11 = FavParticle.transform.localPosition;
        float y6 = localPosition11.y;
        Vector3 localPosition12 = FavParticle.transform.localPosition;
        transform6.localPosition = new Vector3(simulationX, y6, localPosition12.z);
    }

    public float[] CalculatePosition()
    {
        float[] array = new float[2];
        float width = Screen.width;
        float height = Screen.height;
        float requireWidth = 720f;
        float requiredHeight = 1280f;
        float num5 = width / requireWidth;
        float num6 = height / requiredHeight;
        float num7 = num6 - num5;
        float num8 = num7 / 10f;
        float num9 = num8 * 3f;
        float num10 = num6 - num9;
        float num11 = width / num10;
        float num12 = num11 - 485f;
        float num13 = num12 / 2f;
        float num14 = num13 / 2f;
        float num15 = num11 / 2f;
        float num16 = num15 - num14;
        float num17 = array[0] = Mathf.Abs(num16) * -1f;
        array[1] = num16;
        return array;
    }

    private void Start()
    {
        if (PlayerPrefs.instance.IsStoredResolution() == 0)
        {
            Screen.SetResolution(Screen.width, Screen.height, fullscreen: true);
            PlayerPrefs.instance.StoreScreenSize();
        }
        else
        {
            Screen.SetResolution(PlayerPrefs.instance.GetWidth(), PlayerPrefs.instance.GetHeight(), fullscreen: true);
        }
        UnityEngine.PlayerPrefs.SetString("IsLibLoaded", "1");
        if (PlayerPrefs.instance.isStoredObjectPosition() == 0)
        {
            SetPosition(CalculatePosition()[0], CalculatePosition()[1]);
            PlayerPrefs.instance.StoreObjectPosition(CalculatePosition()[0], CalculatePosition()[1]);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            SetPosition(PlayerPrefs.instance.GetLeftPosition(), PlayerPrefs.instance.GetRightPosition());
        }
        else
        {
            SetPosition(CalculatePosition()[0], CalculatePosition()[1]);
        }
    }

}
