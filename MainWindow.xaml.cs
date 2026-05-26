using System.Media;
using System.Windows;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private Random rand = new Random();

        // ✅ ADDED: Chatbot instance
        private Chatbot chatbot;

        //  RANDOM RESPONSE LISTS
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

            // ✅ ADDED: chatbot + audio init
            chatbot = new Chatbot();
            Audio.PlayGreeting();
        }

        //  AUDIO + WELCOME
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

            ChatHistory.Items.Add("Bot: Welcome to the Cybersecurity Awareness Bot!");
        }

        // SEND BUTTON
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                ChatHistory.Items.Add("Bot: Please type a message.");
                return;
            }

            ChatHistory.Items.Add("You: " + input);

            Respond(input);

            // ✅ ADDED: chatbot response integration (optional extra intelligence layer)
            string chatbotResponse = chatbot.GetResponse(input);
            ChatHistory.Items.Add("Bot: " + chatbotResponse);

            UserInput.Clear();
        }

        // EXIT BUTTON
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Respond(string input)
        {
            //  PASSWORD (random)
            if (input.Contains("password"))
            {
                int index = rand.Next(passwordTips.Length);
                ChatHistory.Items.Add("Bot: " + passwordTips[index]);
            }

            //  PHISHING (random)
            else if (input.Contains("phishing"))
            {
                int index = rand.Next(phishingTips.Length);
                ChatHistory.Items.Add("Bot: " + phishingTips[index]);
            }

            // CYBERSECURITY (random)
            else if (input.Contains("cybersecurity"))
            {
                int index = rand.Next(cybersecurityTips.Length);
                ChatHistory.Items.Add("Bot: " + cybersecurityTips[index]);
            }

            //  SCAM
            else if (input.Contains("scam"))
            {
                ChatHistory.Items.Add("Bot: Scams try to trick you into giving money or personal information. Always verify messages before responding.");
            }

            //  PRIVACY
            else if (input.Contains("privacy"))
            {
                ChatHistory.Items.Add("Bot: Protect your privacy by not sharing personal information online and adjusting app settings.");
            }

            //  HELP
            else if (input.Contains("help"))
            {
                ChatHistory.Items.Add("Bot: Ask me about passwords, phishing, scams, privacy, or cybersecurity.");
            }

            //  HOW ARE YOU
            else if (input.Contains("how are you"))
            {
                ChatHistory.Items.Add("Bot: I am feeling curious and ready to help you stay safe online!");
            }

            //  EXIT COMMAND
            else if (input == "exit")
            {
                ChatHistory.Items.Add("Bot: Goodbye!");
                Application.Current.Shutdown();
            }

            //  DEFAULT
            else
            {
                ChatHistory.Items.Add("Bot: I didn't understand that. Try asking about cybersecurity topics like password, phishing, or scams.");
            }
        }

        // TOP BUTTONS
        private void BtnChatbot_Click(object sender, RoutedEventArgs e)
        {
            ChatHistory.Items.Add("Bot: Chatbot is active and ready.");
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            ChatHistory.Items.Add("Bot: Try asking about password safety, phishing, scams, privacy, or cybersecurity.");
        }
    }

    // ✅ ADDED: Audio helper class
    public static class Audio
    {
        public static void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("WelcomeMessage.wav");
                player.Play();
            }
            catch
            {
                // fail silently or optionally log
            }
        }
    }
}