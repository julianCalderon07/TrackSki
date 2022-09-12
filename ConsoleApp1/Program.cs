using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TrackSki
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Inicia...");
                var matriz = GetMatriz();
                var vertices = GetVertex(matriz);
                var edges = GetEdges(matriz);
                Stopwatch time = new Stopwatch();
                time.Start();

                var graph = new Graph<string>(vertices, edges);
                var algorithms = new Algorithms();
                time.Stop();
                Console.WriteLine("crear Grafo: " + time.ElapsedMilliseconds);
                List<HashSet<string>> allTracks = new List<HashSet<string>>();
                time = new Stopwatch();
                time.Start();
                for (int i = 0; i < vertices.Count; i++)
                {
                    var source = new HashSet<string>()
                    {
                        vertices[i]
                    };
                    allTracks.Add(algorithms.DFS<string>(graph, vertices[i], source));
                    algorithms.GetTrack(allTracks);
                }
                time.Stop();
                Console.WriteLine("Analisis de data: " + time.ElapsedMilliseconds);

                Console.WriteLine(string.Join(", ", algorithms.GetRawTrack(allTracks[0])));
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Presiones cualquier tecla para continuar...");
                Console.ReadLine();
            }
        }

        private static void AddEdge(string vertexSource, string vertexNeighbor, List<Tuple<string, string>> edges)
        {
            edges.Add(Tuple.Create(vertexSource, vertexNeighbor));
        }

        private static string CreateVertex(int i, int j, string[,] matriz)
        {
            return $"{i},{j},{matriz[i, j]}";
        }

        private static List<Tuple<string, string>> GetEdges(string[,] matriz)
        {
            var edges = new List<Tuple<string, string>>();
            var rows = matriz.GetLength(0);
            var columns = matriz.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (j < columns - 1)
                    {
                        var vertexSource = CreateVertex(i, j, matriz);
                        var vertexDestiny = CreateVertex(i, j + 1, matriz);
                        AddEdge(vertexSource, vertexDestiny, edges);
                    }
                    if (i > 0)
                    {
                        var vertexSource = CreateVertex(i, j, matriz);
                        var vertexDestiny = CreateVertex(i - 1, j, matriz);
                        AddEdge(vertexSource, vertexDestiny, edges);
                    }
                    if (j > 0)
                    {
                        var vertexSource = CreateVertex(i, j, matriz);
                        var vertexDestiny = CreateVertex(i, j - 1, matriz);
                        AddEdge(vertexSource, vertexDestiny, edges);
                    }
                    if (i < rows - 1)
                    {
                        var vertexSource = CreateVertex(i, j, matriz);
                        var vertexDestiny = CreateVertex(i + 1, j, matriz);
                        AddEdge(vertexSource, vertexDestiny, edges);
                    }
                }
            }
            return edges;
        }

        private static string[,] GetMatriz()
        {
            try
            {
                var pathFile = Path.Combine(Environment.CurrentDirectory, "Resource", "map.txt");
                var lineas = File.ReadAllLines(pathFile);
                Console.WriteLine("Encontro Archivo...");

                //var lineas = File.ReadAllLines(@"C:\Users\lenovo\Desktop\Prueba\4x4.txt");
                if (lineas != null)
                {
                    var limit = lineas[0].Split();
                    string[,] matriz = new string[int.Parse(limit[0].ToString()), int.Parse(limit[1].ToString())];
                    for (int i = 0; i < int.Parse(limit[0]); i++)
                    {
                        string[] vector = lineas[i + 1].Split();
                        for (int j = 0; j < int.Parse(limit[0]); j++)
                        {
                            matriz[i, j] = vector[j];
                        }
                    }
                    return matriz;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static List<string> GetVertex(string[,] matriz)
        {
            var vertex = new List<string>();
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    vertex.Add(CreateVertex(i, j, matriz));
                }
            }
            return vertex;
        }
    }
}