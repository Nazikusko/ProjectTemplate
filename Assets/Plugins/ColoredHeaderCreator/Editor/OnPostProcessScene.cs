using UnityEditor;
using UnityEditor.Callbacks;

namespace Packs.ColoredHeaderCreator.Editor
{
	public static class OnPreprocessScene
	{
		[PostProcessScene]
		private static void OnPostProcessScene()
		{
			if (EditorApplication.isPlaying) {
				return;
			}

			EditorHelper.DeleteAllHeaders();
		}
	}
}
