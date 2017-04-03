using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace M1sakaBot.Module {
    [Serializable]
    public class TestDialog : IDialog<object> {
        string choice;
        public async Task StartAsync(IDialogContext context) {
            await context.PostAsync("Testing for Card, reply 0 to cancel, 1 to test. ");
            context.Wait(TestSendCard);
        }
        public async Task TestSendCard(IDialogContext context, IAwaitable<IMessageActivity> argument) {
            var message = await argument;
            choice = message.Text;
            if (choice == "1") {
                var m = context.MakeMessage();
                List<CardAction> cardButton = new List<CardAction>();
                for (int i = 0; i < 4; i++) {
                    CardAction button = new CardAction() {
                        Title = i.ToString(),
                        Type = "imBack",
                        Value = i.ToString()
                    };
                    cardButton.Add(button);
                }
                HeroCard card = new HeroCard() {
                    Title = "Test Card",
                    Buttons = cardButton
                };
                Attachment repAttachment = card.ToAttachment();
                m.Attachments = new List<Attachment> {repAttachment};
                await context.PostAsync(m);
            }
            else if (choice == "0") {
                await context.PostAsync("Test terminated. ");
            }
            context.Done(0);
        }

    }
}