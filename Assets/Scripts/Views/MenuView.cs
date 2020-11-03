using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Genesis.Tetris
{
    [RequireComponent(typeof(MenuController))]
    public class MenuView : MonoBehaviour
    {
        public GameObject ButtonPrefab;
        public GameObject ButtonsPanel;

        public void InitButtons(List<LevelSettings> levels, bool save, Action<int> callback)
        {
            void closure(string name, int i)
            {
                var button = Instantiate(ButtonPrefab, ButtonsPanel.transform);
                Button buttonComponent = button.GetComponent<Button>();
                buttonComponent.onClick.AddListener(new UnityAction(() => callback(i)));

                Text caption = button.GetComponentInChildren<Text>();
                caption.text = name;
            }

            if(save)
            {
                closure("Continue game", -1);
            }            

            for (int i = 0; i < levels.Count; ++i)
            {
                LevelSettings level = levels[i];
                closure(level.Name, i);
            }
        }
    }
}
