using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using PolyToolkit;
using TMPro;

public class PolyController : MonoBehaviour
{
    public Iris iris;
    public GameObject objectPlacedEffectsPrefab;
    public TextMeshProUGUI statusText;
    public GameObject scrollView;
    public GameObject scrollContent;
    public GameObject scrollItemPrefab;

    private void Start()
    {
        scrollView.SetActive(false);
    }

    public void GetPolyObject(string objName)
    {
        statusText.text = "GettingPolyObject";
        iris.SetMovement(true);
        foreach(Transform child in scrollContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        PolyListAssetsRequest req = new PolyListAssetsRequest
        {
            curated = true,
            keywords = objName,
            maxComplexity = PolyMaxComplexityFilter.MEDIUM,
            orderBy = PolyOrderBy.BEST,
            pageSize = 10
        };
        PolyApi.ListAssets(req, ImportPolyObject);
        statusText.text = "ListingAssets...";
    }


    private void ImportPolyObject(PolyStatusOr<PolyListAssetsResult> result)
    {
        if (!result.Ok)
        {
            statusText.text = "ImportPolyObject Failed";
            return;
        }
        else
        {
            statusText.text = "ImportPolyObject Success!";
            foreach (PolyAsset asset in result.Value.assets)
            {
                PolyApi.FetchThumbnail(asset, GenerateItem);
            }
            scrollView.SetActive(true);
        }
    }

    private void GenerateItem(PolyAsset asset, PolyStatus status)
    {
        if(!status.ok)
        {
            statusText.text = "SetThumbnail Failed";
            return;
        }
        GameObject scrollItemObj = Instantiate(scrollItemPrefab);
        scrollItemObj.transform.SetParent(scrollContent.transform, false);
        scrollItemObj.transform.Find("Thumbnail").gameObject.GetComponent<RawImage>().texture = asset.thumbnailTexture;
        scrollItemObj.transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = asset.authorName;
        scrollItemObj.GetComponent<ScrollItemScript>().asset = asset;
    }
}
