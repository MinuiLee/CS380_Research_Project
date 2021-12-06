using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_State_Logic : MonoBehaviour
{
    public GameObject SliderObject;
    private Slider group;
    public TMPro.TMP_Dropdown density;
    public TMPro.TMP_Dropdown beat;
    public TMPro.TMP_InputField minText;
    public TMPro.TMP_InputField maxText;
    public TMPro.TMP_Text groupTotalText;

    void Start()
    {
        group = SliderObject.GetComponent<Slider>();
        groupTotalText.text = group.value.ToString();
    }

    void Update()
    {
        groupTotalText.text = group.value.ToString();
    }

    public void LoadGenerator()
    {
        Saving_State_Variables.groupCount = (int)group.value;
        Saving_State_Variables.density = (int)density.value;
        Saving_State_Variables.beatPattern = (int)beat.value;
        Saving_State_Variables.minTotalTime = int.Parse(minText.text);
        Saving_State_Variables.maxTotalTime = int.Parse(maxText.text);

        Debug.Log("minTime: " + Saving_State_Variables.minTotalTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
