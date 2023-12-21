using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Event.UI;
using Scripts.UI.Popup;
using Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Manga_Popup : Popup
{
    private enum GameObjects
    {
        Back
    }

    private Transform _transform;
    private List<Sprite> _manga = new();
    public bool MangaOpen { get; set; } = false;

    private void Start()
    {
        Initialized();
    }

    protected override bool Initialized()
    {
        if (!base.Initialized()) return false;
        SetObject(typeof(GameObjects));
        _transform = GetObject((int)GameObjects.Back).transform;
        Manga();
        MangaOpen = true;
        StartCoroutine(PlayManga());
        return true;
    }

    private void Manga()
    {
        for (int i = 0; i < 9; i++)
        {
            var sprite = Main.Resource.Load<Sprite>($"f4Manga.atlas[{i}]");
            _manga.Add(sprite);
        }
    }

    private IEnumerator PlayManga()
    {
        var currentDisplayedCount = 0;
        foreach (var sprite in _manga)
        {
            GameObject imageObject = Main.Resource.InstantiatePrefab($"Index{currentDisplayedCount + 1}.prefab", _transform);
            Image imageComponent = SceneUtility.GetAddComponent<Image>(imageObject);
            imageComponent.sprite = sprite;
            currentDisplayedCount++;
            if (currentDisplayedCount != 3) continue;
            yield return new WaitForSecondsRealtime(4f); // Adjust the time as needed
            foreach (Transform child in _transform) Destroy(child.gameObject);
            currentDisplayedCount = 0;
        }
        if (currentDisplayedCount > 0)
        {
            yield return new WaitForSecondsRealtime(4f);
            foreach (Transform child in _transform) Destroy(child.gameObject);
        }
        MangaOpen = false;
        Close();
    }

    private void Close()
    {
        List<UIEventType> eventTypes = new List<UIEventType> { UIEventType.Click };
        Main.UI.ClosePopUp(this, eventTypes);
        Destroy(gameObject);
    }
}
