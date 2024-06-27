using System.Xml.Linq;

abstract class ConnectFour
{
    protected string P1;
    protected string P2;
    protected string[,] board;
    protected bool GameOver;
    protected int row;
    protected int column;
    public ConnectFour()
    {
        board = new string[7, 7];
        row = board.GetLength(0);
        column = board.GetLength(1);
    }
    protected void Swap(string A, string B)
    {
        string temp = A;
        P1 = B;
        P2 = temp;
    }
    protected void ClearGameBoard()
    {
        for (int i = 0; i < row; i++)
        {
            if (i == row - 1)
            {
                for (int j = 0; j < column; j++)
                {
                    board[i, j] = (j + 1).ToString();
                }
            }
            else
            {
                for (int j = 0; j < column; j++)
                {
                    board[i, j] = "_";
                }
            }
        }
    }
    protected void DisplayBoard()
    {
        Console.Clear();
        string GameBoard ="\n";
        for (int i = 0; i < row; i++)
        {
            GameBoard += "| ";
            for (int j = 0; j < column; j++)
            {
                GameBoard += board[i, j] + " ";
            }
            GameBoard += "|\n";
        }
        Console.WriteLine(GameBoard);
    }
    protected bool CheckWinner()
    {
        for (int r = 0; r < row - 1; r++)
        {
            for (int c = 0; c < column - 3; c++)
            {
                if (board[r, c] != "_")
                {
                    if (board[r, c] == board[r, c + 1] && board[r, c + 1] == board[r, c + 2] && board[r, c + 2] == board[r, c + 3])
                    {
                        return true;
                    }
                }
            }
        }
        for (int c = 0; c < column; c++)
        {
            for (int r = 0; r < row - 4; r++)
            {
                if (board[r, c] != "_")
                {
                    if (board[r, c] == board[r + 1, c] && board[r + 1, c] == board[r + 2, c] && board[r + 2, c] == board[r + 3, c])
                    {
                        return true;
                    }
                }
            }
        }
        for (int r = 0; r < row - 4; r++)
        {
            for (int c = 0; c < column - 3; c++)
            {
                if (board[r, c] != "_")
                {
                    if (board[r, c] == board[r + 1, c + 1] && board[r + 1, c + 1] == board[r + 2, c + 2] && board[r + 2, c + 2] == board[r + 3, c + 3])
                    {
                        return true;
                    }
                }
            }
        }
        for (int r = 3; r < row - 1; r++)
        {
            for (int c = 0; c < column - 3; c++)
            {
                if (board[r, c] != "_")
                {
                    if (board[r, c] == board[r - 1, c + 1] && board[r - 1, c + 1] == board[r - 2, c + 2] && board[r - 2, c + 2] == board[r - 3, c + 3])
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    protected void Winner(string P)
    {
        if (P == P1)
        {
            Console.WriteLine($"Player {P1} won!\n");
        }
        else
        {
            Console.WriteLine($"Player {P2} won!\n");
        }
    }
    public abstract void Start();
}
class Human : ConnectFour
{
    protected Random R = new Random(7);
    protected int RandomFirstMove;
    public Human()
    {

    }
    public Human(string nameA, string nameB)
    {
        P1 = nameA;
        P2 = nameB;
        RandomFirstMove = R.Next(0, 1);
        if (RandomFirstMove == 1)
        {
            Swap(P1, P2);
        }
    }
    protected int ValidNumber()
    {
        int number = 0;
        bool valid = false;
        string input;
        do
        {
            input = Console.ReadLine();
            valid = int.TryParse(input, out number);
            if (!valid)
            {
                Console.Write("Invalid input, enter a number from 1 to 7: ");
            }
            else if (number > column || number < 1)
            {
                valid = false;
                Console.Write("Invalid column, enter a number from 1 to 7: ");
            }
        } while (!valid);
        return number - 1;
    }
    protected void Set(int j, string XO)
    {
        bool availableSpace = true;
        int redo;
        for (int i = row - 2; i >= 0; i--)
        {
            if (board[i, j] == "_")
            {
                board[i, j] = XO;
                availableSpace = true;
                break;
            }
            else if (i == 0)
            {
                availableSpace = false;
                break;
            }
        }
        if (!availableSpace)
        {
            Console.Write($"Column {j + 1} is full! Try again: ");
            redo = ValidNumber();
            Set(redo, XO);
        }
    }
    public override void Start()
    {
        int count = 0;
        ClearGameBoard();
        DisplayBoard();
        GameOver = false;
        string turn = P2;
        int number;
        while (!GameOver)
        {
            if (turn == P2)
            {
                turn = P1;
                Console.Write($"{P1}\'s turn, choose a column: ");
                number = ValidNumber();
                Set(number, "X");
                DisplayBoard();
                Console.WriteLine($"{P1} chose column {number+1}");
            }
            else
            {
                turn = P2;
                Console.Write($"{P2}\'s turn, choose a column: ");
                number = ValidNumber();
                Set(number, "O");
                DisplayBoard();
                Console.WriteLine($"{P2} chose column {number+1}");
            }
            GameOver = CheckWinner();
            count++;
            if (count >= 42 && !GameOver)
            {
                Console.WriteLine("It's a draw!");
                break;
            }
        }
        if (GameOver)
        {
            Winner(turn);
        }
        string answer;
        do
        {
        Console.WriteLine("Rematch? (Y/N)");
        answer = Console.ReadLine().Trim().ToUpper();
            if (answer == "Y")
            {
                Swap(P1, P2);
                Start();
            }
        } while (answer != "N" && answer != "Y");
    }
}
class AI : Human
{
    public AI()
    {

    }
    public AI(string player)
    {
        P1 = player;
        P2 = "AI";
        RandomFirstMove = R.Next(0, 1);
        if (RandomFirstMove == 1)
        {
            Swap(P1, P2);
        }
    }
    protected int AIRNG()
    {
        int number = R.Next(1, 7);

        return number - 1;
    }
    protected void AISet(int j, string XO)
    {
        bool availableSpace = false;
        for (int i = row - 2; i >= 0; i--)
        {
            if (board[i, j] == "_")
            {
                board[i, j] = XO;
                availableSpace = true;
                break;
            }
            else if (i == 0)
            {

                availableSpace = false;
                break;
            }
        }
        if (!availableSpace)
        {
            AISet(AIRNG(), XO);
        }
    }
    public override void Start()
    {
        int count = 0;
        ClearGameBoard();
        DisplayBoard();
        GameOver = false;
        string turn = P2;
        int number;
        while (!GameOver)
        {
            if (turn == P2)
            {
                turn = P1;
                if (P1 != "AI")
                {
                    Console.Write($"{P1}\'s turn, choose a column: ");
                    number = ValidNumber();
                    Set(number, "X");
                }
                else
                {
                    number = AIRNG();
                    AISet(number, "X");
                }
                DisplayBoard();
                Console.WriteLine($"{P1} chose column {number + 1}");
            }
            else
            {
                turn = P2;
                if (P2 != "AI")
                {
                    Console.Write($"{P2}\'s turn, choose a column: ");
                    number = ValidNumber();
                    Set(number, "O");
                }
                else
                {
                    number = AIRNG();
                    AISet(number, "O");
                }
                DisplayBoard();
                Console.WriteLine($"{P2} chose column {number + 1}");
            }
            GameOver = CheckWinner();
            count++;
            if (count >= 42 && !GameOver)
            {
                Console.WriteLine("It's a draw!");
                break;
            }
        }
        if (GameOver) 
        {
            Winner(turn); 
        }
        string answer;
        do
        {
            Console.WriteLine("Rematch? (Y/N)");
            answer = Console.ReadLine().Trim().ToUpper();
            if (answer == "Y")
            {
                Swap(P1, P2);
                Start();
            }
        } while (answer != "N" && answer != "Y");
    }
}
class Program
{
    static void Main(string[] args)
    {
        string welcomeMessage = "Welcome to Connect Four!";
        string instruction = "Enter \"start\" to begin or \"exit\" to close the game\nEnter \"AI\" as one of the player to start single player mode";
        bool playing;
        Console.WriteLine(welcomeMessage);
        do
        {
            playing = true;
            Console.WriteLine(instruction);
            Console.Write("Enter a command: ");
            string cmd = Console.ReadLine().ToLower().Trim();
            if (cmd == "exit")
            {
                playing = false;
            }
            else if (cmd == "start")
            {
                string playerA, playerB;
                do
                {
                    Console.Write("Player A: ");
                    playerA = Console.ReadLine();
                } while (playerA == null);
                do
                {
                    Console.Write("Player B: ");
                    playerB = Console.ReadLine();
                    if (playerB == playerA)
                    {
                        Console.WriteLine("Player B cannot be the same as Player A...");
                    }
                } while (playerB == null || playerB == playerA);
                if (playerA != "AI" && playerB != "AI")
                {
                    Human HumanGame = new Human(playerA, playerB);
                    HumanGame.Start();
                }
                else if (playerA == "AI")
                {
                    AI HumanVsAI = new AI(playerB);
                    HumanVsAI.Start();
                }
                else
                {
                    AI HumanVsAI = new AI(playerA);
                    HumanVsAI.Start();
                }
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Unknown command!");
            }
        } while (playing);
    }
}