using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
    private List<Sprite> _spritesCollection = new List<Sprite>();

    IEnumerator LoadJson(string jsonUrl, Action<List<Sprite>> onComplete = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(jsonUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ImageList imageList = JsonUtility.FromJson<ImageList>(request.downloadHandler.text);
                StartCoroutine(LoadImages(imageList.images, onComplete));
            }
            else
            {
                Debug.LogError("Failed to load JSON: " + request.error);
            }
        }
    }

    IEnumerator LoadImages(List<ImageData> images, Action<List<Sprite>> onComplete = null)
    {
        foreach (var imageData in images)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageData.url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    _spritesCollection.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
                }
                else
                {
                    Debug.LogError("Failed to load image: " + request.error);
                }
            }
        }

        onComplete?.Invoke(_spritesCollection);
    }

    public void LoadImages(string jsonUrl, Action<List<Sprite>> onComplete = null)
    {
        StartCoroutine(LoadJson(jsonUrl, onComplete));
    }
}

[Serializable]
public class ImageData
{
    public string url;
}

[Serializable]
public class ImageList
{
    public List<ImageData> images;
}