using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;

public class TabGroup : MonoBehaviour
{

    public List<TabButton> tabButtons;
    public TabButton selectedTab;
    public TabButton firstButton;

    public RecyclableScrollRect rect;

    public void Start()
    {
        selectedTab = firstButton;
        firstButton.IMG.sprite = firstButton.selectedIMG;
        print("started tabgroup function");
    }

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        //print("enterd");
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            //button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        //print("exited");
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        print("selected a tab");
        selectedTab = button;
        rect.ReloadData();
        print("before reset tab is " + selectedTab);
        ResetTabs();
        print("after reset tab is " + selectedTab);
        selectedTab.IMG.sprite = selectedTab.selectedIMG;
        //button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab!=null && button == selectedTab) { continue; }
            button.IMG.sprite = button.idleIMG;
        }
    }
}
