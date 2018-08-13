using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using CameraShot;
using System.IO;
using VoxelBusters.NativePlugins;
using NatShareU;

public class SelfieHandler : MonoBehaviour {

    public RawImage imageTexture;
    public Texture2D myTexture;
    public string imagePath;
    public string log;
    public string tempFile;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {

        CameraShotEventListener.onImageSaved += OnImageSaved;
        CameraShotEventListener.onImageLoad += OnImageLoad;
        CameraShotEventListener.onError += OnError;
        CameraShotEventListener.onCancel += OnCancel;
    }
    void OnDisable()
    {
        CameraShotEventListener.onImageSaved -= OnImageSaved;
        CameraShotEventListener.onImageLoad -= OnImageLoad;
        CameraShotEventListener.onError -= OnError;
        CameraShotEventListener.onCancel -= OnCancel;
    }

    void OnImageSaved(string path, ImageOrientation orientation)
    {
        imagePath = path;
        byte[] fileData = File.ReadAllBytes(imagePath);
        Texture2D tex= new Texture2D(2, 2);
        tex.LoadImage(fileData);
        Debug.Log("orientation" + orientation);

        Texture2D rotated = null;
        switch (orientation)
        {
            case ImageOrientation.UP:
                rotated = tex;
               break;
            case ImageOrientation.LEFT:
                rotated = rotateTexture(tex, false);
                break;
            case ImageOrientation.RIGHT:
                rotated = rotateTexture(tex, true);
                //rotated = rotateTexture(rotated, false);
                break;
            case ImageOrientation.DOWN:
                rotated = rotateTexture(tex, false);
                rotated = rotateTexture(rotated, false);
                break;
            default:
                rotated = tex;
                break;
        }
        tex = null;
        myTexture = ScaleTexture(rotated, (int)(rotated.width * 0.5), (int)(rotated.height * 0.5));

        NatShare.Share(myTexture, "We solved the mystery! #riddleMiaThis #mia");

    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }



    Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    void OnImageLoad(string path, Texture2D tex, ImageOrientation orientation)
    {
        //imageTexture.texture = tex;
        //byte[] bytes = tex.EncodeToPNG();
        //tempFile = Application.persistentDataPath + "SavedScreen.png";
        //File.WriteAllBytes(tempFile, bytes);
        //GameObject.Find("Cube").GetComponent<Renderer>().material.mainTexture = tex;
        //log += "\nImage Saved to gallery, loaded :" + path + ", orientation : " + orientation;
    }

    void OnError(string errorMsg)
    {
        Debug.Log("Error : " + errorMsg);
        log += "\nError : " + errorMsg;
    }

    void OnCancel()
    {
        Debug.Log("OnCancel");
        log += "\nOnCancel";
    }


    public void snapSelfie() {
        #if UNITY_ANDROID
            AndroidCameraShot.GetTexture2DFromCamera(false, true); // don't crop, save in gallery
        #elif UNITY_IPHONE
            IOSCameraShot.GetTexture2DFromCamera(false, true);   // don't crop, save in gallery
        #endif
    }

    public void shareSelf() {
        //SocialShareSheet _shareSheet = new SocialShareSheet();
        //_shareSheet.Text = "We solved the mystery! #riddleMiaThis #mia";
        //_shareSheet.AttachImageAtPath(tempFile);
        //NPBinding.UI.SetPopoverPointAtLastTouchPosition(); // To show popover at last touch point on iOS. On Android, its ignored.
        //NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
        //Texture2D myText = imageTexture.texture as Texture2D;
        //NatShare.Share(myText, "We solved the mystery! #riddleMiaThis #mia");

    }

    void FinishedSharing(eShareResult _result)
    {
        Debug.Log("Finished sharing");
        Debug.Log("Share Result = " + _result);
    }
}
