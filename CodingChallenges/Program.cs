// See https://aka.ms/new-console-template for more information
using CodingChallenge.HuffmanEncoderDecoder;
using CodingChallenge.JSONParser;
using CodingChallenge.WCTool;

void HuffmanEncoderDecoder()
{
    // This is the sample text provided from the coding challenge I used to validate the algorithm.
    // It contains:
    //      32 -> 'c'
    //      42 -> 'd'
    //      120 -> 'e'
    //      7 -> 'k'
    //      42 -> 'l'
    //      24 -> 'm'
    //      37 -> 'u'
    //      2 -> 'z'

    var sampleText = "cccccccccccccccccccccccccccccccc" +
        "dddddddddddddddddddddddddddddddddddddddddd" +
        "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" +
        "kkkkkkk" +
        "llllllllllllllllllllllllllllllllllllllllll" +
        "mmmmmmmmmmmmmmmmmmmmmmmmuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu" +
        "zz";

    Console.WriteLine("I'm going to encode the following sample text provided from the coding challenge:");
    Console.WriteLine(sampleText);
    Console.WriteLine("Choose an output directory...");
    var outputDirectory = Console.ReadLine();

    var hed = new HuffmanEncoderDecoder();

    hed.Encode(sampleText, outputDirectory);
    hed.Decode();
}

void WCToolStart()
{
    while (true)
    {
        new CCCommandLineParser(Console.ReadLine()).Execute();
    }
}

void JSONParser()
{
    while (true)
    {
        Console.WriteLine(new CCJSONParser(Console.ReadLine()).Execute());
    }
}