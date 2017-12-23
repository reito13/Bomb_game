
#region Include
using System.Threading.Tasks;
using TeamDev.Redis;
#endregion

namespace URedis {
    public class Redis {
        public string ipAddress { get; set; }
        public int port { get; set; }

        private RedisDataAccessProvider client;

        public async Task Connect(string ipAddress, int port=6379) {
            await Task.Run(() => {
                client = new RedisDataAccessProvider();
                client.Configuration.Host = ipAddress;
                client.Configuration.Port = port;
                client.Connect();
            });
        } 

        public async Task Set (string key, string value) {
            await Task.Run(() => {
                client.SendCommand(RedisCommand.SET, key, value);
                client.WaitComplete();
            }); 
        }

        public async Task<string> Get (string key) {
            string ret = "";
            await Task.Run(() => {
                client.SendCommand(RedisCommand.GET, key);
                ret = client.ReadString();
            });
            return ret;
        }
    }
}