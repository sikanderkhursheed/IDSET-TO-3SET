using System;
using System.Collections.Generic;
using System.Linq;

namespace _3SAT
{
	public class Graph
	{
		public List<Clause> Clauses { get; }

		public class Node
		{
			public int Value { get; }
			public int Complement => -Value;
			public HashSet<Node> Neighbours { get; }
			public int Degree => Neighbours.Count;

			public Node ( int value )
			{
				Value      = value;
				Neighbours = new HashSet<Node> ( );
			}

			public void AddNeighbour ( Node node )
			{
				Neighbours.Add ( node );
				node.Neighbours.Add ( this );
			}

			public void AddNeighbours ( IEnumerable<Node> nodes )
			{
				foreach ( var node in nodes )
					AddNeighbour ( node );
			}
		}

		public class Clause
		{
			public int Id { get; }
			public HashSet<Node> Nodes { get; }

			public IEnumerable<Node> OrderedNodes =>
				Nodes
					.OrderBy ( x => x.Degree )
					.ThenBy ( x => Math.Abs ( x.Value ) );

			private static int id;

			public Clause ( IEnumerable<int> clause )
			{
				Id    = ++id;
				Nodes = new HashSet<Node> ( );

				foreach ( var v in clause )
				{
					var node = new Node ( v );
					node.AddNeighbours ( Nodes );
					Nodes.Add ( node );
				}
			}

			public bool Contains ( int value )
			{
				return Nodes.Any ( x => x.Value == value );
			}

			public Node GetNode ( int value )
			{
				return Nodes.FirstOrDefault ( x => x.Value == value );
			}
		}

		public Graph ( Problem3Sat problem )
		{
			Clauses = new List<Clause> ( problem.K );
			foreach ( var clause in problem.Clauses )
				Clauses.Add ( new Clause ( clause ) );

			foreach ( var clause in Clauses )
			{
				foreach ( var node in clause.Nodes )
				{
					node.AddNeighbours (
						Clauses
							.Where ( x => x.Contains ( node.Complement ) )
							.Select ( x => x.GetNode ( node.Complement ) )
					);
				}
			}
		}

		public bool VerifyIdset ( IList<int> solution )
		{
			if ( solution.Count != Clauses.Count )
				return false;

			if ( solution.Any ( i => solution.Contains ( -i ) ) )
				return false;

			var nodes = new List<Node> ( solution.Count );
			for ( var i = 0; i < solution.Count; i++ )
			{
				var node = Clauses[i].GetNode ( solution[i] );
				if ( node is null )
					return false;
				nodes.Add ( node );
			}

			foreach ( var node in nodes )
			foreach ( var neighbour in node.Neighbours )
				if ( nodes.Contains ( neighbour ) )
					return false;

			return true;
		}

		public static IEnumerable<IEnumerable<Node>> GetPossibilities ( IEnumerable<Clause> clauses )
		{
			var enumerable = clauses.ToList ( );

			if ( enumerable.Count == 0 )
				yield break;

			if ( enumerable.Count == 1 )
				foreach ( var node in enumerable[0].OrderedNodes )
					yield return new List<Node> {node};

			foreach ( var node in enumerable[0].OrderedNodes ) // first
			foreach ( var possibility in GetPossibilities ( enumerable.Skip ( 1 ) ) ) // rest
				yield return new List<Node> {node}.Concat ( possibility ); // prepend and return
		}

		public List<int> SolveIdset ( )
		{
			return GetPossibilities ( Clauses )
				.Select ( possibility => possibility.Select ( x => x.Value ).ToList ( ) )
				.FirstOrDefault ( VerifyIdset );
		}
	}
}