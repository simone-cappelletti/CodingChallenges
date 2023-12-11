// See https://aka.ms/new-console-template for more information
using CodingChallenge.JSONParser;
using CodingChallenge.WCTool;

JSONParser();

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
        Console.WriteLine( new CCJSONParser(Console.ReadLine()).Execute());
    }
}