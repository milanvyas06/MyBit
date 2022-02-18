using UnityEngine;
using UnityEngine.UI;

public class ToastTemp : MonoBehaviour
{
    public static ToastTemp instance;

    public Animator animator;

    public Text txt;

    private void Awake()
    {
        instance = this;
    }


    public void Show(string str)
    {
        txt.text = str;
        animator.Play("Toast");
    }

}
