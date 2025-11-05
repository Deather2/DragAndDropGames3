using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // CHANGES FOR ANDROID

public class DragAndDropScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGro;
    private RectTransform rectTra;

    public ObjectScript objectScr;
    public ScreenBoundriesScript screenBou;

    private Vector3 dragOffsetWorld;
    private Camera uiCamera;
    private Canvas canvas;

    void Awake()
    {
        canvasGro = GetComponent<CanvasGroup>();
        rectTra = GetComponent<RectTransform>();

        if (objectScr == null)
        {
            objectScr = Object.FindFirstObjectByType<ObjectScript>();
        }

        if (screenBou == null)
        {
            screenBou = Object.FindFirstObjectByType<ScreenBoundriesScript>();
        }

        canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            uiCamera = canvas.worldCamera;
        }
        else
        {
            Debug.LogError("Canvas not found for DragAndDropScript");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ObjectScript.gameEnded)
            return;

        Debug.Log("OnPointerDown");
        objectScr.effects.PlayOneShot(objectScr.audioCli[0]);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ObjectScript.gameEnded)
            return;

        ObjectScript.drag = true;
        canvasGro.blocksRaycasts = false;
        canvasGro.alpha = 0.6f;

        transform.SetSiblingIndex(transform.parent.childCount - 1);

        Vector3 pointerWorld;
        if (ScreenPointToWorld(eventData.position, out pointerWorld))
        {
            dragOffsetWorld = transform.position - pointerWorld;
        }
        else
        {
            dragOffsetWorld = Vector3.zero;
        }

        ObjectScript.lastDragged = eventData.pointerDrag;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ObjectScript.gameEnded)
            return;

        Vector3 pointerWorld;
        if (!ScreenPointToWorld(eventData.position, out pointerWorld))
            return;

        Vector3 desired = pointerWorld + dragOffsetWorld;
        desired.z = rectTra.position.z;

        screenBou.RecalculateBounds();
        Vector2 clamped = screenBou.GetClampedPosition(desired);

        transform.position = new Vector3(clamped.x, clamped.y, desired.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ObjectScript.gameEnded)
            return;

        objectScr.effects.PlayOneShot(objectScr.audioCli[0]);

        ObjectScript.drag = false;
        canvasGro.blocksRaycasts = true;
        canvasGro.alpha = 1.0f;

        if (objectScr.rightPlace)
        {
            canvasGro.blocksRaycasts = false;
            ObjectScript.lastDragged = null;
        }

        objectScr.rightPlace = false;
    }

    private bool ScreenPointToWorld(Vector2 screenPoint, out Vector3 worldPoint)
    {
        worldPoint = Vector3.zero;

        if (uiCamera == null)
            return false;

        float z = Mathf.Abs(uiCamera.transform.position.z - transform.position.z);
        Vector3 sp = new Vector3(screenPoint.x, screenPoint.y, z);

        worldPoint = uiCamera.ScreenToWorldPoint(sp);
        return true;
    }
}