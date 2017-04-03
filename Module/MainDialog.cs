using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace M1sakaBot.Module {
    [Serializable]
    public class MainDialog:IDialog<object> {
        public async Task StartAsync(IDialogContext context) {
            context.Wait(Continue);
        }
        
        private async Task Callback(IDialogContext context, IAwaitable<object> argument) {
            context.Wait(Continue);
            //throw new NotImplementedException();
        }

        private async Task Continue(IDialogContext context, IAwaitable<IMessageActivity> argument) {
            var message = await argument;
            if (message.Text == "testcard") {
                context.Call(new TestDialog(), Callback);
            }else if (message.Text=="picture-info") {
                context.Call(new GetPicInfoDialog(), Callback);
            }else {
                await context.PostAsync("Other functions are not avaliable right now. ");
                context.Wait(Continue);
            }
            //throw new NotImplementedException();
        }
    }
}