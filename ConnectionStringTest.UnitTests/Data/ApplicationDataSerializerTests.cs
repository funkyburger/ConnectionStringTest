using ConnectionStringTest.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace ConnectionStringTest.UnitTests.Data
{
    [TestClass]
    public class ApplicationDataSerializerTests
    {
        [TestMethod]
        public void ApplicationDataIsDeserializedProperly()
        {
            var serializer = new ApplicationDataSerializer();

            var serializedData = "[History]\n"
                + "Data Source=srouce;Initial Catalog=catalog;Connection Timeout=60;User id=user;Password=p4ssw0rd;\n"
                + "Data Source=othersource;Initial Catalog=blah;Connection Timeout=60;User id=user;Password=p4ssw0rd;\n"
                + "Data Source=toto;Initial Catalog=titi;Connection Timeout=60;User id=tata;Password=tutu;\n";

            var data = serializer.Deserialize(serializedData);

            data.History.ShouldContain("Data Source=srouce;Initial Catalog=catalog;Connection Timeout=60;User id=user;Password=p4ssw0rd;");
            data.History.ShouldContain("Data Source=othersource;Initial Catalog=blah;Connection Timeout=60;User id=user;Password=p4ssw0rd;");
            data.History.ShouldContain("Data Source=toto;Initial Catalog=titi;Connection Timeout=60;User id=tata;Password=tutu;");
        }

        [TestMethod]
        public void ContentWithoutSectionThrowsException()
        {
            var exceptionThrown = false;
            var serializer = new ApplicationDataSerializer();

            var serializedData = "blablabla\n"
                + "[History]\n"
                + "Data Source=srouce;Initial Catalog=catalog;Connection Timeout=60;User id=user;Password=p4ssw0rd;\n"
                + "Data Source=othersource;Initial Catalog=blah;Connection Timeout=60;User id=user;Password=p4ssw0rd;\n"
                + "Data Source=toto;Initial Catalog=titi;Connection Timeout=60;User id=tata;Password=tutu;\n";

            try
            {
                var data = serializer.Deserialize(serializedData);
            }
            catch
            {
                exceptionThrown = true;
            }
            
            exceptionThrown.ShouldBeTrue();
        }

        [TestMethod]
        public void ApplicationDataIsSerializedProperly()
        {
            var serializer = new ApplicationDataSerializer();

            var data = new ApplicationData(new string[] {
                "Data Source=srouce;Initial Catalog=catalog;Connection Timeout=60;User id=user;Password=p4ssw0rd;",
                "Data Source=othersource;Initial Catalog=blah;Connection Timeout=60;User id=user;Password=p4ssw0rd;",
                "Data Source=toto;Initial Catalog=titi;Connection Timeout=60;User id=tata;Password=tutu;"
            });

            serializer.Serialize(data).ShouldBe("[History]\n"
                + "Data Source=srouce;Initial Catalog=catalog;Connection Timeout=60;User id=user;Password=p4ssw0rd;\n"
                + "Data Source=othersource;Initial Catalog=blah;Connection Timeout=60;User id=user;Password=p4ssw0rd;\n"
                + "Data Source=toto;Initial Catalog=titi;Connection Timeout=60;User id=tata;Password=tutu;\n");
        }

        [TestMethod]
        public void DataIsKeptConsistent()
        {
            var serializer = new ApplicationDataSerializer();

            var data = new ApplicationData(new string[] {
                "Data Source=srouce;Initial Catalog=catalog;Connection Timeout=60;User id=user;Password=p4ssw0rd;",
                "Data Source=othersource;Initial Catalog=blah;Connection Timeout=60;User id=user;Password=p4ssw0rd;",
                "Data Source=toto;Initial Catalog=titi;Connection Timeout=60;User id=tata;Password=tutu;"
            });

            var serializedData = serializer.Serialize(data);

            var deserializedData = serializer.Deserialize(serializedData);

            deserializedData.History.ToArray().ShouldBe(data.History.ToArray());
        }

        [TestMethod]
        public void DataIsKeptConsistentOtherWayAround()
        {
            var serializer = new ApplicationDataSerializer();

            var serializedData = "[History]\n"
                + "Data Source=srouce;Initial Catalog=catalog;Connection Timeout=60;User id=user;Password=p4ssw0rd;\n"
                + "Data Source=othersource;Initial Catalog=blah;Connection Timeout=60;User id=user;Password=p4ssw0rd;\n"
                + "Data Source=toto;Initial Catalog=titi;Connection Timeout=60;User id=tata;Password=tutu;\n";

            var deserializedData = serializer.Deserialize(serializedData);

            var redeserializedData = serializer.Serialize(deserializedData);

            redeserializedData.ShouldBe(serializedData);
        }
    }
}
