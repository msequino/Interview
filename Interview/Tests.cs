using System.Diagnostics;
using System.Linq;
using System;
using NUnit.Framework;

namespace Interview
{

	[TestFixture]
	public class Tests
	{

		Repository<Package> repository;

		[OneTimeSetUp]
		public void SetupTest()
		{
			repository = new Repository<Package>();
		}

		[Test]
		public void TestRepository() {
  
			Package package1 = new Package(1, "com.package1");
			Package package2 = new Package(1, "com.package1.sameid");
			Package package3 = new Package(2, "com.package2");

			repository.Print();
			repository.Save(package1);
			Assert.AreEqual(1, repository.Length(), "Repository did not has save " + package1.Name);

			repository.Print();
			repository.Save(package2);
			Assert.AreNotEqual(2, repository.Length(), "Repository cannot have more than one packages with same id");

			repository.Print();
			repository.Save(package3);
			Assert.AreEqual(2, repository.Length(), "Repository did not has save " + package3.Name);

			repository.Delete(package1.Id);
			Assert.AreEqual(1, repository.Length(), 0, "Repository is still full");
			repository.Print();
 
			Package pkg = null;
			for (int i = 0; i < 20; i++)
			{
				pkg = new Package(i, "com.package." + i);
				repository.Save(pkg);
			}
			Assert.AreEqual(20, repository.Length(), 0, "Repository did not save packages");

		}

		[OneTimeTearDown]
		public void TearDownTest()
		{
			/* After all, cleaning repository ready for a new execution */
			repository.Clean();;
		}
	}

}