#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;

namespace _game.BaseClasses
{
    public static class GameScreenResizer
    {
        static object s_GameViewSizes_instance;

        static Type s_GameViewType;
        static MethodInfo s_GameView_SizeSelectionCallback;

        static Type s_GameViewSizesType;
        static MethodInfo s_GameViewSizes_GetGroup;

        static Type s_GameViewSizeSingleType;

        static GameScreenResizer( )
        {
            s_GameViewType = typeof( UnityEditor.Editor ).Assembly.GetType( "UnityEditor.GameView" );
            s_GameView_SizeSelectionCallback = s_GameViewType.GetMethod( "SizeSelectionCallback", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );

            // gameViewSizesInstance  = ScriptableSingleton<GameViewSizes>.instance;
            s_GameViewSizesType = typeof( UnityEditor.Editor ).Assembly.GetType( "UnityEditor.GameViewSizes" );
            s_GameViewSizeSingleType = typeof( ScriptableSingleton<> ).MakeGenericType( s_GameViewSizesType );
            s_GameViewSizes_GetGroup = s_GameViewSizesType.GetMethod( "GetGroup" );

            var instanceProp = s_GameViewSizeSingleType.GetProperty("instance");
            s_GameViewSizes_instance = instanceProp.GetValue( null, null );
        }
        
        public static bool TrySetSize( string sizeText )
        {
            GameViewSizeGroupType currentGroup = GetCurrentGroupType( );
            int foundIndex = FindSize( currentGroup, sizeText );
            if( foundIndex < 0 )
            {
                var data = sizeText.Split("x");
                AddCustomSize(int.Parse(data[0]), int.Parse(data[1]));
                TrySetSize(sizeText);
                return false;
            }

            SetSizeIndex( foundIndex );
            return true;
        }
        
        public static void AddCustomSize(int width, int height)
        {
            var group = GetGroup(GetCurrentGroupType());
            var addCustomSize = s_GameViewSizes_GetGroup.ReturnType.GetMethod("AddCustomSize"); // or group.GetType().
            string assemblyName = "UnityEditor.dll";
            Assembly assembly = Assembly.Load(assemblyName);
            Type gameViewSize = assembly.GetType("UnityEditor.GameViewSize");
            Type gameViewSizeType = assembly.GetType("UnityEditor.GameViewSizeType");
            ConstructorInfo ctor = gameViewSize.GetConstructor(new Type[]
            {
                gameViewSizeType,
                typeof(int),
                typeof(int),
                typeof(string)
            });
            var newSize = ctor.Invoke(new object[] { 1, width, height, $"{width}x{height}" });
            addCustomSize.Invoke(group, new object[] { newSize });
        }
        
        public static void SetSizeIndex( int index )
        {
            EditorWindow currentWindow = EditorWindow.focusedWindow;
            SceneView lastSceneView = SceneView.lastActiveSceneView;

            EditorWindow gv = EditorWindow.GetWindow( s_GameViewType );
            s_GameView_SizeSelectionCallback.Invoke( gv, new object[] { index, null } );

            
            if( lastSceneView != null )
                lastSceneView.Focus( );

            if( currentWindow != null )
                currentWindow.Focus( );
        }
        
        public static int FindSize( GameViewSizeGroupType sizeGroupType, string text )
        {
            var group = GetGroup(sizeGroupType); // class GameViewSizeGroup
            var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
            var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
            for( int i = 0; i < displayTexts.Length; i++ )
            {
                string display = displayTexts[i];

                bool found = display.Contains( text );
                if( found )
                    return i;
            }
            return -1;
        }

        static object GetGroup( GameViewSizeGroupType type )
        {
            return s_GameViewSizes_GetGroup.Invoke( s_GameViewSizes_instance, new object[] { (int)type } );
        }

        public static GameViewSizeGroupType GetCurrentGroupType( )
        {
#if UNITY_STANDALONE
            return GameViewSizeGroupType.Standalone;
#elif UNITY_IOS
            return GameViewSizeGroupType.iOS;
#elif UNITY_ANDROID
            return GameViewSizeGroupType.Android;
#endif
        }

    }
}
#endif