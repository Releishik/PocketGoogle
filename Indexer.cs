using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        Dictionary<string, Dictionary<int, List<int>>> indexes = new Dictionary<string, Dictionary<int, List<int>>>();
        public void Add(int id, string documentText)
        {
            documentText = documentText.ToLower();
            var strings = documentText.Split(new char[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' }).Where(s=>!string.IsNullOrEmpty(s)).ToArray();
			for (int i = 0; i < strings.Length; i++)
			{
                try
				{
                    indexes[strings[i]].Add(id, GetAllSubstringIndexes(documentText, strings[i]));
				}
                catch(KeyNotFoundException)
				{
                    indexes.Add(strings[i], new Dictionary<int, List<int>>());
                    if (!indexes[strings[i]].ContainsKey(id)) indexes[strings[i]].Add(id, GetAllSubstringIndexes(documentText, strings[i]));
				}
				catch(ArgumentException)
				{

				}
            }
        }

        List<int> GetAllSubstringIndexes(string test, string substr)
		{
            List<int> indSet = new List<int>();
            int pos = test.IndexOf(substr);
			while(pos>=0)
			{
                indSet.Add(pos);
                pos = test.IndexOf(substr, pos + substr.Length);
			}
            return indSet;
		}
        public List<int> GetIds(string word)
        {
            try
            {
                return indexes[word.ToLower()].Keys.ToList();
            }
			catch { return new List<int>(); }
        }

        public List<int> GetPositions(int id, string word)
        {
            try
            {
                return indexes[word.ToLower()][id];
            }
			catch
			{
                return new List<int>();
			}
        }

        public void Remove(int id)
        {
            var keys = indexes.Keys.ToArray();
			for (int i = 0; i < indexes.Count; i++)
			{
                var k = keys[i];
                if (indexes[k].ContainsKey(id)) indexes[k].Remove(id);
                if (indexes[k].Count == 0)
                {
                    indexes.Remove(k);
                    i = 0;
                }
			}
            /*
            foreach(var kvp in indexes)
			{
                if (kvp.Value.ContainsKey(id)) indexes[kvp.Key].Remove(id);
                if (indexes[kvp.Key].Count == 0) indexes.Remove(kvp.Key);
			}*/
        }
    }
}
