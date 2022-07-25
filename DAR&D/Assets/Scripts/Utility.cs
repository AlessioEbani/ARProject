using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Rewired.Dev;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;

#endif

public static class Utility {


    public static string GetClockText(float secondsElapsed,bool withZero=false) {
        string minutesText="";
        int minutes = Mathf.FloorToInt(secondsElapsed / 60);
        if (minutes < 10) {
            if(withZero)
                minutesText += "0";
        }
        minutesText += minutes.ToString();
        string secondsText = "";
        int seconds = Mathf.FloorToInt(secondsElapsed % 60);
        if (seconds < 10) {
            if(withZero)
                secondsText += "0";
        }
        secondsText += seconds.ToString();
        string millisecondsText = "";
        int milliseconds = Mathf.FloorToInt((secondsElapsed % 60 - seconds)*1000);
        if (milliseconds < 100) {
            if(withZero)
                millisecondsText += "0";
        }
        if (milliseconds < 10) {
            if(withZero)
                millisecondsText += "0";
        }
        millisecondsText += milliseconds.ToString();
        return $"{minutesText}:{secondsText}:{millisecondsText}";
    }
#if UNITY_EDITOR
	public static void UnpackPrefab(GameObject gameObject) {
		if (PrefabUtility.IsPartOfPrefabInstance(gameObject)) {
			PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
		}
	}

	public static T[] GetAllInstances<T>(string path) where T : ScriptableObject {
		string[] files = Directory.GetFiles(path, "*.asset");
		var a = new T[files.Length];
		for (int i = 0; i < files.Length; i++) {
			var asset = AssetDatabase.LoadAssetAtPath<T>(files[i]);
			a[i] = asset;
		}
		return a;
	}

	public static T[] GetAllInstances<T>() where T : ScriptableObject {
		string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
		var a = new T[guids.Length];
		for (int i = 0; i < guids.Length; i++) {
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
		}
		return a;
	}

	public static Sprite GetSpriteFromSheet(string path, string name) {
		Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
		return Array.Find(sprites, s => s.name == name);
	}

	public static T[] GetAtPath<T>(string path) {
		ArrayList al = new ArrayList();
		string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
		foreach (string fileName in fileEntries) {
			int index = fileName.LastIndexOf("/");
			string localPath = "Assets/" + path;

			if (index > 0)
				localPath += fileName.Substring(index);

			Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

			if (t != null)
				al.Add(t);
		}
		T[] result = new T[al.Count];
		for (int i = 0; i < al.Count; i++)
			result[i] = (T)al[i];

		return result;
	}

	public static T CreateAsset<T>(string path, string name) where T : ScriptableObject {
		var asset = ScriptableObject.CreateInstance<T>();
		AssetDatabase.CreateAsset(asset, $"{path}/{name}.asset");
		EditorUtility.SetDirty(asset);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		return asset;
	}

	public static SpriteAtlas CreateSpriteAtlas(string path) {
		var atlas = new SpriteAtlas();
		var textureSettings = new SpriteAtlasTextureSettings
		{
			filterMode = FilterMode.Point,
			sRGB = true
		};
		var platformSettings = new TextureImporterPlatformSettings
		{
			compressionQuality = 0,
			textureCompression = TextureImporterCompression.Uncompressed,
			crunchedCompression = true,
			maxTextureSize = 4096
		};
		atlas.SetPlatformSettings(platformSettings);
		atlas.SetTextureSettings(textureSettings);
		AssetDatabase.CreateAsset(atlas, path);
		return atlas;
	}

	public static string GetFolderPath(string assetPath) {
		List<string> splits = assetPath.Split('/').ToList();
		splits.RemoveAt(splits.Count - 1);
		return string.Join("/", splits);
	}

	public static string GetFolderPath<T>(T asset) where T : ScriptableObject {
		string assetPath = AssetDatabase.GetAssetPath(asset);
		return GetFolderPath(assetPath);
	}

	public static T CreateOrLoad<T>(string path, T asset) where T : ScriptableObject {
		var n = AssetDatabase.LoadAssetAtPath<T>(path);
		if (n == null) {
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();
			n = AssetDatabase.LoadAssetAtPath<T>(path);
		}
		n.Dirt();
		return n;
	}

