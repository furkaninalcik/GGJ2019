﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIController : MonoBehaviour
{
    private AndroidUtils androidUtils;
    [SerializeField] private GameObject startRecordbtn, stopRecordBtn, galleryBtn;
    private void Start()
    {
        /*if (!AndroidUtils.IsPermitted(AndroidPermission.ACCESS_FINE_LOCATION))//test request permission
            AndroidUtils.RequestPermission(AndroidPermission.ACCESS_FINE_LOCATION);*/
        androidUtils = FindObjectOfType<AndroidUtils>();
    }
    public void OnClickStartRecord()
    {
        startRecordbtn.SetActive(false);
        galleryBtn.SetActive(false);
        stopRecordBtn.SetActive(false);
        androidUtils.PrepareRecorder();
        StartCoroutine(DelayCallRecord());
        Invoke("OnClickStopRecord", 6.00f);
    }
    private IEnumerator DelayCallRecord()
    {
        yield return new WaitForSeconds(0.1f);
        androidUtils.StartRecording();
    }
    public void OnClickStopRecord()
    {
        startRecordbtn.SetActive(true);
        stopRecordBtn.SetActive(false);
        galleryBtn.SetActive(true);
        androidUtils.StopRecording();
        StartCoroutine(DelaySaveVideo());
    }
    private IEnumerator DelaySaveVideo()
    {
        yield return new WaitForSeconds(1);
        androidUtils.SaveVideoToGallery();
    }
    public void OnClickOpenGallery()
    {
        AndroidUtils.OpenGallery();
    }
}
