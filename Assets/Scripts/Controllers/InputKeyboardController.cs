using UnityEngine;

public class InputKeyboardController : InputController
{
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            InstantPlace.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            HorizontalMove.Invoke(-1);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            HorizontalMove.Invoke(1);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            TouchClick.Invoke();
        }
    }
}
