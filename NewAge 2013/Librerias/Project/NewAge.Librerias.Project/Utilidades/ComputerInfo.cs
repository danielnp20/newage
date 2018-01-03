using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.Management;
using System.Net.NetworkInformation;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Manage de PC info
    /// </summary>
    public static class ComputerInfo
    {
        /// <summary>
        /// Get the computers name
        /// </summary>
        /// <returns>Returns the computer name</returns>
        public static string GetComputerName()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// Get the list of local addresses
        /// </summary>
        /// <returns>Return the list of local ips</returns>
        public static IPAddress[] GetLocalAddresses()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            return localIPs;
        }

        /// <summary>
        /// Return the user ip
        /// </summary>
        /// <returns></returns>
        public static string GetLocalAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(GetComputerName());
            string localIP = string.Empty;

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }

            return localIP;
        }

        /// <summary>
        /// Check if an ipaddress is local
        /// </summary>
        /// <param name="host">Host</param>
        /// <returns>Returns true if it is a local ip otherwise returns false</returns>
        public static bool IsLocalIpAddress(string host)
        {
            try
            { // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Get a machine MAC address
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLocalMACAddress()
        {
            //M1
            List<string> result = 
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up && nic.GetPhysicalAddress().ToString() != string.Empty
                    select nic.GetPhysicalAddress().ToString()
                ).ToList();


            //return listMACMain;
            List<string> listMACEthernet = 
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet || 
                        nic.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx || 
                        nic.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT ||
                        nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                    select nic.GetPhysicalAddress().ToString()
                ).ToList();

            foreach (string mac in listMACEthernet)
            {
                if (!result.Contains(mac))
                    result.Add(mac);
            }

            return result;
        }
    }
}
