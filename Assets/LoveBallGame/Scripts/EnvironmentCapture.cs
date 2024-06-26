using UnityEngine;

public class EnvironmentCapture : MonoBehaviour
{
    public RenderTexture renderedTexture;
    public SpriteRenderer renderedTextureImg;
    private void OnLevelCompleteScreenClosed()
    {
        Invoke(nameof(CaptureSnap), 1);
    }

    private void CaptureSnap()
    {
        GameController.Instance.StorySnapingImg = renderedTextureImg.transform; 
        ExportPhoto();
        gameObject.SetActive(false);
    }

    public void ExportPhoto()
    {
        renderedTextureImg.sprite = GetTextureFromRendTexture(renderedTexture);
    }

    public Sprite GetTextureFromRendTexture(RenderTexture rT)
    {
        Texture2D texture2D = new Texture2D(1080, 1080, TextureFormat.ARGB32, false);
        RenderTexture.active = rT;
        texture2D.ReadPixels(new Rect(0, 0, rT.width, rT.height), 0, 0);
        texture2D.Apply();

        // Convert Texture2D to Sprite
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }
}
