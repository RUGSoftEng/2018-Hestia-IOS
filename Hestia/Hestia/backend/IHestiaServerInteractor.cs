using System.Collections.Generic;
using Hestia.backend.models;

namespace Hestia.backend
{
    public interface IHestiaServerInteractor
    {
        List<Device> GetDevices();
        void AddDevice(PluginInfo info);
        void RemoveDevice(Device device);
        List<string> GetCollections();
        List<string> GetPlugins(string collection);
        PluginInfo GetPluginInfo(string collection, string plugin);
        Dictionary<string, string> GetRequiredPluginInfo(string collection, string plugin);
    }
}
