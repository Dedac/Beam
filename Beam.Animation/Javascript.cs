using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Beam.Animation
{
    public class Javascript
    {
        private IJSRuntime _jsRuntime;
        public Javascript(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public static event Action? BeamPassTriggered;
        public ValueTask<string> LoadAnimation(string elementId, int width, int height)
        {
            return _jsRuntime.InvokeAsync<string>
                ("animatedBeam.loadAnimation", elementId, width, height);
        }

        [JSInvokable]
        public static Task BeamPassedBy()
        {
            return Task.Run(() => BeamPassTriggered?.Invoke());
        }
    }
}
