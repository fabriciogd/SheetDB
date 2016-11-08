namespace SheetDB.Tests
{
    using Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Linq;

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

        [TestMethod]
        public void Get_all_database()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.GetAllDatabases().ToList();
        }

        [TestMethod]
        public void Delete_database()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.GetDatabase("Teste").Delete();
        }

        [TestMethod]
        public void Create_table()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.GetDatabase("Teste").CreateTable<Pessoa>("Teste");
        }
    }

    public class Pessoa
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }
}
