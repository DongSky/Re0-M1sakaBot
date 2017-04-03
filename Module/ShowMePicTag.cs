using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace M1sakaBot.Module {
    public class ShowMePicTag {
        private string visionKey;
        private Activity activity;
        private ConnectorClient connector;
        public ShowMePicTag(Activity activity, ConnectorClient connector) {
            this.visionKey = "your key";
            this.activity = activity;
            this.connector = connector;
        }
        public async System.Threading.Tasks.Task executeAsync() {

            //test of vision service
            VisionServiceClient visionClient = new VisionServiceClient(visionKey);
            VisualFeature[] visualFeature = new VisualFeature[] {
                    VisualFeature.Categories,
                    VisualFeature.Tags,
                    VisualFeature.Adult
                };
            AnalysisResult analysisResult = null;

            string imageURL = activity.Attachments.First().ContentUrl;
            WebClient client = new WebClient();
            using (Stream imageFileStream = client.OpenRead(imageURL)) {
                try {
                    analysisResult = await visionClient.AnalyzeImageAsync(imageFileStream, visualFeature);
                }
                catch (Exception e) {
                    analysisResult = null;
                }
            }
            client.Dispose();
            if (activity == null || activity.GetActivityType() != ActivityTypes.Message) {
                Console.WriteLine("Error");
            }
            else {

            }
            Tag t;
            if (analysisResult != null) {
                double AdultScore = analysisResult.Adult.AdultScore;
                var tags = analysisResult.Tags[0].Name + ":" + analysisResult.Tags[0].Confidence.ToString();
                var category = analysisResult.Categories[0].Name + ":" + analysisResult.Categories[0].Score.ToString();
                Console.WriteLine(tags);
                Console.WriteLine(category);
                var repContent = "Tag:" + tags;
                repContent += "\r\n;;;;;;\r\nCategory:" + category;
                //var repContent = "Adult score: " + AdultScore.ToString() +
                //    "\r\nCategory: " + category.ToString() + "\r\nTags: " + tags.ToString();
                var Rep = activity.CreateReply(repContent);
                await connector.Conversations.SendToConversationAsync(Rep);
            }
            else {
                Activity rep = activity.CreateReply("Did you upload an image? ");
                await connector.Conversations.SendToConversationAsync(rep);
            }
        }
    }
}
