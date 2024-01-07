using System;

namespace LifeInDiary.Libs
{
    public class HomeScreenStyle
    {
        public static string GetRandomDarkQuote()
        {
            string[] darkQuotes =
                {
                "Then let me ask you, if your God would allow my madness to flourish across the globe, then wouldn’t it seem to you that any god like that would be just as mad as I?",
                "It’s odd, isn’t it? People die everyday and the world goes on like nothing happened.",
                "Ha! These humans are definitely foolish creatures. Think as hard as those weak brains of yours can manage. Do you humans ever listen to the cries of mercy coming from pigs and cows you slaughter?",
                "Man seeks peace, yet at the same time yearning for war… Those are the two realms belonging solely to man. Thinking of peace whilst spilling blood is something that only humans could do. They’re two sides of the same coin… to protect something… another must be sacrificed",
                "People cannot show each other their true feelings. Fear, suspicion, and resentment never subside",
                "Knowledge and awareness are vague, and perhaps better called illusions. Everyone lives within their own subjective interpretation",
                "In this world, wherever there is light – there are also shadows. As long as the concept of winners exists, there must also be losers. The selfish desire of wanting to maintain peace causes wars, and hatred is born to protect love",
                "Wake up to reality! Nothing ever goes as planned in this world. The longer you live, the more you realize that in this reality only pain, suffering and futility exist",
                "Never trust anyone too much, remember the devil was once an angel",
                "Pretty words aren't always true, and True words aren't always pretty",
                "No one notices your tears. No one notices your sadness. No one notices your pain. But they all notice your mistakes.",
                "Fake people have an image to maintain. Real people just don’t care",
                "Don't depend too much on anyone in this world. Even your shadow leaves you when you are in darkness",
                "Humans - Treat them nicely, you'll look like a fool. Treat them badly, you'll look like a bitch. Treat them normally, they'll find something else to say",
                "Expect nothing and you will never be dissapointed",
                "A villain is just a victim whose story hasn't been told",
                "Always think before you say something because words can leave mental scars that might never heal",
                "Humans are often more stupid then they realise",
                "Give a man a mask and he'll become his true self",
                "In the human world, truth and reality aren’t always one and the same. Humans just call their desires and ambitions as “truth”. Humans will even kill other humans if they have “truth” as an excuse.",
            };

            int random = new Random().Next(0, darkQuotes.Length - 1);
            string darkQuote = darkQuotes[random];
            return darkQuote;
        }

        public static string GetRandomMotivationalQuote()
        {
            string[] quotesArr =
                {
                "Maybe, just maybe, there is no purpose in life… but if you linger a while longer in this world, you might discover something of value in it",
                "Fake people have an image to maintain. Real people just don’t care",
            };
            int random = new Random().Next(0, quotesArr.Length - 1);
            string quote = quotesArr[random];
            return quote;
        }

    }
}
