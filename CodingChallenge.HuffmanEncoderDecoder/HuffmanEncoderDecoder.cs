namespace CodingChallenge.HuffmanEncoderDecoder
{
    public class HuffmanEncoderDecoder
    {
        public string Encode(string inputFile, string outputFile)
        {
            var result = string.Empty;

            try
            {
                var charsFrequencies = ValidateInput(inputFile);
                var huffmanRootNode = CreateHuffmanTree(charsFrequencies);
                var prefixTable = CreatePrefixCodeTable(huffmanRootNode);
                // STEP 4
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
        private CharFrequency[] ValidateInput(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            var text = File.ReadAllText(path);

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
        private Dictionary<char, string> CreatePrefixCodeTable(HuffmanNode root)
        {
            var prefixTable = new Dictionary<char, string>();

            Dfs(root, null, prefixTable);

            return prefixTable;

            void Dfs(HuffmanNode node, string? code, Dictionary<char, string> table)
            {
                if (node.LeftChild is not null)
                {
                    var leftChildCode = code is null ? "1" : "1" + code;

                    Dfs(node.LeftChild, leftChildCode, table);
                }

                if (node.RightChild is not null)
                {
                    var rightChildCode = code is null ? "0" : "0" + code;

                    Dfs(node.RightChild, rightChildCode, table);
                }

                if (node.LeftChild is null &&
                   node.RightChild is null)
                    table.Add(node.Char!.Value, code!);
            }
        }
    }
}
