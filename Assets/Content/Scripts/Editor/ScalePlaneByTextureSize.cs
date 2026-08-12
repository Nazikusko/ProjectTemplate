using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
public class ScalePlaneByTextureSize : MonoBehaviour
{
    [SerializeField, Range(0.1f, 2)] float _basePlaneSize = 4.674462f;
    [SerializeField] bool _autoSetOnEnable;

    private Texture2D _texture;

    [Button]
    void SetScale()
    {
        var texture = GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

        if (texture == null)
        {
            Debug.LogError("No texture found on the material!");
            return;
        }
        
        float texWidth = texture.width;
        float texHeight = texture.height;

        float aspect = texWidth / texHeight;
        float worldWidth = aspect;

        if (_autoSetOnEnable)
        {
            _basePlaneSize = texHeight / 225;
        }

        Vector3 newScale = new Vector3(
            worldWidth * _basePlaneSize,
            _basePlaneSize,
            1
        );

        transform.localScale = newScale;
    }
}

