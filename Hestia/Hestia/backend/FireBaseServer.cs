using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Hestia.backend
{
    public class FireBaseServer
    {
        public ServerInteractor Interactor { get; set; }
        // This boolean is used to enable or disable certain in the selection screen.
        public bool Selected { set; get; }

        public FireBaseServer(bool selected, ServerInteractor interactor)
        {
            Interactor = interactor;
            Selected = selected;
        }
    }
}
