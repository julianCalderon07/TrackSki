using System;
using System.Collections.Generic;
using System.Text;

namespace TrackSki
{
    public class Algorithms
    {
        public HashSet<T> DFS<T>(Graph<T> graph, T start, HashSet<T> visited)
        {
            if (!graph.AdjacencyList.ContainsKey(start))
                return visited;

            List<HashSet<T>> trackList = new List<HashSet<T>>();
            var stack = new Stack<T>();
            var vertex = start;
            var visitedAux = new HashSet<T>(visited);
            foreach (var neighbor in graph.AdjacencyList[vertex])
            {
                if (!visited.Contains(neighbor) && ValidateVertex(vertex, neighbor))
                {
                    if (stack.Count > 0)
                    {
                        trackList.Add(visitedAux);
                        visitedAux = new HashSet<T>(visited);
                    }

                    visitedAux.Add(neighbor);
                    visitedAux.UnionWith(DFS(graph, neighbor, visitedAux));
                    stack.Push(neighbor);
                }
            }
            trackList.Add(visitedAux);

            if (trackList.Count > 1)
            {
                GetTrack(trackList);
            }

            return trackList[0];
        }

        public List<int> GetRawTrack<T>(HashSet<T> ts)
        {
            List<int> rawTrack = new List<int>();
            foreach (var vertex in ts)
            {
                rawTrack.Add(GetVertexValue(vertex));
            }

            return rawTrack;
        }

        public void GetTrack<T>(List<HashSet<T>> trackList)
        {
            while (trackList.Count > 1)
            {
                ValidateTracks(trackList);
            }
        }

        private void GetBetterTrack<T>(List<HashSet<T>> trackList)
        {
            var firstValue = GetDropping(trackList[0]);
            var SecondValue = GetDropping(trackList[1]);

            if (firstValue < SecondValue)
            {
                trackList.Remove(trackList[0]);
                return;
            }
            trackList.Remove(trackList[1]);
        }

        private int GetDropping<T>(HashSet<T> ts)
        {
            List<int> rawTrack = GetRawTrack(ts);

            var drop = rawTrack[0] - rawTrack[^1];
            return drop;
        }

        private int GetVertexValue<T>(T vertex)
        {
            Type typeValue = typeof(T);
            if (typeValue != typeof(string))
                return 0;

            var vertexContent = vertex.ToString().Split(",");
            return Convert.ToInt32(vertexContent[2]);
        }

        private void ValidateTracks<T>(List<HashSet<T>> trackList)
        {
            if (trackList[0].Count < trackList[1].Count)
            {
                trackList.Remove(trackList[0]);
                return;
            }
            if (trackList[0].Count > trackList[1].Count)
            {
                trackList.Remove(trackList[1]);
                return;
            }
            if (trackList[0].Count == trackList[1].Count)
            {
                GetBetterTrack(trackList);
            }
        }

        private bool ValidateVertex<T>(T vertex, T neighbor)
        {
            var valueVertex = GetVertexValue(vertex);
            var valueNeighbor = GetVertexValue(neighbor);
            return valueNeighbor < valueVertex;
        }
    }
}