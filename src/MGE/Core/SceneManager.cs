using System.Collections;
using System;

namespace MGE
{
	public class SceneManager
	{
		static SceneManager _current;
		public static SceneManager current { get => _current; }

		Scene _activeScene;
		public Scene activeScene { get => _activeScene; }
		Scene _queuedScene;
		public Scene queuedScene { get => _queuedScene; }

		public Action onSceneChanged = () => { };

		public SceneManager(Scene scene = null)
		{
			if (_current != null) throw new Exception("Don't create more that one Scene Manager");

			_current = this;
			_activeScene = scene;
		}

		public bool QueueScene(Scene scene)
		{
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

		void DequeueScene()
		{
			_activeScene.onDoneCleaningUp -= () => DequeueScene();

			if (_queuedScene == null) Logger.LogError("Queued scene is null, how did this happen?");
			_activeScene = _queuedScene;
			// _activeScene.Init();
			_queuedScene = null;

			onSceneChanged.Invoke();
		}

		public void FixedUpdate() => activeScene?.FixedUpdate();
		public void Update() => activeScene?.Update();
		public void Draw() => activeScene?.Draw();
		public void DrawUI() => activeScene?.DrawUI();
	}
}