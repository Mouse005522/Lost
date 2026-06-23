using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.Tool
{
    public class InputController : FlowService
    {
        [EasyInject]
        IServiceCollection serviceCollection;
        public InteractInputProvider InteractInput { get; private set; }

        List<InputProvider> inputProviders = new List<InputProvider> ();

        protected override void Init ()
        {
            base.Init ();

            InteractInput = serviceCollection.CreateRequest<KeyboardMouseInteractInput> ();
        }

        protected override void DoUpdate (float deltaTime)
        {
            base.DoUpdate (deltaTime);

            foreach (var provider in inputProviders)
            {
                provider.Update (deltaTime);
            }
        }
    }
}
