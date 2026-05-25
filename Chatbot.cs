using System;
using System.Collections.Generic;


namespace WpfApp2
{
    public class Chatbot
    {
        private Random rand = new Random();

        private Dictionary<string, string[]> responses = new()
        {
            { "password", new[] {
                "Use strong unique passwords for each account.",
                "Never reuse passwords across sites."
            }},
            { "scam", new[] {
                "Be careful of messages asking for personal info.",
                "Scammers often pretend to be trusted companies."
            }},
            { "privacy", new[] {
                "Check your privacy settings regularly.",
                "Avoid oversharing personal info online."
            }},
            { "phishing", new[] {
                "Don’t click unknown links in emails.",
                "Always verify sender identity before responding."
            }}
        };

        public string GetResponse(string input)
        {
            input = input.ToLower();

            // SENTIMENT DETECTION
            if (input.Contains("worried"))
                return "It's okay to feel worried. Let me help you stay safe online.";

            if (input.Contains("curious"))
                return "Great! Curiosity helps you stay cyber-aware.";

            if (input.Contains("frustrated"))
                return "I understand. Let’s go step by step.";

            // KEYWORD MATCHING
            foreach (var key in responses.Keys)
            {
                if (input.Contains(key))
                {
                    var list = responses[key];
                    return list[rand.Next(list.Length)];
                }
            }

            // CONVERSATION FLOW
            if (input.Contains("tell me more"))
                return "Sure! Let’s continue on the topic you asked about.";

            if (input.Contains("another tip"))
                return "Here’s another useful cybersecurity tip for you.";

            // DEFAULT ERROR HANDLING
            return "I'm not sure I understand. Can you try rephrasing?";
        }
    }
}
