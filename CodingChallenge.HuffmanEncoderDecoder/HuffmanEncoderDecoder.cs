using System.Text;

namespace CodingChallenge.HuffmanEncoderDecoder
{
    public class HuffmanEncoderDecoder
    {
        public string Encode(string inputFile, string outputFile)
        {
            var result = string.Empty;

            try
            {
                var charsFrequencies = ValidateInput(inputFile, out var text);
                var huffmanRootNode = CreateHuffmanTree(charsFrequencies);
                var prefixTable = CreatePrefixCodeTable(huffmanRootNode);
                WriteHeader(prefixTable, outputFile);
                EncodeText(prefixTable, text, outputFile);

                // STEP 6
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// STEP 1: Validate input and read text file
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns>Chars frequencies</returns>
        private CharFrequency[] ValidateInput(string path, out string text)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            text = File.ReadAllText(path);

            return text
                .GroupBy(x => x)
                .Select(x => new CharFrequency(x.Key, x.Count()))
                .OrderBy(x => x.Count)
                .ToArray();
        }

        /// <summary>
        /// STEP 2: Create the Huffman tree
        /// </summary>
        /// <param name="charsFrequencies"></param>
        /// <returns>Huffman root node</returns>
        private HuffmanNode CreateHuffmanTree(CharFrequency[] charsFrequencies)
        {
            var queue = new PriorityQueue<HuffmanNode, int>();

            foreach (CharFrequency charFrequency in charsFrequencies)
                queue.Enqueue(
                    new HuffmanNode(charFrequency.Key, charFrequency.Count),
                    charFrequency.Count);

            while (queue.Count > 1)
            {
                var leftChild = queue.Dequeue();
                var rightChild = queue.Dequeue();
                var newParent = new HuffmanNode(
                    @char: null,
                    value: leftChild.Value + rightChild.Value,
                    leftChild: leftChild,
                    rightChild: rightChild);

                queue.Enqueue(newParent, leftChild.Value + rightChild.Value);
            }

            return queue.Dequeue();
        }

        /// <summary>
        /// STEP 3: Create prefix table
        /// </summary>
        /// <param name="root"></param>
        /// <returns>Prefix table</returns>
        private Dictionary<char, int> CreatePrefixCodeTable(HuffmanNode root)
        {
            var prefixTable = new Dictionary<char, int>();

            Dfs(root, null, prefixTable);

            return prefixTable;

            void Dfs(HuffmanNode node, int? code, Dictionary<char, int> table)
            {
                if (node.LeftChild is not null)
                {
                    var leftChildCode = code ?? 1;

                    if (code is not null)
                    {
                        leftChildCode <<= 1;
                        leftChildCode |= 1;
                    }

                    Dfs(node.LeftChild, leftChildCode, table);
                }

                if (node.RightChild is not null)
                {
                    var rightChildCode = code ?? 0;

                    if (code is not null)
                    {
                        rightChildCode <<= 1;
                    }

                    Dfs(node.RightChild, rightChildCode, table);
                }

                if (node.LeftChild is null &&
                   node.RightChild is null)
                    table.Add(node.Char!.Value, code!.Value);
            }
        }

        /// <summary>
        /// STEP 4: Write the header section to the output file
        /// </summary>
        /// <param name="prefixTable"></param>
        /// <param name="outputFile"></param>
        private void WriteHeader(Dictionary<char, int> prefixTable, string outputFile)
        {
            var sb = new StringBuilder();

            sb.AppendLine("---BEGIN HEADER---");

            foreach (var (key, value) in prefixTable)
                sb.AppendLine($"{key} - {value}");

            sb.AppendLine("---END HEADER---");

            File.WriteAllText(outputFile, sb.ToString());
        }

        /// <summary>
        /// STEP 5: Encoding the text in the output file
        /// </summary>
        /// <param name="prefixTable"></param>
        /// <param name="text"></param>
        /// <param name="outputFile"></param>
        private void EncodeText(Dictionary<char, int> prefixTable, string text, string outputFile)
        {
            var bytes = new List<byte[]>();

            foreach (var (key, value) in prefixTable)
                bytes.Add(BitConverter.GetBytes(value));

            var result = bytes.SelectMany(x => x).ToArray();

            using var stream = new FileStream(outputFile, FileMode.Append, FileAccess.Write);
            stream.Write(result, 0, result.Length);
        }
    }
}
