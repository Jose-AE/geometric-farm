using UnityEngine;

public static class MenuManager
{
    public static Transform menusParent;

    public static void OpenMenu(string menuGameObjectName)
    {

        if (menusParent == null)
            menusParent = GameObject.FindGameObjectWithTag("MenusParent").transform;


        foreach (Transform menu in menusParent)
        {
            if (menu.name == menuGameObjectName)
                menu.gameObject.SetActive(true);
            else
                menu.gameObject.SetActive(false);
        }
    }

}
