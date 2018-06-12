
namespace Hestia.backend.models
{
    /// <summary>
    /// This is a wrapper class for a hestia server on the Hestia Web Team server.
    /// </summary>
    public class HestiaServer
    {
        public HestiaServerInteractor Interactor { get; set; }
        public bool Selected { set; get; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }

        public HestiaServer(bool selected, HestiaServerInteractor interactor)
        {
            Interactor = interactor;
            Selected = selected;
        }
    }
}
