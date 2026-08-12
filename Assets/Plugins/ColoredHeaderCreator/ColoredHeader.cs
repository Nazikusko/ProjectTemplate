using UnityEditor;
using UnityEngine;

namespace Packs.ColoredHeaderCreator
{
	public class ColoredHeader : MonoBehaviour
	{
#if UNITY_EDITOR
		public HeaderSettings headerSettings = new HeaderSettings();

		private void OnValidate()
		{
			EditorApplication.delayCall += _OnValidate;
		}
		private void _OnValidate()
		{
			if (this == null) {
				return;
			}

			EditorApplication.RepaintHierarchyWindow();
		}
#endif
	}
}
