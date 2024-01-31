// See https://aka.ms/new-console-template for more information
using CodingChallenge.HuffmanEncoderDecoder;
using CodingChallenge.JSONParser;
using CodingChallenge.WCTool;

HuffmanEncoderDecoder();

void HuffmanEncoderDecoder()
{
    //Console.WriteLine("Insert input file path...");
    //var inputFile = Console.ReadLine();
    //Console.WriteLine("Insert output file path...");
    //var outputFile = Console.ReadLine();

    var inputFile = "C:\\Users\\scappelletti\\Desktop\\LesMiserables.txt";
    var outputFile = "C:\\Users\\scappelletti\\Desktop\\CCEncoderDecoder.txt";

    new HuffmanEncoderDecoder().Encode(inputFile, outputFile);
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