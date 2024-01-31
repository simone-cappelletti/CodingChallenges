namespace CodingChallenge.HuffmanEncoderDecoder
{
    public class CharFrequency
    {
        public char Key { get; set; }
        public int Count { get; set; }

        public CharFrequency(
            char key,
            int count)
        {
            Key = key;
            Count = count;
        }
    }
}
