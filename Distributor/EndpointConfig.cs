using NServiceBus;

namespace Rikstoto.Distributor
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .DefineEndpointName("test")
                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .DisableTimeoutManager()
                .DisableSecondLevelRetries();
        }
    }
}
