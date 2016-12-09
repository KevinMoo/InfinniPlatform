﻿using System.Net;

using InfinniPlatform.Sdk.Http;

namespace InfinniPlatform.Core.Http
{
    internal class HostAddressParser : IHostAddressParser
    {
        public bool IsLocalAddress(string hostNameOrAddress)
        {
            return IsLocalAddress(hostNameOrAddress, out hostNameOrAddress);
        }


        public bool IsLocalAddress(string hostNameOrAddress, out string normalizedAddress)
        {
            var result = false;

            normalizedAddress = hostNameOrAddress;

            if (!string.IsNullOrWhiteSpace(hostNameOrAddress))
            {
                try
                {
                    var hostIPs = Dns.GetHostAddresses(hostNameOrAddress);
                    var localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                    if (hostIPs != null)
                    {
                        foreach (var hostIp in hostIPs)
                        {
                            // localhost
                            if (IPAddress.IsLoopback(hostIp))
                            {
                                normalizedAddress = "+";
                                result = true;
                                break;
                            }

                            if (localIPs != null)
                            {
                                foreach (var localIp in localIPs)
                                {
                                    // local
                                    if (hostIp.Equals(localIp))
                                    {
                                        result = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            return result;
        }
    }
}