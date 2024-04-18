using UnityEditor;
using UnityEngine;

namespace Extensions.TUTORIAL
{
#if UNITY_EDITOR
    class TutorialEditor : EditorWindow {
        
        int step = 0;
        
        [MenuItem ("ValhallaVerse/Tutorial Editor")]
        public static void ShowWindow () {
            GetWindow(typeof(TutorialEditor));
        }
    
        void OnGUI () {
            GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
            step = EditorGUILayout.IntField("Step", step);
            
            if (GUILayout.Button("Set Step"))
            {
                TutorialManager.TutorialStep = step;
            }
        }
    }
#endif

    
    public static class TutorialManager
    {
        public static bool ShowTutorial => PlayerPrefs.GetInt("showTutorial", 1) == 1;
        private static int _collectedHealthCount = 0;

        public static int CollectedHealthCount
        {
            get => _collectedHealthCount;
            set
            {
                _collectedHealthCount = value;
                if (_collectedHealthCount >= 5)
                {
                    // Next phase...
                    PracticeTutorialManager.Instance.OnCollectedEveryHealth();
                }
            }
        }

        // 0 = nothing
        // 1 = practicing
        // 2 = menu
        // 3 = end
        public static int TutorialStep
        {
            get => PlayerPrefs.GetInt("TutorialStep", 0);
            set
            {
                PlayerPrefs.SetInt("TutorialStep", value);
                if (value > 2)
                {
                    PlayerPrefs.SetInt("showTutorial", 0);
                }
            }

        }

        public static void OnEndReadingPlayerData()
        {
            if (ShowTutorial && TutorialStep < 2)
            {
                OpenPracticeScene();
            }
        }
        
        //First phase of the tutorial
        private static void OpenPracticeScene()
        {
            OfflineUIManager.Instance.OpenPracticeScene();
            TutorialStep = 1;
        }
    }
}