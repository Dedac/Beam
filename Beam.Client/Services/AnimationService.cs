using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Beam.Client.Services
{
    public class AnimationService
    {
        private IJSRuntime _jsRuntime;
        
        public AnimationService(IJSRuntime jSRuntime) 
        {
            _jsRuntime = jSRuntime;
        } 

        public ValueTask LoadAnimation(string elementId, int width, int height)
        {
            return _jsRuntime.InvokeVoidAsync("animatedBeam.loadAnimation", elementId, width, height);
        }
    }
    
}