using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlaceScript : MonoBehaviour, IDropHandler
{
    private float placeZRot;
    private float vehicleZRot;
    private float rotDiff;
    private Vector3 placeSiz;
    private Vector3 vehicleSiz;
    private float xSizeDiff;
    private float ySizeDiff;

    public ObjectScript objScript;

    void Start()
    {
        if (objScript == null)
        {
            objScript = Object.FindFirstObjectByType<ObjectScript>();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        var draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();


        // Проверяем совпадают ли теги (машина на своей тени или на чужой)
        if (eventData.pointerDrag.tag.Equals(tag))
        {
            // МАШИНА НА СВОЕЙ ТЕНИ - проверяем точность
            placeZRot = eventData.pointerDrag.GetComponent<RectTransform>().transform.eulerAngles.z;
            vehicleZRot = GetComponent<RectTransform>().transform.eulerAngles.z;
            rotDiff = Mathf.Abs(placeZRot - vehicleZRot);
            Debug.Log("Rotation difference: " + rotDiff);

            placeSiz = eventData.pointerDrag.GetComponent<RectTransform>().localScale;
            vehicleSiz = GetComponent<RectTransform>().localScale;
            xSizeDiff = Mathf.Abs(placeSiz.x - vehicleSiz.x);
            ySizeDiff = Mathf.Abs(placeSiz.y - vehicleSiz.y);
            Debug.Log("X size difference: " + xSizeDiff);
            Debug.Log("Y size difference: " + ySizeDiff);

            // Если ротация И масштаб совпадают - ПРАВИЛЬНАЯ установка
            if ((rotDiff <= 5 || (rotDiff >= 355 && rotDiff <= 360)) && (xSizeDiff <= 0.1f && ySizeDiff <= 0.1f))
            {
                Debug.Log("Correct place - perfect match!");
                objScript.rightPlace = true;

                var dragRect = eventData.pointerDrag.GetComponent<RectTransform>();
                var placeRect = GetComponent<RectTransform>();

                // Идеально выравниваем машину с тенью
                dragRect.anchoredPosition = placeRect.anchoredPosition;
                dragRect.localRotation = placeRect.localRotation;
                dragRect.localScale = placeRect.localScale;

                int index = GetVehicleIndex(eventData.pointerDrag.tag);
                if (index != -1)
                {
                    objScript.PlaceVehicle(index);
                }

                // Играем звук ПРАВИЛЬНОЙ установки
                PlayCorrectSound(eventData.pointerDrag.tag);
            }
            else
            {
                // Машина на своей тени, но ротация/масштаб не совпадают
                // Машина остается где упала, НО не засчитывается как правильно установленная
                Debug.Log("On correct shadow but rotation/scale mismatch - stays but not completed");
                objScript.rightPlace = false;
                // Звука НЕТ - машина просто остается где есть
            }
        }
        else
        {
            // МАШИНА НА ЧУЖОЙ ТЕНИ (теги не совпадают)
            Debug.Log("Wrong shadow - returning to start");
            objScript.rightPlace = false;

            // Играем звук ОШИБКИ
            objScript.effects.PlayOneShot(objScript.audioCli[1]);

            // Возвращаем машину на стартовые координаты
            int index = System.Array.IndexOf(objScript.vehicles, eventData.pointerDrag);
            if (index != -1)
            {
                draggedRect.localPosition = objScript.startCoordinates[index];
            }
        }
    }

    private void PlayCorrectSound(string tag)
    {
        switch (tag)
        {
            case "Garbage":
                objScript.effects.PlayOneShot(objScript.audioCli[2]);
                break;
            case "Medicine":
                objScript.effects.PlayOneShot(objScript.audioCli[3]);
                break;
            case "Fire":
                objScript.effects.PlayOneShot(objScript.audioCli[4]);
                break;
            case "Bus":
                objScript.effects.PlayOneShot(objScript.audioCli[5]);
                break;
            case "b2":
                objScript.effects.PlayOneShot(objScript.audioCli[6]);
                break;
            case "Cement":
                objScript.effects.PlayOneShot(objScript.audioCli[7]);
                break;
            case "E46":
                objScript.effects.PlayOneShot(objScript.audioCli[8]);
                break;
            case "E61":
                objScript.effects.PlayOneShot(objScript.audioCli[9]);
                break;
            case "Excavator":
                objScript.effects.PlayOneShot(objScript.audioCli[10]);
                break;
            case "Police":
                objScript.effects.PlayOneShot(objScript.audioCli[11]);
                break;
            case "Tractor1":
                objScript.effects.PlayOneShot(objScript.audioCli[12]);
                break;
            case "Tractor2":
                objScript.effects.PlayOneShot(objScript.audioCli[13]);
                break;
            default:
                Debug.Log("Unknown tag detected");
                break;
        }
    }

    private int GetVehicleIndex(string tag)
    {
        switch (tag)
        {
            case "Garbage": return 0;
            case "Medicine": return 1;
            case "Fire": return 2;
            case "Bus": return 3;
            case "b2": return 4;
            case "Cement": return 5;
            case "E46": return 6;
            case "E61": return 7;
            case "Excavator": return 8;
            case "Police": return 9;
            case "Tractor1": return 10;
            case "Tractor2": return 11;
            default:
                Debug.Log("Unknown tag for index");
                return -1;
        }
    }
}