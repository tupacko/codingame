using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

internal class SkynetRevolutionEp1
{
	private class Network : IEnumerable, IEnumerable<Node>
	{
		public Network()
		{
			this.nodes = new List<Node>();
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

		public IEnumerable<Node> FindShortestExitPath(uint startIndex)
		{
			var exitGateways = this.nodes.Where(n => n.IsExit);
			var startNode = GetNode(startIndex);
			var exitPaths = exitGateways.SelectMany(e => e.FindPathsTo(startNode));

			return exitPaths.OrderBy(CalculatePathScore).FirstOrDefault();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.nodes.GetEnumerator();
		}

		IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
		{
			return this.nodes.GetEnumerator();
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

		private void ReadExitGateways(uint exitGatewaysCount)
		{
			for (uint i = 0; i < exitGatewaysCount; i++)
			{
				uint exitIndex = uint.Parse(Console.ReadLine());

				GetNode(exitIndex).SetExit();
			}
		}

		private Node GetNode(uint index)
		{
			return this.nodes.Single(n => n.IsAtIndex(index));
		}

		private Node GetOrCreateNode(uint index)
		{
			var node = this.nodes.SingleOrDefault(n => n.IsAtIndex(index));
			if (ReferenceEquals(null, node))
			{
				node = new Node(index);

				this.nodes.Add(node);
			}

			return node;
		}

		private readonly IList<Node> nodes;
	}

	private class Node
	{
		public Node(uint index)
		{
			this.number = index;

			this.links = new List<Node>();
		}

		public bool IsExit
		{
			get
			{
				return this.isExitNode;
			}
		}

		internal uint Index
		{
			get
			{
				return this.number;
			}
		}

		public bool IsAtIndex(uint index)
		{
			return this.number == index;
		}

		public void SetExit()
		{
			this.isExitNode = true;
		}

		public void ConnectTo(Node other)
		{
			other.ConnectToInternal(this);
			ConnectToInternal(other);
		}

		public void DisconnectFrom(Node other)
		{
			other.DisconnectFromInternal(this);
			DisconnectFromInternal(other);
		}

		public bool IsDirectConnection(Node other)
		{
			return this.links.Contains(other);
		}

		public IEnumerable<IEnumerable<Node>> FindPathsTo(Node other)
		{
			return FindPathsInternal(this, other, new Node[0]);
		}

		public override string ToString()
		{
			return Convert.ToString(this.number);
		}

		private IEnumerable<IEnumerable<Node>> FindPathsInternal(Node start, Node end, IEnumerable<Node> skip)
		{
			if (ReferenceEquals(start, end))
			{
				yield return new[] { end };
				yield break;
			}

			var completeSkip = skip.Concat(start.links).ToList();

			foreach (var next in start.links)
			{
				if (skip.Contains(next))
				{
					continue;
				}

				var paths = FindPathsInternal(next, end, completeSkip);
				foreach (var path in paths)
				{
					yield return new[] { start }.Concat(path);
				}
			}
		}

		private void ConnectToInternal(Node other)
		{
			if (this.links.Contains(other))
			{
				return;
			}

			this.links.Add(other);
		}

		private void DisconnectFromInternal(Node other)
		{
			if (!this.links.Contains(other))
			{
				return;
			}

			this.links.Remove(other);
		}

		private readonly IList<Node> links;

		private readonly uint number;

		private bool isExitNode;
	}

	private class ShortestExitPathCriticalLink : LinkBreakerStrategy
	{
		public ShortestExitPathCriticalLink(Network network)
			: base(network) { }

		protected override Tuple<Node, Node> GetCriticalLinkInternal(uint agentIndex, Tuple<Node, Node> fallbackLink)
		{
			var criticalPath = Network.FindShortestExitPath(agentIndex);
			var lastNodes = criticalPath.Skip(criticalPath.Count() - 2).ToArray();

			return new Tuple<Node, Node>(lastNodes[0], lastNodes[1]);
		}
	}

	private class ExitRingCriticalPath : LinkBreakerStrategy
	{
		public ExitRingCriticalPath(Network network)
			: base(network) { }

		protected override Tuple<Node, Node> GetCriticalLinkInternal(uint agentIndex, Tuple<Node, Node> fallbackLink)
		{
			var agent = Network.Single(n => n.IsAtIndex(agentIndex));
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

		private Node GetNonAgentNode(Tuple<Node, Node> link, Node agent)
		{
			if (!ReferenceEquals(agent, link.Item1))
			{
				return link.Item1;
			}

			return link.Item2;
		}

		private Tuple<Node, Node> GetCriticalRingLink(Node agent)
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

		private Tuple<Node, Node> GetWeakestRingLink(IEnumerable<Node> ring, Node agent)
		{
			var first = ring.FirstOrDefault(n => !agent.IsDirectConnection(n) && ring.Any(i => !agent.IsDirectConnection(i) && n.IsDirectConnection(i)));
			if (ReferenceEquals(null, first))
			{
				return null;
			}

			return new Tuple<Node, Node>(first, ring.First(n => !agent.IsDirectConnection(n) && first.IsDirectConnection(n)));
		}
	}

	private abstract class LinkBreakerStrategy
	{
		protected LinkBreakerStrategy(Network network)
		{
			this.network = network;
		}

		public LinkBreakerStrategy Fallback
		{
			get; set;
		}

		protected Network Network
		{
			get
			{
				return this.network;
			}
		}

		public Tuple<Node, Node> GetMostCriticalLink(uint agentIndex)
		{
			var fallbackLink = GetFallbackCriticalLink(agentIndex);
			var criticalLink = GetCriticalLinkInternal(agentIndex, fallbackLink);

			return criticalLink;
		}

		protected abstract Tuple<Node, Node> GetCriticalLinkInternal(uint agentIndex, Tuple<Node, Node> fallbackLink);

		private Tuple<Node, Node> GetFallbackCriticalLink(uint agentIndex)
		{
			if (ReferenceEquals(null, Fallback))
			{
				return null;
			}

			return Fallback.GetMostCriticalLink(agentIndex);
		}

		private readonly Network network;
	}

	private static void Main(string[] args)
	{
		var network = new Network();
		network.Read();

		var criticalPathFinder = new ExitRingCriticalPath(network)
		{
			Fallback = new ShortestExitPathCriticalLink(network)
		};

		while (true)
		{
			uint agentIndex = uint.Parse(Console.ReadLine());
			var mostCriticalLink = criticalPathFinder.GetMostCriticalLink(agentIndex);

			mostCriticalLink.Item1.DisconnectFrom(mostCriticalLink.Item2);

			Console.WriteLine("{0} {1}", mostCriticalLink.Item1, mostCriticalLink.Item2);
		}
	}
}