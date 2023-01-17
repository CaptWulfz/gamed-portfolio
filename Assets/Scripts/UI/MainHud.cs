using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHud : MonoBehaviour
{
    //private Controls uiControls;
    private void OnEnable()
    {
        //if (this.uiControls == null)
        //    this.uiControls = InputManager.Instance.GetControls();

        //this.uiControls.MainHUD.Enable();
        //this.uiControls.MainHUD.Pause.performed += context => 
        //{
        //    OnMenuButtonClicked();
        //};
    }

    private void OnDisable()
    {
        //this.uiControls.MainHUD.Disable();
    }

    public void OnMenuButtonClicked()
    {
        //MenuPopup menu = PopupManager.Instance.ShowPopup<MenuPopup>(PopupNames.MENU_POPUP);
        //menu.Show();
    }
}
