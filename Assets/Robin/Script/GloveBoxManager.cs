using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveBoxManager : MonoBehaviour
{
    public GameObject GloveBox;
    public GameObject PanelAbility;

    public List<GameObject> PanelAbilitys { get; set; } = new List<GameObject>();   
    public int IndexPanelDisplayed { get; set; } = 0;
    public bool IsOpen { get; set; } = false;

    // Start is called before the first frame update
    void Awake()
    {
        for (int index = 0; index < PanelAbility.transform.childCount; index++)
        {
            PanelAbilitys.Add(PanelAbility.transform.GetChild(index).gameObject);
        }


    }
  
    public void NextPanel()
    {
        PanelAbilitys[IndexPanelDisplayed].gameObject.SetActive(false);
        IndexPanelDisplayed++;
        if (IndexPanelDisplayed >= PanelAbilitys.Count)
        {
            IndexPanelDisplayed = 0;
        }

        PanelAbilitys[IndexPanelDisplayed].gameObject.SetActive(true);
    }

    public void PreviousPanel()
    {
        PanelAbilitys[IndexPanelDisplayed].gameObject.SetActive(false);
        IndexPanelDisplayed--;
        if (IndexPanelDisplayed < 0)
        {
            IndexPanelDisplayed = PanelAbilitys.Count - 1;
        }

        PanelAbilitys[IndexPanelDisplayed].gameObject.SetActive(true);
    }

    public void OpenCloseGloveBox()
    {
        
            GloveBox.SetActive(!GloveBox.activeSelf);
       
    }
    


}
