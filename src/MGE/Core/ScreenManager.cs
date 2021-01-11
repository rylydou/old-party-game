using System.Collections;
using System;

namespace MGE
{
	public class ScreenManager
	{
		static internal ScreenManager _current;
		public static ScreenManager current { get => _current; }

		Screen _activeScreen;
		public Screen activeScreen { get => _activeScreen; }
		Screen _queuedScreen;
		public Screen queuedScreen { get => _queuedScreen; }

		public Action onScreenChanged = () => { };

		public bool QueueScreen(Screen screen)
		{
			if (_queuedScreen == null)
			{
				if (activeScreen != null)
				{
					_queuedScreen = screen;
					_activeScreen.CleanUp();

					_activeScreen.onDoneCleaningUp += () => DequeueScreen();
				}
				else
				{
					_activeScreen = screen;
					_activeScreen.Init();
					_activeScreen.onDoneCleaningUp += () => DequeueScreen();

					onScreenChanged.Invoke();
				}

				return true;
			}
			else
			{
				Logger.LogError($"Can not queue a new screen because there is aready a screen queued!");
			}

			return false;
		}

		void DequeueScreen()
		{
			_activeScreen.onDoneCleaningUp -= () => DequeueScreen();

			if (_queuedScreen == null) Logger.LogError("Queued screen is null, how did this happen?");
			_activeScreen = _queuedScreen;
			_activeScreen.Init();
			_queuedScreen = null;

			onScreenChanged.Invoke();
		}
	}
}