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

        [TestMethod]
        public void Get_table()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.GetDatabase("Teste").GetTable<Pessoa>("Teste");
        }

        [TestMethod]
        public void Add_Permission()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.GetDatabase("Teste").AddPermission("fabriciodupont@hotmail.com", Enum.Role.reader, Enum.Type.user);
        }

        [TestMethod]
        public void Get_Permission()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.GetDatabase("Teste").GetPermission("fabriciodupont@hotmail.com");
        }

        [TestMethod]
        public void Delete_Permission()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.GetDatabase("Teste").GetPermission("fabriciodupont@hotmail.com").Delete();
        }

        [TestMethod]
        public void Update_Permission()
        {
            var managment = new Managment(
                clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
                privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
            );

            managment.GetDatabase("Teste").GetPermission("fabriciodupont@hotmail.com").Update(Enum.Role.writer);
        }

        [TestMethod]
        public void Delete_table()
        {
            var managment = new Managment(
               clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
               privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
           );

            managment.GetDatabase("Teste").GetTable<Pessoa>("Teste").Delete();
        }

        [TestMethod]
        public void Rename_table()
        {
            var managment = new Managment(
               clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
               privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
           );

            managment.GetDatabase("Teste").GetTable<Pessoa>("Teste").Rename("Teste2");
        }

        [TestMethod]
        public void Add_Record()
        {
            var managment = new Managment(
               clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
               privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
           );

            managment.GetDatabase("Teste").GetTable<Pessoa>("Teste").Add(new Pessoa() { Id = 1, Nome = "a" });
        }

        [TestMethod]
        public void Delete_Record()
        {
            var managment = new Managment(
               clientEmail: "teste-502@subtle-girder-125713.iam.gserviceaccount.com",
               privateKey: File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + "\\SheetDB.p12")
           );

            managment.GetDatabase("Teste").GetTable<Pessoa>("Teste").Add(new Pessoa() { Id = 1, Nome = "c" }).Delete();
        }
    }

    public class Pessoa
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }
}
