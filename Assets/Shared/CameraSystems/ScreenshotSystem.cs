using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotSystem : MonoBehaviour
{
	#region variables
	public new Camera camera;

	int maxScreenshotIndex = 0;
	string screenshotFolder = "Screenshots";

	public bool loadImages = true;
	#endregion

	#region initialization

	void Awake()
	{

#if UNITY_WEBGL
		//int resWidth = Mathf.Max(Screen.width, 1920);
		//int resHeight = Mathf.Max(Screen.height, 1080);

		// if (PlayerPrefs.HasKey("MaxImages"))
		// {
		// 	maxScreenshotIndex = PlayerPrefs.GetInt("MaxImages");

		// 	for (int i = 0; i < maxScreenshotIndex; i++)
		// 	{
		// 		string name = "Image" + i;
		// 		if (PlayerPrefs.HasKey(name))
		// 		{
		// 			var dataString = PlayerPrefs.GetString(name);
		// 			byte[] bytes = Convert.FromBase64String(dataString);
		// 			LoadImage(bytes, resWidth, resHeight, i);
		// 		}
		// 	}

		// 	Debug.LogError("MI: " + maxScreenshotIndex);
		// }
#else
		CheckFolder();

		var files = Directory.GetFiles(screenshotFolder);

		maxScreenshotIndex = 0;

		foreach (var file in files)
		{
			if (file.EndsWith(".png"))
			{
				var name = Path.GetFileName(file).Replace(".png", "");
				int index = -1;
				try
				{
					index = int.Parse(name.Substring(5));
				}
				catch (Exception e)
				{
					Debug.LogError(e.Message);
					continue;
				}

				if (index == -1)continue;

				if (index + 1 > maxScreenshotIndex)maxScreenshotIndex = index + 1;

				//var bytes = File.ReadAllBytes(file);
				//LoadImage(bytes, resWidth, resHeight, index);
			}
		}
#endif
	}
	#endregion

	#region logic
#if !UNITY_WEBGL
	void LateUpdate()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.P))
			TakeScreenshot(true);
#else

#endif

	}
#endif

	#endregion

	#region public interface

	public void TakeScreenshot(bool transparent)
	{
		int resWidth = Mathf.Max(Screen.width, 1920);
		int resHeight = Mathf.Max(Screen.height, 1080);

		RenderTexture rt = new RenderTexture(resWidth, resHeight, 24, RenderTextureFormat.ARGB32);
		camera.targetTexture = rt;
		Texture2D texture = new Texture2D(resWidth, resHeight, transparent ? TextureFormat.ARGB32 : TextureFormat.RGB24, false);
		camera.Render();

		RenderTexture.active = rt;
		texture.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
		texture.Apply();
		camera.targetTexture = null;
		RenderTexture.active = null;
		Destroy(rt);

		byte[] bytes = texture.EncodeToPNG();

#if UNITY_WEBGL
		/*string name = "Image" + maxScreenshotIndex;
			var bytesString = Convert.ToBase64String(bytes);
			
			PlayerPrefs.SetString(name, bytesString);
			Debug.Log(bytesString);
*/
#else
		string filename = ScreenshotName(maxScreenshotIndex);
		System.IO.File.WriteAllBytes(filename, bytes);
#endif

		maxScreenshotIndex++;

#if UNITY_WEBGL
		//PlayerPrefs.SetInt("MaxImages", maxScreenshotIndex);
#endif
	}

	public static void RemoveAllImages()
	{
#if UNITY_WEBGL
		var maxScreenshotIndex = PlayerPrefs.GetInt("MaxImages");

		for (int i = 0; i < maxScreenshotIndex; i++)
		{
			string name = "Image" + i;
			if (PlayerPrefs.HasKey(name))
				PlayerPrefs.DeleteKey(name);
		}

		PlayerPrefs.DeleteKey("MaxImages");
#endif
	}

	#endregion

	#region private interface

	// private void LoadImage(byte[] bytes, int resWidth, int resHeight, int index)
	// {
	// 	var texture = new Texture2D(resWidth, resHeight);
	// 	if (ImageConversion.LoadImage(texture, bytes))
	// 	{
	// 		var image = new ImageData();
	// 		image.index = index;
	// 		image.texture = texture;
	// 	}
	// }

#if !UNITY_WEBGL
	private string ScreenshotName(int imageIndex)
	{
		return Path.Combine(screenshotFolder, "Image" + imageIndex + ".png");
	}

	private void CheckFolder()
	{
		if (!Directory.Exists(screenshotFolder))
			Directory.CreateDirectory(screenshotFolder);
	}
#endif
	#endregion
}