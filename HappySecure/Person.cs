using System;
using SQLite;
namespace Test
{
	public class Person
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string email { get; set; }

		public string password { get; set; }

        public bool firstUse { get; set; }

		public override string ToString()
		{
            return string.Format("[Person: ID={0}, Email={1}, Password={2}, FirstUse={3}],", ID, email, password, firstUse);
		}
	}
}
