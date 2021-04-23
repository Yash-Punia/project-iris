using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VoiceController))]
public class VoiceTest : MonoBehaviour {

    public GameObject infoPanelPrefab;
    public GameObject polyController;
    public GameObject arcam;

    VoiceController voiceController;

    private bool isCreated { get; set; }
    private string action { get; set; }

    public void GetSpeech(string actionToPerform)
    {
        isCreated = false;
        action = actionToPerform;
        voiceController.GetSpeech();
    }

    void Start() => voiceController = GetComponent<VoiceController>();

    void OnEnable() => VoiceController.resultRecieved += OnVoiceResult;

    void OnDisable() => VoiceController.resultRecieved -= OnVoiceResult;

    void OnVoiceResult(string text)
    {
        if(!isCreated)
        {
            isCreated = true;
            if(action.Equals("querySearch"))
            {
                Vector3 camPos = arcam.transform.position;
                Vector3 camDirection = arcam.transform.forward;
                Vector3 spawnPos = camPos + (camDirection * 1f);
                Quaternion camRotation = arcam.transform.rotation;
                GameObject infoPanel = Instantiate(infoPanelPrefab, spawnPos, camRotation);
                infoPanel.GetComponent<GoogleSearch>().SearchQuery(text);
            }
            else if(action.Equals("polySearch"))
            {
                polyController.GetComponent<PolyController>().GetPolyObject(text);
            }
        }
    }
}
