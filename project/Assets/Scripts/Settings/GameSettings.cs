using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Settings
{
    public class GameSettings : ScriptableObject
    {
        public Sprite StoneFace;
        public Sprite ScissorsFace;
        public Sprite PaperFace;
        public Sprite Shirt;
        
        private static GameSettings _instance;

        [MenuItem("Assets/Create/Game Settings", false, 10000)]
        private static void GetAndSelectSettingsInstance()
        {
            GetSettingsInstance();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _instance;
        }

        private static void GetSettingsInstance()
        {
            if (!AssetDatabase.FindAssets("t:GameSettings").Any(guid =>
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                _instance = AssetDatabase.LoadAssetAtPath<GameSettings>(path);
                return true;
            }))
            {
                _instance = CreateInstance<GameSettings>();
                // TODO: Initialize here

                AssetDatabase.CreateAsset(_instance, "Assets/GameSettings.asset");
                AssetDatabase.SaveAssets();
            }
        }

        public static GameSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    GetSettingsInstance();
                }

                return _instance;
            }
        }
    }
}