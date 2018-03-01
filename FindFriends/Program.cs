using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFriends
{
	public class Person
	{
		public Person(string name)
		{
			Name = name;			
		}
		public string Name { get; set; }
		List<Person> FriendsList = new List<Person>();

		public List<Person> Friends
		{
			get
			{
				return FriendsList;
			}
		}

		public void IsFriendOf(Person p)
		{
			FriendsList.Add(p);
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public class BreadthFirstAlgorithm
	{
		public Person BuildFriendGraph()
		{
			var Lukasz = new Person("Łukasz");
			var Adam = new Person("Adam");
			var Franek = new Person("Franek");
			Lukasz.IsFriendOf(Adam);
			Adam.IsFriendOf(Lukasz);
			Lukasz.IsFriendOf(Franek);
			Franek.IsFriendOf(Lukasz);

			var Asia = new Person("Asia");
			Adam.IsFriendOf(Asia);
			Asia.IsFriendOf(Adam);

			var Jozek = new Person("Józek");
			Franek.IsFriendOf(Jozek);
			Jozek.IsFriendOf(Franek);
			Lukasz.IsFriendOf(Jozek);
			Jozek.IsFriendOf(Lukasz);

			var Krysia = new Person("Krysia");
			Krysia.IsFriendOf(Franek);
			Franek.IsFriendOf(Krysia);
			Krysia.IsFriendOf(Asia);
			Asia.IsFriendOf(Krysia);

			var Janek = new Person("Janek");
			Janek.IsFriendOf(Jozek);
			Jozek.IsFriendOf(Janek);

			var Zosia = new Person("Zosia");
			Zosia.IsFriendOf(Jozek);
			Jozek.IsFriendOf(Zosia);
			Zosia.IsFriendOf(Adam);
			Adam.IsFriendOf(Zosia);

			var Michal = new Person("Michał");
			Michal.IsFriendOf(Krysia);
			Krysia.IsFriendOf(Michal);
			var Kuba = new Person("Kuba");
			Kuba.IsFriendOf(Janek);
			Janek.IsFriendOf(Kuba);
			//var p11 = new Person("Gosia");
			//var p12 = new Person("Monika");
			return Lukasz;
		}

		public HashSet<Person> FindNthLevelFriends(Person root, int level)
		{
			Queue<Person> friendsToVisit = new Queue<Person>();
			HashSet<Person> visitedFriends = new HashSet<Person>();
			HashSet<Person> nthLevelFriends = new HashSet<Person>();
			friendsToVisit.Enqueue(root);
			visitedFriends.Add(root);

			int currentLevel = 0;
			int elementsToDepthIncrease = 1;
			int nextElementsToDephtIncrease = 0;

			while(friendsToVisit.Count > 0)
			{
				var currentFriend = friendsToVisit.Dequeue();

				if (currentLevel == level)
					nthLevelFriends.Add(currentFriend);

				nextElementsToDephtIncrease += currentFriend.Friends.Where(f => !visitedFriends.Contains(f)).Count();

				if (--elementsToDepthIncrease == 0)
				{
					if (++currentLevel > level) return nthLevelFriends;
					elementsToDepthIncrease = nextElementsToDephtIncrease;
					nextElementsToDephtIncrease = 0;
				}

				foreach(var friend in currentFriend.Friends)
				{
					if (!visitedFriends.Contains(friend))
					{
						friendsToVisit.Enqueue(friend);
						visitedFriends.Add(friend);
					}
				}
			}
			return nthLevelFriends;
		}

		#region zakryty
		public Queue<Person> Traverse(Person root, int maxDepth)
		{			
			Queue<Person> result = new Queue<Person>();
			Queue<Person> Q = new Queue<Person>();
			HashSet<Person> S = new HashSet<Person>();
			Q.Enqueue(root);
			S.Add(root);
			var currentDepth = 0;
			var elementsToDepthIncrease = 1;
			var nextElementsToDepthIncrease = 0;

			while (Q.Count > 0)
			{
				Person p = Q.Dequeue();
				if (currentDepth == maxDepth)
					result.Enqueue(p);

				nextElementsToDepthIncrease += p.Friends.Where(f => !S.Contains(f)).Count();
				if (--elementsToDepthIncrease == 0)
				{
					if (++currentDepth > maxDepth) return result;
					elementsToDepthIncrease = nextElementsToDepthIncrease;
					nextElementsToDepthIncrease = 0;
				}
				foreach (Person friend in p.Friends)
				{					
					if (!S.Contains(friend))
					{
						Q.Enqueue(friend);
						S.Add(friend);
					}
				}
			}
			return result;
		}
		#endregion

		public HashSet<Person> GetNthLevelFriends(Person root, int level)
		{
			Queue<Person> friendsToVisit = new Queue<Person>();
			HashSet<Person> visitedFriends = new HashSet<Person>();
			HashSet<Person> result = new HashSet<Person>();
			Dictionary<Person, int> levels = new Dictionary<Person, int>();

			friendsToVisit.Enqueue(root);
			visitedFriends.Add(root);
			levels.Add(root, 0);

			int currentLevel = 0;
			//int elementsToLevelIncrease = 1;
			//int nextElementsToLevelIncrease = 0;

			while (friendsToVisit.Count > 0)
			{
				var person = friendsToVisit.Dequeue();
				currentLevel = levels[person];
				if (currentLevel == level)
					result.Add(person);

				//nextElementsToLevelIncrease += person.Friends.Where(f => !visitedFriends.Contains(f)).Count();
				//if (--elementsToLevelIncrease == 0)
				//{
				//	if (++currentLevel > level) return result;
				//	elementsToLevelIncrease = nextElementsToLevelIncrease;
				//	nextElementsToLevelIncrease = 0;													
				//}

				if (currentLevel > level) return result;

				foreach (var friend in person.Friends)
				{
					if (!visitedFriends.Contains(friend))
					{
						friendsToVisit.Enqueue(friend);
						visitedFriends.Add(friend);

						levels.Add(friend, currentLevel + 1);
					}
				}
			}
			return result;
		}

	}


	class Program
	{
		static void Main(string[] args)
		{
			BreadthFirstAlgorithm b = new BreadthFirstAlgorithm();
			Person root = b.BuildFriendGraph();
			Console.WriteLine("Traverse\n------");
			//var result = b.Traverse(root, 3);
			//while (result.Count > 0)
			//{
			//	Person p = result.Dequeue();
			//	Console.WriteLine(p);
			//}
			var result = b.GetNthLevelFriends(root, 3);
			foreach (var p in result)
				Console.WriteLine(p);
			Console.ReadKey();
		}
	}
}
