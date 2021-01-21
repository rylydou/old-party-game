using System;
using System.IO;
using MGE.FileIO;

namespace MGE.FileIO
{
	public class Folder : IDisposable
	{
		#region Static

		#region Methods
		#endregion

		#endregion

		#region Object
		string _path;
		public string path
		{
			get => _path;
			set
			{
				_path = IO.CleanPath(value);
			}
		}

		public Folder(string path)
		{
			IO.CleanPath(path);

			if (path.EndsWith('/'))
				this._path = path.Remove(path.Length - 1, 1);
			else
				this._path = path;
		}

		public void Dispose()
		{
		}

		#region File
		public void DeleteFile(string path)
		{
			File.Delete(GetFullPath(path));
		}

		public void MoveFile(string from, string to, bool overwrite = true)
		{
			File.Move(from, to, overwrite);
		}

		public FileInfo GetFileInfo(string path)
		{
			return new FileInfo(GetFullPath(path));
		}

		public FileStream OpenFile(string path, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.Read)
		{
			return File.Open($"{_path}/{path}", mode, access, share);
		}

		public bool FileExists(string path)
		{
			return File.Exists(GetFullPath(path));
		}
		#endregion

		#region Dir
		public void CreateDir(string path)
		{
			Directory.CreateDirectory(GetFullPath(path));
		}

		public void MoveDir(string from, string to)
		{
			Directory.Move(GetFullPath(from), GetFullPath(to));
		}

		public void DeleteDir(string path, bool recursive = true)
		{
			Directory.Delete(GetFullPath(path), recursive);
		}

		public bool DirExists(string path)
		{
			return Directory.Exists(GetFullPath(path));
		}

		public string[] GetFilesInDir()
		{
			return Directory.GetFiles(path);
		}

		public string[] GetFilesInDir(string path)
		{
			return Directory.GetFiles(path);
		}

		public string[] GetDirsInDir()
		{
			return Directory.GetDirectories(path);
		}

		public string[] GetDirsInDir(string path)
		{
			return Directory.GetDirectories(path);
		}

		public DirectoryInfo GetDirInfo(string path)
		{
			return new DirectoryInfo(GetFullPath(path));
		}
		#endregion

		#region Utils
		public string GetFullPath(string path)
		{
			GetFullPath(ref path);
			return path;
		}

		public void GetFullPath(ref string path)
		{
			if (path.StartsWith('/'))
				path = _path + path;
			else
				path = $"{_path}/{path}";
		}

		public string GetRelitivePath(string path)
		{
			return IO.CleanPath(path).Replace(IO.CleanPath(_path) + '/', "");
		}
		#endregion

		#endregion
	}
}