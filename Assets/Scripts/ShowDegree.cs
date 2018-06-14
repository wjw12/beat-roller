using UnityEngine;
using UnityEngine.UI;

public class ShowDegree : MonoBehaviour
{
    public GameObject obj;
    public GameObject btn;
    Button button;
    bool show;

    public void Start()
    {
        show = false;
        obj.SetActive(show);
        button = btn.GetComponent<Button>();
        button.onClick.AddListener(delegate ()
        {
            show = !show;
            obj.SetActive(show);
        }
        );
    }
}