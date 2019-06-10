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

        public event Action BeamPassTriggered;
        public Task LoadAnimation(string elementId, int width, int height)
        {
            return _jsRuntime.InvokeAsync<object>
                ("animatedBeam.loadAnimation", elementId, width, height);
        }

        [JSInvokable]
        public Task BeamPassedBy()
        {
            return Task.Run(() => BeamPassTriggered?.Invoke());
        }
    }
}
