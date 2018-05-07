namespace Hestia.backend
{
    public class WebServer
    {
        public ServerInteractor Interactor { get; set; }
        public bool Selected { set; get; }
        public string Name { get; set; }
        public string Id { get; set; }

        public WebServer(bool selected, ServerInteractor interactor)
        {
            Interactor = interactor;
            Selected = selected;
        }
    }
}
