using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private Random rand = new Random();

        // ---------------- FILES ----------------
        private readonly string historyFile = "ChatHistory.txt";
        private readonly string interestsFile = "UserInterests.txt";

        // ---------------- USER MEMORY ----------------
        private string userName = "";
        private string favouriteTopic = "";
        private string lastTopic = "";
        private bool waitingForName = true;
        private bool waitingForEmotion = false;
        private bool inTopicFlow = false;

        private List<string> chatLog = new List<string>();

        // ---------------- TIP DATA ----------------
        private string[] phishingTips =
        {
            "Never click suspicious links in emails — hover over them first to check the real URL.",
            "Always verify the sender's email address before responding to any request.",
            "Be wary of urgent messages asking for passwords or personal info — that's a red flag.",
            "Legitimate companies will never ask for your password via email.",
            "Look out for misspellings in email addresses, like 'support@paypa1.com'."
        };

        private string[] passwordTips =
        {
            "Use a mix of uppercase, lowercase, numbers, and symbols in your passwords.",
            "Never reuse the same password across multiple sites.",
            "Avoid using personal info like your name or birthday in passwords.",
            "Consider using a password manager.",
            "Enable two-factor authentication wherever possible."
        };

        private string[] cybersecurityTips =
        {
            "Keep your operating system and apps updated.",
            "Use reputable antivirus software.",
            "Avoid public Wi-Fi for sensitive accounts without a VPN.",
            "Back up important data regularly.",
            "Unknown USB drives can carry malware."
        };

        private string[] scamTips =
        {
            "If an offer sounds too good to be true, it probably is.",
            "Never send money to someone you've only met online.",
            "Government agencies never demand urgent payment over the phone.",
            "Verify charities before donating.",
            "Be suspicious of unexpected calls asking for personal details."
        };

        private string[] privacyTips =
        {
            "Review social media privacy settings regularly.",
            "Avoid sharing too much personal information online.",
            "Check app permissions before installing.",
            "Use a VPN on public networks.",
            "Private browsers can help reduce tracking."
        };

        private Dictionary<string, string[]> topicTips;

        // ---------------- CONSTRUCTOR ----------------
        public MainWindow()
        {
            InitializeComponent();

            topicTips = new Dictionary<string, string[]>
            {
                { "phishing", phishingTips },
                { "password", passwordTips },
                { "cybersecurity", cybersecurityTips },
                { "scam", scamTips },
                { "privacy", privacyTips }
            };

            Audio.PlayGreeting();
        }

        // ---------------- WINDOW LOADED ----------------
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChatHistory();
            LoadInterest();

            Say("Bot: Welcome to the Cybersecurity Awareness Bot! 🔐");
            Say("Bot: What is your name?");
        }

        // ---------------- SAVE CHAT ----------------
        private void SaveChatToFile(string msg)
        {
            try
            {
                File.AppendAllText(historyFile, msg + Environment.NewLine);
            }
            catch
            {
            }
        }

        // ---------------- LOAD CHAT HISTORY ----------------
        private void LoadChatHistory()
        {
            try
            {
                if (File.Exists(historyFile))
                {
                    ChatHistory.Items.Add("========== START OF CHAT HISTORY ==========");

                    string[] lines = File.ReadAllLines(historyFile);

                    foreach (string line in lines)
                    {
                        ChatHistory.Items.Add(line);
                    }

                    ChatHistory.Items.Add("=========== END OF CHAT HISTORY ===========");
                }
            }
            catch
            {
                ChatHistory.Items.Add("Bot: Failed to load chat history.");
            }
        }

        // ---------------- SAVE INTEREST ----------------
        private void SaveInterest()
        {
            try
            {
                if (!string.IsNullOrEmpty(favouriteTopic))
                {
                    File.WriteAllText(interestsFile, favouriteTopic);
                }
            }
            catch
            {
            }
        }

        // ---------------- LOAD INTEREST ----------------
        private void LoadInterest()
        {
            try
            {
                if (File.Exists(interestsFile))
                {
                    favouriteTopic = File.ReadAllText(interestsFile);

                    if (!string.IsNullOrEmpty(favouriteTopic))
                    {
                        lastTopic = GetMatchingTopic(favouriteTopic);
                    }
                }
            }
            catch
            {
            }
        }

        // ---------------- SEND BUTTON ----------------
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
                return;

            Say("You: " + input);

            UserInput.Clear();

            string lower = input.ToLower();

            // GET NAME
            if (waitingForName)
            {
                userName = input;
                waitingForName = false;

                Say("Bot: Nice to meet you, " + userName + "! 😊");
                Say("Bot: I'm here to help you stay safe online.");
                Say("Bot: Ask me about phishing, passwords, scams, privacy, or cybersecurity.");
                return;
            }

            // EMOTION REPLY
            if (waitingForEmotion)
            {
                waitingForEmotion = false;
                HandleEmotionReply(lower);
                return;
            }

            // DETECT EMOTION
            bool sentimentDetected = DetectEmotion(lower);

            // FOLLOW-UP
            if (inTopicFlow && IsFollowUp(lower))
            {
                HandleFollowUp();
                return;
            }

            // MAIN RESPONSE
            Respond(lower, sentimentDetected);
        }

        // ---------------- FOLLOW-UP DETECTION ----------------
        private bool IsFollowUp(string input)
        {
            return input.Contains("another tip") ||
                   input.Contains("tell me more") ||
                   input.Contains("more info") ||
                   input.Contains("more tips") ||
                   input.Contains("yes") ||
                   input.Contains("ok") ||
                   input.Contains("okay");
        }

        // ---------------- HANDLE FOLLOW-UP ----------------
        private void HandleFollowUp()
        {
            if (!string.IsNullOrEmpty(lastTopic) && topicTips.ContainsKey(lastTopic))
            {
                string tip = topicTips[lastTopic][rand.Next(topicTips[lastTopic].Length)];

                Say("Bot: Here's another " + lastTopic + " tip:");
                Say("Bot: " + tip);
                Say("Bot: Want another tip or another topic?");
            }
        }

        // ---------------- MAIN RESPONSE ----------------
        private void Respond(string input, bool sentimentDetected)
        {
            try
            {
                // SAVE INTEREST
                if (input.Contains("i'm interested in") || input.Contains("i am interested in"))
                {
                    string topic = input
                        .Replace("i'm interested in", "")
                        .Replace("i am interested in", "")
                        .Trim();

                    favouriteTopic = topic;
                    lastTopic = GetMatchingTopic(topic);

                    SaveInterest();

                    inTopicFlow = true;

                    Say("Bot: I'll remember that you're interested in " + favouriteTopic + ".");

                    if (!string.IsNullOrEmpty(lastTopic))
                    {
                        Say("Bot: " + topicTips[lastTopic][rand.Next(topicTips[lastTopic].Length)]);
                    }

                    return;
                }

                // REMIND INTEREST
                if (input.Contains("remind me") ||
                    input.Contains("what do i like") ||
                    input.Contains("what am i interested in"))
                {
                    if (!string.IsNullOrEmpty(favouriteTopic))
                    {
                        Say("Bot: You're interested in " + favouriteTopic + ".");
                    }
                    else
                    {
                        Say("Bot: I don't have any saved interests yet.");
                    }

                    return;
                }

                // PASSWORD
                if (input.Contains("password"))
                {
                    lastTopic = "password";
                    inTopicFlow = true;

                    Say("Bot: " + passwordTips[rand.Next(passwordTips.Length)]);
                    Say("Bot: Want another password tip?");
                    return;
                }

                // PHISHING
                if (input.Contains("phishing"))
                {
                    lastTopic = "phishing";
                    inTopicFlow = true;

                    Say("Bot: " + phishingTips[rand.Next(phishingTips.Length)]);
                    Say("Bot: Want another phishing tip?");
                    return;
                }

                // CYBERSECURITY
                if (input.Contains("cybersecurity") || input.Contains("cyber security"))
                {
                    lastTopic = "cybersecurity";
                    inTopicFlow = true;

                    Say("Bot: " + cybersecurityTips[rand.Next(cybersecurityTips.Length)]);
                    Say("Bot: Want another cybersecurity tip?");
                    return;
                }

                // SCAMS
                if (input.Contains("scam"))
                {
                    lastTopic = "scam";
                    inTopicFlow = true;

                    Say("Bot: " + scamTips[rand.Next(scamTips.Length)]);
                    Say("Bot: Want another scam tip?");
                    return;
                }

                // PRIVACY
                if (input.Contains("privacy"))
                {
                    lastTopic = "privacy";
                    inTopicFlow = true;

                    Say("Bot: " + privacyTips[rand.Next(privacyTips.Length)]);
                    Say("Bot: Want another privacy tip?");
                    return;
                }

                // HOW ARE YOU
                if (input.Contains("how are you"))
                {
                    Say("Bot: I'm doing well 😊");
                    Say("Bot: How are you feeling today?");
                    waitingForEmotion = true;
                    return;
                }

                // HELP
                if (input.Contains("help"))
                {
                    Say("Bot: I can help with:");
                    Say("Bot: • Phishing");
                    Say("Bot: • Passwords");
                    Say("Bot: • Scams");
                    Say("Bot: • Privacy");
                    Say("Bot: • Cybersecurity");
                    return;
                }

                // DEFAULT
                Say("Bot: I didn't quite understand that.");
                Say("Bot: Try asking about phishing, passwords, scams, privacy, or cybersecurity.");
            }
            catch
            {
                Say("Bot: Something went wrong 😅");
            }
        }

        // ---------------- MATCH TOPIC ----------------
        private string GetMatchingTopic(string text)
        {
            if (text.Contains("phishing")) return "phishing";
            if (text.Contains("password")) return "password";
            if (text.Contains("scam")) return "scam";
            if (text.Contains("privacy")) return "privacy";
            if (text.Contains("cyber")) return "cybersecurity";

            return "";
        }

        // ---------------- HANDLE EMOTION ----------------
        private void HandleEmotionReply(string input)
        {
            if (input.Contains("worried") ||
                input.Contains("nervous") ||
                input.Contains("scared"))
            {
                Say("Bot: It's okay to feel that way. Cybersecurity can feel overwhelming sometimes.");
                Say("Bot: Here's a helpful tip:");
                Say("Bot: " + cybersecurityTips[rand.Next(cybersecurityTips.Length)]);
            }
            else if (input.Contains("happy") ||
                     input.Contains("good") ||
                     input.Contains("great"))
            {
                Say("Bot: That's awesome 😊");
                Say("Bot: What cybersecurity topic would you like to explore?");
            }
            else if (input.Contains("frustrated") ||
                     input.Contains("confused"))
            {
                Say("Bot: I understand. I'll keep things simple for you.");
                Say("Bot: " + cybersecurityTips[rand.Next(cybersecurityTips.Length)]);
            }
            else
            {
                Say("Bot: Thanks for sharing 😊");
            }
        }

        // ---------------- DETECT EMOTION ----------------
        private bool DetectEmotion(string input)
        {
            if (input.Contains("worried") ||
                input.Contains("nervous"))
            {
                Say("Bot: I understand you're worried. Let's work through it together.");
                return true;
            }

            if (input.Contains("curious"))
            {
                Say("Bot: I like your curiosity 😊");
                return true;
            }

            if (input.Contains("frustrated") ||
                input.Contains("confused"))
            {
                Say("Bot: I understand. Cybersecurity can get confusing sometimes.");
                return true;
            }

            return false;
        }

        // ---------------- QUICK TOPICS ----------------
        private void QuickTopic_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;

            if (btn == null)
                return;

            string tag = btn.Tag?.ToString() ?? "";

            if (string.IsNullOrEmpty(tag))
                return;

            UserInput.Text = tag;

            SendButton_Click(sender, e);
        }

        // ---------------- CLEAR CHAT ----------------
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ChatHistory.Items.Clear();
            chatLog.Clear();

            try
            {
                if (File.Exists(historyFile))
                {
                    File.Delete(historyFile);
                }
            }
            catch
            {
            }

            Say("Bot: Chat history cleared 😊");
        }

        // ---------------- SAY METHOD ----------------
        private void Say(string msg)
        {
            ChatHistory.Items.Add(msg);

            chatLog.Add(msg);

            SaveChatToFile(msg);

            ChatHistory.ScrollIntoView(ChatHistory.Items[ChatHistory.Items.Count - 1]);
        }

        // ---------------- CHAT HISTORY BUTTON ----------------
        private void BtnChatbot_Click(object sender, RoutedEventArgs e)
        {
            ChatHistory.Items.Add("========== START OF CHAT HISTORY ==========");

            foreach (var msg in chatLog)
            {
                ChatHistory.Items.Add(msg);
            }

            ChatHistory.Items.Add("=========== END OF CHAT HISTORY ===========");

            ChatHistory.ScrollIntoView(ChatHistory.Items[ChatHistory.Items.Count - 1]);
        }

        // ---------------- HELP BUTTON ----------------
        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            Say("Bot: Ask me about:");
            Say("Bot: • Phishing");
            Say("Bot: • Passwords");
            Say("Bot: • Scams");
            Say("Bot: • Privacy");
            Say("Bot: • Cybersecurity");
        }

        // ---------------- EXIT BUTTON ----------------
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Say("Bot: Goodbye, " + userName + "! Stay safe online 😊");

            Application.Current.Shutdown();
        }
    }

    // ---------------- AUDIO ----------------
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
            }
        }
    }
}