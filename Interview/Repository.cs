using System;
using System.Collections.Generic;
using System.IO;
namespace Interview
{
	
	public class Repository<T> : IRepository<T> where T : Package
	{
		private static String ADD_OP = "ADD";
		private static String REMOVE_OP = "REM";

		private readonly String PERSISTENCE_FILE = AppDomain.CurrentDomain.BaseDirectory + @"/persistence.txt";

		private static List<T> _repository;
		/* Singleton class which allows to initialize just one object. Moreover, it loads data from 
		*	file which contains backup of done operations (reference @redis:aof).*/
		public List<T> Instance
		{
			get
			{
				if (_repository == null)
				{
					_repository = new List<T>();

					if (File.Exists(PERSISTENCE_FILE)) { 
						//Here I load the file which contains backup (for persistence purpose)
						using (StreamReader file =
	   						new StreamReader(PERSISTENCE_FILE))
						{
							String line;
							while ((line = file.ReadLine()) != null)
							{
								String[] data = line.Split(';');
								Package package = new Package(data[1], data[2]);

								if (ADD_OP.CompareTo(data[0]) == 0)
								{
									Save((T)package, false);
								}
								else if (REMOVE_OP.CompareTo(data[0]) == 0)
								{
									Delete(package.Id, false);
								}
							}
						}
					}
				}
				return _repository;
			}

		}

		public Repository(){

			_repository = Instance;
		}

		public IEnumerable<T> All() {

			return _repository;
		}

		private void Delete(IComparable id, Boolean mode)
		{
			/* Copying original repository to a local copy*/
			lock (_repository)
			{
				T[] LocalCopy = new T[Length()];
				_repository.CopyTo(LocalCopy);
				foreach (T package in LocalCopy)
				{
					if (package.Id.CompareTo(id) == 0)
					{
						Console.Out.WriteLine("Removing element");
						_repository.Remove(package);
						if(mode)
							WriteLog(REMOVE_OP + ";" + id + ";");
					}
				}
			}
		}

		public void Delete(IComparable id)
		{
			Delete(id, true);
		}


		private void Save(T item,Boolean mode)
		{

			lock (_repository)
			{
				if (FindById(item.Id) == null)
				{
					Console.Out.WriteLine("Pushing into repository");
					_repository.Add(item);

					if(mode)
						WriteLog(ADD_OP + ";" + item.Id + ";" + item.Name);
				}
				else
				{
					Console.Error.WriteLine("id field should be unique");
				}
			}
		}

		public void Save(T item) {
			Save(item, true);
		}

		public T FindById(IComparable id) { 
		
			foreach (T package in _repository)
			{
				Console.Out.WriteLine(package.Id + "*****" + id);
				if (package.Id.CompareTo(id) == 0)
				{
					Console.Out.WriteLine("Found package with id[" + id + "]");
					return package;
				}
			}
			return null;
		}


		/* * Some utility methods * */
		public int Length() {
			return _repository.Count;
		}

		public void Print()
		{
			if (Length() == 0)
			{
				Console.Error.WriteLine("No packages found");
			}
			foreach (T package in _repository)
			{
				Console.Out.WriteLine("Package with id[" + package.Id + "] with name[" + package.Name + "]");
			}
		}

		/**
		*	WriteLog allows users to write operation into a file (for persistence purpose)
		*/
		public void WriteLog(String text)
		{

			Console.Out.WriteLine("Writing data to file");
			using (StreamWriter file =
				   new StreamWriter(PERSISTENCE_FILE, true))
			{
				file.WriteLine(text);
			}

		}

		/* * Cleaning instance of class, deleting file and create new empty instance * */
		public void Clean()
		{

			_repository = null;
			File.Delete(PERSISTENCE_FILE);
			_repository = Instance;

		}


	}
}
