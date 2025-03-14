using Shouldly;
using StdEx.Net;
using System.Net;
using System.Net.Sockets;

namespace StdEx.Tests.Net;

public class PortUtilsTests
{
    [Fact]
    public void FindAvailable_ShouldReturnAvailablePort()
    {
        // Act
        var port = PortUtils.FindAvailable(8000);

        // Assert
        port.ShouldBeGreaterThanOrEqualTo(8000);
        // Verify the port is actually available
        Should.NotThrow(() =>
        {
            var listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            listener.Stop();
        });
    }

    [Theory]
    [InlineData(21)]  // FTP port
    [InlineData(22)]  // SSH port
    public void FindAvailable_WithUnsafeStartPort_ShouldSkipUnsafePorts(int unsafePort)
    {
        // Act
        var port = PortUtils.FindAvailable(unsafePort);

        // Assert
        port.ShouldBeGreaterThan(unsafePort);
    }

    [Theory]
    [InlineData(8000, 70000)]  // endPort too large
    [InlineData(9000, 8000)]   // startPort > endPort
    public void FindAvailable_WithInvalidPortRange_ShouldThrowArgumentException(int startPort, int endPort)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => PortUtils.FindAvailable(startPort, endPort));
    }

    [Fact]
    public void FindAvailable_WhenAllPortsAreTaken_ShouldThrowException()
    {
        // Arrange
        var listeners = new List<TcpListener>();
        try
        {
            // Take up several ports
            for (int i = 8000; i < 8010; i++)
            {
                var listener = new TcpListener(IPAddress.Loopback, i);
                listener.Start();
                listeners.Add(listener);
            }

            // Act & Assert
            var exception = Should.Throw<Exception>(() => 
                PortUtils.FindAvailable(8000, 8009));
            exception.Message.ShouldBe("Can't find available port between 8000 and 8009");
        }
        finally
        {
            // Cleanup
            foreach (var listener in listeners)
            {
                listener.Stop();
            }
        }
    }
}
