using System;
namespace Interview
{
	public class Package : IStoreable
	{
		public IComparable Id { get; set; }
		public String Name { get; set; }

		public Package() { }

		public Package(IComparable Id, String Name)
		{
			this.Id = Id;
			this.Name = Name;
		}

		public int CompareTo(Package other)
		{
			return this.Id.CompareTo(other.Id);
		}
	}
}