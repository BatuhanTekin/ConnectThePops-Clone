#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
namespace _game.PostProcessors
{
    public class TextureAssetImportPostProcessor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            var textureImporter = (TextureImporter) assetImporter;
            if (assetImporter.assetPath.Contains("/Ui"))
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.textureShape = TextureImporterShape.Texture2D;
            }
        }
    }
}

#endif
