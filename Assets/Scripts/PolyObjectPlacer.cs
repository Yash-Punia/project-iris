using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using DigitalRubyShared;

public class PolyObjectPlacer : MonoBehaviour
{
    private bool isPlaced;
    private ARRaycastManager raycastManager;
    private GameObject objectPlacedEffects;
    private AudioSource audioSource;
    private AudioClip placedSFX;
    private ScaleGestureRecognizer scaleGesture;
    private TapGestureRecognizer tapGesture;
    private Touch touch;
    private Quaternion rotationY;
    private float rotationSpeed;

    void Start()
    {
        rotationSpeed = 0.2f;
        isPlaced = false;
        gameObject.AddComponent<AudioSource>();
        placedSFX = Resources.Load<AudioClip>("ObjectPlaced");
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        raycastManager = FindObjectOfType<ARRaycastManager>();

        CreateTapGesture();
        CreateScaleGesture();
    }
    
    void Update()
    {
        if (!isPlaced)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon);
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = rotationY * transform.rotation;
            }
            HandleSwipe();
        }
    }

    public void SetPlacedEffects(GameObject particleEffects)
    {
        objectPlacedEffects = particleEffects;
    }

    private void CreateScaleGesture()
    {
        scaleGesture = new ScaleGestureRecognizer();
        scaleGesture.StateUpdated += ScaleGestureCallback;
        FingersScript.Instance.AddGesture(scaleGesture);
    }

    private void CreateTapGesture()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.StateUpdated += TapGestureCallback;
        FingersScript.Instance.AddGesture(tapGesture);
    }

    private void ScaleGestureCallback(GestureRecognizer gesture)
    {
        if(gesture.State == GestureRecognizerState.Executing)
        {
            gameObject.transform.localScale *= scaleGesture.ScaleMultiplier;
            objectPlacedEffects.transform.localScale *= scaleGesture.ScaleMultiplier;
        }
    }

    private void TapGestureCallback(GestureRecognizer gesture)
    {
        if(gesture.State == GestureRecognizerState.Ended)
        {
            isPlaced = true;
            audioSource.clip = placedSFX;
            audioSource.Play();
            Instantiate(objectPlacedEffects, transform.position, objectPlacedEffects.transform.rotation);
            FingersScript.Instance.RemoveGesture(scaleGesture);
            FingersScript.Instance.RemoveGesture(tapGesture);
            Destroy(this);
        }
    }
    private void HandleSwipe()
    {
        if(Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            if(touch.phase == UnityEngine.TouchPhase.Moved)
            {
                rotationY = Quaternion.Euler(0f,-touch.deltaPosition.x * rotationSpeed,0f);
            }
            else
            {
                rotationY = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