	public static T CreateOrLoad<T>(string path) where T : ScriptableObject {
		var n = AssetDatabase.LoadAssetAtPath<T>(path);
		if (n == null) {
			n = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(n, path);
			AssetDatabase.SaveAssets();
			n = AssetDatabase.LoadAssetAtPath<T>(path);
		}
		n.Dirt();
		return n;
	}

	public static bool DeleteAsset<T>(T asset) where T : ScriptableObject {
		string path = AssetDatabase.GetAssetPath(asset);
		if (path.Length == 0)
			return false;
		if (AssetDatabase.DeleteAsset(path)) {
			AssetDatabase.Refresh();
			return true;
		}
		return false;
	}

	public static void Rename<T>(T asset, string newName) where T : ScriptableObject {
		var path = AssetDatabase.GetAssetPath(asset);
		if (!path.IsNullOrEmpty())
			AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(asset), newName);
		asset.Dirt();
	}
    public static string[] GetRewiredCategoryActions(string category) {
		FieldInfo[] temp = typeof(RewiredConsts.Action).GetAllFieldsWithAttribute(typeof(ActionIdFieldInfoAttribute)).ToArray();
		var infos = new List<FieldInfo>();
		foreach (FieldInfo prop in temp) {
			object[] attrs = prop.GetCustomAttributes(true);
			foreach (object attr in attrs) {
				if (attr is ActionIdFieldInfoAttribute actionAttr) {
					if (actionAttr.categoryName == category) {
						infos.Add(prop);
					}
				}
			}
		}
		return infos.ToArray().Select(f => f.Name).ToArray();
	}

#endif

	public static void Swap<T>(IList<T> list, int indexA, int indexB) {
		Debug.Log($"Swapping {indexA} <-> {indexB}");
		T tmp = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = tmp;
	}

	public static void Slide<T>(IList<T> list, int index, int positions) {
		Debug.Log($"Sliding {index} of positions {positions}");
		if (positions < 0) {
			for (int i = index; i > index + positions; i--) {
				Swap(list, i, i - 1);
			}
		}
		else if (positions > 0) {
			for (int i = index; i < index + positions; i++) {
				Swap(list, i, i + 1);
			}
		}
	}

	///Random Weighted elements
	///https://forum.unity.com/threads/random-numbers-with-a-weighted-chance.442190/
	public static int GetRandomWeightedIndex(float[] weights) {
		if (weights == null || weights.Length == 0) {
			return -1;
		}
		float totalWeight = 0;

		for (int i = 0; i < weights.Length; i++) {
			if (float.IsPositiveInfinity(weights[i])) {
				return i;
			}
			else if (weights[i] >= 0f && !float.IsNaN(weights[i])) {
				totalWeight += weights[i];
			}
		}

		float randomPick = Random.value;
		float s = 0f;

		for (int i = 0; i < weights.Length; i++) {
			float currentWeight = weights[i];
			if (float.IsNaN(currentWeight) || currentWeight <= 0f) {
				continue;
			}
			s += currentWeight / totalWeight;
			if (s >= randomPick) {
				return i;
			}
		}

		return -1;
	}

	public static string GetCurrentJsonDate() {
		return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss" + ".000Z");
	}

	public static Rect RectTransformToScreenSpace(RectTransform rectTransform) {
		Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);
		return new Rect((Vector2)rectTransform.position - (size * 0.5f), size);
	}

	public static int[] GetRandomIntSequence(int length) {
		int[] tombolaBag = new int[length];
		for (int i = 0; i < tombolaBag.Length; i++) {
			tombolaBag[i] = i;
		}
		tombolaBag.Shuffle();
		return tombolaBag;
	}

	private static byte[] ConvertStringToBytes(string blob) {
		var output = new MemoryStream(16);
		var writer = new BinaryWriter(output);
		writer.Write(blob);
		writer.Close();
		return output.GetBuffer();
	}

	public static string ConvertBytesToString(byte[] buffer) {
		var input = new MemoryStream(buffer);
		var reader = new BinaryReader(input);
		string blob = reader.ReadString();
		reader.Close();
		return blob;
	}
}
