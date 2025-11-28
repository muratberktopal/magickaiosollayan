using UnityEngine;
using TMPro; // TextMeshPro kullanýmý için þart

public class BotNameGenerator : MonoBehaviour
{
    [Header("UI Referansý")]
    public TextMeshProUGUI nameText; // Ýsim yazacak Text objesi

    // Hafýzayý yormamasý için listeyi 'static' yapýyoruz
    private static string[] botNames = {
        // --- PRO GAMER ÝSÝMLERÝ ---
        "ProGamer", "NoobSlayer", "HeadHunter", "Viper", "Ghost", "Shadow",
        "Terminator", "Legend", "Alpha", "Omega", "Ninja", "Samurai",
        "Beast", "Storm", "Ranger", "Sniper", "Tank", "Healer", "Winner",
        "Loser", "King", "Queen", "Prince", "Joker", "Ace", "Zeus", "Hades",
        "Ares", "Thor", "Loki", "Odin", "IronMan", "Hulk", "Spider", "BatMan",
        "Flash", "Sonic", "Mario", "Luigi", "Link", "Zelda", "Goku", "Vegeta",
        "Naruto", "Sasuke", "Luffy", "Zoro", "Nami", "Sanji", "Ichigo",
        
        // --- COOL & EDGY ---
        "DarkSoul", "NightMare", "BloodSeeker", "DeathWish", "LoneWolf",
        "SilentKill", "Venom", "Carnage", "Doom", "Quake", "Rage", "Fury",
        "Havoc", "Chaos", "Anarchy", "Rebel", "Outlaw", "Bandit", "Pirate",
        "Viking", "Knight", "Warrior", "Soldier", "General", "Captain",
        "Major", "Admiral", "Commander", "Boss", "Chief", "Master",
        "Sensei", "Guru", "Wizard", "Mage", "Warlock", "Druid", "Shaman",
        "Paladin", "Rogue", "Assassin", "Hunter", "Slayer", "Destroyer",
        
        // --- RANDOM & SAYILI ---
        "Player1", "Player2", "User_99", "Guest_123", "Bot_007", "Unknown",
        "Anonymous", "NoName", "Blank", "Error404", "LagMaster", "HighPing",
        "AFK_Guy", "Disconnect", "Reconnecting", "Loading...", "System",
        "Admin", "Moderator", "Dev", "Tester", "Hacker", "Cheater", "Glitch",
        "Bug", "Crash", "Lag", "Ping", "FPS_Drop", "LowBattery",
        
        // --- EÐLENCELÝ & TROLL ---
        "Potato", "Tomato", "Banana", "Apple", "Orange", "Lemon", "Lime",
        "Cherry", "Berry", "Melon", "Kiwi", "Grape", "Peach", "Pear",
        "Plum", "Mango", "Papaya", "Coconut", "Almond", "Peanut", "Walnut",
        "Hazelnut", "Cashew", "Pistachio", "Cookie", "Cake", "Pie",
        "Donut", "Muffin", "Cupcake", "Brownie", "Candy", "Sweet", "Sour",
        "Salty", "Bitter", "Spicy", "Hot", "Cold", "Ice", "Fire",
        
        // --- KISA & ÖZ ---
        "X", "Y", "Z", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
        "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V",
        "B", "N", "M", "Max", "Leo", "Rex", "Fox", "Wolf", "Bear", "Lion",
        "Tiger", "Cat", "Dog", "Rat", "Bat", "Owl", "Hawk", "Eagle",
        "Crow", "Raven", "Dove", "Swan", "Duck", "Goose", "Frog",
        
        // --- ELEMENTLER & DOÐA ---
        "Wind", "Earth", "Water", "Light", "Dark", "Void", "Abyss",
        "Sky", "Sea", "Ocean", "River", "Lake", "Pond", "Rain", "Snow",
        "Hail", "Mist", "Fog", "Cloud", "Star", "Moon", "Sun", "Planet",
        "Comet", "Meteor", "Asteroid", "Galaxy", "Universe", "Space",
        "Time", "Dimension", "Reality", "Dream", "Night", "Day", "Dusk",
        "Dawn", "Twilight", "Eclipse", "Horizon", "Vista", "View",
        
        // --- EKSTRALAR ---
        "KebabSlayer", "DonerMaster", "Lahmacun", "Baklava", "CayiVer",
        "Osman", "Ali", "Veli", "Mehmet", "Ayse", "Fatma", "Zeynep",
        "TurkPower", "Ist_Guard", "Ankara06", "Izmir35", "Bursa16",
        "Trabzon61", "Adana01", "Antalya07"
    };

    void Start()
    {
        if (nameText != null)
        {
            // 1. Rastgele Ýsim Seç
            string randomName = botNames[Random.Range(0, botNames.Length)];

            // 2. Yazýya Ata
            nameText.text = randomName;

            // 3. (Ýsteðe Baðlý) Rastgele Renk Ver - IO oyunu hissi için
            // nameText.color = Random.ColorHSV(0.5f, 1f, 0.5f, 1f, 0.8f, 1f);
        }
    }
}