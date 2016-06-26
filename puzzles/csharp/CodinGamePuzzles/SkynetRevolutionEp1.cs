using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

internal class SkynetRevolutionEp1
{
	private static void Main(string[] args)
	{
		var network = new Network();
		network.Read();

		var criticalPathFinder = new TrapPath(network)
		{
			Fallback = new ExitRingCriticalPath(network)
			{
				Fallback = new ShortestExitPathCriticalLink(network)
			}
		};

		while (true)
		{
			uint agentIndex = uint.Parse(Console.ReadLine());
			var mostCriticalLink = criticalPathFinder.GetMostCriticalLink(agentIndex);

			mostCriticalLink.Break();

			Console.WriteLine("{0} {1}", mostCriticalLink.Start, mostCriticalLink.End);
		}
	}

	private class Network : IEnumerable, IEnumerable<Node>
	{
		public Network()
		{
			this.nodes = new ConcurrentDictionary<uint, Node>();
		}

		public void Read()
		{
			string[] inputs;
			inputs = Console.ReadLine().Split(' ');

			uint nodesCount = uint.Parse(inputs[0]);
			uint linksCount = uint.Parse(inputs[1]);
			uint exitGatewaysCount = uint.Parse(inputs[2]);

			ReadLinks(linksCount);
			ReadExitGateways(exitGatewaysCount);
		}

		private void ReadLinks(uint linksCount)
		{
			string[] inputs;
			for (uint i = 0; i < linksCount; i++)
			{
				inputs = Console.ReadLine().Split(' ');
				uint nodeOneIndex = uint.Parse(inputs[0]);
				uint nodeTwoIndex = uint.Parse(inputs[1]);

				var node1 = GetOrCreateNode(nodeOneIndex);
				var node2 = GetOrCreateNode(nodeTwoIndex);

				node1.ConnectTo(node2);
			}
		}

		private Node GetOrCreateNode(uint index)
		{
			return this.nodes.GetOrAdd(index, x => new Node(x));
		}

		private void ReadExitGateways(uint exitGatewaysCount)
		{
			for (uint i = 0; i < exitGatewaysCount; i++)
			{
				uint exitIndex = uint.Parse(Console.ReadLine());
				this.nodes[exitIndex].SetExit();
			}
		}

		public IEnumerable<IEnumerable<Node>> FindExitPaths(uint startIndex)
		{
			var exitGateways = this.nodes.Values.Where(n => n.IsExit);
			var startNode = this.nodes[startIndex];
			var exitPaths = exitGateways.SelectMany(e => e.FindPathsTo(startNode));

			return exitPaths.OrderBy(CalculatePathScore);
		}

		private long CalculatePathScore(IEnumerable<Node> path)
		{
			var count = path.Count();
			var subScoreNode = path.Skip(count - 2).FirstOrDefault();
			if (ReferenceEquals(null, subScoreNode))
			{
				return count * 100;
			}

			return count * 100 + subScoreNode.Index;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.nodes.Values.GetEnumerator();
		}

		IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
		{
			return this.nodes.Values.GetEnumerator();
		}

		private readonly ConcurrentDictionary<uint, Node> nodes;
	}

	private class Node
	{
		public Node(uint index)
		{
			Index = index;

			this.links = new List<Node>();
		}

		public bool IsExit
		{
			get; private set;
		}

		public uint Index
		{
			get; private set;
		}

		public void SetExit()
		{
			IsExit = true;
		}

		public void ConnectTo(Node other)
		{
			other.ConnectToInternal(this);
			ConnectToInternal(other);
		}

		private void ConnectToInternal(Node other)
		{
			if (this.links.Contains(other))
			{
				return;
			}

			this.links.Add(other);
		}

		public void DisconnectFrom(Node other)
		{
			other.DisconnectFromInternal(this);
			DisconnectFromInternal(other);
		}

		private void DisconnectFromInternal(Node other)
		{
			if (!this.links.Contains(other))
			{
				return;
			}

			this.links.Remove(other);
		}

		public bool IsDirectConnection(Node other)
		{
			return this.links.Contains(other);
		}

		public IEnumerable<IEnumerable<Node>> FindPathsTo(Node other)
		{
			return FindPathsInternal(this, other, new Node[0]);
		}

		private IEnumerable<IEnumerable<Node>> FindPathsInternal(Node start, Node end, IEnumerable<Node> skip)
		{
			if (ReferenceEquals(start, end))
			{
				yield return new[] { end };
				yield break;
			}

			var allSkipped = skip.Concat(start.links).ToList();
			foreach (var next in start.links)
			{
				if (skip.Contains(next))
				{
					continue;
				}

				var subpaths = FindPathsInternal(next, end, allSkipped);
				foreach (var path in subpaths)
				{
					yield return new[] { start }.Concat(path);
				}
			}
		}

