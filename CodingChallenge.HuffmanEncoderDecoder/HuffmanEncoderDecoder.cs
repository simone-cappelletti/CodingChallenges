using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace CodingChallenge.HuffmanEncoderDecoder
{
    public class HuffmanEncoderDecoder
    {
        private const int HF_LEFT_SIDE = 0;
        private const int HF_RIGHT_SIDE = 1;

        private const char GS_CHAR = (char)29;

        private const string HEADER_START = "::BEGIN HEADER::";
        private const string HEADER_END = "::END HEADER::";
        private const string HEADER_REGEX = @$"{HEADER_START}([\s\S]*?){HEADER_END}";

        private string EncodedFile = string.Empty;
        private string DecodedFile = string.Empty;

        /// <summary>
        /// Encoding the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="outputDirectory"></param>
        public void Encode(string text, string outputDirectory)
        {
            try
            {
                EncodedFile = Path.Combine(outputDirectory, "encodedText.txt");
                DecodedFile = Path.Combine(outputDirectory, "decodedText.txt");

                var charsFrequencies = ValidateInput(text, outputDirectory);
                var huffmanTree = CreateHuffmanTree(charsFrequencies);
                var prefixTable = CreatePrefixTable(huffmanTree);

                WriteHeader(prefixTable);
                EncodeText(prefixTable, text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Decoding the text
        /// </summary>
        /// <returns></returns>
        public void Decode()
        {
            try
            {
                var prefixTable = GetHeader();

                Decode(prefixTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// STEP 1: Validate input and read text file
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="text"></param>
        /// <returns>Chars frequencies</returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        private CharFrequency[] ValidateInput(string text, string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException();

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

            foreach (var charFrequency in charsFrequencies)
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
        private Dictionary<char, int> CreatePrefixTable(HuffmanNode root)
        {
            var prefixTable = new Dictionary<char, int>();

            Dfs(root, null, prefixTable);

            return prefixTable;

            void Dfs(HuffmanNode node, int? code, Dictionary<char, int> table)
            {
                if (node.LeftChild is not null)
                {
                    var leftChildCode = code ?? HF_LEFT_SIDE;

                    if (code is not null)
                    {
                        leftChildCode <<= 1;
                    }

                    Dfs(node.LeftChild, leftChildCode, table);
                }

                if (node.RightChild is not null)
                {
                    var rightChildCode = code ?? HF_RIGHT_SIDE;

                    if (code is not null)
                    {
                        rightChildCode <<= 1;
                        rightChildCode |= HF_RIGHT_SIDE;
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
        private void WriteHeader(Dictionary<char, int> prefixTable)
        {
            var sb = new StringBuilder();

            sb.Append(HEADER_START);

            foreach (var (key, value) in prefixTable)
                sb.Append($"{key}{value}{GS_CHAR}");

            sb.Append(HEADER_END);

            File.WriteAllText(EncodedFile, sb.ToString());
        }

        /// <summary>
        /// STEP 5: Encoding the text in the output file
        /// </summary>
        /// <param name="prefixTable"></param>
        /// <param name="text"></param>
        private void EncodeText(Dictionary<char, int> prefixTable, string text)
        {
            // TO DO: In order to save space I need to use all the 32 bit instead of creating every time a new array for each char

            const int BIT_ARRAY_SIZE = 32;

            using (var binaryWriter = new BinaryWriter(File.Open(EncodedFile, FileMode.Append, FileAccess.Write)))
            {
                var totalBitArray = new BitArray(BIT_ARRAY_SIZE);
                foreach (var @char in text)
                {
                    var currentBitArray = new BitArray(new int[] { prefixTable[@char] });

                    Write(currentBitArray, binaryWriter);
                }
            }

            void Write(BitArray bitArray, BinaryWriter binaryWriter)
            {
                var bytes = new byte[BIT_ARRAY_SIZE / 8];
                bitArray.CopyTo(bytes, 0);
                binaryWriter.Write(bytes);
            }
        }

        /// <summary>
        /// STEP 6: Retrieve the header section from input file and build the prefix table.
        /// </summary>
        /// <returns>Prefix table</returns>
        private Dictionary<char, int> GetHeader()
        {
            var prefixTable = new Dictionary<char, int>();
            var text = File.ReadAllText(EncodedFile);
            var headerRegex = Regex.Matches(text, HEADER_REGEX);

            if (headerRegex.Count == 0)
                throw new Exception("No header found");

            var headerText = headerRegex[0].Groups[1].Value;
            var headerElements = headerText.Split(GS_CHAR, StringSplitOptions.RemoveEmptyEntries);

            foreach (var headerElement in headerElements)
            {
                var key = headerElement[0];
                var value = int.Parse(headerElement.Substring(1));

                prefixTable.Add(key, value);
            }

            File.WriteAllText(EncodedFile, text.Replace(headerRegex[0].Groups[0].Value, string.Empty));

            return prefixTable;
        }

        /// <summary>
        /// STEP 7: Decode the text with the prefix table
        /// </summary>
        /// <param name="prefixTable"></param>
        private void Decode(Dictionary<char, int> prefixTable)
        {
            var sb = new StringBuilder();
            var text = File.OpenRead(EncodedFile);

            using (var reader = new BinaryReader(text))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var value = reader.ReadInt32();
                    var key = prefixTable.Single(x => x.Value == value).Key;

                    sb.Append(key);
                }
            }

            File.WriteAllText(DecodedFile, sb.ToString());
        }
    }
}
