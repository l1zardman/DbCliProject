using ArangoDBNetStandard.Transport.Http;

namespace DbcliCoreUtility;

public class DbConnector {
    public static HttpApiTransport GetApiTransport(ConfigParameters parameters) {
        return HttpApiTransport.UsingBasicAuth(
            new Uri(parameters.DbUri),
            parameters.DbName,
            parameters.Username,
            parameters.Password
        );
    }
}