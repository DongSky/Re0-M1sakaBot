using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Vision;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Web;
using System.IO;
using M1sakaBot.Module;
using Microsoft.Bot.Builder.Dialogs;

namespace M1sakaBot {
    [BotAuthentication]
    public class MessagesController : ApiController {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity) {
            if (activity.Type == ActivityTypes.Message) {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await Conversation.SendAsync(activity, () => new MainDialog());
            }
            else {
                HandleSystemMessage(activity);
            }
            //if (activity.Type == ActivityTypes.Message) {
            //    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            //    await Conversation.SendAsync(activity, () => new MainDialog(activity.Text));
                
            //    //else {
            //    //    Activity rep = activity.CreateReply("Other functions are not avaliable right now. ");
            //    //    await connector.Conversations.SendToConversationAsync(rep);
            //    //}
            //    // return our reply to the user
            //    //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
            //    //await connector.Conversations.ReplyToActivityAsync(reply);
            //}
            //else {
            //    HandleSystemMessage(activity);
            //}
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message) {
            if (message.Type == ActivityTypes.DeleteUserData) {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate) {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate) {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing) {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping) {
            }

            return null;
        }
    }
}