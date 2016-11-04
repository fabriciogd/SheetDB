namespace SheetDB.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using Transport;

    [TestClass]
    public class TransportTests
    {
        [TestMethod]
        public void Get_token()
        {
            IConnector connector = ConnectorFactory.Create(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            var request = connector.CreateRequest();
        }
    }
}
