using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
#if UNITY_EDITOR
using System.Reflection;
#endif
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Extensions {
	#region GameObject

	public static void DestroyChildren(this GameObject obj) {
		foreach (Transform child in obj.transform) {
			Object.Destroy(child.gameObject);
		}
	}

	public static T GetOrAddComponent<T>(this GameObject obj) where T : Component {
		return obj.TryGetComponent(out T oldComp) ? oldComp : obj.AddComponent<T>();
	}

	#endregion

	#region Button

	public static void SetNavigation(this Selectable button, Selectable up, Selectable down, Selectable left, Selectable right) {
		var nav = new Navigation
		{
			mode = Navigation.Mode.Explicit,
			selectOnUp = up,
			selectOnDown = down,
			selectOnLeft = left,
			selectOnRight = right
		};
		button.navigation = nav;
	}

	#endregion

	#region Canvas Group

	public static void Show(this CanvasGroup cg, bool value) {
		cg.alpha = value.ToInt();
		cg.interactable = value;
		cg.blocksRaycasts = value;
	}

	#endregion

	#region Monobehaviour

	public static Coroutine ExecuteNextFrame(this MonoBehaviour mb, Action action) {
		IEnumerator CallNextFrame(Action a) {
			yield return null;
			a.Invoke();
		}

		return mb.StartCoroutine(CallNextFrame(action));
	}

	public static Coroutine ExecuteAfterFrames(this MonoBehaviour mb, Action action, int frames) {
		IEnumerator WaitFrames(Action a) {
			var frame = new WaitForEndOfFrame();
			for (int i = 0; i < frames; i++) {
				yield return frame;
			}
			a.Invoke();
		}

		return mb.StartCoroutine(WaitFrames(action));
	}

	public static Coroutine WaitFrames(this MonoBehaviour mb, int frames) {
		IEnumerator Wait() {
			var frame = new WaitForEndOfFrame();
			for (int i = 0; i < frames; i++) {
				yield return frame;
			}
		}

		return mb.StartCoroutine(Wait());
	}

	public static Coroutine ExecuteAfterDelay(this MonoBehaviour mb, Action action, float time) {
		IEnumerator WaitToCall(Action a) {
			yield return new WaitForSeconds(time);
			a.Invoke();
		}

		return mb.StartCoroutine(WaitToCall(action));
	}

	#endregion

	#region ScriptableObject

#if UNITY_EDITOR
	public static void Dirt(this ScriptableObject so) {
		EditorUtility.SetDirty(so);
	}
#endif

	#endregion

	#region List

	public static T First<T>(this List<T> list) {
		return list[0];
	}

	public static T Last<T>(this List<T> list) {
		return list[list.Count - 1];
	}

	public static List<T> Distinct<T>(this List<T> list) {
		var temp = new List<T>();
		foreach (T element in list) {
			if (!temp.Contains(element)) {
				temp.Add(element);
			}
		}
		return temp;
	}

	public static void Shuffle<T>(this IList<T> list) {
		for (int i = 0; i < list.Count - 1; i++) {
			int index = Random.Range(i, list.Count);
			T temp = list[i];
			list[i] = list[index];
			list[index] = temp;
		}
	}

	public static void Remove<T>(this IList<T> list, IEnumerable<T> toRemove) {
		foreach (T element in toRemove) {
			list.Remove(element);
		}
	}

	public static bool FindAndRemove<T>(this List<T> list, Predicate<T> match) {
		T element = list.Find(match);
		return element != null && list.Remove(element);
	}

	public static T GetRandom<T>(this IList<T> list) {
		return list[Random.Range(0, list.Count)];
	}

	public static T GetRandom<T>(this IEnumerable<T> list, T toIgnore) {
		var l = new List<T>(list);
		l.Remove(toIgnore);
		return l.GetRandom();
	}


	public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action) {
		foreach (T obj in ie)
			action(obj);
	}

	public static List<T> ShiftLeft<T>(this List<T> list, int shiftBy) {
		if (list.Count <= shiftBy) {
			return list;
		}

		var result = list.GetRange(shiftBy, list.Count - shiftBy);
		result.AddRange(list.GetRange(0, shiftBy));
		return result;
	}

	public static List<T> ShiftRight<T>(this List<T> list, int shiftBy) {
		if (list.Count <= shiftBy) {
			return list;
		}

		var result = list.GetRange(list.Count - shiftBy, shiftBy);
		result.AddRange(list.GetRange(0, list.Count - shiftBy));
		return result;
	}
	
	public static T Find<T>(this T[] list, Predicate<T> match) => Array.Find(list, match);
	
	#endregion

	#region String

	public static string UppercaseFirst(this string s) {
		if (string.IsNullOrEmpty(s)) {
			return string.Empty;
		}
		return char.ToUpper(s[0]) + s.Substring(1);
	}

	public static string SnakeCaseToCapitalizedCase(this string s) {
		if (string.IsNullOrEmpty(s)) {
			return string.Empty;
		}
		string[] sA = s.Split('_');
		for (int i = 0; i < sA.Length; i++) {
			sA[i] = sA[i].UppercaseFirst();
		}
		return string.Join(" ", sA);
	}

	public static string SnakeCaseToUpperCase(this string s) {
		if (string.IsNullOrEmpty(s)) {
			return string.Empty;
		}
		string[] sA = s.Split('_');
		for (int i = 0; i < sA.Length; i++) {
			sA[i] = sA[i].ToUpper();
		}
		return string.Join(" ", sA);
	}

	public static string CapitalizedCaseToSnakeCase(this string s) {
		if (string.IsNullOrEmpty(s)) {
			return string.Empty;
		}
		var ss = s.Replace(" ", "_");
		return ss.ToLower();
	}

	public static string CapitalizedCaseToCamelCase(this string s) {
		s = char.ToLower(s[0]) + s.Substring(1);
		return s.Replace(" ", "");
	}

	public static bool IsNullOrEmpty(this string toCheck) => string.IsNullOrEmpty(toCheck);

	public static string SubstringByWords(this string text, int i, char separator = ' ') {
		string[] words = text.Split(separator);
		if (i <= words.Length) {
			string result = string.Empty;
			for (int j = 0; j <= i - 1; j++) {
				result += (words[j] + separator);
			}
			return result.TrimEnd(separator);
		}
		else {
			return "";
		}
	}
	
	public static string GetWord(this string text, int i) {
		string[] words = text.Split(' ');
		return i < words.Length + 1 ? words[i - 1] : "";
	}
	
	public static string GetWord(this string text, char separator, int i) {
		string[] words = text.Split(separator);
		return i < words.Length + 1 ? words[i - 1] : "";
	}
	
	public static string FirstWord(this string text) => text.Split(' ')[0];

	public static string FirstWord(this string text, char separator) => text.Split(separator)[0];

	public static string LastWord(this string text, char separator) {
		string[] t = text.Split(separator);
		return t[t.Length - 1];
	}

	public static string SecondLastWord(this string text, char separator) {
		string[] t = text.Split(separator);
		return t[t.Length - 2];
	}

	public static T ToEnum<T>(this string value) {
		return (T)Enum.Parse(typeof(T), value, true);
	}
	
	public static string RTBold(this string text) => $"<b>{text}</b>";

	public static string RTSize(this string text, int size) => $"<size={size}>{text}</size>";

	public static string RTColor(this string text, RTColors color) => $"<color={color.ToString().ToLower()}>{text}</color>";
	
	#endregion

	#region Vector3

	public static Vector3 OppositeDirection(this Vector3 vector) {
		return new Vector3(-vector.x, vector.y, -vector.z).normalized;
	}

	public static Vector3 PerpendicularClockwise(this Vector3 vector) {
		return new Vector3(vector.z, 0, -vector.x);
	}

	public static Vector3 PerpendicularCounterClockwise(this Vector3 vector) {
		return new Vector3(-vector.z, 0, vector.x);
	}

	#endregion

	#region Int

	public static string ToRoundFormat(this int number) {
		if (number < 1000) {
			return number.ToString();
		}
		if (number < 1000000) {
			return (number / (float)1000).ToString("#.00") + "K";
		}
		if (number < 1000000000d) {
			return (number / (float)1000000).ToString("#.00") + "M";
		}
		return (number / (float)1000000000).ToString("#.00") + "B";
	}

	public static int Remap(this int number, int fromMin, int fromMax, int toMin, int toMax) {
		return toMin + (number - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	private static string GetOrdinalStringEnd(this int number) {
		int x = number % 10;
		switch (x) {
			case 1: return "st";
			case 2: return "nd";
			case 3: return "rd";
			default: return "th";
		}
	}

	#endregion

	#region Float

	public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax) {
		return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	public static float Remap(this int number, int fromMin, int fromMax, float toMin, float toMax) {
		return toMin + (number - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	public static float Distance(this float a, float b) => Mathf.Abs(a - b);

	#endregion

	#region Bool

	public static int ToInt(this bool value) => value ? 1 : 0;

	#endregion

	#region Enum

	public static string GetEnumName<T>(this T e) {
		return Enum.GetName(typeof(T), e);
	}

	#endregion

	#region Type

#if UNITY_EDITOR

	public static IEnumerable<FieldInfo> GetAllFieldsWithAttribute(this Type objectType, Type attributeType) {
		return objectType.GetFields().Where(
			f => f.GetCustomAttributes(attributeType, false).Any());
	}

#endif

	#endregion

	#region Sprite

	public static Texture2D GetTexture(this Sprite sprite) {
		if (sprite == null) {
			return null;
		}
		if (sprite.rect.width != sprite.texture.width) {
			var texture2D = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
			Color[] colors = sprite.texture.GetPixels(Mathf.CeilToInt(sprite.rect.x),
				Mathf.CeilToInt(sprite.rect.y),
				Mathf.CeilToInt(sprite.rect.width),
				Mathf.CeilToInt(sprite.rect.height));
			texture2D.SetPixels(colors);
			texture2D.Apply();
			return texture2D;
		}
		else
			return sprite.texture;
	}

	public static Color32[] GetPixels32(this Sprite sprite) {
		if (sprite == null) {
			return null;
		}
		if (sprite.rect.width != sprite.texture.width) {
			var texture2D = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
			Color[] colors = sprite.texture.GetPixels(Mathf.CeilToInt(sprite.rect.x),
				Mathf.CeilToInt(sprite.rect.y),
				Mathf.CeilToInt(sprite.rect.width),
				Mathf.CeilToInt(sprite.rect.height));
			texture2D.SetPixels(colors);
			return texture2D.GetPixels32();
		}
		else
			return sprite.texture.GetPixels32();
	}

	#endregion

	#region Color32

	public static bool IsEqualTo(this Color32 color1, Color32 color2) {
		return (color1.r == color2.r && color1.g == color2.g && color1.b == color2.b && color1.a == color2.a);
	}

	#endregion
	
	#region Color
	
	public static Color Opaque(this Color color) => new Color(color.r, color.g, color.b);
	
	public static Color Invert(this Color color) => new Color(1 - color.r, 1 - color.g, 1 - color.b, color.a);

	public static Color WithAlpha(this Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);
	
	#endregion

	#region Rect

	public static Vector2 TopLeft(this Rect rect) {
		return new Vector2(rect.xMin, rect.yMin);
	}

	public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint) {
		Rect result = rect;
		result.x -= pivotPoint.x;
		result.y -= pivotPoint.y;
		result.xMin *= scale;
		result.xMax *= scale;
		result.yMin *= scale;
		result.yMax *= scale;
		result.x += pivotPoint.x;
		result.y += pivotPoint.y;
		return result;
	}
	
	#endregion
	
	#region RectTransform
	
	public static RectTransform Copy(this RectTransform target) {
		var copy = new RectTransform
		{
			localScale = target.localScale,
			anchorMin = target.anchorMin,
			anchorMax = target.anchorMax,
			pivot = target.pivot,
			sizeDelta = target.sizeDelta,
			anchoredPosition3D = target.anchoredPosition3D,
			rotation = target.rotation
		};
		return copy; 
	}

	public static void ResetScaleAndPosition(this RectTransform rect) {
		rect.anchoredPosition = Vector2.zero;
		rect.localScale = Vector3.one;
		
	}
	
	#endregion
	
}

public enum RTColors{ Black, Blue, Brown, Cyan, Darkblue, Green, Grey, Lightblue, Lime, Magenta, Maroon, Navy, Olive, Orange, Purple, Red, Silver, Teal, White, Yellow }
