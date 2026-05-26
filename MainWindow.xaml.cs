using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private Random rand = new Random();

        private string lastTopic = "";
        private string userName = "";
        private string favouriteTopic = "";
        private bool showingHistory = false;

        private List<string> chatLog = new List<string>();

        private bool waitingForName = true;

        // RANDOM RESPONSE LISTS
        private string[] phishingTips =
        {
            "Be cautious of emails asking for personal information.",
            "Never click suspicious links in messages or emails.",
            "Check the sender's email address carefully before responding.",
            "Scammers often pretend to be trusted organisations.",
            "Do not download attachments from unknown sources."
        };

        private string[] passwordTips =
        {
            "Use at least 12 characters with symbols and numbers.",
            "Never reuse the same password across different accounts.",
            "Avoid using personal information in passwords.",
            "Use a password manager for better security."
        };

        private string[] cybersecurityTips =
        {
            "Cybersecurity protects computers, networks, and data from attacks.",
            "Always keep your software and antivirus updated.",
            "Use strong passwords and enable two-factor authentication.",
            "Avoid public Wi-Fi when accessing sensitive accounts.",
            "Be cautious when opening unknown emails or attachments."
        };

        public MainWindow()
        {
            InitializeComponent();
            Audio.PlayGreeting();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("WelcomeMessage.wav");
                player.Play();
            }
            catch
            {
                ChatHistory.Items.Add("Bot: Voice greeting could not be played.");
            }

            string welcome = "Bot: Welcome to the Cybersecurity Awareness Bot!";
            ChatHistory.Items.Add(welcome);
            chatLog.Add(welcome);

            string askName = "Bot: What is your name?";
            ChatHistory.Items.Add(askName);
            chatLog.Add(askName);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                ChatHistory.Items.Add("Bot: Please type a message.");
                return;
            }

            ChatHistory.Items.Add("You: " + input);
            chatLog.Add("You: " + input);

            input = input.ToLower();

            // FIRST TIME NAME
            if (waitingForName)
            {
                userName = input;
                waitingForName = false;

                string msg = "Bot: Nice to meet you, " + userName + "!";
                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);

                UserInput.Clear();
                return;
            }

            Respond(input);

            UserInput.Clear();
        }

        private void Respond(string input)
        {
            // FOLLOW-UP
            if (input.Contains("more") || input.Contains("another") || input.Contains("explain"))
            {
                string msg;

                if (lastTopic == "password")
                    msg = "Bot: Another password tip is to avoid birthdays and names.";
                else if (lastTopic == "phishing")
                    msg = "Bot: Another phishing tip is to never click urgent login links.";
                else if (lastTopic == "cybersecurity")
                    msg = "Bot: Keep your software updated regularly.";
                else
                    msg = "Bot: Tell me what topic you want more info on.";

                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            // MEMORY
            if (input.Contains("i'm interested in"))
            {
                favouriteTopic = input.Replace("i'm interested in", "").Trim();

                string msg = "Bot: Great! I'll remember you're interested in " + favouriteTopic + ".";
                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            if (input.Contains("remind me"))
            {
                string msg;

                if (!string.IsNullOrEmpty(favouriteTopic))
                    msg = "Bot: You are interested in " + favouriteTopic + ".";
                else
                    msg = "Bot: I don't have any saved preferences.";

                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            // PASSWORD
            if (input.Contains("password"))
            {
                lastTopic = "password";
                string msg = "Bot: " + passwordTips[rand.Next(passwordTips.Length)];
                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            // PHISHING
            if (input.Contains("phishing"))
            {
                lastTopic = "phishing";
                string msg = "Bot: " + phishingTips[rand.Next(phishingTips.Length)];
                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            // CYBERSECURITY
            if (input.Contains("cybersecurity"))
            {
                lastTopic = "cybersecurity";
                string msg = "Bot: " + cybersecurityTips[rand.Next(cybersecurityTips.Length)];
                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            // SCAM
            if (input.Contains("scam"))
            {
                string msg = "Bot: Scams try to trick you into giving personal info or money.";
                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            // PRIVACY
            if (input.Contains("privacy"))
            {
                string msg;

                if (!string.IsNullOrEmpty(userName))
                    msg = "Bot: " + userName + ", protecting your privacy is important.";
                else
                    msg = "Bot: Protect your privacy by not sharing personal information online.";

                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            // HELP
            if (input.Contains("help"))
            {
                string msg = "Bot: Ask about passwords, phishing, scams, privacy, or cybersecurity.";
                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            // HOW ARE YOU
            if (input.Contains("how are you"))
            {
                string msg = "Bot: I am ready to help you stay safe online!";
                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                return;
            }

            // EXIT
            if (input == "exit")
            {
                string msg = "Bot: Goodbye!";
                ChatHistory.Items.Add(msg);
                chatLog.Add(msg);
                Application.Current.Shutdown();
                return;
            }

            // DEFAULT
            string defaultMsg = "Bot: I didn't understand that. Try cybersecurity topics.";
            ChatHistory.Items.Add(defaultMsg);
            chatLog.Add(defaultMsg);
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Bot: Goodbye!";
            ChatHistory.Items.Add(msg);
            chatLog.Add(msg);

            Application.Current.Shutdown();
        }
        // CHAT HISTORY BUTTON
        private void BtnChatbot_Click(object sender, RoutedEventArgs e)
        {
            if (!showingHistory)
            {
                ChatHistory.Items.Add("Bot: --- CHAT HISTORY ---");

                foreach (var msg in chatLog)
                {
                    ChatHistory.Items.Add(msg);
                }

                ChatHistory.Items.Add("Bot: --- END OF HISTORY ---");

                showingHistory = true;
            }
            else
            {
                ChatHistory.Items.Add("Bot: History already shown.");
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Bot: Try asking about password safety, phishing, scams, privacy, or cybersecurity.";
            ChatHistory.Items.Add(msg);
            chatLog.Add(msg);
        }
    }

    public static class Audio
    {
        public static void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("WelcomeMessage.wav");
                player.Play();
            }
            catch { }
        }
    }
 
}