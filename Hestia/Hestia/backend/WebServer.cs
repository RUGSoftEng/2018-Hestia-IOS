namespace Hestia.backend
{
    public class WebServer
    {
        public ServerInteractor Interactor { get; set; }
        // This boolean is used to enable or disable certain in the selection screen.
        public bool Selected { set; get; }

        public WebServer(bool selected, ServerInteractor interactor)
        {
            Interactor = interactor;
            Selected = selected;
        }
    }
}
