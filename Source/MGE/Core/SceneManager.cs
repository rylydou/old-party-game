using System.Collections;
using System;

namespace MGE
{
	public class SceneManager
	{
		static Scene _activeScene;
		public static Scene activeScene { get => _activeScene; }
		static Scene _queuedScene;
		public static Scene queuedScene { get => _queuedScene; }

		public static Action onSceneChanged = () => { };

		public static bool QueueScene(Scene scene)
		{
			if (scene == null)
				throw new Exception("New Scene can not be null!");

			if (_activeScene == scene)
				throw new Exception("Can not queue the same Scene!");

			if (_queuedScene == null)
			{
				if (activeScene != null)
				{
					_queuedScene = scene;
					_activeScene.CleanUp();

					_activeScene.onDoneCleaningUp += () => DequeueScene();
				}
				else
				{
					_activeScene = scene;
					// _activeScene.Init();
					_activeScene.onDoneCleaningUp += () => DequeueScene();

					onSceneChanged.Invoke();
				}

				return true;
			}
			else
			{
				Logger.LogError($"Can not queue a new scene because there is aready a scene queued!");
			}

			return false;
		}

		static void DequeueScene()
		{
			_activeScene.onDoneCleaningUp -= () => DequeueScene();

			if (_queuedScene == null)
				throw new Exception("Queued Scene is null, how did this happen");

			_activeScene = _queuedScene;
			// _activeScene.Init();
			_queuedScene = null;

			onSceneChanged.Invoke();
		}

		public static void FixedUpdate() => activeScene?.FixedUpdate();
		public static void Update() => activeScene?.Update();
		public static void Draw() => activeScene?.Draw();
		public static void DrawUI() => activeScene?.DrawUI();
	}
}