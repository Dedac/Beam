using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beam.Animation
{
    public static class Javascript
    {
        public static event Action BeamPassTriggered;
        public static Task LoadAnimation(string elementId, int width, int height)
        {
            return JSRuntime.Current.InvokeAsync<object>
                ("animatedBeam.loadAnimation", elementId, width, height);
        }

        [JSInvokable]
        public static Task BeamPassedBy()
        {
            return Task.Run(() => BeamPassTriggered?.Invoke());
        }
    }
}
