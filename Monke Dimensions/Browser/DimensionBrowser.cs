#if EDITOR

#else
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using UnityEngine.UI;

namespace Monke_Dimensions.Browser;

public class DimensionBrowser : MonoBehaviour
{
    public static DimensionBrowser instance;
    public static bool inBrowser;

    private List<List<Item.DimensionItemData>> dimensionPages = new List<List<Item.DimensionItemData>>();
    private int currentPageIndex = 0;

    private Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D>();

    private async void Start()
    {
        instance = this;
        await LoadDimensionsAsync();
    }

    public async void OnBrowserEnabled()
    {
        await LoadDimensionsAsync();
        ShowCurrentPage();
    }

    public void UnloadBrowser()
    {
        HideAllPages();
        dimensionPages.Clear();
        imageCache.Clear();
        currentPageIndex = 0;
        inBrowser = false;
    }

    private async Task LoadDimensionsAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync("https://chin0303.github.io/DimensionBrowser/List.json");

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            string json = await response.Content.ReadAsStringAsync();
            Item.DimensionItemDataWrapper dataWrapper = JsonConvert.DeserializeObject<Item.DimensionItemDataWrapper>(json);

            List<Item.DimensionItemData> dimensionItems = dataWrapper.Dimensions;

            dimensionPages.Clear();

            List<Item.DimensionItemData> currentPage = new List<Item.DimensionItemData>();
            foreach (Item.DimensionItemData item in dimensionItems)
            {
                if (currentPage.Count >= 6)
                {
                    dimensionPages.Add(currentPage);
                    currentPage = new List<Item.DimensionItemData>();
                }
                currentPage.Add(item);
            }
            if (currentPage.Count > 0)
            {
                dimensionPages.Add(currentPage);
            }
        }
    }

    private void ShowCurrentPage()
    {
        if (currentPageIndex >= 0 && currentPageIndex < dimensionPages.Count)
        {
            HideAllPages();

            List<Item.DimensionItemData> currentPage = dimensionPages[currentPageIndex];

            GameObject pageObject;

            if (transform.childCount > currentPageIndex)
            {
                pageObject = transform.GetChild(currentPageIndex).gameObject;
            }
            else
            {
                pageObject = Instantiate(Main.assetBundle.LoadAsset<GameObject>("PagePanel"), transform);
            }

            pageObject.SetActive(true);

            Transform itemContainer = pageObject.transform;

            foreach (Transform child in itemContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < currentPage.Count; i++)
            {
                Item.DimensionItemData currentItem = currentPage[i];
                GameObject itemObject = Instantiate(Main.assetBundle.LoadAsset<GameObject>("Item"));
                itemObject.transform.SetParent(itemContainer, false);

                RectTransform rectTransform = itemObject.GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0);
                rectTransform.localRotation = Quaternion.identity;
                rectTransform.localScale = new Vector3(1.17f, 0.425f, 1f);

                Item itemComponent = itemObject.AddComponent<Item>();
                itemComponent.MapName = currentItem.Name;
                itemComponent.MapDownload = currentItem.Download;
                itemComponent.MapImageUrl = currentItem.Image;

                RawImage rawImage = itemObject.GetComponent<RawImage>();
                StartCoroutine(LoadImage(currentItem.Image, rawImage));
            }
        }
    }

    private void HideAllPages()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private IEnumerator LoadImage(string url, RawImage rawImage)
    {
        if (imageCache.TryGetValue(url, out Texture2D cachedTexture))
        {
            rawImage.texture = cachedTexture;
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                rawImage.texture = texture;

                imageCache[url] = texture;
            }
        }
    }

    public void NextPage()
    {
        if (currentPageIndex < dimensionPages.Count - 1)
        {
            currentPageIndex++;
            ShowCurrentPage();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowCurrentPage();
        }
    }
}
#endif