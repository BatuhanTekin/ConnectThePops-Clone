using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public struct GridSaver
    {
        private const string GridSave = "grid_save";
        private static GridSaveData _saveData;


        public static void Save(GridController[,] array, int size = 5)
        {
            _saveData = new GridSaveData();
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    _saveData.list.Add(array[y, x].GetLevel());
                }
            }
            
            PlayerPrefs.SetString(GridSave, JsonUtility.ToJson(_saveData));
            PlayerPrefs.Save();
        }

        public static bool GetSave(out GridSaveData save)
        {
            save = JsonUtility.FromJson<GridSaveData>(GetPref());

            return !GetPref().Equals("");
        }

        private static string GetPref()
        {
            return PlayerPrefs.GetString(GridSave, "");
        }
    }
}