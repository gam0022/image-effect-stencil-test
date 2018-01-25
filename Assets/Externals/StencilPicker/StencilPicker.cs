using UnityEngine;

// https://qiita.com/kodai100/items/fff5983f2b52f2ba5cff
public class StencilPicker : MonoBehaviour {

  [SerializeField] private Shader shader;

  private Material material;

  void Awake() {
    if (material == null) {
      material = new Material(shader);
    }
  }

  [ImageEffectOpaque] // !!!!!!!!!!!!!!
  void OnRenderImage(RenderTexture source, RenderTexture destination) {
      Graphics.Blit(source, destination, material);
  }
}
