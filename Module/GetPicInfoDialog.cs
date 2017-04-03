using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace M1sakaBot.Module {
    [Serializable]
    public class GetPicInfoDialog : IDialog<object> {
        private string imageUrl=null;
        private string visionKey = "your key";
        public async Task StartAsync(IDialogContext context) {
            await context.PostAsync("Send a Photo to me please.");
            context.Wait(Process);
        }
        public async Task Process(IDialogContext context, IAwaitable<IMessageActivity> argument) {
            var message = await argument;
            if (message.Attachments != null) {
                imageUrl = message.Attachments.First().ContentUrl;
            }
            if (imageUrl!=null) {
                imageUrl = message.Attachments.First().ContentUrl;
                //await context.PostAsync("Get");
                VisionServiceClient visionClient = new VisionServiceClient(visionKey);
                VisualFeature[] visualFeature = new VisualFeature[] {
                    VisualFeature.Categories,
                    VisualFeature.Tags,
                    VisualFeature.Adult
                };
                AnalysisResult analysisResult = null;
                WebClient client = new WebClient();
                //await context.PostAsync("Start Analyzing");
                using (Stream imageFileStream = client.OpenRead(imageUrl)) {
                    try {
                        analysisResult = await visionClient.AnalyzeImageAsync(imageFileStream, visualFeature);
                    }
                    catch (Exception e) {
                        analysisResult = null;
                    }
                }
                //client.Dispose();
                string tags;
                if (analysisResult == null) await context.PostAsync("Error");
                if (analysisResult != null) {
                    double AdultScore = analysisResult.Adult.AdultScore;
                    if (analysisResult.Tags.Length > 0) {
                        tags = analysisResult.Tags[0].Name + ":" + analysisResult.Tags[0].Confidence.ToString();
                    }
                    else {
                        tags = "None ";
                    }
                        var category = analysisResult.Categories[0].Name + ":" + analysisResult.Categories[0].Score.ToString();
                    //Console.WriteLine(tags);
                    //Console.WriteLine(category);
                    var repContent = "Tag:" + tags + ", " + "Category:";
                    await context.PostAsync(repContent);
                }
                else {
                    await context.PostAsync("Error: Infomation get function failed.");
                }
            }
            else {
                await context.PostAsync("Invalid content.");
            }
        }
    }
}
