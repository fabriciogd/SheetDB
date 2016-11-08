namespace SheetDB.Tests
{
    using Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;

    [TestClass]
    public class ManagmentTests
    {
        [TestMethod]
        public void Create_database()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.CreateDatabase("Teste");
        }

        [TestMethod]
        public void Get_database()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.GetDatabase("Teste");
        }
    }
}
