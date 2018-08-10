
using UnityEngine;

namespace CameraShot
{

	public class AndroidCameraShot
	{
		#if UNITY_ANDROID
		static AndroidJavaClass _plugin;

		static AndroidCameraShot()
		{
			_plugin = new AndroidJavaClass("com.astricstore.camerashots.CameraShots");
		}

		public static void LaunchCameraForImageCapture(bool cropping = false, bool save = true)
		{
			CameraShot.mode = 0;
			LaunchCameraForImage (cropping, 1, 1, save);
		}

		public static void LaunchCameraForImageCapture(bool cropping, int aspectX=1, int aspectY=1,bool save = true)
		{
			CameraShot.mode = 0;
			LaunchCameraForImage (cropping,aspectX,aspectY,save);
		}
		
		public static void GetTexture2DFromCamera(bool cropping = false, bool save = true)
		{
			CameraShot.mode = 1;
			LaunchCameraForImage (cropping, 1, 1, save);
		}

		public static void GetTexture2DFromCamera(bool cropping, int aspectX=1, int aspectY=1, bool save = true)
		{
			CameraShot.mode = 1;
			LaunchCameraForImage (cropping,aspectX,aspectY,save);
		}
		

		// for video
		public static void LaunchCameraForVideoCapture(int maxDuration = 0, bool save = true)
		{
			LaunchCameraForVideo(maxDuration,save);
		}


		private static void LaunchCameraForImage(bool cropping, int aspectX=1, int aspectY=1, bool save = true)
		{
			_plugin.CallStatic("launchCameraForImageCapture",cropping,aspectX,aspectY,save);

		}

		private static void LaunchCameraForVideo(int maxDuration, bool save)
		{
			_plugin.CallStatic("launchCameraForVideoCapture",maxDuration,save);

		}
#endif
	}
}


