namespace CodingChallenge.HuffmanEncoderDecoder
{
    public class HuffmanNode
    {
        public char? @Char { get; set; }
        public int Value { get; set; }
        public HuffmanNode? LeftChild { get; set; }
        public HuffmanNode? RightChild { get; set; }

        public HuffmanNode(
            char? @char,
            int value = 0,
            HuffmanNode? leftChild = null,
            HuffmanNode? rightChild = null)
        {
            @Char = @char;
            Value = value;
            LeftChild = leftChild;
            RightChild = rightChild;
        }

    }
}
