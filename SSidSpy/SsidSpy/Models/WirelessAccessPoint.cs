using NativeWifi;

namespace SsidSpy.Models
{
    //From https://www.cnblogs.com/ywf520/p/6502452.html
    class WirelessAccessPoint
    {
        public string SSID { get; set; }
        public string Dot11DefaultAuthAlgorithm { get; set; }
        public string Dot11DefaultCipherAlgorithm { get; set; }
        public bool NetworkConnectable { get; set; }
        public string WlanNotConnectableReason { get; set; }
        public int WlanSignalQuality { get; set; }
        public WlanClient.WlanInterface wlanInterface { get; set; }

        public WirelessAccessPoint()
        {
            SSID = "NONE";
            Dot11DefaultAuthAlgorithm = "";
            Dot11DefaultCipherAlgorithm = "";
            NetworkConnectable = true;
            WlanNotConnectableReason = "";
            WlanSignalQuality = 0;
            wlanInterface = null;
        }
    }

}
