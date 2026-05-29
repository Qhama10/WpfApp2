#  Cybersecurity Awareness Bot

A desktop chatbot built with **C# and WPF** that helps users stay safe online by providing tips and guidance on common cybersecurity topics.

---

##  About

The Cybersecurity Awareness Bot is an interactive Windows desktop application that engages users in a friendly conversation about online safety. It remembers user preferences, detects emotional tone, and delivers targeted tips on topics like phishing, passwords, scams, and privacy.

---

##  Features

- **Interactive Chat Interface** — Conversational bot that greets users by name and maintains context throughout the session.
- **Cybersecurity Tips** — Randomised, practical tips across five key topics:
  - 🎣 Phishing
  - 🔑 Passwords
  - 🛡️ Cybersecurity
  - 🚨 Scams
  - 🔒 Privacy
- **Sentiment Detection** — Recognises emotions like worry, curiosity, or frustration and responds empathetically.
- **User Memory** — Remembers the user's name, favourite topic, and last active topic across sessions using local file storage.
- **Favourite Topics** — Save, revisit, and remove favourite cybersecurity topics from a persistent sidebar panel.
- **Chat History** — Automatically saves all conversations to `ChatHistory.txt` and reloads them on startup.
- **Quick Topic Buttons** — One-click shortcuts to jump straight into any cybersecurity topic.
- **Audio Greeting** — Plays a welcome sound (`WelcomeMessage.wav`) on launch.
- **Follow-up Flow** — Supports follow-up phrases like *"another tip"*, *"tell me more"*, and *"yes"* to continue a topic.

---

##  Project Structure

```
WpfApp2/
├── App.xaml                  # Application entry point
├── App.xaml.cs
├── MainWindow.xaml           # Main UI layout (XAML)
├── MainWindow.xaml.cs        # Core chatbot logic and UI code-behind
├── Chatbot.cs                # Standalone keyword-based response engine
├── Audio.cs                  # Audio playback helper
├── AssemblyInfo.cs
├── WelcomeMessage.wav        # Greeting audio file
├── WpfApp2.csproj            # Project file
├── WpfApp2.slnx              # Solution file
└── .github/workflows/        # GitHub Actions CI workflows
```

**Runtime-generated files:**

| File | Purpose |
|---|---|
| `ChatHistory.txt` | Persists all chat messages across sessions |
| `UserInterests.txt` | Saves the user's favourite topic |
| `UserFavorites.txt` | Saves the full list of bookmarked topics |

---

## Getting Started

### Prerequisites

- Windows 10 or later
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download) or later
- Visual Studio 2022 (recommended) or any IDE with WPF support

### Running the App

1. Clone the repository:
   ```bash
   git clone https://github.com/Qhama10/WpfApp2.git
   cd WpfApp2
   ```

2. Open `WpfApp2.slnx` in Visual Studio.

3. Build and run the project (`F5` or **Debug > Start Debugging**).

---

## 💬 How to Use

1. **Enter your name** when prompted — the bot will greet you and personalise the session.
2. **Ask about a topic** by typing or clicking a quick-topic button:
   - *"Tell me about phishing"*
   - *"Give me a password tip"*
   - *"What should I know about scams?"*
3. **Save your interests** by saying *"I'm interested in privacy"* — the bot will remember this for future sessions.
4. **Follow up** with phrases like *"another tip"*, *"tell me more"*, or *"yes"* to keep exploring a topic.
5. **Check your mood** — say *"I'm feeling worried"* or *"I'm curious"* and the bot will respond accordingly.
6. **Revisit interests** by saying *"Remind me what I'm interested in"*.
7. **Get help** at any time by typing `help`.

---

##  Built With

- **C# / .NET** — Core application logic
- **WPF (Windows Presentation Foundation)** — Desktop UI framework
- **XAML** — UI layout and styling
- **System.Media.SoundPlayer** — Audio playback
- **File I/O** — Local persistence for chat history and user preferences

---

##  Releases

See the [Releases page](https://github.com/Qhama10/WpfApp2/releases) for the latest build downloads.

---

##  Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.

---

## Author

**Qhama10** — [GitHub Profile](https://github.com/Qhama10)
