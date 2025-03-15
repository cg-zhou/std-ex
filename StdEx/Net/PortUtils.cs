using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace StdEx.Net
{
    /// <summary>
    /// Provides utility methods for working with network ports.
    /// </summary>
    public static class PortUtils
    {
        /// <summary>
        /// List of well-known ports that should be avoided for dynamic allocation.
        /// </summary>
        private static readonly int[] UnsafePorts = {
            1, 7, 9, 11, 13, 15, 17, 19, 20, 21, 22, 23, 25, 37, 42,
            43, 53, 77, 79, 87, 95, 101, 102, 103, 104, 109, 110, 111, 113, 115, 117,
            119, 123, 135, 139, 143, 179, 389, 465, 512, 513, 514, 515, 526, 530, 531,
            532, 540, 556, 563, 587, 601, 636, 993, 995, 2049, 3659, 4045, 6000, 6665,
            6666, 6667, 6668, 6669
        };

        /// <summary>
        /// Finds an available TCP port within the specified range.
        /// </summary>
        /// <param name="startPort">The lower bound of the port range to search (inclusive). Defaults to 6000.</param>
        /// <param name="endPort">The upper bound of the port range to search (inclusive). Defaults to 65535.</param>
        /// <returns>The first available port number within the range.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when endPort is greater than 65535 or startPort is greater than endPort.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when no available port can be found in the specified range.
        /// </exception>
        public static int FindAvailable(int startPort = 6000, int endPort = 65535)
        {
            if (endPort > 65535)
                throw new ArgumentException("End port cannot be greater than 65535", nameof(endPort));
            if (startPort > endPort)
                throw new ArgumentException("Start port cannot be greater than end port", nameof(startPort));

            var port = startPort;
            bool isAvailable = false;

            while (!isAvailable && port <= endPort)
            {
                if (UnsafePorts.Contains(port))
                {
                    port++;
                    continue;
                }

                try
                {
                    var listener = new TcpListener(IPAddress.Loopback, port);
                    listener.Start();
                    listener.Stop();
                    isAvailable = true;
                }
                catch
                {
                    port++;
                }
            }

            if (!isAvailable)
            {
                throw new Exception($"Can't find available port between {startPort} and {endPort}");
            }

            return port;
        }
    }
}
