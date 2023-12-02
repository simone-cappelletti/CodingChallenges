// See https://aka.ms/new-console-template for more information
using CodingChallenge.WCTool;

void WCToolStart()
{
    while (true)
    {
        new CodingChallengeParser(Console.ReadLine()).Execute();
    }
}