#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;

namespace Settings
{
    public class GameSettings : ScriptableObject
    {
        /// <summary>
        /// Скин карты Камень.
        /// </summary>
        public Sprite StoneFace;
        
        /// <summary>
        /// Скин карты Ножницы.
        /// </summary>
        public Sprite ScissorsFace;
        
        /// <summary>
        /// Скин карты Бумага.
        /// </summary>
        public Sprite PaperFace;
        
        /// <summary>
        /// Скин рубашки карты.
        /// </summary>
        public Sprite Shirt;

        
        // Пресеты для игровых настроек.
        
        /// <summary>
        /// Флаг честная/нечестная игра.
        /// </summary>
        public bool Cheeting;
        
        /// <summary>
        /// Процент удачи при нечестной игре.
        /// </summary>
        [Range(0f, 1f)] public float LuckPercent = 0.5f;

        
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Game Settings", false, 10000)]
        private static void GetAndSelectSettingsInstance()
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = GetSettingsInstance();
        }

        public static GameSettings GetSettingsInstance()
        {
            GameSettings instance = null;
            if (!AssetDatabase.FindAssets("t:GameSettings").Any(guid =>
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                instance = AssetDatabase.LoadAssetAtPath<GameSettings>(path);
                return true;
            }))
            {
                instance = CreateInstance<GameSettings>();
                // TODO: Initialize here

                AssetDatabase.CreateAsset(instance, "Assets/GameSettings.asset");
                AssetDatabase.SaveAssets();
            }

            return instance;
        }
#endif
    }
}