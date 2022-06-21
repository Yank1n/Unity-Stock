using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<Menu> _menus = new List<Menu>();

    private void Awake()
    {
        ShowMenu(_menus[0]);
    }

    public void ShowMenu(Menu menuToShow)
    {
        if (!_menus.Contains(menuToShow))
        {
            Debug.LogErrorFormat("{0} is not in the list of menus", menuToShow.name);
            return;
        }

        foreach (var otherMenu in _menus)
        {
            if (otherMenu == menuToShow)
            {
                otherMenu.gameObject.SetActive(true);
                otherMenu.menuDidAppear.Invoke();
            }
            else
            {
                if (otherMenu.gameObject.activeInHierarchy)
                {
                    otherMenu.menuWillDisappear.Invoke();
                }

                otherMenu.gameObject.SetActive(false);
            }
        }

    }
}
