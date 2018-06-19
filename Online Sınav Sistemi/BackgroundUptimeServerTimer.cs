using Microsoft.AspNet.SignalR;
using Online_Sınav_Sistemi.Hubs;
using System;
using System.Threading;
using System.Web.Hosting;
using Humanizer;
namespace Online_Sınav_Sistemi
{
    public class BackgroundUptimeServerTimer : IRegisteredObject
    {
       
        private readonly DateTime _internetBirthDate = On.April.The30th.In(2018);
        private readonly IHubContext _uptimeHub;
        private Timer _timer;
        private  int a = 0;
        private  int b=0;
        public BackgroundUptimeServerTimer()
        {
            _uptimeHub = GlobalHost.ConnectionManager.GetHubContext<UptimeHub>();

            StartTimer();
        }
        private void StartTimer()
        {
            var delayStartby = 2.Seconds();
            var repeatEvery = 1.Seconds();
            
            _timer = new Timer(BroadcastUptimeToClients,null,delayStartby, repeatEvery);

        }
        private void BroadcastUptimeToClients(object state)
        {
            TimeSpan uptime = DateTime.Now - _internetBirthDate;
             b = 50 - a;
            _uptimeHub.Clients.All.internetUpTime(b.ToString());
            //_uptimeHub.Clients.All.internetUpTime(uptime.Humanize(3));
            a++;
        }

        public void Stop(bool immediate)
        {
            _timer.Dispose();

            HostingEnvironment.UnregisterObject(this);
        }
        //public void Stop(bool immediate)
        //{
        //    throw new NotImplementedException();
        //}
    }
}