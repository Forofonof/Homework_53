using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        PlayersFactory playersFactory = new PlayersFactory();
        Database database = new Database(playersFactory);
        Server server = new Server(database);

        database.AddPlayersInList();

        server.Work();
    }
}

class Server
{
    private readonly int _topPlayers = 3;

    private readonly Database _database;

    public Server(Database database)
    {
        _database = database;
    }

    public void Work()
    {
        ShowPlayersByLevel();

        ShowPlayersByPower();
    }

    private void ShowPlayers(List<Player> players)
    {
        foreach (Player player in players)
        {
            player.ShowInfo();
        }
    }

    private void ShowPlayersByLevel()
    {
        var playersByLevel = _database.GetPlayers().OrderByDescending(_players => _players.Level).Take(_topPlayers).ToList();

        Console.WriteLine("Топ игроков по уровню:\n");
        ShowPlayers(playersByLevel);
    }

    private void ShowPlayersByPower()
    {
        var playersByPower = _database.GetPlayers().OrderByDescending(_players => _players.PowerPoints).Take(_topPlayers).ToList();

        Console.WriteLine("Топ игроков по силе:\n");
        ShowPlayers(playersByPower);
    }
}

class Database
{
    private readonly List<Player> _players = new List<Player>();
    private readonly PlayersFactory _playersFactory;

    public Database(PlayersFactory playersFactory)
    {
        _playersFactory = playersFactory;
    }

    public List<Player> AddPlayersInList()
    {
        _players.AddRange(_playersFactory.CreateMultiplePlayers());

        return _players;
    }

    public IReadOnlyList<Player> GetPlayers()
    {
        return _players.AsReadOnly();
    }
}

class PlayersFactory
{
    private readonly string[] _nicknames =
    {
        "ПуЛи_От_БаБуЛи","I30SS","4ekHyTbIu","М0НАХ","БаБуШкИн_ак-47",
        "_КрYчE_Бога_u_ЦаРя_", "S.T.A.L.K.E.R", "П_У_Л_И 0T Д_Е_Д_У_Л_И",
        "Gachi_B0y","Рву_Asss", "КоТэ_ПоД_НаРкОтЭ", "Сделанно_В_СССР", "Снайпер_Саха"
    };

    private readonly Random _random = new Random();

    public List<Player> CreateMultiplePlayers()
    {
        int numberOfPlayers = 15;

        List<Player> players = new List<Player>();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player player = CreatePlayer();
            players.Add(player);
        }

        return players;
    }

    private Player CreatePlayer()
    {
        int levelMinimum = 1;
        int levelMaximum = 101;

        int powerPointMinimum = 250;
        int powerPointMaximum = 1500;

        int level = _random.Next(levelMinimum, levelMaximum);
        int powerPoints = _random.Next(powerPointMinimum, powerPointMaximum);
        string nickname = _nicknames[_random.Next(_nicknames.Length)];

        return new Player(nickname, level, powerPoints);
    }
}

class Player
{
    public Player(string nickname, int level, int powerPoints)
    {
        Nickname = nickname;
        Level = level;
        PowerPoints = powerPoints;
    }

    public int Level { get; private set; }

    public int PowerPoints { get; private set; }

    public string Nickname { get; private set; }

    public void ShowInfo()
    {
        Console.WriteLine($"Никнейм - {Nickname}. Уровень - {Level}. Очков силы - {PowerPoints}.\n");
    }
}