using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.Tetris
{
    public class InputTouchController : InputController
    {

        private bool touched = false;
        private bool forceCancel = false;
        private float forceTime = 0;

        private Vector2 forceVector;
        private float dragHorizontal;

        void Update()
        {

            // mobile touch ?
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        Debug.Log(touch.position);

                        touched = true;
                        forceCancel = false;
                        forceTime = Time.time;
                        forceVector = Vector2.zero;
                        break;

                    case TouchPhase.Moved:
                        if (touched)
                        {
                            if (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y) * 2f)
                            {
                                dragHorizontal += touch.deltaPosition.x * Time.deltaTime;
                                if(Math.Abs(dragHorizontal) > GameSettings.Instance.BlockSize)
                                {
                                    dragHorizontal = 0;
                                    forceCancel = true;
                                    HorizontalMove.Invoke(dragHorizontal > 0 ? 1 : -1);                                    
                                }                               
                            }
                            else
                            {
                                if (Mathf.Abs(touch.deltaPosition.y) > Mathf.Abs(touch.deltaPosition.x) * 2)
                                {
                                    forceVector += touch.deltaPosition;
                                }
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                        if (touched)
                        {
                            touched = false;
                            float delta = Time.time - forceTime;
                            if (delta < 0.184f)
                            {
                                TouchClick.Invoke();
                            }
                            else if (delta < 0.44f)
                            {
                                if (!forceCancel)
                                {
                                    if (Mathf.Abs(forceVector.y) > Mathf.Abs(forceVector.x) * 6)
                                    {
                                        InstantPlace.Invoke();
                                    }
                                }
                            }

                        }

                        break;
                }
            }

        }
    }
}