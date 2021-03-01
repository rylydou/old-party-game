using System;

namespace MGE.ECS
{
	public static class SceneManager
	{
		public static Scene activeScene { get; private set; }
		public static Scene queuedScene { get; private set; }

		public static Action onSceneChanged = () => { };

		public static bool QueueScene(Scene scene)
		{
			if (scene == null)
				throw new Exception("New Scene can not be null!");

			// if (_activeScene == scene)
			// throw new Exception("Can not queue the same Scene!");

			if (queuedScene == null)
			{
				if (activeScene != null)
				{
					queuedScene = scene;
					activeScene.CleanUp();

					activeScene.onDoneCleaningUp += () => DequeueScene();
				}
				else
				{
					activeScene = scene;
					// _activeScene.Init();
					activeScene.onDoneCleaningUp += () => DequeueScene();

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
			activeScene.onDoneCleaningUp -= () => DequeueScene();

			if (queuedScene == null)
				throw new Exception("Queued Scene is null, how did this happen");

			activeScene = queuedScene;
			// activeScene.Init();
			queuedScene = null;

			onSceneChanged.Invoke();
		}

		public static void FixedUpdate() => activeScene?.FixedUpdate();
		public static void Update() => activeScene?.Update();
		public static void Draw() => activeScene?.Draw();
		public static void DrawUI() => activeScene?.DrawUI();
	}
}