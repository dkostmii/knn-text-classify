using static Util;
using static KNN;

IEnumerable<(Tag, string)> dataset = new List<(Tag, string)>
{
    (Tag.Good, "Hello, World!"),
    (Tag.Wrong, "hello World"),
    (Tag.Wrong, "helo word"),
    (Tag.Wrong, "helloworld"),
    (Tag.Wrong, "halowrld"),
    (Tag.Wrong, "hello"),
    (Tag.Wrong, "Hello"),
    (Tag.Wrong, "Hello World!"),
    (Tag.Wrong, "Hello,,,,World!"),
    (Tag.Wrong, "hello, wrld!")
};

void Run()
{
    while (true)
    {
        Console.WriteLine("Write your Hello, World! (enter Q to quit): ");

        string hello = Console.ReadLine()!;
        Console.WriteLine();

        if (hello.ToLower() == "q")
        {
            return;
        }

        DisplayResult(KNearestNeighbours(dataset, hello, 2));
        Console.WriteLine();
    }
}

Run();