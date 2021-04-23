using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;

public class GoogleSearch : MonoBehaviour
{
    public Iris iris;

    private string query, info;
    private Texture displayImageTexture;

    [SerializeField]
    private GameObject imageScreen;

    public void SearchQuery(string text)
    {
        iris.SetMovement(true);
        query = text;
        query.Replace(" ", "+");
        StartCoroutine(GetInfoRequest());
        StartCoroutine(GetImageRequest());
    }

    IEnumerator GetInfoRequest()
    {
        string url = "https://www.googleapis.com/customsearch/v1?key=AIzaSyA3mQqdwbT5_D31xIVbqdAWsxlwFgkplMQ&cx=011621024514007583474:ualbbp4czwi&q=" + query + "+Wikipedia";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jo = JObject.Parse(request.downloadHandler.text);
            info = jo["items"][0]["snippet"].ToString();
            info = info.Split('.')[0];
            gameObject.GetComponentInChildren<TextMeshPro>().text = info;

        }
    }

    IEnumerator GetImageRequest()
    {
        string url = "https://www.googleapis.com/customsearch/v1?key=AIzaSyA3mQqdwbT5_D31xIVbqdAWsxlwFgkplMQ&cx=011621024514007583474:ualbbp4czwi&searchType=image&q=" + query;
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if(request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jo = JObject.Parse(request.downloadHandler.text);
            string imgUrl = jo["items"][0]["image"]["thumbnailLink"].ToString();
            StartCoroutine(ProcessImageUrls(imgUrl));
        }
    }

    IEnumerator ProcessImageUrls(string url)
    {
        UnityWebRequest imgRequest = UnityWebRequestTexture.GetTexture(url);
        yield return imgRequest.SendWebRequest();
        displayImageTexture = DownloadHandlerTexture.GetContent(imgRequest);
        imageScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", displayImageTexture);
        float scale = displayImageTexture.width / displayImageTexture.height;
        imageScreen.transform.localScale = new Vector3(0.5f,scale*0.5f,1f);
    }
}