		public override string ToString()
		{
			return Convert.ToString(Index);
		}

		private readonly IList<Node> links;
	}

	private class Link : IEquatable<Link>
	{
		public Link(Node start, Node end)
		{
			Start = start;
			End = end;
		}

		public Link(Node[] ends)
		{
			Start = ends[0];
			End = ends[1];
		}

		public Node Start
		{
			get; private set;
		}

		public Node End
		{
			get; private set;
		}

		public void Break()
		{
			Start.DisconnectFrom(End);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Link);
		}

		public bool Equals(Link other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return (Equals(Start, other.Start) && Equals(End, other.End))
				|| (Equals(Start, other.End) && Equals(End, other.Start));
		}
	}

	private abstract class LinkBreakerStrategy
	{
		protected LinkBreakerStrategy(Network network)
		{
			Network = network;
		}

		protected Network Network
		{
			get; private set;
		}

		public LinkBreakerStrategy Fallback
		{
			get; set;
		}

		public Link GetMostCriticalLink(uint agentIndex)
		{
			var fallbackLink = GetFallbackCriticalLink(agentIndex);
			var criticalLink = GetCriticalLinkInternal(agentIndex, fallbackLink);

			return criticalLink;
		}

		private Link GetFallbackCriticalLink(uint agentIndex)
		{
			if (ReferenceEquals(null, Fallback))
			{
				return null;
			}

			return Fallback.GetMostCriticalLink(agentIndex);
		}

		protected abstract Link GetCriticalLinkInternal(uint agentIndex, Link fallbackLink);
	}

	private class TrapPath : LinkBreakerStrategy
	{
		public TrapPath(Network network)
			: base(network) { }

		protected override Link GetCriticalLinkInternal(uint agentIndex, Link fallbackLink)
		{
			var criticalPaths = Network.FindExitPaths(agentIndex);
			var agentFirstSteps = criticalPaths.Select(x => new Link(x.Skip(x.Count() - 2).ToArray()));
			var somePath = agentFirstSteps.FirstOrDefault();

			if (agentFirstSteps.All(x => somePath.Equals(x)))
			{
				return somePath;
			}

			return fallbackLink;
		}
	}

	private class ShortestExitPathCriticalLink : LinkBreakerStrategy
	{
		public ShortestExitPathCriticalLink(Network network)
			: base(network) { }

		protected override Link GetCriticalLinkInternal(uint agentIndex, Link fallbackLink)
		{
			var criticalPath = Network.FindExitPaths(agentIndex).First();
			var lastNodes = criticalPath.Skip(criticalPath.Count() - 2).ToArray();

			return new Link(lastNodes[0], lastNodes[1]);
		}
	}

	private class ExitRingCriticalPath : LinkBreakerStrategy
	{
		public ExitRingCriticalPath(Network network)
			: base(network) { }

		protected override Link GetCriticalLinkInternal(uint agentIndex, Link fallbackLink)
		{
			var agent = Network.Single(n => Equals(n.Index, agentIndex));
			var nonAgentNode = GetNonAgentNode(fallbackLink, agent);
			if (nonAgentNode.IsExit)
			{
				return fallbackLink;
			}

			var criticalRingLink = GetCriticalRingLink(agent);
			if (ReferenceEquals(null, criticalRingLink))
			{
				return fallbackLink;
			}

			return criticalRingLink;
		}

		private Node GetNonAgentNode(Link link, Node agent)
		{
			if (!ReferenceEquals(agent, link.Start))
			{
				return link.Start;
			}

			return link.End;
		}

		private Link GetCriticalRingLink(Node agent)
		{
			var ringsDesc = GetConnectedExitRings(agent).OrderByDescending(x => x.Count());
			if (!ringsDesc.Any())
			{
				return null;
			}

			return GetWeakestRingLink(ringsDesc.First(), agent);
		}

		private IEnumerable<IEnumerable<Node>> GetConnectedExitRings(Node agent)
		{
			var exitNodes = Network.Where(n => n.IsExit);
			var connectedExitNodes = exitNodes.Where(n => n.FindPathsTo(agent).Any());

			return connectedExitNodes.Select(n => GetRingAround(n));
		}

		private IEnumerable<Node> GetRingAround(Node center)
		{
			return Network.Where(n => center.IsDirectConnection(n));
		}

		private Link GetWeakestRingLink(IEnumerable<Node> ring, Node agent)
		{
			var first = ring.FirstOrDefault(n => !agent.IsDirectConnection(n) && ring.Any(i => !agent.IsDirectConnection(i) && n.IsDirectConnection(i)));
			if (ReferenceEquals(null, first))
			{
				return null;
			}

			return new Link(first, ring.First(n => !agent.IsDirectConnection(n) && first.IsDirectConnection(n)));
		}
	}
}