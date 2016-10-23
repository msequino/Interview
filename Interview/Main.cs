using System;
using System.IO;

namespace Interview
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.Out.WriteLine("Welcome to app");

			Console.Out.WriteLine("Checking if persistent file exists");
			Repository<Package> repo = new Repository<Package>();

			Console.Out.WriteLine("Printing packages inside repository");
			repo.Print();

			repo.Clean();

			repo = new Repository<Package>();

		}
	}
}
