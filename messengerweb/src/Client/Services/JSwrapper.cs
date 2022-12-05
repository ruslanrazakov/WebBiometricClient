using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace MessengerWeb.Client.Services
{
    public class JSwrapper
    {
        private readonly IJSRuntime jSRuntime;

        public JSwrapper(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public async Task StartVideo()
            => await jSRuntime.InvokeVoidAsync("startVideo");

        public async Task StopVideo()
            => await jSRuntime.InvokeVoidAsync("stopVideo");

        public async Task SnapFrame()
            => await jSRuntime.InvokeVoidAsync("Snap", "video", "capturedImage");

        public async Task<string> GetImageData()
            => await jSRuntime.InvokeAsync<string>("GetImageData", "capturedImage", "image/jpeg");

        public async Task<string> RecordVideoAndPost(string url, string engineId)
            => await jSRuntime.InvokeAsync<string>("recordVideoAndSendToServer", url, engineId);
    }
}
